using Cards.Application.Cards.Commands;
using Cards.Application.Cards.Queries;
using Cards.Application.Cards.ViewModels;
using Cards.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cards.Api.Controllers;

[Authorize]
public class CardsController : BaseApiController
{
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<ApiResponse<CardVm>>> GetCardById([FromRoute] Guid id)
    {
        return CustomResponse(await Mapper.Send(new GetCardByIdQuery(id)));
    }
    
    [HttpGet]
    public async Task<ActionResult<PaginatedApiResponse<CardVm>>> GetCards([FromQuery] GetCardsQuery query)
    {
        return CustomResponse(await Mapper.Send(query));
    }
    
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CardVm>>> CreateCard([FromBody] CreateCardCommand command)
    {
        return CustomResponse(await Mapper.Send(command));
    }
    
    [HttpPut("{id:Guid}")]
    public async Task<ActionResult<ApiResponse<CardVm>>> UpdateCard([FromRoute] Guid id, [FromBody] UpdateCardCommand command)
    {
        return CustomResponse(await Mapper.Send(command with {Id = id}));
    }
    
    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteCard([FromRoute] Guid id)
    {
        return CustomResponse(await Mapper.Send(new DeleteCardCommand(id)));
    }
}