import { useState } from 'react';
import { Link } from 'react-router-dom';
import { reviewsService, Review } from '../../services/reviews';
import { Plus, Search, Star } from 'lucide-react';

export default function ReviewsList() {
    const [bookingIdSearch, setBookingIdSearch] = useState('');
    const [review, setReview] = useState<Review | null>(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const handleSearch = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError('');
        setReview(null);
        try {
            const data = await reviewsService.getByBookingId(bookingIdSearch);
            setReview(data);
        } catch (err) {
            setError('Review not found for that booking.');
        } finally {
            setLoading(false);
        }
    };

    const renderStars = (rating: number) => {
        return Array.from({ length: 5 }).map((_, i) => (
            <Star key={i} size={16} className={i < rating ? 'text-yellow-400 fill-current' : 'text-gray-300'} />
        ));
    };

    return (
        <div className="bg-white rounded-lg shadow p-6">
            <div className="flex justify-between items-center mb-6">
                <h3 className="text-lg font-medium text-gray-900">Reviews</h3>
                <Link to="/reviews/new" className="bg-brand-600 text-white px-4 py-2 rounded-md hover:bg-brand-700 flex items-center gap-2">
                    <Plus size={18} /> Add Review
                </Link>
            </div>

            <form onSubmit={handleSearch} className="flex gap-4 mb-8 max-w-lg">
                <input
                    type="text"
                    required
                    placeholder="Search by Booking ID..."
                    value={bookingIdSearch}
                    onChange={e => setBookingIdSearch(e.target.value)}
                    className="flex-1 px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-brand-500 focus:border-brand-500"
                />
                <button type="submit" disabled={loading} className="px-4 py-2 bg-gray-800 text-white rounded-md hover:bg-gray-900 flex items-center gap-2">
                    <Search size={18} /> {loading ? 'Searching...' : 'Search'}
                </button>
            </form>

            {error && <div className="text-red-600 bg-red-50 p-4 rounded-md mb-4">{error}</div>}

            {review && (
                <div className="border border-gray-200 rounded-lg overflow-hidden p-6">
                    <div className="flex items-center gap-1 mb-4">
                        {renderStars(review.rating)}
                        <span className="ml-2 font-medium text-gray-700">{review.rating} / 5</span>
                    </div>
                    <p className="text-gray-600 mt-2">{review.comment}</p>
                    <div className="mt-4 pt-4 border-t border-gray-100 text-sm text-gray-500">
                        Booking ID: {review.bookingId}
                    </div>
                </div>
            )}
        </div>
    );
}
