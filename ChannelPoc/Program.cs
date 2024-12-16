using ChannelPoc;
using ChannelPoc.BackgroundServices;
using ChannelPoc.BackgroundServices.Job;
using ChannelPoc.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Threading.Channels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
builder.Services.AddScoped<ICLientService, ClientService>();

builder.Services.AddSingleton(_ =>
{
    var channel = Channel.CreateBounded<ImportClientsJob>(new BoundedChannelOptions(5)
    {
        FullMode = BoundedChannelFullMode.Wait
    });

    return channel;
});
builder.Services.AddSingleton<ConcurrentDictionary<Guid, ImportClientsStatus>>();
builder.Services.AddHostedService<ImportClientBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
