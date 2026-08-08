using System;
using System.Threading.Tasks;
using HotelBookingApi.Domain.Entities;

namespace HotelBookingApi.Application.Interfaces.Repositories;

public interface IGuestRepository : IGenericRepository<Guest>
{
    Task<Guest?> GetByEmailAsync(string email);
}
