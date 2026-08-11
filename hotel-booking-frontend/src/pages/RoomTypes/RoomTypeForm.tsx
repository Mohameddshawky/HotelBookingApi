import { useEffect, useState } from 'react';
import { useNavigate, useParams, Link } from 'react-router-dom';
import { roomTypesService } from '../../services/roomTypes';
import { amenitiesService, Amenity } from '../../services/amenities';

export default function RoomTypeForm() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [name, setName] = useState('');
    const [pricePerNight, setPricePerNight] = useState(0);
    const [maxGuests, setMaxGuests] = useState(1);
    const [amenityIds, setAmenityIds] = useState<string[]>([]);
    
    const [availableAmenities, setAvailableAmenities] = useState<Amenity[]>([]);
    const [loading, setLoading] = useState(false);
    const isEditing = !!id && id !== 'new';

    useEffect(() => {
        const loadInitialData = async () => {
            try {
                const amenitiesData = await amenitiesService.getAll();
                setAvailableAmenities(amenitiesData);
                
                if (isEditing) {
                    const data = await roomTypesService.getById(id!);
                    setName(data.name);
                    setPricePerNight(data.pricePerNight);
                    setMaxGuests(data.maxGuests);
                    // The backend GetById might return amenities if configured, assuming it doesn't map them back easily.
                    // For simplicity, we just leave it empty if not provided by backend Dto
                }
            } catch (error) {
                console.error('Failed to load data', error);
            }
        };
        loadInitialData();
    }, [id, isEditing]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        try {
            const payload = { name, pricePerNight, maxGuests, amenityIds };
            if (isEditing) {
                await roomTypesService.update(id!, payload);
            } else {
                await roomTypesService.create(payload);
            }
            navigate('/room-types');
        } catch (error) {
            console.error('Failed to save', error);
            alert('Failed to save room type');
        } finally {
            setLoading(false);
        }
    };

    const toggleAmenity = (amenityId: string) => {
        setAmenityIds(prev => 
            prev.includes(amenityId) 
                ? prev.filter(a => a !== amenityId)
                : [...prev, amenityId]
        );
    };

    return (
        <div className="max-w-2xl mx-auto bg-white rounded-lg shadow p-6">
            <h3 className="text-lg font-medium text-gray-900 mb-6">
                {isEditing ? 'Edit Room Type' : 'New Room Type'}
            </h3>
            <form onSubmit={handleSubmit} className="space-y-6">
                <div>
                    <label className="block text-sm font-medium text-gray-700">Name</label>
                    <input
                        type="text"
                        required
                        value={name}
                        onChange={e => setName(e.target.value)}
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                    />
                </div>
                <div className="grid grid-cols-2 gap-4">
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Price Per Night</label>
                        <input
                            type="number"
                            min="0"
                            step="0.01"
                            required
                            value={pricePerNight}
                            onChange={e => setPricePerNight(parseFloat(e.target.value))}
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                        />
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Max Guests</label>
                        <input
                            type="number"
                            min="1"
                            required
                            value={maxGuests}
                            onChange={e => setMaxGuests(parseInt(e.target.value))}
                            className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                        />
                    </div>
                </div>
                
                <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">Amenities</label>
                    <div className="grid grid-cols-2 gap-2">
                        {availableAmenities.map(amenity => (
                            <label key={amenity.id} className="flex items-center space-x-2">
                                <input
                                    type="checkbox"
                                    checked={amenityIds.includes(amenity.id)}
                                    onChange={() => toggleAmenity(amenity.id)}
                                    className="rounded border-gray-300 text-brand-600 focus:ring-brand-500"
                                />
                                <span className="text-sm text-gray-700">{amenity.name}</span>
                            </label>
                        ))}
                    </div>
                </div>

                <div className="flex justify-end gap-3 pt-4 border-t border-gray-200">
                    <Link to="/room-types" className="px-4 py-2 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 hover:bg-gray-50">
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
