import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { roomsService, Room, PagedResult } from '../../services/rooms';
import { Plus, PowerOff } from 'lucide-react';

export default function RoomsList() {
    const [rooms, setRooms] = useState<Room[]>([]);
    const [loading, setLoading] = useState(true);

    const loadRooms = async () => {
        try {
            const data: PagedResult<Room> = await roomsService.getAll();
            setRooms(data.items || []);
        } catch (error) {
            console.error('Failed to load rooms', error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadRooms();
    }, []);

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

    const getStatusText = (status: number) => {
        switch (status) {
            case 0: return 'Available';
            case 1: return 'Booked';
            case 2: return 'Maintenance';
            default: return 'Unknown';
        }
    };

    const getStatusColor = (status: number) => {
        switch (status) {
            case 0: return 'bg-green-100 text-green-800';
            case 1: return 'bg-blue-100 text-blue-800';
            case 2: return 'bg-red-100 text-red-800';
            default: return 'bg-gray-100 text-gray-800';
        }
    };

    if (loading) return <div className="text-center py-10">Loading...</div>;

    return (
        <div className="bg-white rounded-lg shadow">
            <div className="px-6 py-4 border-b border-gray-200 flex justify-between items-center">
                <h3 className="text-lg font-medium text-gray-900">Rooms</h3>
                <Link to="/rooms/new" className="bg-brand-600 text-white px-4 py-2 rounded-md hover:bg-brand-700 flex items-center gap-2">
                    <Plus size={18} /> Add Room
                </Link>
            </div>
            <div className="overflow-x-auto">
                <table className="min-w-full divide-y divide-gray-200">
                    <thead className="bg-gray-50">
                        <tr>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Number</th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Room Type ID</th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
                            <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
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
                                <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                                    <div className="flex justify-end gap-3">
                                        <button onClick={() => handleDeactivate(room.id)} className="text-red-600 hover:text-red-900" title="Deactivate">
                                            <PowerOff size={18} />
                                        </button>
                                    </div>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}
