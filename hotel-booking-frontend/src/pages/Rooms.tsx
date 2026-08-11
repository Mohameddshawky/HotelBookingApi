import { useEffect, useState } from 'react';
import { api } from '../services/api';

interface Room {
    id: string;
    number: string;
    roomTypeId: string;
    pricePerNight?: number;
}

export default function Rooms() {
    const [rooms, setRooms] = useState<Room[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchRooms = async () => {
            try {
                // Fetching rooms for today and tomorrow as a baseline search
                const checkIn = new Date().toISOString().split('T')[0];
                const checkOut = new Date(Date.now() + 86400000).toISOString().split('T')[0];
                
                const data = await api.get(`/rooms/available?checkIn=${checkIn}&checkOut=${checkOut}`);
                setRooms(data);
            } catch (err) {
                console.error("Failed to fetch rooms", err);
            } finally {
                setLoading(false);
            }
        };
        fetchRooms();
    }, []);

    return (
        <div className="page-container">
            <h2 className="section-title">Available Rooms</h2>
            {loading ? (
                <div className="loading">Loading luxury rooms...</div>
            ) : (
                <div className="room-grid">
                    {rooms.length === 0 && <p>No rooms available for these dates.</p>}
                    {rooms.map(room => (
                        <div key={room.id} className="room-card card">
                            <div className="room-image-placeholder"></div>
                            <div className="room-info">
                                <h3>Room {room.number}</h3>
                                <p className="price">${room.pricePerNight ?? 299} <span className="per-night">/ night</span></p>
                                <button className="book-btn">Book Now</button>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}
