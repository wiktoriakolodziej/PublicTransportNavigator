using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.IO.Compression;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PublicTransportNavigator.Models;
using Microsoft.CodeAnalysis.Differencing;
using PublicTransportNavigator.DTOs.SyncData;
using PublicTransportNavigator.DTOs.old;

namespace PublicTransportNavigator.Services
{
    public class FetchDataService(IServiceScopeFactory serviceScopeFactory, IMapper mapper) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = serviceScopeFactory;
        private const string Pattern = @"https:\/\/otwartedane\.metropoliagzm\.pl\/dataset\/.+\/download\/schedule_ztm_.+\.zip";
        private readonly IMapper _mapper = mapper;

        private const string Url =
            "https://otwartedane.metropoliagzm.pl/dataset/rozklady-jazdy-i-lokalizacja-przystankow-gtfs-wersja-rozszerzona";

        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(30); 
        private readonly TimeSpan _delayBetweenRetries = TimeSpan.FromSeconds(1);
        private const string DestinationPath = "data/zip/output.zip";
        private const string ExtractionPath = "data/unzipped/";
        private const string RouteRegexPattern = """(\d+),\d+,"([^"]+)",[^,]+,[^,]+,(\d+),.+""";
        private const string RoutesPath = "routes.txt";
        private const string StopRegexPattern = """(\d+),[^,]+,"([^,]+),(\d+\.\d+),(\d+\.\d+),.+""";
        private const string StopWithoutCoordsRegexPattern = """^(\d+),[^,]+,"([^"]+).+""";
        private const string StopsPath = "stops.txt";
        private const string TripRegexPattern = """^(\d+),(\d+),[^_]+_(\d+),[^,]+,[^,]+,[^,]+,[^,]+,(\d),.+""";
        private const string TripsPath = "trips.txt";
        private const string StopTimesPath = "stop_times.txt";
        private const string StopTimesRegexPattern = """^\d_(\d+),(\d+):(\d+):(\d+),[\d:]+,(\d+),(\d+),.+"""; //trip_id, arrival_time, stop_id
        private const string CalendarPath = "calendar.txt";
        private const string CalendarRegexPattern = """^(\d),(\d),(\d),(\d),(\d),(\d),(\d),(\d),.+""";
        private Timer? _timer;


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var now = DateTime.Now;
            var midnight = now.Subtract(now.TimeOfDay).AddDays(1);
            var delay = midnight - now;
            _timer = new Timer(UpdateData, null, delay, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private async void UpdateData(object? state)
        {
            await LoadDataToDisk();
            try
            {
                await UpdateBusStops();
                await UpdateCalendar();
                await UpdateBuses();
                
            }
            catch (Exception ex)
            {

            }

        }

        private async Task UpdateCalendar()
        {
            var regex = new Regex(CalendarRegexPattern);
            var lines = await File.ReadAllLinesAsync(ExtractionPath + CalendarPath);
            var dataFromFile = lines
                .Select(l => regex.Match(l))
                .Where(m => m.Success)
                .Select(m => new Calendar
                {
                    Id = long.Parse(m.Groups[1].Value),
                    Monday = m.Groups[2].Value == "1",
                    Tuesday = m.Groups[3].Value == "1",
                    Wednesday = m.Groups[4].Value == "1",
                    Thursday = m.Groups[5].Value == "1",
                    Friday = m.Groups[6].Value == "1",
                    Saturday = m.Groups[7].Value == "1",
                    Sunday = m.Groups[8].Value == "1",
                    LastModified = DateTime.UtcNow
                })
                .ToDictionary(c => c.Id, c => c);
            var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<PublicTransportNavigatorContext>();
            var dataFromDatabase = context.Calendar;
            await dataFromDatabase.ForEachAsync(c =>
            {
                dataFromFile.TryGetValue(c.Id, out var source);
                if (source == null)
                {
                    context.Remove(c.Id);
                    return;
                }
                CompareAndUpdateCalendar(c, source);
                dataFromFile.Remove(c.Id);
            });
            await context.AddRangeAsync(dataFromFile.Values);
            await context.SaveChangesAsync();
        }

        private static void CompareAndUpdateCalendar(Calendar dest, Calendar source)
        {
            if (dest.Id != source.Id) return;
            if (dest.Monday != source.Monday)
            {
                dest.Monday = source.Monday;
                dest.LastModified = source.LastModified;
            }

            if (dest.Tuesday != source.Tuesday)
            {
                dest.Tuesday = source.Tuesday;
                dest.LastModified = source.LastModified;
            }

            if (dest.Wednesday != source.Wednesday)
            {
                dest.Wednesday = source.Wednesday;
                dest.LastModified = source.LastModified;
            }

            if (dest.Thursday != source.Thursday)
            {
                dest.Thursday = source.Thursday;
                dest.LastModified = source.LastModified;
            }

            if (dest.Friday != source.Friday)
            {
                dest.Friday = source.Friday;
                dest.LastModified = source.LastModified;
            }

            if (dest.Saturday != source.Saturday)
            {
                dest.Saturday = source.Saturday;
                dest.LastModified = source.LastModified;
            }

            if (dest.Sunday == source.Sunday) return;
            dest.Sunday = source.Sunday;
            dest.LastModified = source.LastModified;
        }

        private async Task UpdateBuses()
        {
            var regex = new Regex(TripRegexPattern);
            var lines = await File.ReadAllLinesAsync(ExtractionPath + TripsPath);
            var dataFromFile = lines
                .Select(l => regex.Match(l))
                .Where(m => m.Success)
                .Select(m => new BusSyncDTO
                {
                    RouteId = long.Parse(m.Groups[1].Value),
                    CalendarId = long.Parse(m.Groups[2].Value),
                    Id = long.Parse(m.Groups[3].Value),
                    WheelchairAccessible = m.Groups[4].Value == "1",
                    LastModified = DateTime.UtcNow
                })
                .ToDictionary(b => b.Id, b => b);
            await UpdateBusesFromRoutes(dataFromFile);

        }

        private async Task UpdateBusesFromRoutes(Dictionary<long, BusSyncDTO> dataFromFile)
        {
            var regex = new Regex(RouteRegexPattern);
            var lines = await File.ReadAllLinesAsync(ExtractionPath + RoutesPath);
            List<BusType> types;

            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<PublicTransportNavigatorContext>();
                types = await context.BusType.ToListAsync();
            }

            var matches = lines
                .Select(l => regex.Match(l))
                .Where(m => m.Success);
      
            foreach (var match in matches)
            {
                var routeId = long.Parse(match.Groups[1].Value);

                try
                {
                    var matchingEntries = dataFromFile
                        .Where(b => b.Value.RouteId == routeId)
                        .Select(b => b.Value)
                        .ToList();


                    foreach (var entry in matchingEntries)
                    {
                        entry.Number = match.Groups[2].Value;
                        entry.TypeId = types.First(t => (int)t.Type == int.Parse(match.Groups[3].Value)).Id;
                    }
                }
                catch { }
            }

            await UpdateTimetables(dataFromFile);
        }

