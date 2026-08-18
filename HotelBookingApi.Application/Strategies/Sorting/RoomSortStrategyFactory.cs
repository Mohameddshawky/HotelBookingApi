using System;
using System.Collections.Generic;

namespace HotelBookingApi.Application.Strategies.Sorting;

public class RoomSortStrategyFactory
{
    private readonly IEnumerable<IRoomSortStrategy> _strategies;

    public RoomSortStrategyFactory(IEnumerable<IRoomSortStrategy> strategies)
    {
        _strategies = strategies;
    }

    public IRoomSortStrategy GetStrategy(string? sortBy)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return new DefaultRoomSortStrategy();

        return sortBy.ToLower() switch
        {
            "price" => GetStrategyOfType<SortByPriceStrategy>(),
            "name" => GetStrategyOfType<SortByNameStrategy>(),
            "rating" => GetStrategyOfType<SortByRatingStrategy>(),
            _ => new DefaultRoomSortStrategy()
        };
    }

    private IRoomSortStrategy GetStrategyOfType<T>() where T : IRoomSortStrategy
    {
        foreach (var strategy in _strategies)
        {
            if (strategy is T)
            {
                return strategy;
            }
        }
        return new DefaultRoomSortStrategy();
    }
}
