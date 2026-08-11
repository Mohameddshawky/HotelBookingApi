import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { bookingsService } from '../../services/bookings';

export default function BookingForm() {
    const navigate = useNavigate();
    const [guestId, setGuestId] = useState('');
    const [roomId, setRoomId] = useState('');
    const [checkInDate, setCheckInDate] = useState('');
    const [checkOutDate, setCheckOutDate] = useState('');
    const [loading, setLoading] = useState(false);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        try {
            await bookingsService.create({ 
                guestId, 
                roomId, 
                checkInDate: new Date(checkInDate).toISOString(), 
                checkOutDate: new Date(checkOutDate).toISOString() 
            });
            navigate('/bookings');
        } catch (error) {
            console.error('Failed to save', error);
            alert('Failed to save booking');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="max-w-2xl mx-auto bg-white rounded-lg shadow p-6">
            <h3 className="text-lg font-medium text-gray-900 mb-6">New Booking</h3>
            <form onSubmit={handleSubmit} className="space-y-6">
                <div>
                    <label className="block text-sm font-medium text-gray-700">Guest ID</label>
                    <input
                        type="text"
                        required
                        value={guestId}
                        onChange={e => setGuestId(e.target.value)}
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                    />
                </div>
                <div>
                    <label className="block text-sm font-medium text-gray-700">Room ID</label>
                    <input
                        type="text"
                        required
                        value={roomId}
                        onChange={e => setRoomId(e.target.value)}
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                    />
                </div>
                <div className="grid grid-cols-2 gap-4">
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Check In Date</label>
                        <input
                            type="date"
                            required
                            value={checkInDate}
                            onChange={e => setCheckInDate(e.target.value)}
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                        />
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Check Out Date</label>
                        <input
                            type="date"
                            required
                            value={checkOutDate}
                            onChange={e => setCheckOutDate(e.target.value)}
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                        />
                    </div>
                </div>
                <div className="flex justify-end gap-3 pt-4 border-t border-gray-200">
                    <Link to="/bookings" className="px-4 py-2 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 hover:bg-gray-50">
                        Cancel
                    </Link>
                    <button type="submit" disabled={loading} className="px-4 py-2 bg-brand-600 text-white rounded-md hover:bg-brand-700">
                        {loading ? 'Saving...' : 'Create Booking'}
                    </button>
                </div>
            </form>
        </div>
    );
}
