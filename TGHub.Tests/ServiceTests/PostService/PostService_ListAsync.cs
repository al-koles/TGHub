using Microsoft.EntityFrameworkCore;
using TGHub.Application.Interfaces;
using TGHub.Application.Services.Posts.Data;
using TGHub.Domain.Entities;
using TGHub.SqlDb;

namespace TGHub.Tests.ServiceTests.PostService;

public class PostService_ListAsync
{
    [Fact]
    public async Task ListAsync_NoFilter_ReturnsAllPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            new() { Id = 1, Title = "Post 1", Content = "Post 1" },
            new() { Id = 2, Title = "Post 2", Content = "Post 2" },
            new() { Id = 3, Title = "Post 3", Content = "Post 3" }
        };

        await using var dbContext = CreateDbContext();
        dbContext.Posts.AddRange(posts);
        await dbContext.SaveChangesAsync();

        var postService = new Application.Services.Posts.PostService(dbContext);

        // Act
        var result = await postService.ListAsync();

        // Assert
        Assert.Equal(posts.Count, result.Count);
        Assert.Equal(posts.Select(p => p.Id), result.Select(p => p.Id));
    }

    [Fact]
    public async Task ListAsync_WithFilter_ReturnsFilteredPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            new() { Id = 1, Title = "Post 1", Content = "Post 1", ReleaseDateTime = new DateTime(2023, 1, 1) },
            new() { Id = 2, Title = "Post 2", Content = "Post 2", ReleaseDateTime = new DateTime(2023, 2, 1) },
            new() { Id = 3, Title = "Post 3", Content = "Post 3", ReleaseDateTime = new DateTime(2023, 3, 1) }
        };

        await using var dbContext = CreateDbContext();
        dbContext.Posts.AddRange(posts);
        await dbContext.SaveChangesAsync();

        var postFilter = new PostFilter
        {
            From = new DateTime(2023, 2, 1),
            To = new DateTime(2023, 3, 1)
        };

        var postService = new Application.Services.Posts.PostService(dbContext);

        // Act
        var result = await postService.ListAsync(postFilter);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(new[] { 2, 3 }, result.Select(p => p.Id));
    }

    // Helper method to create an in-memory DbContext for testing
    private ITgHubDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<TgHubDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        return new TgHubDbContext(options);
    }
}