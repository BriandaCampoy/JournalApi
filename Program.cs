using Microsoft.OpenApi.Models;
using journalapi;
using journalapi.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Version = "v1",
            Title = "Journal API",
            Description =
                "An ASP.NET Core Web API for managing Journals, researchers and subscriptions",
        }
    );
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Configure the database connection using the "cnJournal" connection string from the appsettings.json.
builder.Services.AddSqlServer<JournalContext>(
    builder.Configuration.GetConnectionString("cnJournal")
);

// Add the services to the DI container.
builder.Services.AddScoped<IJournalService, JournalService>();
builder.Services.AddScoped<IReasearcherService, ReasearcherService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

// Configure Cross-Origin Resource Sharing (CORS) to allow requests from any origin, method, and header.
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowCorsPolicy",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    );
});

var app = builder.Build();

// Enable CORS with the previously configured policy.
app.UseCors("AllowCorsPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger middleware in development environment.
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Map controllers to the endpoints.
app.MapControllers();

// Run the application.
app.Run();
