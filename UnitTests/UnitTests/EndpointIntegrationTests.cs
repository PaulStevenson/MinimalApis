using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace UnitTests
{
    public class EndpointIntegrationTests
    {
        [Fact]
        public async Task GetById_StatusShouldBe401_WhenNoAuth()
        {
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            var response = await client.GetAsync("/articles/1");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Get_StatusShouldBe401_WhenNoAuth()
        {
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            var response = await client.GetAsync("/articles");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Post_StatusShouldBe200_WhenValidPayload()
        {
            var datetimeNow = DateTime.Now;

            var payload = new ArticleRequest("title", "Content", datetimeNow, 4);

            await using var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => 
                    builder.ConfigureServices(s => 
                        s.AddScoped<IArticleService, TestArticleService>()));

            using var client = application.CreateClient();

            var response = await client.PostAsJsonAsync("/articles", payload);
            var content = await response.Content.ReadFromJsonAsync<Article>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().BeOfType<Article>();
            content.Id.Should().Be(1);
        }

        [Fact]
        public async Task Post_StatusShouldBe_WhenInvalidPayload_MyNumber()
        {
            var datetimeNow = DateTime.Now;

            var payload = new ArticleRequest("title", "Content", datetimeNow, 0);

            await using var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                    builder.ConfigureServices(s =>
                        s.AddScoped<IArticleService, TestArticleService>()));

            using var client = application.CreateClient();

            var response = await client.PostAsJsonAsync("/articles", payload);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_StatusShouldBe_WhenInvalidPayload_Content()
        {
            var datetimeNow = DateTime.Now;

            var payload = new ArticleRequest("title", "Content", datetimeNow, 0);

            await using var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                    builder.ConfigureServices(s =>
                        s.AddScoped<IArticleService, TestArticleService>()));

            using var client = application.CreateClient();

            var response = await client.PostAsJsonAsync("/articles", payload);
            //var content = await response.Content.ReadFromJsonAsync<Article>();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        internal class TestArticleService : IArticleService, IDisposable
        {
            protected readonly ApiContext _context;
            private readonly IMapper _mapper;

            public TestArticleService(IMapper mapper)
            {
                _mapper = mapper;

                var options = new DbContextOptionsBuilder<ApiContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;

                _context = new ApiContext(options);

                _context.Database.EnsureCreated();

                var entList = new List<ArticleEntity>();

                for (int i = 1; i < 6; i++)
                {
                    var entity = new ArticleEntity
                    {
                        Id = i,
                        Title = $"Test{i}",
                        Content = $"This is test entity {i}",
                        PublishedAt = DateTime.Today.AddDays(-i),
                        MyNumber = 0 + i
                    };

                    entList.Add(entity);
                }

                _context.Articles.AddRange(entList);

                _context.SaveChanges();
            }

            public Task<IList<Article>> GetAll()
            {
                throw new NotImplementedException();
            }

            public Task<Article> GetById(int id)
            {
                throw new NotImplementedException();
            }

            public async Task<Article> Post(ArticleRequest article)
            {
                //var response = new Article
                //{
                //    Id = 1,
                //    Title = article.Title,
                //    Content = article.Content,
                //    PublishedAt = article.PublishedAt,
                //    MyNumber = article.MyNumber
                //};

                var ent = new ArticleEntity
                {
                    Title = article.Title,
                    Content = article.Content,
                    PublishedAt = article.PublishedAt,
                    MyNumber = article.MyNumber
                };

                _context.Articles.Add(ent);
                var createdArticle = await _context.SaveChangesAsync();

                var response = _mapper.Map<Article>(createdArticle);

                return response;
            }

            public Task<IResult> PostWithValidation(Article newArticle)
            {
                throw new NotImplementedException();
            }

            public Task<IResult> Delete(int id)
            {
                throw new NotImplementedException();
            }

            public Task<IResult> Put(int id, ArticleRequest article)
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                _context.Database.EnsureDeleted();
                _context.Dispose();
            }
        }
    }
}
