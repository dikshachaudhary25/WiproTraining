using System.Security.Cryptography;
using System.Text;
using VacationMode.Models;

namespace VacationMode.Data;

public static class DataSeeder
{
    public static void Seed(AppDbContext context)
    {
        SeedUsers(context);
        SeedFeatures(context);
        SeedProperties(context);
        SeedPropertyImages(context);
        SeedPropertyFeatures(context);
        SeedReservations(context);
    }

    private static string HashPassword(string password) =>
        Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));

    

    private static void SeedUsers(AppDbContext context)
    {
        if (context.Users.Any()) return;

        context.Users.AddRange(
            new User { FullName = "Arjun Mehta",   Email = "arjun@vacationmode.com",  PhoneNumber = "9810001001", Role = "Owner",  PasswordHash = HashPassword("Password123") },
            new User { FullName = "Rohan Kapoor",  Email = "rohan@vacationmode.com",  PhoneNumber = "9820002002", Role = "Owner",  PasswordHash = HashPassword("Password123") },
            new User { FullName = "Neha Sharma",   Email = "neha@vacationmode.com",   PhoneNumber = "9830003003", Role = "Owner",  PasswordHash = HashPassword("Password123") },
            new User { FullName = "Aditi Verma",   Email = "aditi@vacationmode.com",  PhoneNumber = "9840004004", Role = "Renter", PasswordHash = HashPassword("Password123") },
            new User { FullName = "Rahul Gupta",   Email = "rahul@vacationmode.com",  PhoneNumber = "9850005005", Role = "Renter", PasswordHash = HashPassword("Password123") },
            new User { FullName = "Sneha Iyer",    Email = "sneha@vacationmode.com",  PhoneNumber = "9860006006", Role = "Renter", PasswordHash = HashPassword("Password123") },
            new User { FullName = "Samriddhi",     Email = "samriddhi@gmail.com",     PhoneNumber = "1234567890", Role = "Owner",  PasswordHash = HashPassword("Samriddhi") },
            new User { FullName = "Suprabha",      Email = "suprabha@gmail.com",      PhoneNumber = "0987654321", Role = "Renter", PasswordHash = HashPassword("Suprabha") }
        );

        context.SaveChanges();
    }

    

    private static void SeedFeatures(AppDbContext context)
    {
        if (context.Features.Any()) return;

        context.Features.AddRange(
            new Feature { FeatureName = "WiFi" },
            new Feature { FeatureName = "Parking" },
            new Feature { FeatureName = "Air Conditioning" },
            new Feature { FeatureName = "Swimming Pool" },
            new Feature { FeatureName = "Mountain View" },
            new Feature { FeatureName = "Sea View" },
            new Feature { FeatureName = "Balcony" },
            new Feature { FeatureName = "Kitchen" },
            new Feature { FeatureName = "Garden" },
            new Feature { FeatureName = "Breakfast Included" },
            new Feature { FeatureName = "Bonfire Area" },
            new Feature { FeatureName = "Private Terrace" }
        );

        context.SaveChanges();
    }

    

    private static void SeedProperties(AppDbContext context)
    {
        if (context.Properties.Any()) return;

        var arjun = context.Users.First(u => u.Email == "arjun@vacationmode.com");
        var rohan = context.Users.First(u => u.Email == "rohan@vacationmode.com");
        var neha  = context.Users.First(u => u.Email == "neha@vacationmode.com");

        context.Properties.AddRange(

            new Property
            {
                OwnerId       = arjun.UserId,
                Title         = "Luxury Beach Villa",
                Description   = "A beautiful beachfront villa located in North Goa, perfect for relaxing holidays with stunning sunset views and easy access to nearby cafes and beaches. The villa features a private pool, a sun deck, and spacious interiors with modern amenities. Ideal for families or groups wanting the full Goa experience.",
                Location      = "Candolim Beach Road",
                City          = "Goa",
                State         = "Goa",
                Country       = "India",
                PropertyType  = "Villa",
                PricePerNight = 12000m,
                MaxGuests     = 10,
                CreatedAt     = DateTime.UtcNow.AddDays(-90)
            },

            new Property
            {
                OwnerId       = rohan.UserId,
                Title         = "Himalayan Mountain Cabin",
                Description   = "Tucked away in the pine forests of Manali, this cosy wooden cabin offers panoramic views of snow-capped Himalayan peaks. Light a bonfire in the evening, sip on hot chai on the private deck, and wake up to crisp mountain air. A dream retreat for nature lovers and trekkers.",
                Location      = "Old Manali Road",
                City          = "Manali",
                State         = "Himachal Pradesh",
                Country       = "India",
                PropertyType  = "Cabin",
                PricePerNight = 5500m,
                MaxGuests     = 6,
                CreatedAt     = DateTime.UtcNow.AddDays(-80)
            },

            new Property
            {
                OwnerId       = neha.UserId,
                Title         = "Lake View Cottage",
                Description   = "A charming whitewashed cottage on the banks of Lake Pichola in Udaipur, with breathtaking views of the City Palace and surrounding Aravalli hills. The cottage blends Rajasthani décor with modern comforts and is just minutes from the old city's markets and temples.",
                Location      = "Gangaur Ghat Road",
                City          = "Udaipur",
                State         = "Rajasthan",
                Country       = "India",
                PropertyType  = "Cottage",
                PricePerNight = 7500m,
                MaxGuests     = 4,
                CreatedAt     = DateTime.UtcNow.AddDays(-75)
            },

            new Property
            {
                OwnerId       = arjun.UserId,
                Title         = "Heritage Haveli Stay",
                Description   = "Step back in time at this lovingly restored 200-year-old haveli in the heart of Jaipur's Pink City. Hand-painted frescoes, arched courtyards, and antique furniture tell the story of Rajput grandeur. Enjoy rooftop breakfasts with views over the old city and the majestic Amer Fort.",
                Location      = "Chandpole Bazaar",
                City          = "Jaipur",
                State         = "Rajasthan",
                Country       = "India",
                PropertyType  = "Haveli",
                PricePerNight = 9000m,
                MaxGuests     = 8,
                CreatedAt     = DateTime.UtcNow.AddDays(-70)
            },

            new Property
            {
                OwnerId       = rohan.UserId,
                Title         = "Modern Sea-View Apartment",
                Description   = "A sleek, fully furnished apartment on the 18th floor in Bandra West, offering sweeping views of the Arabian Sea and the Mumbai skyline. Walking distance from cafes, boutiques, and the iconic Bandstand promenade. Perfect for business travelers or couples wanting a stylish urban stay.",
                Location      = "Hill Road, Bandra West",
                City          = "Mumbai",
                State         = "Maharashtra",
                Country       = "India",
                PropertyType  = "Apartment",
                PricePerNight = 8500m,
                MaxGuests     = 4,
                CreatedAt     = DateTime.UtcNow.AddDays(-65)
            },

            new Property
            {
                OwnerId       = neha.UserId,
                Title         = "Tea Garden Retreat",
                Description   = "Nestled among rolling tea estates in the misty hills of Munnar, this eco-retreat lets you wake up to the scent of fresh tea leaves and cool mountain fog. Guided plantation walks, freshly brewed estate tea, and silence broken only by birdsong make this a true escape from city life.",
                Location      = "Pallivasal Estate Road",
                City          = "Munnar",
                State         = "Kerala",
                Country       = "India",
                PropertyType  = "Resort",
                PricePerNight = 6800m,
                MaxGuests     = 6,
                CreatedAt     = DateTime.UtcNow.AddDays(-60)
            },

            new Property
            {
                OwnerId       = arjun.UserId,
                Title         = "Hilltop Resort",
                Description   = "Perched at 2200 metres above sea level in Shimla, this boutique resort commands sweeping views of the Shivalik ranges and the valley below. Colonial architecture, crackling fireplaces, and personalised service make every stay memorable. Ideal for honeymooners and families escaping the summer heat.",
                Location      = "Jakhu Hill Road",
                City          = "Shimla",
                State         = "Himachal Pradesh",
                Country       = "India",
                PropertyType  = "Resort",
                PricePerNight = 10500m,
                MaxGuests     = 6,
                CreatedAt     = DateTime.UtcNow.AddDays(-55)
            },

            new Property
            {
                OwnerId       = rohan.UserId,
                Title         = "Backwater Villa",
                Description   = "Float into serenity at this traditional Kerala villa built over the quiet backwaters of Alleppey. Watch local fishermen cast their nets at dawn, enjoy a sunset cruise on a private houseboat, and savour authentic Kerala meals cooked fresh each day. An experience that stays with you.",
                Location      = "Punnamada Lake Road",
                City          = "Alleppey",
                State         = "Kerala",
                Country       = "India",
                PropertyType  = "Villa",
                PricePerNight = 11000m,
                MaxGuests     = 8,
                CreatedAt     = DateTime.UtcNow.AddDays(-50)
            },

            new Property
            {
                OwnerId       = neha.UserId,
                Title         = "Riverside Cottage",
                Description   = "A peaceful stone cottage sitting right on the banks of the Ganges in Rishikesh, surrounded by forest and within walking distance of the famous Lakshman Jhula. Meditate on the private riverside deck, take a sunrise yoga class, or join a white-water rafting trip just minutes away.",
                Location      = "Swarg Ashram Road",
                City          = "Rishikesh",
                State         = "Uttarakhand",
                Country       = "India",
                PropertyType  = "Cottage",
                PricePerNight = 4500m,
                MaxGuests     = 4,
                CreatedAt     = DateTime.UtcNow.AddDays(-45)
            },

            new Property
            {
                OwnerId       = arjun.UserId,
                Title         = "Luxury Penthouse",
                Description   = "An expansive sky penthouse on the 32nd floor in Indiranagar, one of Bangalore's most vibrant neighbourhoods. Floor-to-ceiling windows frame the city's glittering skyline, while the private rooftop terrace with a jacuzzi is perfect for unwinding after a long day. Premium furnishings, concierge service, and fast WiFi throughout.",
                Location      = "100 Feet Road, Indiranagar",
                City          = "Bangalore",
                State         = "Karnataka",
                Country       = "India",
                PropertyType  = "Apartment",
                PricePerNight = 18000m,
                MaxGuests     = 6,
                CreatedAt     = DateTime.UtcNow.AddDays(-40)
            }
        );

        context.SaveChanges();
    }

    

    private static void SeedPropertyImages(AppDbContext context)
    {
        if (context.PropertyImages.Any()) return;

        var pm = context.Properties.ToDictionary(p => p.Title);

        var images = new List<PropertyImage>
        {
            
            new PropertyImage { PropertyId = pm["Luxury Beach Villa"].PropertyId,        ImageUrl = "https://images.unsplash.com/photo-1507525428034-b723cf961d3e?w=800&auto=format&fit=crop&q=80",  IsPrimary = true  },
            new PropertyImage { PropertyId = pm["Luxury Beach Villa"].PropertyId,        ImageUrl = "https://images.unsplash.com/photo-1499793983690-e29da59ef1c2?w=800&auto=format&fit=crop&q=80",  IsPrimary = false },

            
            new PropertyImage { PropertyId = pm["Himalayan Mountain Cabin"].PropertyId,  ImageUrl = "https://images.unsplash.com/photo-1510798831971-661eb04b3739?w=800&auto=format&fit=crop&q=80",  IsPrimary = true  },
            new PropertyImage { PropertyId = pm["Himalayan Mountain Cabin"].PropertyId,  ImageUrl = "https://images.unsplash.com/photo-1449158743715-0a90ebb6d2d8?w=800&auto=format&fit=crop&q=80",  IsPrimary = false },

            
            new PropertyImage { PropertyId = pm["Lake View Cottage"].PropertyId,         ImageUrl = "https://images.unsplash.com/photo-1505691723518-36a5ac3b2f72?w=800&auto=format&fit=crop&q=80",  IsPrimary = true  },
            new PropertyImage { PropertyId = pm["Lake View Cottage"].PropertyId,         ImageUrl = "https://images.unsplash.com/photo-1518780664697-55e3ad937233?w=800&auto=format&fit=crop&q=80",  IsPrimary = false },

            
            new PropertyImage { PropertyId = pm["Heritage Haveli Stay"].PropertyId,      ImageUrl = "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?w=800&auto=format&fit=crop&q=80",  IsPrimary = true  },
            new PropertyImage { PropertyId = pm["Heritage Haveli Stay"].PropertyId,      ImageUrl = "https://images.unsplash.com/photo-1613977257363-707ba9348227?w=800&auto=format&fit=crop&q=80",  IsPrimary = false },

            
            new PropertyImage { PropertyId = pm["Modern Sea-View Apartment"].PropertyId, ImageUrl = "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?w=800&auto=format&fit=crop&q=80",  IsPrimary = true  },
            new PropertyImage { PropertyId = pm["Modern Sea-View Apartment"].PropertyId, ImageUrl = "https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?w=800&auto=format&fit=crop&q=80",  IsPrimary = false },

            
            new PropertyImage { PropertyId = pm["Tea Garden Retreat"].PropertyId,        ImageUrl = "https://images.unsplash.com/photo-1507089947368-19c1da9775ae?w=800&auto=format&fit=crop&q=80",  IsPrimary = true  },
            new PropertyImage { PropertyId = pm["Tea Garden Retreat"].PropertyId,        ImageUrl = "https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?w=800&auto=format&fit=crop&q=80",  IsPrimary = false },

            
            new PropertyImage { PropertyId = pm["Hilltop Resort"].PropertyId,            ImageUrl = "https://images.unsplash.com/photo-1571896349842-33c89424de2d?w=800&auto=format&fit=crop&q=80",  IsPrimary = true  },
            new PropertyImage { PropertyId = pm["Hilltop Resort"].PropertyId,            ImageUrl = "https://images.unsplash.com/photo-1553653924-39b70295f8da?w=800&auto=format&fit=crop&q=80",  IsPrimary = false },

            
            new PropertyImage { PropertyId = pm["Backwater Villa"].PropertyId,           ImageUrl = "https://images.unsplash.com/photo-1613490493576-7fde63acd811?w=800&auto=format&fit=crop&q=80",  IsPrimary = true  },
            new PropertyImage { PropertyId = pm["Backwater Villa"].PropertyId,           ImageUrl = "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?w=800&auto=format&fit=crop&q=80",  IsPrimary = false },

            
            new PropertyImage { PropertyId = pm["Riverside Cottage"].PropertyId,         ImageUrl = "https://images.unsplash.com/photo-1500534314209-a25ddb2bd429?w=800&auto=format&fit=crop&q=80",  IsPrimary = true  },
            new PropertyImage { PropertyId = pm["Riverside Cottage"].PropertyId,         ImageUrl = "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85?w=800&auto=format&fit=crop&q=80",  IsPrimary = false },

            
            new PropertyImage { PropertyId = pm["Luxury Penthouse"].PropertyId,          ImageUrl = "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=800&auto=format&fit=crop&q=80",  IsPrimary = true  },
            new PropertyImage { PropertyId = pm["Luxury Penthouse"].PropertyId,          ImageUrl = "https://images.unsplash.com/photo-1484154218962-a197022b5858?w=800&auto=format&fit=crop&q=80",  IsPrimary = false },
        };

        context.PropertyImages.AddRange(images);
        context.SaveChanges();
    }

    

    private static void SeedPropertyFeatures(AppDbContext context)
    {
        if (context.PropertyFeatures.Any()) return;

        var pm = context.Properties.ToDictionary(p => p.Title);
        var fm = context.Features.ToDictionary(f => f.FeatureName);

        var assignments = new Dictionary<string, List<string>>
        {
            ["Luxury Beach Villa"]       = new() { "WiFi", "Sea View", "Swimming Pool", "Balcony", "Parking", "Air Conditioning" },
            ["Himalayan Mountain Cabin"] = new() { "Mountain View", "Bonfire Area", "Kitchen", "Garden", "WiFi" },
            ["Lake View Cottage"]        = new() { "WiFi", "Balcony", "Kitchen", "Breakfast Included", "Parking" },
            ["Heritage Haveli Stay"]     = new() { "WiFi", "Breakfast Included", "Private Terrace", "Parking", "Air Conditioning" },
            ["Modern Sea-View Apartment"] = new() { "WiFi", "Sea View", "Air Conditioning", "Balcony", "Parking" },
            ["Tea Garden Retreat"]       = new() { "WiFi", "Garden", "Breakfast Included", "Mountain View", "Kitchen" },
            ["Hilltop Resort"]           = new() { "WiFi", "Mountain View", "Breakfast Included", "Bonfire Area", "Private Terrace", "Parking" },
            ["Backwater Villa"]          = new() { "WiFi", "Balcony", "Kitchen", "Breakfast Included", "Parking" },
            ["Riverside Cottage"]        = new() { "WiFi", "Garden", "Kitchen", "Bonfire Area", "Private Terrace" },
            ["Luxury Penthouse"]         = new() { "WiFi", "Swimming Pool", "Air Conditioning", "Private Terrace", "Parking", "Balcony" }
        };

        var rows = new List<PropertyFeature>();

        foreach (var (title, features) in assignments)
        {
            if (!pm.TryGetValue(title, out var property)) continue;

            foreach (var name in features)
            {
                if (!fm.TryGetValue(name, out var feature)) continue;

                rows.Add(new PropertyFeature
                {
                    PropertyId = property.PropertyId,
                    FeatureId  = feature.FeatureId
                });
            }
        }

        context.PropertyFeatures.AddRange(rows);
        context.SaveChanges();
    }

    

    private static void SeedReservations(AppDbContext context)
    {
        if (context.Reservations.Any()) return;

        var aditi  = context.Users.First(u => u.Email == "aditi@vacationmode.com");
        var rahul  = context.Users.First(u => u.Email == "rahul@vacationmode.com");
        var sneha  = context.Users.First(u => u.Email == "sneha@vacationmode.com");

        var pm = context.Properties.ToDictionary(p => p.Title);

        var goa      = pm["Luxury Beach Villa"];
        var manali   = pm["Himalayan Mountain Cabin"];
        var udaipur  = pm["Lake View Cottage"];
        var mumbai   = pm["Modern Sea-View Apartment"];
        var shimla   = pm["Hilltop Resort"];

        context.Reservations.AddRange(

            new Reservation
            {
                UserId            = aditi.UserId,
                PropertyId        = goa.PropertyId,
                CheckInDate       = new DateTime(2026, 6, 15, 0, 0, 0, DateTimeKind.Utc),
                CheckOutDate      = new DateTime(2026, 6, 20, 0, 0, 0, DateTimeKind.Utc),
                TotalAmount       = goa.PricePerNight * 5,
                ReservationStatus = "Confirmed",
                CreatedAt         = DateTime.UtcNow.AddDays(-20)
            },

            new Reservation
            {
                UserId            = rahul.UserId,
                PropertyId        = manali.PropertyId,
                CheckInDate       = new DateTime(2026, 7, 2, 0, 0, 0, DateTimeKind.Utc),
                CheckOutDate      = new DateTime(2026, 7, 5, 0, 0, 0, DateTimeKind.Utc),
                TotalAmount       = manali.PricePerNight * 3,
                ReservationStatus = "Pending",
                CreatedAt         = DateTime.UtcNow.AddDays(-10)
            },

            new Reservation
            {
                UserId            = sneha.UserId,
                PropertyId        = udaipur.PropertyId,
                CheckInDate       = new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc),
                CheckOutDate      = new DateTime(2026, 8, 14, 0, 0, 0, DateTimeKind.Utc),
                TotalAmount       = udaipur.PricePerNight * 4,
                ReservationStatus = "Confirmed",
                CreatedAt         = DateTime.UtcNow.AddDays(-15)
            },

            new Reservation
            {
                UserId            = aditi.UserId,
                PropertyId        = mumbai.PropertyId,
                CheckInDate       = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                CheckOutDate      = new DateTime(2026, 5, 3, 0, 0, 0, DateTimeKind.Utc),
                TotalAmount       = mumbai.PricePerNight * 2,
                ReservationStatus = "Cancelled",
                CreatedAt         = DateTime.UtcNow.AddDays(-30)
            },

            new Reservation
            {
                UserId            = rahul.UserId,
                PropertyId        = shimla.PropertyId,
                CheckInDate       = new DateTime(2026, 9, 5, 0, 0, 0, DateTimeKind.Utc),
                CheckOutDate      = new DateTime(2026, 9, 9, 0, 0, 0, DateTimeKind.Utc),
                TotalAmount       = shimla.PricePerNight * 4,
                ReservationStatus = "Pending",
                CreatedAt         = DateTime.UtcNow.AddDays(-5)
            }
        );

        context.SaveChanges();
    }
}
