namespace MinimalApiDemo.Config
{
    public static class ServiceCollections
    {
        public static void ConfigureServiceCollection(WebApplicationBuilder builder)
        {
            // Register services
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("api"));
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddValidatorsFromAssemblyContaining(typeof(MinimalApiDemo.Validation.Validators));
            RegisterJwtAuthentication(builder);
            builder.Services.AddAuthorization();
            RegisterSwagger(builder);

            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<ILoginInService, LoginService>();
        }

        private static void RegisterSwagger(WebApplicationBuilder builder)
        {
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
        }

        private static void RegisterJwtAuthentication(WebApplicationBuilder builder)
        {
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
        }
    }



}
