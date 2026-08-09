using System.Text.Json;

namespace HotelBookingApi.Middlewares;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; } // Optional: used in development for stack traces

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    }
}