        private static void CompareAndUpdateBuses(Bus dest, BusSyncDTO source)
        {
            if (source.Id != dest.Id) return;
            if (source.Number != null && source.Number != dest.Number)
            {
                dest.Number = source.Number;
                dest.LastModified = source.LastModified;
            }

            if (source.FirstBusStopId != null && source.FirstBusStopId != dest.FirstBusStopId)
            {
                dest.FirstBusStopId = source.FirstBusStopId.Value;
                dest.LastModified = source.LastModified;
            }

            if (source.LastBusStopId != null && source.LastBusStopId != dest.LastBusStopId)
            {
                dest.LastBusStopId = source.LastBusStopId.Value;
                dest.LastModified = source.LastModified;
            }

            if (source.WheelchairAccessible != null && source.WheelchairAccessible != dest.WheelchairAccessible)
            {
                dest.WheelchairAccessible = source.WheelchairAccessible.Value;
                dest.LastModified = source.LastModified;
            }
            if ( source.TypeId != null && source.TypeId == dest.TypeId) return;
            
            dest.TypeId = source.TypeId.Value;
            dest.LastModified = source.LastModified;
        }

        private async Task UpdateTimetables(Dictionary<long, BusSyncDTO> busesFromFile)
        {
            var regex = new Regex(StopTimesRegexPattern);
            var lines = await File.ReadAllLinesAsync(ExtractionPath + StopTimesPath);
            try
            {
                var dataFromFile = lines
                    .Select(l => regex.Match(l))
                    .Where(m => m.Success)
                    .Select(m => new TimetableSyncDTO
                    {
                        Id = long.Parse(m.Groups[1].Value + m.Groups[5].Value + m.Groups[6].Value),
                        BusId = long.Parse(m.Groups[1].Value),
                        Time = TimeSpan.Parse((int.Parse(m.Groups[2].Value)%24).ToString() + ":" + m.Groups[3].Value + ":" + m.Groups[4].Value),
                        BusStopId = long.Parse(m.Groups[5].Value),
                        CalendarId = busesFromFile
                            .Where(b => b.Key == long.Parse(m.Groups[1].Value))
                            .Select(b => b.Value.CalendarId).First(),
                        LastModified = DateTime.UtcNow,
                        Sequence = int.Parse(m.Groups[6].Value)
                    })
                    .ToDictionary(b => b.Id, b => b);

                foreach (var bus in busesFromFile.Values)
                {
                    var stops = dataFromFile.Where(t => t.Value.BusId == bus.Id).OrderBy(t => t.Value.Sequence).ToList();
                    if (!stops.Any())
                    {
                        busesFromFile.Remove(bus.Id);
                        continue;
                    }
                    bus.FirstBusStopId = stops.First().Value.BusStopId;
                    bus.LastBusStopId = stops.Last().Value.BusStopId;
                }
                var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<PublicTransportNavigatorContext>();

                var dataFromDatabase = context.Buses;
                await dataFromDatabase.ForEachAsync(b =>
                {
                    busesFromFile.TryGetValue(b.Id, out var source);
                    if (source == null)
                    {
                        context.Buses.Remove(b);
                        return;
                    }
                    CompareAndUpdateBuses(b, source!);
                    busesFromFile.Remove(source!.Id);
                });

                await context.Buses.AddRangeAsync(mapper.Map<IEnumerable<Bus>>(busesFromFile.Values));
                await context.SaveChangesAsync();

                var timetablesFromDatabase = context.Timetables;
                await timetablesFromDatabase.ForEachAsync(t =>
                {
                    dataFromFile.TryGetValue(t.Id, out var source);
                    if (source == null)
                    {
                        context.Timetables.Remove(t);
                        return;
                    }
                    CompareAndUpdateTimetables(t, source!);
                    dataFromFile.Remove(source!.Id);
                });
                await context.Timetables.AddRangeAsync(_mapper.Map<IEnumerable<Timetable>>(dataFromFile.Values));

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }

           

           
        }

