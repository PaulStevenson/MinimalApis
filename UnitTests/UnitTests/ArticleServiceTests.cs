using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinimalApiDemo.Entities;
using MinimalApiDemo.Infastructure;
using MinimalApiDemo.Models;
using MinimalApiDemo.Services;
using Moq;
using MockQueryable;
using MockQueryable.Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace UnitTests;

public class ArticleServiceTests
{
    private readonly Mock<ApiContext> _mockApiContext = new Mock<ApiContext>();
    private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();

    [Fact]
    public async Task GetById_Exists_ReturnsDto()
    {
        // assert
        var dtos = GetTestDtos(1, 6);
        var expected = dtos.First();

        var entList = GetTestEntities(1, 6);
        var firstEntity = entList.First();

        var mockEnt = entList.BuildMock().BuildMockDbSet();

        mockEnt
            .Setup(x => x.FindAsync(1))
            .ReturnsAsync(entList
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
        // assert
        var entList = GetTestEntities(1, 6);
        var firstEntity = entList.First();

        var mockEnt = entList.BuildMock().BuildMockDbSet();

        mockEnt
            .Setup(x => x.FindAsync(1))
            .ReturnsAsync(entList
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

    private List<ArticleEntity> GetTestEntities(int index, int count)
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

    private List<Article> GetTestDtos(int index, int count)
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
}
