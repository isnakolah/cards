using System.Net;
using Cards.Application.Cards.ViewModels;
using Cards.Application.Common.Models;
using FluentAssertions;

namespace Cards.Tests.E2E.Controllers;

public class GetCardsTests : TestBase
{
    [Fact]
    public async Task GetCards_ShouldReturnError_WhenNoCardsArePresent()
    {
        // Arrange
        // await ResetStateAsync();

        await AuthenticateAsAdminAsync();

        // Act
        var response = await _client.GetAsync("/api/Cards");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseContent = await response.Content.DeserializeAsync<PaginatedApiResponse<CardVm>>();

        responseContent.Should().NotBeNull();
        responseContent!.IsSuccess.Should().BeFalse();
        responseContent.Data.Should().BeNull();
    }
    
    
    [Fact]
    public async Task GetCardById_ShouldReturnCard_WhenCardIsPresent()
    {
        // Arrange
        await AuthenticateAsAdminAsync();
        
        var cardId = await CreateCardAsync("Test Card", "Test Card Description", "#FF0000");

        // Act
        var response = await _client.GetAsync($"/api/Cards/{cardId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.DeserializeAsync<ApiResponse<CardVm>>();

        responseContent.Should().NotBeNull();
        responseContent!.IsSuccess.Should().BeTrue();
        responseContent.Data.Should().NotBeNull();
        responseContent.Data.Name.Should().Be("Test Card");
        responseContent.Data.Description.Should().Be("Test Card Description");
        responseContent.Data.Color.Should().Be("#FF0000");
    }
}