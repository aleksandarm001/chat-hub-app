using backend.Extensions;
using backend.Hubs;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// configuring Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("Redis");
    if (string.IsNullOrEmpty(configuration))
    {
        throw new InvalidOperationException("Redis connection string is not configured.");
    }
    return ConnectionMultiplexer.Connect(configuration);
});

// configuring PostgreSql
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"))
);

// Adding services
builder.Services.AddServices();
builder.Services.AddRepositories();

// adding signalR
builder.Services.AddSignalR();

// adding authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// cors

var app = builder.Build();

app.UseCors(builder =>
    builder.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();


app.MapHub<ChatHub>("/chathub");

app.UseAuthentication();  
app.UseAuthorization();   


app.Run();