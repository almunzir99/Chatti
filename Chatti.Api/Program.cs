
using Chatti.Core.Settings;
using Chatti.Persistence.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
var mongoDBSettings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDbSettings>();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseMongoDB(mongoDBSettings!.AtlasURI, mongoDBSettings.DatabaseName);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
