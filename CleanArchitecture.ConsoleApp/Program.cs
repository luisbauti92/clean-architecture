using CleanArchitecture.Data;
using CleanArchitecture.Domain;
using Microsoft.EntityFrameworkCore;

StreamerDbContext dbContext = new();
await TrackingAndNotTracking();
//await QueryLinq();
//await QueryMethods();
//await QueryFilter();
Console.WriteLine("Presione cualquier tecla para terminar el programa");
Console.ReadKey();

async Task TrackingAndNotTracking()
{
    var streamerWithTracking = await dbContext!.Streamers!.FirstOrDefaultAsync(x => x.Id == 1);
    var streamerWithNotTracking = await dbContext!.Streamers!.AsNoTracking().FirstOrDefaultAsync(x => x.Id == 2);

    streamerWithTracking.Nombre = "Netflix Super";
    streamerWithNotTracking.Nombre = "Amazon Plus";

    await dbContext.SaveChangesAsync();
}

async Task QueryLinq()
{
    Console.WriteLine($"Ingrese el servicio de streaming: ");
    var streamerNombre = Console.ReadLine();
    var streamers = await (from i in dbContext.Streamers
                           where EF.Functions.Like(i.Nombre, $"%{streamerNombre}%")
                           select i).ToListAsync();
    foreach (var streamer in streamers)
    {
        Console.WriteLine($"{streamer.Id} - {streamer.Nombre}");
    }
}

async Task QueryMethods()
{
    var streamer = dbContext!.Streamers!;
    var firstAsync = await streamer.Where(s => s.Nombre.Contains("a")).FirstAsync();
    var firstOrDefaultAsync = await streamer!.Where(s => s.Nombre.Contains("a")).FirstOrDefaultAsync();
    var firstOrDefault_v2 = await streamer!.FirstOrDefaultAsync(s => s.Nombre.Contains("a"));

    var singleAsync = await streamer.SingleAsync();
    var singleOrDefaultAsync = await streamer.Where(y => y.Id == 1).SingleOrDefaultAsync();

    var resultado = await streamer.FindAsync(1);
}

async Task QueryFilter()
{
    Console.WriteLine($"Ingrese una Compania de streaming: ");
    var streamingName = Console.ReadLine();
    var streamers = await dbContext!.Streamers!.Where(x => x.Nombre!.Equals(streamingName)).ToListAsync();
    foreach(var streamer in streamers)
    {
        Console.WriteLine($"{streamer.Id} -  {streamer.Nombre}");
    }

    //var streamerPartialResults = await dbContext!.Streamers!.Where(x => x.Nombre!.Contains(streamingName)).ToListAsync();
    var streamerPartialResults = await dbContext!.Streamers!.Where(x => EF.Functions.Like(x.Nombre, $"%{streamingName}%")).ToListAsync();

    foreach (var streamer in streamerPartialResults)
    {
        Console.WriteLine($"{streamer.Id} -  {streamer.Nombre}");
    }
}

void QueryStreaming()
{
    var streamers = dbContext!.Streamers!.ToList();
    foreach(var streamer in streamers)
    {
        Console.WriteLine($"{streamer.Id} - {streamer.Nombre}");
    }
}

async Task AddNewRecords()
{
    Streamer streamer = new()
    {
        Nombre = "Disneyplus",
        Url = "http://disneyplus.com"
    };

    dbContext!.Streamers!.Add(streamer);

    await dbContext.SaveChangesAsync();

    var movies = new List<Video>
{
    new Video
    {
        Nombre = "Sirenita",
        StreamerId = streamer.Id,

    },
    new Video
    {
        Nombre = "El Rey Leon",
        StreamerId = streamer.Id,

    },
    new Video
    {
        Nombre = "Star Wars",
        StreamerId = streamer.Id,

    },
    new Video
    {
        Nombre = "Red",
        StreamerId = streamer.Id,

    },
};

    await dbContext.AddRangeAsync(movies);

    await dbContext.SaveChangesAsync();
}