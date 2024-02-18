using System.Net.Http.Headers;
using Cards.Application.Cards.ViewModels;
using Cards.Application.Common.Models;
using Cards.Application.Users.ViewModels;
using Cards.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Cards.Tests.E2E;

public abstract class TestBase : IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    protected readonly HttpClient _client;
    protected readonly List<CardVm> _cards = new();
    private bool UsersSeeded { get; set; }

    protected TestBase()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    if (dbContextDescriptor != null)
                    {
                        services.Remove(dbContextDescriptor);
                    }

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });

                });
            });

        _client = _factory.CreateClient();
    }
    
    // // method to clean up the database after each test
    // protected async Task ResetStateAsync()
    // {
    //     // reset the database
    //     using var scope = _factory.Services.CreateScope();
    //     
    //     var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //     
    //     await context.Database.EnsureDeletedAsync();
    //     
    //     await context.Database.EnsureCreatedAsync();
    //     
    //     UsersSeeded = false;
    //
    //     _client.DefaultRequestHeaders.Authorization = null;
    // }

    protected async Task AuthenticateAsAdminAsync()
    {
        await LoginAsync("admin@cards.com", "StrongAdminPassword123");
    }
    
    protected async Task AuthenticateAsUserOneAsync()
    {
        await LoginAsync("john.doe@cards.com", "StrongMemberOnePassword123");
    }
    
    protected async Task AuthenticateAsUserTwoAsync()
    {
        await LoginAsync("jane.doe@cards.com", "StrongMemberTwoPassword123");
    }
    
    protected async Task<Guid> CreateCardAsync(string cardName, string? cardDescription, string? cardColor)
    {
        var payload = new
        {
            name = cardName,
            description = cardDescription,
            color = cardColor
        };

        var createResponse = await _client.PostAsync("/api/Cards", payload.ToStringContent());
        var createResponseContent = await createResponse.Content.DeserializeAsync<ApiResponse<CardVm>>();
        
        _cards.Add(createResponseContent!.Data);
        
        return createResponseContent!.Data.Id;
    }

    private async Task LoginAsync(string email, string password)
    {
        if (!UsersSeeded)
        {
            await SeedUsersAsync();

            UsersSeeded = true;
        }

        var loginPayload = new
        {
            email, password
        };
        
        var response = await _client.PostAsync("/api/Auth/login", loginPayload.ToStringContent());

        var responseContent = await response.Content.DeserializeAsync<ApiResponse<AccessTokenVm>>();

        var token = responseContent?.Data.Token ?? throw new InvalidOperationException("Token not found in response");
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    protected async Task SeedUsersAsync()
    {
        await _client.PostAsync("api/auth/seed-users", null);
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }

}