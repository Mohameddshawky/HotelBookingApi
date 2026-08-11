import { Link, Outlet, useLocation, useNavigate } from 'react-router-dom';
import { 
  Building2, 
  LogOut, 
  LayoutDashboard, 
  BedDouble, 
  Users, 
  CalendarRange, 
  Star, 
  Coffee, 
  BarChart 
} from 'lucide-react';
import { useAuth } from '../contexts/AuthContext';

export default function Layout() {
  const { logout } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  const navItems = [
    { name: 'Dashboard', path: '/', icon: LayoutDashboard },
    { name: 'Rooms', path: '/rooms', icon: BedDouble },
    { name: 'Room Types', path: '/room-types', icon: BedDouble },
    { name: 'Amenities', path: '/amenities', icon: Coffee },
    { name: 'Guests', path: '/guests', icon: Users },
    { name: 'Bookings', path: '/bookings', icon: CalendarRange },
    { name: 'Reviews', path: '/reviews', icon: Star },
    { name: 'Reports', path: '/reports', icon: BarChart },
  ];

  return (
    <div className="min-h-screen bg-gray-100 flex">
      {/* Sidebar */}
      <aside className="w-64 bg-brand-900 text-white flex flex-col">
        <div className="p-6 flex items-center gap-3 border-b border-brand-800">
          <Building2 size={32} className="text-brand-500" />
          <h1 className="text-xl font-bold tracking-wider">Grand Horizon</h1>
        </div>
        <nav className="flex-1 px-4 py-6 space-y-2 overflow-y-auto">
          {navItems.map((item) => {
            const Icon = item.icon;
            const isActive = location.pathname === item.path || (item.path !== '/' && location.pathname.startsWith(item.path));
            return (
              <Link
                key={item.name}
                to={item.path}
                className={`flex items-center gap-3 px-4 py-3 rounded-lg transition-colors ${
                  isActive 
                    ? 'bg-brand-800 text-white' 
                    : 'text-brand-100 hover:bg-brand-800 hover:text-white'
                }`}
              >
                <Icon size={20} />
                <span className="font-medium">{item.name}</span>
              </Link>
            );
          })}
        </nav>
        <div className="p-4 border-t border-brand-800">
          <button
            onClick={handleLogout}
            className="flex items-center gap-3 px-4 py-3 w-full text-left text-brand-100 hover:bg-brand-800 hover:text-white rounded-lg transition-colors"
          >
            <LogOut size={20} />
            <span className="font-medium">Logout</span>
          </button>
        </div>
      </aside>

      {/* Main Content */}
      <main className="flex-1 flex flex-col overflow-hidden">
        <header className="bg-white shadow-sm h-16 flex items-center px-8">
          <h2 className="text-xl font-semibold text-gray-800">
            {navItems.find(item => location.pathname === item.path || (item.path !== '/' && location.pathname.startsWith(item.path)))?.name || 'Dashboard'}
          </h2>
        </header>
        <div className="flex-1 overflow-auto p-8">
          <Outlet />
        </div>
      </main>
    </div>
  );
}
