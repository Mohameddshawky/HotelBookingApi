import { api } from './api';

export interface Room {
    id: string;
    number: string;
    roomTypeId: string;
    status: number;
}

export interface PagedResult<T> {
    items: T[];
    totalCount: number;
    pageNumber: number;
    pageSize: number;
    totalPages: number;
}

export interface CreateOrUpdateRoomDto {
    number: string;
    roomTypeId: string;
}

export const roomsService = {
    getAll: (pageNumber = 1, pageSize = 100) => api.get(`/rooms?pageNumber=${pageNumber}&pageSize=${pageSize}`),
    getAvailableRooms: (checkIn: string, checkOut: string) => api.get(`/rooms/available?checkIn=${encodeURIComponent(checkIn)}&checkOut=${encodeURIComponent(checkOut)}`),
    getById: (id: string) => api.get(`/rooms/${id}`),
    create: (data: CreateOrUpdateRoomDto) => api.post('/rooms', data),
    deactivate: (id: string) => api.put(`/rooms/${id}/deactivate`, {})
};
