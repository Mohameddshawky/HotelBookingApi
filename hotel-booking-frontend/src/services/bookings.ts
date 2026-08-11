import { api } from './api';

export interface Booking {
    id: string;
    guestId: string;
    roomId: string;
    checkInDate: string;
    checkOutDate: string;
    totalPrice: number;
    status: number;
}

export interface CreateOrUpdateBookingDto {
    guestId: string;
    roomId: string;
    checkInDate: string;
    checkOutDate: string;
}

export const bookingsService = {
    getById: (id: string) => api.get(`/bookings/${id}`),
    getGuestBookings: (guestId: string) => api.get(`/bookings/guest/${guestId}`),
    create: (data: CreateOrUpdateBookingDto) => api.post('/bookings', data),
    confirm: (id: string) => api.put(`/bookings/${id}/confirm`, {}),
    cancel: (id: string) => api.put(`/bookings/${id}/cancel`, {}),
    checkIn: (id: string) => api.put(`/bookings/${id}/checkin`, {}),
    checkOut: (id: string) => api.put(`/bookings/${id}/checkout`, {})
};
