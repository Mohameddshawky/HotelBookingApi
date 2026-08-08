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
    IAmenityRepository Amenities { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
