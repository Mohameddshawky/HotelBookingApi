import { useEffect, useState } from 'react';
import { reportsService, OccupancyReport, RoomTypeRatingReport } from '../../services/reports';
import { BarChart3, Users, Home, TrendingUp, Star } from 'lucide-react';

export default function Dashboard() {
    const [occupancy, setOccupancy] = useState<OccupancyReport | null>(null);
    const [ratings, setRatings] = useState<RoomTypeRatingReport[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const loadReports = async () => {
            try {
                const occ = await reportsService.getOccupancy();
                setOccupancy(occ);
                
                const rat = await reportsService.getRoomTypeRatings();
                setRatings(rat);
            } catch (error) {
                console.error('Failed to load reports', error);
            } finally {
                setLoading(false);
            }
        };
        loadReports();
    }, []);

    if (loading) return <div className="text-center py-10">Loading reports...</div>;

    return (
        <div className="space-y-6">
            <h3 className="text-xl font-semibold text-gray-900">Hotel Dashboard</h3>
            
            {/* KPI Cards */}
            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-6 flex items-center">
                    <div className="p-3 bg-blue-50 text-blue-600 rounded-lg">
                        <TrendingUp size={24} />
                    </div>
                    <div className="ml-4">
                        <p className="text-sm font-medium text-gray-500">Occupancy Rate</p>
                        <p className="text-2xl font-semibold text-gray-900">{occupancy?.occupancyPercentage.toFixed(1)}%</p>
                    </div>
                </div>
                <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-6 flex items-center">
                    <div className="p-3 bg-brand-50 text-brand-600 rounded-lg">
                        <Users size={24} />
                    </div>
                    <div className="ml-4">
                        <p className="text-sm font-medium text-gray-500">Rooms Occupied</p>
                        <p className="text-2xl font-semibold text-gray-900">{occupancy?.occupiedRooms}</p>
                    </div>
                </div>
                <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-6 flex items-center">
                    <div className="p-3 bg-purple-50 text-purple-600 rounded-lg">
                        <Home size={24} />
                    </div>
                    <div className="ml-4">
                        <p className="text-sm font-medium text-gray-500">Total Rooms</p>
                        <p className="text-2xl font-semibold text-gray-900">{occupancy?.totalRooms}</p>
                    </div>
                </div>
            </div>

            {/* Room Type Ratings */}
            <div className="bg-white rounded-xl shadow-sm border border-gray-100">
                <div className="px-6 py-4 border-b border-gray-100">
                    <h4 className="text-lg font-medium text-gray-900 flex items-center gap-2">
                        <BarChart3 size={20} className="text-brand-500" />
                        Room Type Ratings
                    </h4>
                </div>
                <div className="p-6">
                    <div className="space-y-4">
                        {ratings.map(rating => (
                            <div key={rating.roomTypeId} className="flex items-center justify-between">
                                <span className="font-medium text-gray-700">{rating.roomTypeName}</span>
                                <div className="flex items-center gap-2">
                                    <div className="w-48 bg-gray-200 rounded-full h-2.5">
                                        <div 
                                            className="bg-yellow-400 h-2.5 rounded-full" 
                                            style={{ width: `${(rating.averageRating / 5) * 100}%` }}
                                        ></div>
                                    </div>
                                    <span className="flex items-center gap-1 text-sm font-medium text-gray-600 w-12 justify-end">
                                        {rating.averageRating.toFixed(1)} <Star size={14} className="text-yellow-400 fill-current" />
                                    </span>
                                </div>
                            </div>
                        ))}
                        {ratings.length === 0 && (
                            <p className="text-gray-500 text-center py-4">No rating data available.</p>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
}
