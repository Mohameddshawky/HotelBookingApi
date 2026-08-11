import { api } from './api';

export interface Guest {
    id: string;
    fullName: string;
    email: string;
    phoneNumber: string;
}

export interface CreateOrUpdateGuestDto {
    fullName: string;
    email: string;
    phoneNumber: string;
}

export const guestsService = {
    getById: (id: string) => api.get(`/guests/${id}`),
    getByEmail: (email: string) => api.get(`/guests/email/${email}`),
    create: (data: CreateOrUpdateGuestDto) => api.post('/guests', data),
    update: (id: string, data: CreateOrUpdateGuestDto) => api.put(`/guests/${id}`, data),
    getBookingHistory: (id: string) => api.get(`/guests/${id}/bookings`)
};
