import { useEffect, useState } from 'react';
import { useNavigate, useParams, Link } from 'react-router-dom';
import { amenitiesService } from '../../services/amenities';

export default function AmenityForm() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [loading, setLoading] = useState(false);
    const isEditing = !!id && id !== 'new';

    useEffect(() => {
        if (isEditing) {
            const fetchAmenity = async () => {
                try {
                    const data = await amenitiesService.getById(id!);
                    setName(data.name);
                    setDescription(data.description);
                } catch (error) {
                    console.error('Failed to load amenity', error);
                }
            };
            fetchAmenity();
        }
    }, [id, isEditing]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        try {
            if (isEditing) {
                await amenitiesService.update(id!, { name, description });
            } else {
                await amenitiesService.create({ name, description });
            }
            navigate('/amenities');
        } catch (error) {
            console.error('Failed to save', error);
            alert('Failed to save amenity');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="max-w-2xl mx-auto bg-white rounded-lg shadow p-6">
            <h3 className="text-lg font-medium text-gray-900 mb-6">
                {isEditing ? 'Edit Amenity' : 'New Amenity'}
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
                <div>
                    <label className="block text-sm font-medium text-gray-700">Description</label>
                    <textarea
                        rows={3}
                        value={description}
                        onChange={e => setDescription(e.target.value)}
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                    />
                </div>
                <div className="flex justify-end gap-3">
                    <Link to="/amenities" className="px-4 py-2 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 hover:bg-gray-50">
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
