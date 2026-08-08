using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelBookingApi.Domain.Entities;
using HotelBookingApi.Application.Interfaces.Repositories;
using HotelBookingApi.Infrastructure.Data;

namespace HotelBookingApi.Infrastructure.Repositories;

public class GuestRepository : GenericRepository<Guest>, IGuestRepository
{
    public GuestRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Guest?> GetByEmailAsync(string email)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(g => g.Email == email);
    }
}
