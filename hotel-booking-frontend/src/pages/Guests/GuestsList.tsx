import { useState } from 'react';
import { Link } from 'react-router-dom';
import { guestsService, Guest } from '../../services/guests';
import { Plus, Edit, Search } from 'lucide-react';

export default function GuestsList() {
    const [emailSearch, setEmailSearch] = useState('');
    const [guest, setGuest] = useState<Guest | null>(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const handleSearch = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError('');
        setGuest(null);
        try {
            const data = await guestsService.getByEmail(emailSearch);
            setGuest(data);
        } catch (err) {
            setError('Guest not found with that email.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="bg-white rounded-lg shadow p-6">
            <div className="flex justify-between items-center mb-6">
                <h3 className="text-lg font-medium text-gray-900">Guests</h3>
                <Link to="/guests/new" className="bg-brand-600 text-white px-4 py-2 rounded-md hover:bg-brand-700 flex items-center gap-2">
                    <Plus size={18} /> Add Guest
                </Link>
            </div>

            <form onSubmit={handleSearch} className="flex gap-4 mb-8 max-w-lg">
                <input
                    type="email"
                    required
                    placeholder="Search by email..."
                    value={emailSearch}
                    onChange={e => setEmailSearch(e.target.value)}
                    className="flex-1 px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                />
                <button type="submit" disabled={loading} className="px-4 py-2 bg-gray-800 text-white rounded-md hover:bg-gray-900 flex items-center gap-2">
                    <Search size={18} /> {loading ? 'Searching...' : 'Search'}
                </button>
            </form>

            {error && <div className="text-red-600 bg-red-50 p-4 rounded-md mb-4">{error}</div>}

            {guest && (
                <div className="border border-gray-200 rounded-lg overflow-hidden">
                    <table className="min-w-full divide-y divide-gray-200">
                        <thead className="bg-gray-50">
                            <tr>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Full Name</th>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Email</th>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Phone</th>
                                <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
                            </tr>
                        </thead>
                        <tbody className="bg-white divide-y divide-gray-200">
                            <tr>
                                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{guest.fullName}</td>
                                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{guest.email}</td>
                                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{guest.phoneNumber}</td>
                                <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                                    <div className="flex justify-end gap-3">
                                        <Link to={`/guests/${guest.id}`} className="text-blue-600 hover:text-blue-900">
                                            <Edit size={18} />
                                        </Link>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            )}
        </div>
    );
}
