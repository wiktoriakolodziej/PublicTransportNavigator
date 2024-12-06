using PublicTransportNavigator.Dijkstra.AStar;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace PublicTransportNavigator.Dijkstra
{
    public class PathFinderManager : IPathFinderManager
    {
        private readonly int _checkIntervalMilliseconds;
        private readonly int _timeoutMilliseconds;
        private readonly ConcurrentQueue<DijkstraPathFinder> _workers = [];
        private readonly bool _properlyInitialized = true;

        private const string PathFinderName = "PathFinderType";
        private const string NumOfWorkers = "NumOfWorkers";
        private const string CheckIntervalMilliseconds = "CheckIntervalMilliseconds";
        private const string TimeoutMilliseconds = "TimeoutMilliseconds";
        private void InitializeWorkers(string? pathFinderType, int numberOfWorkers, IServiceScopeFactory serviceProvider)
        {
            switch (pathFinderType)
            {
                case "AStar":
                    for (var i = 0; i < numberOfWorkers; i++)
                        _workers.Enqueue(new AStarPathFinder(serviceProvider));
                    break;
                case "Dijkstra":
                    for (var i = 0; i < numberOfWorkers; i++)
                        _workers.Enqueue(new DijkstraPathFinder(serviceProvider));
                    break;
                default:
                    throw new ArgumentException($"Argument named {PathFinderName} has a wrong value");
            }
            foreach (var worker in _workers)
            {
                worker.PrepareGraphs();
            }
        }
        public PathFinderManager(IConfiguration configuration, IServiceScopeFactory serviceProvider)
        {

            var pathFinderSettings = configuration.GetSection("PathFinderSettings");
            int.TryParse(pathFinderSettings[NumOfWorkers], out var numberOfWorkers);
            int.TryParse(pathFinderSettings[CheckIntervalMilliseconds], out _checkIntervalMilliseconds);
            int.TryParse(pathFinderSettings[TimeoutMilliseconds], out _timeoutMilliseconds);
            var pathFinderType = (pathFinderSettings[PathFinderName]);

            try
            {
                InitializeWorkers(pathFinderType, numberOfWorkers, serviceProvider);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Couldn't initialize path finding workers in class {nameof(PathFinderManager)} reason: {ex.Message}");
                _properlyInitialized = false;
            }
        }
        public async Task<RouteDetails?> FindPath(long sourceBusStopId, long destinationBusStopId, TimeSpan departureTime, long calendarId)
        {
            if (!_properlyInitialized) return null;
            var stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < _timeoutMilliseconds)
            {
                if (_workers.TryDequeue(out var worker))
                {
                    if (worker == null)
                        throw new Exception(
                            $"Unexpected exception in {nameof(PathFinderManager.FindPath)}, worker was null");
                    await worker.Available;
                    var result = await worker.FindPath(sourceBusStopId, destinationBusStopId, departureTime, calendarId);

                    CleanUpNodes(worker, calendarId);

                    return result;
                }

                // Wait for the given interval before trying again
                await Task.Delay(_checkIntervalMilliseconds);
            }

            throw new TimeoutException($"Waiting for available worker took too long, class {nameof(PathFinderManager)}");
        }
        private void CleanUpNodes(DijkstraPathFinder worker, long calendarId)
        {
            Task.Run(async () =>
            {
                worker.CleanUpNodes(calendarId);
                await worker.Available;
                _workers.Enqueue(worker);
            });
        }
    }
}
