namespace Cd.Cms.Shared.Responses
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static ApiResponse<T> Success(string message, T? data = default) => new()
            { IsSuccess = true, Code = ResponseCodes.SUCCESS, Message = message, Data = data };
        public static ApiResponse<T> Error(string message, string code = ResponseCodes.SERVER_ERROR) => new()
            { IsSuccess = false, Code = code, Message = message };
        public static ApiResponse<T> NotFound(string message = "Resource not found.") => new()
            { IsSuccess = false, Code = ResponseCodes.NOT_FOUND, Message = message };
        public static ApiResponse<T> Unauthorized(string message = "Unauthorized.") => new()
            { IsSuccess = false, Code = ResponseCodes.UNAUTHORIZED, Message = message };
        public static ApiResponse<T> Forbidden(string message = "Access denied.") => new()
            { IsSuccess = false, Code = ResponseCodes.FORBIDDEN, Message = message };
        public static ApiResponse<T> ValidationError(string message) => new()
            { IsSuccess = false, Code = ResponseCodes.VALIDATION_ERROR, Message = message };
    }

    public static class ResponseCodes
    {
        public const string SUCCESS          = "200";
        public const string BAD_REQUEST      = "400";
        public const string UNAUTHORIZED     = "401";
        public const string FORBIDDEN        = "403";
        public const string NOT_FOUND        = "404";
        public const string VALIDATION_ERROR = "422";
        public const string SERVER_ERROR     = "500";
    }
}
