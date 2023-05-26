using Microsoft.AspNetCore.Http.HttpResults;

namespace MinimalApiDemo.Endpoints
{
    public static class ArticleEndpoints
    {
        public static void MapArticleEndpoints(WebApplication app)
        {
            app.MapGet("/articles", GetAllArticles)
                .RequireAuthorization();

            app.MapGet("/articles/{id}", GetByIdArticles)
                .RequireAuthorization();

            app.MapPost("/articles", CreateArticle)
                .AddEndpointFilter<ValidationFilter<ArticleRequest>>()
                .RequireAuthorization();

            app.MapPost("/articlesWithValidation", CreateArticleWithValidation)
                .AddEndpointFilter<ValidationFilter<ArticleRequest>>()
                .RequireAuthorization();

            app.MapPut("/articles/{id}", UpdateArticle)
                .AddEndpointFilter<ValidationFilter<ArticleRequest>>()
                .RequireAuthorization();

            app.MapDelete("/articles/{id}", DeleteArticle)
                .RequireAuthorization();
        }

        private static async Task<IResult> GetAllArticles(IArticleService service)
        {
            var response = await service.GetAll();

            return Results.Ok(response);
        }

        private static async Task<IResult> GetByIdArticles(int id, IArticleService service)
        {
            var response = await service.GetById(id);
            if (response != null)
            {
               return  Results.Ok(response);
            }

            return Results.NotFound($"No article was found with ID {id}.");
        }

        private static async Task<IResult> CreateArticle(ArticleRequest request, IArticleService service)
        {
            var response = await service.Post(request);

            return Results.Created($"articles/{response.Id}", response);
        }

        private static async Task<IResult> CreateArticleWithValidation(Article request, IArticleService service)
        {
            var response = await service.PostWithValidation(request);

            return Results.Created($"articlesWithValidation/{response}", response);
        }

        private static async Task<IResult> UpdateArticle(int id, ArticleRequest request, IArticleService service)
        {
            var articleToUpdate = await service.GetById(id);

            if (articleToUpdate == null)
            {
                return Results.NotFound($"No article was found with ID {id}.");
            } 
            
            var response = await service.Put(id, request);
            return Results.Ok(response);
        }

        private static async Task<IResult> DeleteArticle(int id, IArticleService service)
        {
            var response = await service.Delete(id);

            return Results.NoContent();

        }
    }
}