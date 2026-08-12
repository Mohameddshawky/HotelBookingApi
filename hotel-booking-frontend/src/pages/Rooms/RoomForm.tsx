import { useEffect, useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { roomsService } from '../../services/rooms';
import { roomTypesService, RoomType } from '../../services/roomTypes';

export default function RoomForm() {
    const navigate = useNavigate();
    const [number, setNumber] = useState('');
    const [roomTypeId, setRoomTypeId] = useState('');
    
    const [availableRoomTypes, setAvailableRoomTypes] = useState<RoomType[]>([]);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        const loadInitialData = async () => {
            try {
                const typesData = await roomTypesService.getAll();
                setAvailableRoomTypes(typesData);
                if (typesData.length > 0) {
                    setRoomTypeId(typesData[0].id);
                }
            } catch (error) {
                console.error('Failed to load room types', error);
            }
        };
        loadInitialData();
    }, []);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        try {
            await roomsService.create({ number, roomTypeId });
            navigate('/');
        } catch (error) {
            console.error('Failed to save', error);
            alert('Failed to save room');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="max-w-2xl mx-auto bg-white rounded-lg shadow p-6">
            <h3 className="text-lg font-medium text-gray-900 mb-6">New Room</h3>
            <form onSubmit={handleSubmit} className="space-y-6">
                <div>
                    <label className="block text-sm font-medium text-gray-700">Room Number</label>
                    <input
                        type="text"
                        required
                        value={number}
                        onChange={e => setNumber(e.target.value)}
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                    />
                </div>
                
                <div>
                    <label className="block text-sm font-medium text-gray-700">Room Type</label>
                    <select
                        required
                        value={roomTypeId}
                        onChange={e => setRoomTypeId(e.target.value)}
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                    >
                        {availableRoomTypes.map(rt => (
                            <option key={rt.id} value={rt.id}>
                                {rt.name} (${rt.pricePerNight}/night)
                            </option>
                        ))}
                    </select>
                </div>

                <div className="flex justify-end gap-3 pt-4 border-t border-gray-200">
                    <Link to="/" className="px-4 py-2 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 hover:bg-gray-50">
                        Cancel
                    </Link>
                    <button type="submit" disabled={loading} className="px-4 py-2 bg-brand-600 text-white rounded-md hover:bg-brand-700">
                        {loading ? 'Saving...' : 'Save'}
                    </button>
                </div>
            </form>
        </div>
    );
}
