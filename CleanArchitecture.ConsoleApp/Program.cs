using CleanArchitecture.Data;
using CleanArchitecture.Domain;
using Microsoft.EntityFrameworkCore;

StreamerDbContext dbContext = new();

await QueryFilter();

async Task QueryFilter()
{
    var streamers = await dbContext!.Streamers!.Where(x => x.Nombre.Equals("Netflix")).ToListAsync();
    foreach(var streamer in streamers)
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