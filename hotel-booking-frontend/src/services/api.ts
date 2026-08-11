const API_BASE_URL = 'https://localhost:7248/api';

const getHeaders = () => {
    const token = localStorage.getItem('token');
    return {
        'Content-Type': 'application/json',
        ...(token ? { 'Authorization': `Bearer ${token}` } : {})
    };
};

export const api = {
    get: async (endpoint: string) => {
        const res = await fetch(`${API_BASE_URL}${endpoint}`, {
            headers: getHeaders()
        });
        if (!res.ok) throw new Error('Network response was not ok');
        return res.json();
    },
    post: async (endpoint: string, body: any) => {
        const res = await fetch(`${API_BASE_URL}${endpoint}`, {
            method: 'POST',
            headers: getHeaders(),
            body: JSON.stringify(body)
        });
        if (!res.ok) throw new Error('Network response was not ok');
        return res.json();
    },
    put: async (endpoint: string, body: any) => {
        const res = await fetch(`${API_BASE_URL}${endpoint}`, {
            method: 'PUT',
            headers: getHeaders(),
            body: JSON.stringify(body)
        });
        if (!res.ok) throw new Error('Network response was not ok');
        return res.status === 204 ? null : res.json();
    },
    delete: async (endpoint: string) => {
        const res = await fetch(`${API_BASE_URL}${endpoint}`, {
            method: 'DELETE',
            headers: getHeaders()
        });
        if (!res.ok) throw new Error('Network response was not ok');
        return res.status === 204 ? null : res.json();
    }
};
