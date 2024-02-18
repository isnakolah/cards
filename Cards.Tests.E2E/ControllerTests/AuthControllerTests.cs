using System.Net;
using Cards.Application.Common.Models;
using Cards.Application.Users.ViewModels;
using FluentAssertions;

namespace Cards.Tests.E2E.Controllers;

public class AuthControllerTests : TestBase
{
    [Fact]
    public async Task Login_ShouldFail_WhenCredentialsAreInvalid()
    {
        // Arrange
        var body = new
        {
            email = "user@example.com",
            password = "Password123!"
        };

        // Act
        var response = await _client.PostAsync("/api/Auth/login", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseContent = await response.Content.DeserializeAsync<ApiResponse<AccessTokenVm>>();
        
        responseContent.Should().NotBeNull();
        responseContent?.IsSuccess.Should().BeFalse();
        responseContent?.Data.Should().BeNull();
        responseContent?.Message.Should().Be("Invalid email or password");
    }

    [Fact]
    public async Task Login_ShouldReturnAccessToken_WhenCredentialsAreValid()
    {
        await SeedUsersAsync();

        // Arrange
        var body = new
        {
            email = "admin@cards.com",
            password = "StrongAdminPassword123"
        };

        // Act
        var response = await _client.PostAsync("/api/Auth/login", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.DeserializeAsync<ApiResponse<AccessTokenVm>>();

        responseContent.Should().NotBeNull();
        responseContent?.IsSuccess.Should().BeTrue();
        responseContent?.Data.Token.Should().NotBeNullOrEmpty();
    }
}