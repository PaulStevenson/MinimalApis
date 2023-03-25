using Microsoft.EntityFrameworkCore;
using MinimalApiDemo.Profiles;
using Moq;

namespace UnitTests;

public class ArticleServiceTests
{
    private readonly Mock<ApiContext> _mockApiContext = new Mock<ApiContext>();
    private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();

    #region GetByID Tests

    [Fact]
    public async Task GetById_Exists_ReturnsDto()
    {
        // arrange
        var dtos = GetTestDtos(1, 6);
        var expected = dtos.First();

        var entList = GetTestEntities(1, 6);
        var firstEntity = entList.First();

        var mockEnt = entList.BuildMock().BuildMockDbSet();

        mockEnt
            .Setup(x => x.FindAsync(1))
            .ReturnsAsync(entList.ToList()
            .Find(e => e.Id == 1));

        _mockApiContext.Setup(x => x.Articles).Returns(mockEnt.Object);

        _mockMapper.Setup(x => x.Map<Article>(firstEntity)).Returns(expected);

        var service = new ArticleService(_mockApiContext.Object, _mockMapper.Object);

        // act
        var result = await service.GetById(1);

        // assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Article>();

        _mockMapper.Verify(m => m.Map<Article>(firstEntity), Times.Once());
        _mockApiContext.Verify(c => c.Articles.FindAsync(1), Times.Once());
    }

    [Fact]
    public async Task GetById_DoesNotExists_ReturnsNull()
    {
        // arrange
        var entList = GetTestEntities(1, 6);
        var firstEntity = entList.First();

        var mockEnt = entList.BuildMock().BuildMockDbSet();

        mockEnt
            .Setup(x => x.FindAsync(1))
            .ReturnsAsync(entList.ToList()
            .Find(e => e.Id == 10));

        _mockApiContext.Setup(x => x.Articles).Returns(mockEnt.Object);

        _mockMapper.Setup(x => x.Map<Article>(firstEntity)).Returns(() => null);

        var service = new ArticleService(_mockApiContext.Object, _mockMapper.Object);

        // act
        var result = await service.GetById(1);

        // assert
        result.Should().BeNull();

        _mockMapper.Verify(m => m.Map<Article>(firstEntity), Times.Never());
        _mockApiContext.Verify(c => c.Articles.FindAsync(1), Times.Once());
    }
    #endregion

    #region GetAll Test

    [Fact]
    public async Task GetAll_Exists_ReturnsListOfDtos()
    {
        // arrange
        var dtos = GetTestDtos(1, 6);

        var entList = GetTestEntities(1, 6);

        var mockEnt = entList.AsQueryable().BuildMockDbSet();

        _mockApiContext.Setup(x => x.Articles).Returns(mockEnt.Object);

        _mockMapper.Setup(x => x.Map<IList<Article>>(entList)).Returns(dtos);

        var service = new ArticleService(_mockApiContext.Object, _mockMapper.Object);

        // act
        var result = await service.GetAll();

        // assert
        result.Should().NotBeNull();
        result.Count().Should().Be(5);
        result.Should().BeOfType<List<Article>>();

        result.Should().BeSameAs(dtos);

        _mockMapper.Verify(m => m.Map<IList<Article>>(entList), Times.Once());
    }


    [Fact]
    public async Task GetAll_NoneExist_ReturnsEmptyList()
    {
        // arrange
        var entList = new List<ArticleEntity>();

        var mockEnt = entList.AsQueryable().BuildMockDbSet();

        _mockApiContext.Setup(x => x.Articles).Returns(mockEnt.Object);

        _mockMapper.Setup(x => x.Map<IList<Article>>(It.IsAny<ArticleEntity>)).Returns(() => null);

        var service = new ArticleService(_mockApiContext.Object, _mockMapper.Object);

        // act
        var result = await service.GetAll();

        // assert
        result.Should().BeNull();

        _mockMapper.Verify(m => m.Map<IList<Article>>(It.IsAny<ArticleEntity>), Times.Never());
    }
    #endregion

    [Fact]
    public async Task Post_ReturnsNewArticle()
    {
        var dto = GetTestDtos(1, 2).First();

        var articleRequest = new ArticleRequest(

            dto.Title,
            dto.Content,
            dto.PublishedAt,
            dto.MyNumber
        );

        var mapper = MappingProfile();

        var dbContextOptions = SetUpDbContext();

        using (var dbContext = new ApiContext(dbContextOptions))
        {
            var articleController = new ArticleService(dbContext, mapper);

            // Act
            var result = await articleController.Post(articleRequest);

            // Assert
            result.Should().BeOfType<Article>();
            result.Should().BeEquivalentTo(dto);
            result.Id.Should().Be(1);
            result.Title.Should().Be(dto.Title);
            result.Content.Should().Be(dto.Content);
            result.PublishedAt.Should().Be(dto.PublishedAt);
            result.MyNumber.Should().Be(dto.MyNumber);
        }
    }

    #region Private Methods

    private IList<ArticleEntity> GetTestEntities(int index, int count)
    {
        var entList = new List<ArticleEntity>();

        for (int i = index; i < count; i++)
        {
            var entity = new ArticleEntity
            {
                Id = i,
                Title = $"Test{i}",
                Content = $"This is test entity {i}",
                PublishedAt = DateTime.Today.AddDays(-i),
                MyNumber = 5 + i
            };

            entList.Add(entity);
        }

        return entList;
    }

    private IList<Article> GetTestDtos(int index, int count)
    {
        var entList = new List<Article>();

        for (int i = index; i < count; i++)
        {
            var entity = new Article
            {
                Id = i,
                Title = $"Test{i}",
                Content = $"This is test entity {i}",
                PublishedAt = DateTime.Today.AddDays(-i),
                MyNumber = 5 + i
            };

            entList.Add(entity);
        }

        return entList;
    }

    private static IMapper MappingProfile()
    {
        var mc = new MapperConfiguration(c =>
        c.AddProfile(new ArticleProfile()));

        var m = mc.CreateMapper();
        return m;
    }

    private static DbContextOptions<ApiContext> SetUpDbContext()
    {
        return new DbContextOptionsBuilder<ApiContext>()
            .UseInMemoryDatabase(databaseName: "MyDatabase")
        .Options;
    }
    #endregion
}
