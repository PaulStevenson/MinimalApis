var builder = WebApplication.CreateBuilder(args);

ServiceCollections.ConfigureServiceCollection(builder);

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}

AppMiddleWare.AddMiddleware(app);

ArticleEndpoints.MapArticleEndpoints(app);
LoginEndpoints.MapLoginEndpoints(app);

app.Run();

