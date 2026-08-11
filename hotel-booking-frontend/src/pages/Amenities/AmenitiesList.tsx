import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { amenitiesService, Amenity } from '../../services/amenities';
import { Plus, Edit, Trash2 } from 'lucide-react';

export default function AmenitiesList() {
    const [amenities, setAmenities] = useState<Amenity[]>([]);
    const [loading, setLoading] = useState(true);

    const loadAmenities = async () => {
        try {
            const data = await amenitiesService.getAll();
            setAmenities(data);
        } catch (error) {
            console.error('Failed to load amenities', error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadAmenities();
    }, []);

    const handleDelete = async (id: string) => {
        if (window.confirm('Are you sure you want to delete this amenity?')) {
            try {
                await amenitiesService.delete(id);
                setAmenities(amenities.filter(a => a.id !== id));
            } catch (error) {
                console.error('Failed to delete', error);
            }
        }
    };

    if (loading) return <div className="text-center py-10">Loading...</div>;

    return (
        <div className="bg-white rounded-lg shadow">
            <div className="px-6 py-4 border-b border-gray-200 flex justify-between items-center">
                <h3 className="text-lg font-medium text-gray-900">All Amenities</h3>
                <Link to="/amenities/new" className="bg-brand-600 text-white px-4 py-2 rounded-md hover:bg-brand-700 flex items-center gap-2">
                    <Plus size={18} /> Add Amenity
                </Link>
            </div>
            <div className="overflow-x-auto">
                <table className="min-w-full divide-y divide-gray-200">
                    <thead className="bg-gray-50">
                        <tr>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Name</th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Description</th>
                            <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
                        </tr>
                    </thead>
                    <tbody className="bg-white divide-y divide-gray-200">
                        {amenities.map(amenity => (
                            <tr key={amenity.id}>
                                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{amenity.name}</td>
                                <td className="px-6 py-4 text-sm text-gray-500">{amenity.description}</td>
                                <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                                    <div className="flex justify-end gap-3">
                                        <Link to={`/amenities/${amenity.id}`} className="text-blue-600 hover:text-blue-900">
                                            <Edit size={18} />
                                        </Link>
                                        <button onClick={() => handleDelete(amenity.id)} className="text-red-600 hover:text-red-900">
                                            <Trash2 size={18} />
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
