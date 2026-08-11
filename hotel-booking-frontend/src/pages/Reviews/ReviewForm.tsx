import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { reviewsService } from '../../services/reviews';
import { Star } from 'lucide-react';

export default function ReviewForm() {
    const navigate = useNavigate();
    const [bookingId, setBookingId] = useState('');
    const [rating, setRating] = useState(5);
    const [comment, setComment] = useState('');
    const [loading, setLoading] = useState(false);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        try {
            await reviewsService.create({ bookingId, rating, comment });
            navigate('/reviews');
        } catch (error) {
            console.error('Failed to save', error);
            alert('Failed to save review');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="max-w-2xl mx-auto bg-white rounded-lg shadow p-6">
            <h3 className="text-lg font-medium text-gray-900 mb-6">New Review</h3>
            <form onSubmit={handleSubmit} className="space-y-6">
                <div>
                    <label className="block text-sm font-medium text-gray-700">Booking ID</label>
                    <input
                        type="text"
                        required
                        value={bookingId}
                        onChange={e => setBookingId(e.target.value)}
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                    />
                </div>
                
                <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">Rating</label>
                    <div className="flex gap-2">
                        {[1, 2, 3, 4, 5].map(star => (
                            <button
                                key={star}
                                type="button"
                                onClick={() => setRating(star)}
                                className="focus:outline-none"
                            >
                                <Star size={24} className={star <= rating ? 'text-yellow-400 fill-current' : 'text-gray-300'} />
                            </button>
                        ))}
                    </div>
                </div>

                <div>
                    <label className="block text-sm font-medium text-gray-700">Comment</label>
                    <textarea
                        rows={4}
                        required
                        value={comment}
                        onChange={e => setComment(e.target.value)}
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                    />
                </div>

                <div className="flex justify-end gap-3 pt-4 border-t border-gray-200">
                    <Link to="/reviews" className="px-4 py-2 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 hover:bg-gray-50">
                        Cancel
                    </Link>
                    <button type="submit" disabled={loading} className="px-4 py-2 bg-brand-600 text-white rounded-md hover:bg-brand-700">
                        {loading ? 'Saving...' : 'Submit Review'}
                    </button>
                </div>
            </form>
        </div>
    );
}
