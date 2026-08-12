import { useState } from 'react';
import { Link } from 'react-router-dom';
import { bookingsService, Booking } from '../../services/bookings';
import { Plus, Search, CheckCircle, XCircle, LogIn, LogOut } from 'lucide-react';

export default function BookingsList() {
    const [guestIdSearch, setGuestIdSearch] = useState('');
    const [bookings, setBookings] = useState<Booking[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const handleSearch = async (e: React.FormEvent) => {
        e.preventDefault();
        loadBookings(guestIdSearch);
    };

    const loadBookings = async (guestId: string) => {
        setLoading(true);
        setError('');
        setBookings([]);
        try {
            const data = await bookingsService.getGuestBookings(guestId);
            setBookings(data);
            if (data.length === 0) setError('No bookings found for this guest.');
        } catch (err) {
            setError('Error fetching bookings or guest not found.');
        } finally {
            setLoading(false);
        }
    };

    const handleAction = async (id: string, action: 'confirm' | 'cancel' | 'checkIn' | 'checkOut') => {
        try {
            await bookingsService[action](id);
            loadBookings(guestIdSearch); // refresh list
        } catch (error) {
            console.error(`Failed to ${action}`, error);
            alert(`Action failed`);
        }
    };

    const getStatusText = (status: string | number) => {
        if (typeof status === 'number') {
            switch (status) {
                case 0: return 'Pending';
                case 1: return 'Confirmed';
                case 2: return 'Cancelled';
                case 3: return 'CheckedIn';
                case 4: return 'CheckedOut';
                default: return 'Unknown';
            }
        }
        return status;
    };

    const getStatusColor = (status: string | number) => {
        const statusStr = typeof status === 'number' ? getStatusText(status) : status;
        switch (statusStr) {
            case 'Pending': return 'bg-yellow-100 text-yellow-800';
            case 'Confirmed': return 'bg-blue-100 text-blue-800';
            case 'Cancelled': return 'bg-red-100 text-red-800';
            case 'CheckedIn': return 'bg-green-100 text-green-800';
            case 'CheckedOut': return 'bg-gray-100 text-gray-800';
            default: return 'bg-gray-100 text-gray-800';
        }
    };

    return (
        <div className="bg-white rounded-lg shadow p-6">
            <div className="flex justify-between items-center mb-6">
                <h3 className="text-lg font-medium text-gray-900">Bookings</h3>
                <Link to="/bookings/new" className="bg-brand-600 text-white px-4 py-2 rounded-md hover:bg-brand-700 flex items-center gap-2">
                    <Plus size={18} /> New Booking
                </Link>
            </div>

            <form onSubmit={handleSearch} className="flex gap-4 mb-8 max-w-lg">
                <input
                    type="text"
                    required
                    placeholder="Search by Guest ID..."
                    value={guestIdSearch}
                    onChange={e => setGuestIdSearch(e.target.value)}
                    className="flex-1 px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                />
                <button type="submit" disabled={loading} className="px-4 py-2 bg-gray-800 text-white rounded-md hover:bg-gray-900 flex items-center gap-2">
                    <Search size={18} /> {loading ? 'Searching...' : 'Search'}
                </button>
            </form>

            {error && <div className="text-red-600 bg-red-50 p-4 rounded-md mb-4">{error}</div>}

            {bookings.length > 0 && (
                <div className="border border-gray-200 rounded-lg overflow-hidden">
                    <table className="min-w-full divide-y divide-gray-200">
                        <thead className="bg-gray-50">
                            <tr>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Room ID</th>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Dates</th>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Total Price</th>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
                                <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
                            </tr>
                        </thead>
                        <tbody className="bg-white divide-y divide-gray-200">
                            {bookings.map(b => (
                                <tr key={b.id}>
                                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500" title={b.roomId}>{b.roomId.substring(0, 8)}...</td>
                                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                                        {new Date(b.checkInDate).toLocaleDateString()} - {new Date(b.checkOutDate).toLocaleDateString()}
                                    </td>
                                    <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">${b.totalPrice}</td>
                                    <td className="px-6 py-4 whitespace-nowrap text-sm">
                                        <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${getStatusColor(b.status)}`}>
                                            {getStatusText(b.status)}
                                        </span>
                                    </td>
                                    <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                                        <div className="flex justify-end gap-2">
                                            {(b.status === 0 || b.status === 'Pending') && (
                                                <>
                                                    <button onClick={() => handleAction(b.id, 'confirm')} className="text-blue-600 hover:text-blue-900" title="Confirm"><CheckCircle size={18} /></button>
                                                    <button onClick={() => handleAction(b.id, 'cancel')} className="text-red-600 hover:text-red-900" title="Cancel"><XCircle size={18} /></button>
                                                </>
                                            )}
                                            {(b.status === 1 || b.status === 'Confirmed') && (
                                                <button onClick={() => handleAction(b.id, 'checkIn')} className="text-green-600 hover:text-green-900" title="Check In"><LogIn size={18} /></button>
                                            )}
                                            {(b.status === 3 || b.status === 'CheckedIn') && (
                                                <button onClick={() => handleAction(b.id, 'checkOut')} className="text-gray-600 hover:text-gray-900" title="Check Out"><LogOut size={18} /></button>
                                            )}
                                        </div>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}
        </div>
    );
}