        private static void CompareAndUpdateTimetables(Timetable dest, TimetableSyncDTO source)
        {
            if (source.Id != dest.Id) return;
            if (source.BusId != null && source.BusId != dest.BusId)
            {
                dest.BusId = source.BusId;
                dest.LastModified = source.LastModified;
            }
            if (source.BusStopId != null && source.BusStopId != dest.BusStopId)
            {
                dest.BusStopId = source.BusStopId;
                dest.LastModified = source.LastModified;
            }
            if (source.Time != null && source.Time != dest.Time)
            {
                dest.Time = source.Time;
                dest.LastModified = source.LastModified;
            }

            if (source.CalendarId == null || source.CalendarId == dest.CalendarId) return;
            dest.CalendarId = source.CalendarId;
            dest.LastModified = source.LastModified;
        }

        private async Task UpdateBusStops()
        {
            var regex = new Regex(StopRegexPattern);
 
            var lines = await File.ReadAllLinesAsync(ExtractionPath + StopsPath);

            var dataFromFile = lines
                .Select(line => regex.Match(line))
                .Where(match => match.Success)
                .Select(match => new BusStop
                 {
                     Id = long.Parse(match.Groups[1].Value),
                    Name = match.Groups[2].Value.Length > 0
                        ? match.Groups[2].Value[..^1]
                        : match.Groups[2].Value,
                    CoordX = float.Parse(match.Groups[3].Value),
                     CoordY = float.Parse(match.Groups[4].Value),
                     LastModified = DateTime.UtcNow
                 })
                .ToDictionary(t => t.Id, t => t);

            using var scope = _scopeFactory.CreateScope();
            var context =
                scope.ServiceProvider
                    .GetRequiredService<PublicTransportNavigatorContext>();
            var dataFromDatabase = context.BusStops;
            foreach (var busStop in dataFromDatabase)
            {
                dataFromFile.TryGetValue(busStop.Id, out var source);
                if (source == null)
                {
                    context.BusStops.Remove(busStop);
                    continue;
                }

                CompareAndUpdateBusStops(busStop, source!);
                dataFromFile.Remove(source!.Id);
            }

            await context.BusStops.AddRangeAsync(dataFromFile.Values);
            await context.SaveChangesAsync();

        }

