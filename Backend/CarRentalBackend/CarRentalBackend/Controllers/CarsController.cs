using BusinessLogicLayer.Models;
using DataAccessLayer.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text;

namespace CarRentalBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class CarsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("allcars")]
        public ActionResult<IEnumerable<Car>>GetCars()
        {
            return _context.Cars.ToList();
        }

        //[Authorize]
        [HttpGet("availablecars")]
        public ActionResult<IEnumerable<Car>> GetAvailableCars()
        {
            return _context.Cars.Where(car => car.IsAvailable).ToList();
        }
        [HttpGet("search")]
        public ActionResult<IEnumerable<Car>> SearchCars([FromQuery] CarSearchCriteria criteria)
        {
            var filteredCars = _context.Cars
                .Where(car =>
                    (string.IsNullOrEmpty(criteria.Maker) || car.Maker == criteria.Maker) &&
                    (string.IsNullOrEmpty(criteria.Model) || car.Model == criteria.Model))
                .ToList();

            return filteredCars;
        }


        [HttpGet("{id}")]
        public ActionResult<Car> GetCar(int id)
        {
           var car = _context.Cars.Find(id);

          if (car == null)
            {
                return NotFound();
            }

            return car;
        }

        [HttpPost]
        public ActionResult<Car> PostCar(Car car)
        {
            _context.Cars.Add(car);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
        }

        [HttpPut("{id}")]
        public IActionResult PutCar(int id, Car car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }

            _context.Entry(car).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCar(int id)
        {
            var car = _context.Cars.Find(id);

            if (car == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(car);
            _context.SaveChanges();

            return NoContent();
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }

        //[HttpPost("userlogin")]
        //public ActionResult<bool> UserLogin([FromBody] User request)
        //{
        //    bool userExists = _context.Users.Any(u => u.email == request.email && u.Password == request.Password);
        //   if (userExists == false)
        //    {
        //       return Unauthorized(new { message = "Invalid email or password" });
        //   }


        //  return Ok(userExists);
        //}

        [HttpPost("userlogin")]
        public ActionResult<UserLoginResponse> UserLogin([FromBody] User request)
        {
            bool userExists = _context.Users.Any(u => u.email == request.email && u.Password == request.Password);
            if (userExists == false)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("chaitanyavasuis@goodboy");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                 new Claim("email", request.email) // You can add more claims as needed
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Create a custom response object including the token
            var response = new UserLoginResponse
            {
                Token = tokenString,
                UserExists = userExists // Assuming UserLoginResponse has a property for UserExists
            };

            return Ok(response);
            //return Ok(new { Token = tokenString });
        }




        [HttpPost("adminlogin")]
        public ActionResult<bool> AdminLogin([FromBody] Admin request)
        {
            bool userExists = _context.Admins.Any(u => u.email == request.email && u.Password == request.Password);
            if (userExists == false)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }
            return Ok(userExists);
            
        }
        [HttpPost("bookings")]
        public ActionResult<Booking> PostBooking([FromBody] BookingRequest bookingRequest)
        {
            // Create a new Booking entity
            var booking = new Booking
            {
                UserEmail = bookingRequest.UserEmail,
                CarId = bookingRequest.CarId,
                RentalTime = bookingRequest.RentalTime,
                IsReturning = false,
                Returned=false
            };
            // Check if car is available
            var car = _context.Cars.Find(booking.CarId);
            if (car == null || !car.IsAvailable)
            {
                return BadRequest(new { message = "Car is not available for booking." });
            }

            booking.RentalTime = DateTime.Now;
            _context.Bookings.Add(booking);

            // Update car availability
            car.IsAvailable = false;

            _context.SaveChanges();

            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
        }


        [HttpGet("booking/{id}")]
        public ActionResult<Booking> GetBooking(int id)
        {
           var booking = _context.Bookings.Find(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }
        [HttpGet("userbookings/{userEmail}")]
        public ActionResult<IEnumerable<Booking>> GetUserBookings(string userEmail)
        {
            var bookings = _context.Bookings.Where(b => b.UserEmail == userEmail).ToList();
            return bookings;
        }
        [HttpPost("addNotification")]
        public IActionResult AddNotification([FromBody] NotificationRequest notificationRequest)
        {
            try
            {
                // Map the NotificationRequest to a Notification entity
                var notification = new Notification
                {
                    Sender = notificationRequest.Sender,
                    Receiver = notificationRequest.Receiver,
                    Message = notificationRequest.Message,
                    IsRead = notificationRequest.IsRead,
                    CarId = notificationRequest.CarId
                };

                // Add the notification to the database
                _context.Notifications.Add(notification);
                _context.SaveChanges();

                return Ok(new { Message = "Notification added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while adding the notification", ex.Message });
            }
        }
        [HttpGet("unreadnotifications")]
        public ActionResult<IEnumerable<Notification>> GetUnreadNotifications()
        {
            var unreadNotifications = _context.Notifications.Where(n => !n.IsRead).ToList();
            return unreadNotifications;
        }
        [HttpPost("markasread/{notificationId}")]
        public IActionResult MarkNotificationAsRead(int notificationId)
        {
            var notification = _context.Notifications.Find(notificationId);

            if (notification == null)
            {
                return NotFound();
            }

            notification.IsRead = true;
            _context.SaveChanges();

            // Update car availability status
            var car = _context.Cars.Find(notification.CarId);
            if (car != null)
            {
                car.IsAvailable = true; // Assuming you have an IsAvailable property in your Car model
                _context.SaveChanges();
            }

            // Update booking status
            var booking = _context.Bookings.FirstOrDefault(b => b.CarId == notification.CarId && !b.Returned);
            if (booking != null)
            {
                booking.IsReturning = false;
                booking.Returned = true;
                _context.SaveChanges();
            }

            return Ok();
        }

        [HttpPost("returningcar/{bookingId}")]
        public IActionResult ReturningCar(int bookingId)
        {
            var booking = _context.Bookings.Find(bookingId);
            if (booking == null)
            {
                return NotFound();
            }

            booking.IsReturning = true;
            _context.SaveChanges();
            return Ok();
        }

    }
}
       
    

