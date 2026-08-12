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
    guestEmail: string;
    guestFirstName: string;
    guestLastName: string;
    guestPhone: string;
    roomId: string;
    checkInDate: string;
    checkOutDate: string;
}

export const bookingsService = {
    getAll: () => api.get('/bookings'),
    getById: (id: string) => api.get(`/bookings/${id}`),
    getGuestBookings: (guestId: string) => api.get(`/bookings/guest/${guestId}`),
    getGuestBookingsByEmail: (email: string) => api.get(`/bookings/guest/by-email/${encodeURIComponent(email)}`),
    create: (data: CreateOrUpdateBookingDto) => api.post('/bookings', data),
    confirm: (id: string) => api.put(`/bookings/${id}/confirm`, {}),
    cancel: (id: string) => api.put(`/bookings/${id}/cancel`, {}),
    checkIn: (id: string) => api.put(`/bookings/${id}/checkin`, {}),
    checkOut: (id: string) => api.put(`/bookings/${id}/checkout`, {})
};
