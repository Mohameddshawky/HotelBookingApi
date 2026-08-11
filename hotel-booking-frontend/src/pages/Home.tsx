import { Link } from 'react-router-dom';

export default function Home() {
  return (
    <div className="home">
      <div className="hero">
        <h2>Experience Ultimate Luxury</h2>
        <p>Book your perfect stay with us today.</p>
        <Link to="/rooms" className="cta-button">View Rooms</Link>
      </div>
      
      <section className="features">
        <h3 className="section-title">Why Choose Us?</h3>
        <div className="feature-grid">
          <div className="card">
            <h4>Premium Comfort</h4>
            <p>Our rooms are designed to provide the highest level of comfort and relaxation.</p>
          </div>
          <div className="card">
            <h4>World-Class Dining</h4>
            <p>Experience culinary masterpieces at our award-winning restaurants.</p>
          </div>
          <div className="card">
            <h4>Perfect Location</h4>
            <p>Situated in the heart of the city, with easy access to all major attractions.</p>
          </div>
        </div>
      </section>
    </div>
  );
}
