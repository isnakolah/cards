using System.Net;
using Cards.Application.Cards.ViewModels;
using Cards.Application.Common.Models;
using FluentAssertions;

namespace Cards.Tests.E2E.Controllers;

public class CreateCardsTests : TestBase
{
    [Fact]
    public async Task CreateCard_ShouldCreateCard_WhenValidCardIsProvided()
    {
        // Arrange
        await AuthenticateAsAdminAsync();

        var content = new
        {
            name = "Test Card",
            description = "Test Card Description",
            color = "#FF0000",
        };

        // Act
        var response = await _client.PostAsync("/api/Cards", content.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.DeserializeAsync<ApiResponse<CardVm>>();

        responseContent.Should().NotBeNull();
        responseContent?.IsSuccess.Should().BeTrue();
        responseContent?.Data.Should().NotBeNull();
        responseContent?.Data?.Name.Should().Be("Test Card");
        responseContent?.Data?.Description.Should().Be("Test Card Description");
        responseContent?.Data?.Color.Should().Be("#FF0000");
    }
    
    [Theory]
    [InlineData("Test Card Without Description", null, null)]
    [InlineData("Test Card With Description but Without Color", "Test Card Description", null)]
    [InlineData("Test Card Without Description and Color", null, "#FF0000")]
    [InlineData("Test Card With Empty Description and Color", "", "")]
    [InlineData("Test Card With Whitespace Description and Color", "  ", "   ")]
    [InlineData("Test Card With Trailing Whitespace in Color", "", "  #FF0000")]
    [InlineData("Test Card With Leading Whitespace in Description", "  Test Card Description", "")]
    public async Task CreateCard_ShouldCreateCard_WhenOptionalPropertiesAreNotProvided(string name, string? description, string? color)
    {
        // Arrange
        await AuthenticateAsAdminAsync();

        var content = new
        {
            name,
            description,
            color,
        };

        // Act
        var response = await _client.PostAsync("/api/Cards", content.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.DeserializeAsync<ApiResponse<CardVm>>();

        responseContent.Should().NotBeNull();
        responseContent!.IsSuccess.Should().BeTrue();
        responseContent.Data.Should().NotBeNull();
        responseContent.Data.Name.Should().Be(name.Trim());
        responseContent.Data.Description.Should().Be(string.IsNullOrWhiteSpace(description) ? null : description.Trim());
        responseContent.Data.Color.Should().Be(string.IsNullOrWhiteSpace(color) ? null : color.Trim());
    }
    
    [Theory]
    [InlineData("", "Test Card With Empty Name Description", "#FF0000")]
    [InlineData(null, "Test Card With Null Name Description", "#FF0000")]
    public async Task CreateCard_ShouldReturnBadRequest_WhenNameIsNullOrEmpty(string? name, string description, string color)
    {
        // Arrange
        await AuthenticateAsAdminAsync();

        var content = new
        {
            name,
            description,
            color,
        };

        // Act
        var response = await _client.PostAsync("/api/Cards", content.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}