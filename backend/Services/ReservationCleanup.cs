
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PublicTransportNavigator.Services
{
    public class ReservationCleanup(IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        private Timer? _timer;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(Cleanup, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }

        private async void Cleanup(object? state)
        {
            //await _context.Database.ExecuteSqlRawAsync(
            //    "DELETE FROM public.reserved_seats WHERE valid_until < {0}", DateTime.UtcNow);
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var context =
                    scope.ServiceProvider
                        .GetRequiredService<PublicTransportNavigatorContext>();
                await context.ReservedSeats.Where(rs => (rs.ValidUntil != null && rs.ValidUntil < DateTime.UtcNow))
                    .ExecuteDeleteAsync();
            }
            catch (OperationCanceledException)
            {
                
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _timer.Change(Timeout.Infinite, 0);
            await base.StopAsync(stoppingToken);
        }

        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }
    }
}
