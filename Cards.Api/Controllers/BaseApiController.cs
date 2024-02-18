using Cards.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cards.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private ISender? _mapper;

    private protected ISender Mapper => _mapper ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    
    protected ActionResult<ApiResponse<T>> CustomResponse<T>(ApiResponse<T> result)
    {
        return result.IsSuccess
            ? Ok(result)
            : BadRequest(result);
    }
    
    protected ActionResult<PaginatedApiResponse<T>> CustomResponse<T>(PaginatedApiResponse<T> result)
    {
        return result.IsSuccess
            ? Ok(result)
            : BadRequest(result);
    }
}