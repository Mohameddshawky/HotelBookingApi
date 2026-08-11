import { api } from './api';

export interface OccupancyReport {
    occupancyPercentage: number;
    totalRooms: number;
    occupiedRooms: number;
}

export interface RoomTypeRatingReport {
    roomTypeId: string;
    roomTypeName: string;
    averageRating: number;
}

export const reportsService = {
    getOccupancy: () => api.get('/reports/occupancy'),
    getRoomTypeRatings: () => api.get('/reports/room-type-ratings')
};
