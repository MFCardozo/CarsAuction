using BiddingService.Consumers;
using BiddingService.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>(); 

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("bids", false));

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host => 
        {
            host.Username(builder.Configuration.GetValue("RabbitMq:Username","guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password","guest"));
        });
        
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer( options =>
    {
        options.Authority = builder.Configuration["IdentityServiceUrl"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.NameClaimType = "username";
    });
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHostedService<CheckAuctionFinished>();
builder.Services.AddScoped<GrpcAuctionClient>();

var app = builder.Build();


app.UseAuthorization();

app.MapControllers();

/*await DB.InitAsync("BidDb", 
MongoClientSettings.FromConnectionString(builder.Configuration.GetConnectionString("MongoDbConnection")));*/
var connectionString = "";
try
{
 connectionString = builder.Configuration.GetConnectionString("BidDbConnection");
var settings = MongoClientSettings.FromConnectionString(connectionString);
await DB.InitAsync("BidDb", settings);
}
catch(Exception ex)
{
    Console.WriteLine($"MongoDB init error: {ex.Message}");
    throw; // re-throw if needed
}
var client = new MongoClient(connectionString); // or use your connection string
var db = client.GetDatabase("BidDb");
var collections = await db.ListCollectionNames().ToListAsync();
Console.WriteLine("Connected. Collections:");
collections.ForEach(Console.WriteLine);
app.Run();
