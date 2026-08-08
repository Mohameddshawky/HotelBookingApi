using System.Threading;
using System.Threading.Tasks;

namespace HotelBookingApi.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    IRoomTypeRepository RoomTypes { get; }
    IRoomRepository Rooms { get; }
    IGuestRepository Guests { get; }
    IBookingRepository Bookings { get; }
    IReviewRepository Reviews { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
