using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalApiDemo.Infastructure;
using MinimalApiDemo.Models;
using MinimalApiDemo.Services;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Register Services
// Register services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("api"));
builder.Services.AddAutoMapper(typeof(Program));

// JWT Set Up
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "https://localhost:7163",
        ValidAudience = "https://localhost:7163",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Articles",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
#endregion

#region IOC
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<ILoginInService, LoginService>();
#endregion

#region App Builder
var app = builder.Build();

{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
#endregion

#region Article Endpoints
app.MapGet("/articles", async (
IArticleService articleService) =>
await articleService.GetAll())
.RequireAuthorization();

app.MapGet("/articles/{id}", async (
    int id,
    IArticleService articleService) =>
await articleService.GetById(id) is Article article
? Results.Ok(article)
: Results.NotFound())
.RequireAuthorization();

app.MapPost("/articles", async (
    [FromBody] ArticleRequest article,
    IArticleService articleService) =>
await articleService.Post(article))
.RequireAuthorization();

app.MapDelete("articles/{id}", async (
    int id,
    IArticleService articleService) =>
await articleService.Delete(id))
.RequireAuthorization();

app.MapPut("/articles/{id}", async (
    int id,
    ArticleRequest article,
    IArticleService articleService) =>
await articleService.Put(id, article))
.RequireAuthorization();

#endregion

#region Login Endpoints
app.MapPost("/login", (
Login user,
ILoginInService loginInService) =>
loginInService.Login(user));
#endregion

// Starting point
app.Run();

