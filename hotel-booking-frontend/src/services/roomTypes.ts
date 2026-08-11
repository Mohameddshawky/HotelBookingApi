import { api } from './api';

export interface RoomType {
    id: string;
    name: string;
    pricePerNight: number;
    maxGuests: number;
}

export interface CreateOrUpdateRoomTypeDto {
    name: string;
    pricePerNight: number;
    maxGuests: number;
    amenityIds: string[];
}

export const roomTypesService = {
    getAll: () => api.get('/roomtypes'),
    getById: (id: string) => api.get(`/roomtypes/${id}`),
    create: (data: CreateOrUpdateRoomTypeDto) => api.post('/roomtypes', data),
    update: (id: string, data: CreateOrUpdateRoomTypeDto) => api.put(`/roomtypes/${id}`, data),
    delete: (id: string) => api.delete(`/roomtypes/${id}`)
};
