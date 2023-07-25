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
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Journal API",
        Description = "An ASP.NET Core Web API for managing Journals, researchers and subscriptions",
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddSqlServer<JournalContext>(builder.Configuration.GetConnectionString("cnJournal"));

builder.Services.AddScoped<IJournalService, JournalService>();
builder.Services.AddScoped<IReasearcherService, ReasearcherService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

builder.Services.AddCors(options =>{
    options.AddPolicy("AllowCorsPolicy",
        builder => {
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

var app = builder.Build();
app.UseCors("AllowCorsPolicy");

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
