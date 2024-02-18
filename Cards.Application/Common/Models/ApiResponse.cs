namespace Cards.Application.Common.Models;

public record ApiResponse<T>
{
    public T Data { get; private protected init; } = default!;
    
    public bool IsSuccess { get; private protected init; }
    public string Message { get; private protected init; } = string.Empty;
    public string[] Errors { get; private protected init; } = Array.Empty<string>();
    
    public static ApiResponse<T> Success(T Data)
    {
        return new ApiResponse<T>
        {
            Data = Data,
            IsSuccess = true
        };
    }

    public static ApiResponse<T> Success(T Data, string message)
    {
        return new ApiResponse<T>
        {
            Data = Data,
            IsSuccess = true,
            Message = message
        };
    }

    public static ApiResponse<T> Error(string message)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Message = message
        };
    }
    
    public static ApiResponse<T> Error(string message, string[] errors)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors
        };
    }
}

public record PaginatedApiResponse<T> : ApiResponse<IEnumerable<T>>
{
    public int Page { get; private init; }
    public int PageSize { get; private init; }
    public int TotalPages { get; private init; }
    public int TotalCount { get; private init; }
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;

    public static PaginatedApiResponse<T> Success(IEnumerable<T> Data, int page, int pageSize, int totalCount)
    {
        return new PaginatedApiResponse<T>
        {
            Data = Data,
            IsSuccess = true,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
            TotalCount = totalCount
        };
    }

    public new static PaginatedApiResponse<T> Error(string message)
    {
        return new PaginatedApiResponse<T>
        {
            IsSuccess = false,
            Message = message
        };
    }
    
    public new static PaginatedApiResponse<T> Error(string message, string[] errors)
    {
        return new PaginatedApiResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors
        };
    }
}