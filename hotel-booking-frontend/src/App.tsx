import { BrowserRouter, Routes, Route } from 'react-router-dom'
import { AuthProvider } from './contexts/AuthContext'
import Login from './pages/Login'
import Register from './pages/Register'
import Layout from './components/Layout'
import ProtectedRoute from './components/ProtectedRoute'
import AmenitiesList from './pages/Amenities/AmenitiesList'
import AmenityForm from './pages/Amenities/AmenityForm'
import RoomTypesList from './pages/RoomTypes/RoomTypesList'
import RoomTypeForm from './pages/RoomTypes/RoomTypeForm'
import RoomsList from './pages/Rooms/RoomsList'
import RoomForm from './pages/Rooms/RoomForm'
import GuestsList from './pages/Guests/GuestsList'
import GuestForm from './pages/Guests/GuestForm'
import BookingsList from './pages/Bookings/BookingsList'
import BookingForm from './pages/Bookings/BookingForm'
import ReviewsList from './pages/Reviews/ReviewsList'
import ReviewForm from './pages/Reviews/ReviewForm'
import Dashboard from './pages/Reports/Dashboard'

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          
          <Route element={<ProtectedRoute />}>
            <Route path="/" element={<Layout />}>
              <Route index element={<Dashboard />} />
              <Route path="amenities" element={<AmenitiesList />} />
              <Route path="amenities/:id" element={<AmenityForm />} />
              <Route path="room-types" element={<RoomTypesList />} />
              <Route path="room-types/:id" element={<RoomTypeForm />} />
              <Route path="rooms" element={<RoomsList />} />
              <Route path="rooms/new" element={<RoomForm />} />
              <Route path="guests" element={<GuestsList />} />
              <Route path="guests/:id" element={<GuestForm />} />
              <Route path="bookings" element={<BookingsList />} />
              <Route path="bookings/new" element={<BookingForm />} />
              <Route path="reviews" element={<ReviewsList />} />
              <Route path="reviews/new" element={<ReviewForm />} />
              <Route path="reports" element={<Dashboard />} />
              {/* Other CRUD routes will go here */}
            </Route>
          </Route>
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  )
}

export default App
