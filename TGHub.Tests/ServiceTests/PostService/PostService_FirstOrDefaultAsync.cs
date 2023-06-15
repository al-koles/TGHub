using Microsoft.EntityFrameworkCore;
using TGHub.Application.Interfaces;
using TGHub.Domain.Entities;
using TGHub.SqlDb;

namespace TGHub.Tests.ServiceTests.PostService;

public class PostService_FirstOrDefaultAsync
{
    [Fact]
    public async Task FirstOrDefaultAsync_WithPredicate_ReturnsMatchingPost()
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
        var result = await postService.FirstOrDefaultAsync(p => p.Id == 2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Id);
        Assert.Equal("Post 2", result.Title);
    }

    [Fact]
    public async Task FirstOrDefaultAsync_WithoutPredicate_ReturnsFirstPost()
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
        var result = await postService.FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Post 1", result.Title);
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