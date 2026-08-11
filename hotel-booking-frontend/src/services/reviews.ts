import { api } from './api';

export interface Review {
    id: string;
    bookingId: string;
    rating: number;
    comment: string;
}

export interface CreateOrUpdateReviewDto {
    bookingId: string;
    rating: number;
    comment: string;
}

export const reviewsService = {
    getByBookingId: (bookingId: string) => api.get(`/reviews/booking/${bookingId}`),
    create: (data: CreateOrUpdateReviewDto) => api.post('/reviews', data)
};
