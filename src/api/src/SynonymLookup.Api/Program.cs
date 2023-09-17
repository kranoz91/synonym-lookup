using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using SynonymLookup.Api.Extensions;
using SynonymLookup.Api.Features.SynonymReader;
using SynonymLookup.Api.Features.SynonymWriter;
using SynonymLookup.Database;

var builder = WebApplication.CreateBuilder(args);

var appConfigUri = builder.Configuration.GetValue<string>("AppConfigUri");

if (!string.IsNullOrEmpty(appConfigUri))
{
    builder.Configuration.AddAzureAppConfiguration(
        new Uri(appConfigUri),
        "SynonymLookup",
        builder.Environment.EnvironmentName);
}

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.RegisterDatabase();

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Synonym Lookup API", Version = "v1" });
    opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow()
            {
                AuthorizationUrl = new Uri($"https://login.microsoftonline.com/{builder.Configuration["AzureAd:TenantId"]}/oauth2/v2.0/authorize"),
                TokenUrl = new Uri($"https://login.microsoftonline.com/{builder.Configuration["AzureAd:TenantId"]}/oauth2/v2.0/token"),
                Scopes = new Dictionary<string, string> { { builder.Configuration["AzureAd:Scopes"]!, "Default access" } }
            }
        }
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                },
                Scheme = "oauth2",
                Name = "oauth2",
                In = ParameterLocation.Header
            },
            new List <string> ()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(opt =>
{
    opt.OAuthClientId(app.Configuration["AzureAd:SwaggerClientId"]);
    opt.OAuthUsePkce();
});

app.UseHttpsRedirection();

app.MapSynonymReader();
app.MapSynonymWriter();

app.Run();