        private static void CompareAndUpdateBusStops(BusStop dest, BusStop source)
        {
            if (!source.CoordX.Equals(dest.CoordX))
            {
                dest.CoordX = source.CoordX;
                dest.LastModified = source.LastModified;
            }

            if (!source.CoordY.Equals(dest.CoordY))
            {
                dest.CoordY = source.CoordY;
                dest.LastModified = source.LastModified;
            }

            if (source.Name == dest.Name) return;
          
            source.Name = dest.Name;
            dest.LastModified = source.LastModified;
        }

        private async Task LoadDataToDisk()
        {
            var fileUrl = await GetFilePath();
            using var client = new HttpClient();
            try
            {
                var fileBytes = await GetFileFromUrlAsync(fileUrl);
                await File.WriteAllBytesAsync(DestinationPath, fileBytes);
                if (File.Exists(DestinationPath))
                {
                    Directory.CreateDirectory(ExtractionPath);
                    ZipFile.ExtractToDirectory(DestinationPath, ExtractionPath, overwriteFiles: true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task<byte[]?> GetFileFromUrlAsync(string url)
        {
            using var client = new HttpClient();
            var cts = new CancellationTokenSource(_timeout);
            while (!cts.Token.IsCancellationRequested)
            {
                try
                {
                    var fileBytes = await client.GetByteArrayAsync(url, cts.Token);
                    if (fileBytes.Length > 0)
                    {
                        return fileBytes;
                    }
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                }
                await Task.Delay(_delayBetweenRetries, cts.Token);
            }

            return null;
        }

        private async Task<HttpResponseMessage?> GetResponseWithRetriesAsync(string url)
        {
            using var client = new HttpClient();
            var cts = new CancellationTokenSource(_timeout);

            try
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        var response = await client.GetAsync(url, cts.Token);

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            return response;
                        }
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                    }

                    await Task.Delay(_delayBetweenRetries, cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                
            }
            return null;
        }
        private async Task<string> GetFilePath()
        {
            var regex = new Regex(Pattern);

            var response = await GetResponseWithRetriesAsync(Url);
            if (response == null) throw new TimeoutException("ZTM server didn't respond");

            var htmlContent = await response.Content.ReadAsStringAsync();

            var matches = regex.Matches(htmlContent);
            return matches.Last().Value;
        }
    }
}
