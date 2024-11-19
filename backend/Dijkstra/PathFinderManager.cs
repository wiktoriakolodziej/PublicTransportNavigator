using System.Collections.Concurrent;
using System.Configuration;
using System.Diagnostics;
using PublicTransportNavigator.Dijkstra.AStar;

namespace PublicTransportNavigator.Dijkstra
{
    public class PathFinderManager<TNode> : IPathFinderManager where TNode : Node
    {
        private readonly int _checkIntervalMilliseconds;
        private readonly int _timeoutMilliseconds;
        private readonly ConcurrentQueue<DijkstraPathFinder<TNode>> _workers = [];
        private readonly IServiceScopeFactory _serviceProvider;

        private void InitializeWorkers()
        {
            foreach (var worker in _workers)
            {
                worker.SyncNodes();
            }
        }

        public PathFinderManager(IConfiguration configuration, IServiceScopeFactory serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var pathFinderSettings = configuration.GetSection("PathFinderSettings");
            int.TryParse(pathFinderSettings["NumOfWorkers"], out var numberOfWorkers);
            int.TryParse(pathFinderSettings["CheckIntervalMilliseconds"], out _checkIntervalMilliseconds);
            int.TryParse(pathFinderSettings["TimeoutMilliseconds"], out _timeoutMilliseconds);



            for (var i = 0; i < numberOfWorkers; i++)
            {
                if (typeof(TNode) == typeof(NodeAs))
                {
                    _workers.Enqueue(new AStarPathFinder(_serviceProvider) as DijkstraPathFinder<TNode>);
                }
                else
                {
                    _workers.Enqueue(new DijkstraPathFinder<TNode>(_serviceProvider));
                }
            }

            InitializeWorkers();
            //return Task.CompletedTask;
        }

        public async Task<RouteDetails> FindPath(long sourceBusStopId, long destinationBusStopId, TimeSpan departureTime)
        {
            var stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < _timeoutMilliseconds)
            {
                if (_workers.TryDequeue(out var worker))
                {
                    if (worker == null)
                        throw new Exception(
                            $"Unexpected exception in {nameof(PathFinderManager<TNode>.FindPath)}, worker was null");
                    await worker.Available;
                    var result = worker.FindPath(sourceBusStopId, destinationBusStopId, departureTime);

                    CleanUpNodes(worker);

                    return result;
                }

                // Wait for the given interval before trying again
                await Task.Delay(_checkIntervalMilliseconds);
            }

            throw new TimeoutException($"Waiting for available worker took too long, class {nameof(PathFinderManager<TNode>)}");
        }

        private void CleanUpNodes(DijkstraPathFinder<TNode> worker)
        {
            Task.Run(async () =>
            {
                worker.CleanUpNodes();
                await worker.Available;
                _workers.Enqueue(worker);
            });
        }

    }
}
