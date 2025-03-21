
using Chatti.Api.AutoMapper;
using Chatti.Api.Extensions;
using Chatti.Api.Middlewares;
using Chatti.Core.Settings;
using Chatti.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel(options =>
{
    options.ListenAnyIP(5098);
});
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//configure swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
//configure db context
var mongoDBSettings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDbSettings>();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseMongoDB(mongoDBSettings!.AtlasURI, mongoDBSettings.DatabaseName);
});
// configure automapper
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
// configure jwt
builder.Services.AddJwtAuthorization(jwtSettings!);

builder.Services.AddBusinessServices();
// Configure the HTTP request pipeline.

var app = builder.Build();
// add Error Handling Middleware
app.UseErrorHandlingMiddleware();
app.UseRouting();
// run database seed 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.UseStaticFiles();
app.DbContextSeedAsync().Wait();
app.UseHttpsRedirection();


app.MapControllers();

app.Run();
