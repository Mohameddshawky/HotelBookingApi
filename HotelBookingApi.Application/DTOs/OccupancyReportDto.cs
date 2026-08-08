namespace HotelBookingApi.Application.DTOs;

public class OccupancyReportDto
{
    public decimal OccupancyPercentage { get; set; }
    public int TotalRooms { get; set; }
    public int OccupiedRooms { get; set; }
}
