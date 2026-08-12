import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { bookingsService } from '../../services/bookings';
import { roomsService, Room } from '../../services/rooms';
import { Search } from 'lucide-react';

export default function BookingForm() {
    const navigate = useNavigate();
    const [guestEmail, setGuestEmail] = useState('');
    const [guestFirstName, setGuestFirstName] = useState('');
    const [guestLastName, setGuestLastName] = useState('');
    const [guestPhone, setGuestPhone] = useState('');
    
    const [roomId, setRoomId] = useState('');
    const [checkInDate, setCheckInDate] = useState('');
    const [checkOutDate, setCheckOutDate] = useState('');
    
    const [availableRooms, setAvailableRooms] = useState<any[]>([]);
    const [loadingRooms, setLoadingRooms] = useState(false);
    const [loading, setLoading] = useState(false);

    const handleSearchRooms = async () => {
        if (!checkInDate || !checkOutDate) {
            alert('Please select check-in and check-out dates first');
            return;
        }
        setLoadingRooms(true);
        try {
            const data = await roomsService.getAvailableRooms(checkInDate, checkOutDate);
            setAvailableRooms(Array.isArray(data) ? data : []);
            setRoomId(''); // reset selection
        } catch (error) {
            console.error('Failed to load rooms', error);
            alert('Failed to search available rooms');
        } finally {
            setLoadingRooms(false);
        }
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!roomId) {
            alert('Please select a room');
            return;
        }
        setLoading(true);
        try {
            await bookingsService.create({ 
                guestEmail,
                guestFirstName,
                guestLastName,
                guestPhone,
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
                <div className="grid grid-cols-2 gap-4">
                    <div>
                        <label className="block text-sm font-medium text-gray-700">First Name</label>
                        <input
                            type="text"
                            required
                            value={guestFirstName}
                            onChange={e => setGuestFirstName(e.target.value)}
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                        />
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Last Name</label>
                        <input
                            type="text"
                            required
                            value={guestLastName}
                            onChange={e => setGuestLastName(e.target.value)}
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                        />
                    </div>
                </div>

                <div className="grid grid-cols-2 gap-4">
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Email Address</label>
                        <input
                            type="email"
                            required
                            value={guestEmail}
                            onChange={e => setGuestEmail(e.target.value)}
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                        />
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Phone Number</label>
                        <input
                            type="tel"
                            required
                            value={guestPhone}
                            onChange={e => setGuestPhone(e.target.value)}
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                        />
                    </div>
                </div>

                <div className="border-t border-gray-200 pt-6">
                    <h4 className="text-md font-medium text-gray-900 mb-4">Stay Details</h4>
                    <div className="grid grid-cols-2 gap-4 mb-4">
                        <div>
                            <label className="block text-sm font-medium text-gray-700">Check In Date</label>
                            <input
                                type="date"
                                required
                                value={checkInDate}
                                onChange={e => { setCheckInDate(e.target.value); setAvailableRooms([]); setRoomId(''); }}
                                className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                            />
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-gray-700">Check Out Date</label>
                            <input
                                type="date"
                                required
                                value={checkOutDate}
                                onChange={e => { setCheckOutDate(e.target.value); setAvailableRooms([]); setRoomId(''); }}
                                className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                            />
                        </div>
                    </div>
                    
                    <div className="mb-4">
                        <button type="button" onClick={handleSearchRooms} disabled={loadingRooms || !checkInDate || !checkOutDate} className="bg-gray-800 text-white px-4 py-2 rounded-md hover:bg-gray-900 flex items-center gap-2">
                            <Search size={16} /> {loadingRooms ? 'Searching...' : 'Find Available Rooms'}
                        </button>
                    </div>

                    {availableRooms.length > 0 && (
                        <div>
                            <label className="block text-sm font-medium text-gray-700">Select Room</label>
                            <select
                                required
                                value={roomId}
                                onChange={e => setRoomId(e.target.value)}
                                className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                            >
                                <option value="">-- Choose a room --</option>
                                {availableRooms.map(room => (
                                    <option key={room.id} value={room.id}>
                                        Room {room.number} - ${room.pricePerNight}/night
                                    </option>
                                ))}
                            </select>
                        </div>
                    )}
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
