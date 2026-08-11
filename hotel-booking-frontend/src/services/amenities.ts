import { api } from './api';

export interface Amenity {
    id: string;
    name: string;
    description: string;
}

export const amenitiesService = {
    getAll: () => api.get('/amenities'),
    getById: (id: string) => api.get(`/amenities/${id}`),
    create: (data: Omit<Amenity, 'id'>) => api.post('/amenities', data),
    update: (id: string, data: Omit<Amenity, 'id'>) => api.put(`/amenities/${id}`, data),
    delete: (id: string) => api.delete(`/amenities/${id}`)
};
