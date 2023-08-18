using Microsoft.OpenApi.Models;
using journalapi;
using journalapi.Services;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using journalApi.Models;
using journalApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddJsonFile("appsettings.json");
var secretkey = builder.Configuration.GetSection("settings").GetSection("secretkey").ToString();
var keyBytes = Encoding.UTF8.GetBytes(secretkey);

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.AddControllers().AddOData((options) => 
    options.Select().Filter().Count().OrderBy().Expand()
    .AddRouteComponents("odata", GetEdmModel()));

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

// Add the services to the DI container
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

app.UseAuthentication();

app.UseAuthorization();

// Map controllers to the endpoints.
app.MapControllers();

// Run the application.
app.Run();

static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
    modelBuilder.EntitySet<University>("UniversityOData");
    return modelBuilder.GetEdmModel();
}