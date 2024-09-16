
using Chatti.Api.Extensions;
using Chatti.Core.Settings;
using Chatti.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//configure swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSwagger();


//configure db context
var mongoDBSettings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDbSettings>();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseMongoDB(mongoDBSettings!.AtlasURI, mongoDBSettings.DatabaseName);
});
var app = builder.Build();
// configure automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);


var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
// configure jwt
builder.Services.AddJwtAuthorization(jwtSettings!);
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
