using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using Restaurant_Booking.Data;
using Restaurant_Booking.DTO;
using Restaurant_Booking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant_Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly Restaurant_BookingDbContext _context;

        public ReservationController(Restaurant_BookingDbContext context)
        {
            _context = context;
        }

        //GET: api/Reservation
       [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            return await _context.Reservation.ToListAsync();
        }

        //// GET: api/Reservation/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Reservation>> GetReservation(int id)
        //{
        //    var reservation = await _context.Reservation.FindAsync(id);

        //    if (reservation == null)
        //    {
        //        return NotFound();
        //    }

        //    return reservation;
        //}



        //// POST: api/Reservation
        //[HttpPost]
        //public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
        //{
        //    //var table = _context.Restaurant.Find(reservation.NoOfTables);
        //    //table.TotalTables = table.TotalTables - reservation.NoOfTables;
        //    //_context.Restaurant.Update(table);

        //    _context.Reservation.Add(reservation);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetReservation), new { id = reservation.ReqId }, reservation);
        //}


        //[HttpPost]
        //public ActionResult<Reservation> BookReservation([FromBody] Booking Booking)
        //{
        //    var Table = _context.Restaurant.Find(Booking.Restaurant_Id);
        //    Table.TotalTables = Table.TotalTables - Booking.NoOfTables;
        //    _context.Restaurant.Update(Table);

        //    Reservation Reservation = new Reservation()
        //    {
        //     Date = Booking.Date,
        //     Time =Booking.Time,
        //     Restaurant_Id =Booking.Restaurant_Id,
        //     NoOfTables=Booking.NoOfTables,
        //     Customer_Id=Booking.Customer_Id
        //    };

        //    _context.Reservation.Add(Reservation);
        //    _context.SaveChanges();
        //    return Ok();    

        //}



        [HttpPost]
        public ActionResult<Reservation> BookReservation([FromBody] Booking Booking)
        {
            var Table = _context.Restaurant.Find(Booking.Restaurant_Id);
            Table.TotalTables = Table.TotalTables - Booking.NoOfTables;
            _context.Restaurant.Update(Table);


            if (Table == null)
            {
                return NotFound("Restaurant not found");
            }

            // Check availability
            var existingReservations = _context.Reservation
                .Where(r => r.Date == Booking.Date && r.Time == Booking.Time)
                .ToList();

            if (existingReservations.Count >= Table.TotalTables)
            {
                return BadRequest("Table not available for the specified time");
            }

            // Create a new reservation
            var newReservation = new Reservation
            {
                Date = Booking.Date,
                Time = Booking.Time,
                Restaurant_Id = Booking.Restaurant_Id,
                NoOfTables = Booking.NoOfTables,
                Customer_Id = Booking.Customer_Id
            };

            _context.Reservation.Add(newReservation);
            _context.SaveChanges();

            return Ok();
        }

        // PUT: api/Reservation/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, Reservation reservation)
        {
            if (id != reservation.ReqId)
            {
                return BadRequest();
            }

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
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

        // DELETE: api/Reservation/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteReservation(int id)
        //{
        //    var reservation = await _context.Reservation.FindAsync(id);
        //    if (reservation == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Reservation.Remove(reservation);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            // Retrieve the number of tables booked in this reservation
            int tablesToBeBooked = reservation.NoOfTables;

            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();

            // Update the number of available tables
            var restaurant = await _context.Restaurant.FirstOrDefaultAsync();
            restaurant.TotalTables += tablesToBeBooked;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.ReqId == id);
        }


        //// GET: api/Reservation
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        //{
        //    // Get the current customer's ID from the authentication token or session
        //    var customerId = GetCurrentCustomerId();

        //    // Filter reservations based on the current customer's ID
        //    var reservations = await _context.Reservation
        //        .Where(r => r.Customer_Id == customerId)
        //        .ToListAsync();

        //    return Ok(reservations);
        //}

        //private int GetCurrentCustomerId()
        //{
        //    int customerId = 0;
        //    // Implement a method to get the current customer's ID from the authentication token or session
        //    // For demonstration purposes, let's assume the customer's ID is stored in a variable called customerId
        //    return customerId;
        //}
    }
}