using Microsoft.EntityFrameworkCore;
using HumanFrogApi.Data;
using HumanFrogApi.Services.EventService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HumanFrogDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEventService, EventService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
