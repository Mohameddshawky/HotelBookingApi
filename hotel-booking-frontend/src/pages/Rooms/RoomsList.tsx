import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { roomsService, Room } from '../../services/rooms';
import { Plus, PowerOff, Search, X } from 'lucide-react';
import { useAuth } from '../../contexts/AuthContext';

export default function RoomsList() {
    const [rooms, setRooms] = useState<Room[]>([]);
    const [loading, setLoading] = useState(true);
    const [checkIn, setCheckIn] = useState('');
    const [checkOut, setCheckOut] = useState('');
    const [isSearching, setIsSearching] = useState(false);
    const { isAuthenticated } = useAuth();

    const loadRooms = async (inDate?: string, outDate?: string) => {
        setLoading(true);
        try {
            let data: any;
            if (inDate && outDate) {
                data = await roomsService.getAvailableRooms(inDate, outDate);
                setIsSearching(true);
            } else {
                data = await roomsService.getAll();
                setIsSearching(false);
            }
            console.log('Rooms API Response:', data);
            // AvailableRooms API returns an array, GetAll returns PagedResult
            if (Array.isArray(data)) {
                setRooms(data);
            } else {
                setRooms(data.items || data.Items || []);
            }
        } catch (error) {
            console.error('Failed to load rooms', error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadRooms();
    }, []);

    const handleSearch = (e: React.FormEvent) => {
        e.preventDefault();
        if (checkIn && checkOut) {
            loadRooms(checkIn, checkOut);
        }
    };

    const clearSearch = () => {
        setCheckIn('');
        setCheckOut('');
        loadRooms();
    };

    const handleDeactivate = async (id: string) => {
        if (window.confirm('Are you sure you want to deactivate this room?')) {
            try {
                await roomsService.deactivate(id);
                loadRooms(); // Reload to get updated status
            } catch (error) {
                console.error('Failed to deactivate', error);
            }
        }
    };

    const getStatusText = (status?: string | number) => {
        if (status === undefined || status === null) return 'Available';
        if (typeof status === 'number') {
            switch (status) {
                case 0: return 'Available';
                case 1: return 'Occupied';
                case 2: return 'Maintenance';
                default: return 'Unknown';
            }
        }
        return status; // It's already a string like "Available", "Occupied"
    };

    const getStatusColor = (status?: string | number) => {
        const statusStr = typeof status === 'number' ? getStatusText(status) : (status || 'Available');
        switch (statusStr) {
            case 'Available': return 'bg-green-100 text-green-800';
            case 'Occupied': return 'bg-blue-100 text-blue-800';
            case 'Maintenance': return 'bg-red-100 text-red-800';
            default: return 'bg-gray-100 text-gray-800';
        }
    };

    if (loading) return <div className="text-center py-10">Loading...</div>;

    return (
        <div className="bg-white rounded-lg shadow">
            <div className="px-6 py-4 border-b border-gray-200 flex flex-col md:flex-row md:justify-between md:items-center gap-4">
                <h3 className="text-lg font-medium text-gray-900">Rooms</h3>
                
                <form onSubmit={handleSearch} className="flex flex-1 max-w-xl items-end gap-3">
                    <div className="flex-1">
                        <label className="block text-xs font-medium text-gray-700 mb-1">Check-in</label>
                        <input
                            type="date"
                            required
                            value={checkIn}
                            onChange={(e) => setCheckIn(e.target.value)}
                            className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500 text-sm"
                        />
                    </div>
                    <div className="flex-1">
                        <label className="block text-xs font-medium text-gray-700 mb-1">Check-out</label>
                        <input
                            type="date"
                            required
                            value={checkOut}
                            onChange={(e) => setCheckOut(e.target.value)}
                            className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500 text-sm"
                        />
                    </div>
                    <button type="submit" disabled={loading} className="px-4 py-2 bg-gray-800 text-white rounded-md hover:bg-gray-900 flex items-center gap-2 h-[38px]">
                        <Search size={16} /> <span className="hidden sm:inline">Search</span>
                    </button>
                    {isSearching && (
                        <button type="button" onClick={clearSearch} className="px-3 py-2 text-gray-600 hover:text-gray-900 bg-gray-100 rounded-md h-[38px]" title="Clear Search">
                            <X size={16} />
                        </button>
                    )}
                </form>

                {isAuthenticated && (
                    <Link to="/rooms/new" className="bg-brand-600 text-white px-4 py-2 rounded-md hover:bg-brand-700 flex items-center gap-2 h-[38px] whitespace-nowrap">
                        <Plus size={18} /> Add Room
                    </Link>
                )}
            </div>
            
            {rooms.length === 0 && !loading && (
                <div className="p-8 text-center text-gray-500">
                    No rooms found matching your criteria.
                </div>
            )}

            {rooms.length > 0 && (
                <div className="overflow-x-auto">
                    <table className="min-w-full divide-y divide-gray-200">
                    <thead className="bg-gray-50">
                        <tr>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Number</th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Room Type ID</th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
                            {isAuthenticated && (
                                <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
                            )}
                        </tr>
                    </thead>
                    <tbody className="bg-white divide-y divide-gray-200">
                        {rooms.map(room => (
                            <tr key={room.id}>
                                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{room.number}</td>
                                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500" title={room.roomTypeId}>
                                    {room.roomTypeId.substring(0, 8)}...
                                </td>
                                <td className="px-6 py-4 whitespace-nowrap text-sm">
                                    <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${getStatusColor(room.status)}`}>
                                        {getStatusText(room.status)}
                                    </span>
                                </td>
                                {isAuthenticated && (
                                    <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                                        <div className="flex justify-end gap-3">
                                            <button onClick={() => handleDeactivate(room.id)} className="text-red-600 hover:text-red-900" title="Deactivate">
                                                <PowerOff size={18} />
                                            </button>
                                        </div>
                                    </td>
                                )}
                            </tr>
                        ))}
                    </tbody>
                    </table>
                </div>
            )}
        </div>
    );
}
