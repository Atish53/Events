using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Events.Models;

namespace Events.Controllers
{
    public class EventBookingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EventBookings
        public async Task<ActionResult> Index()
        {
            var eventBookings = db.EventBookings.Include(e => e.Event);
            return View(await eventBookings.ToListAsync());
        }

        // GET: EventBookings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBooking eventBooking = await db.EventBookings.FindAsync(id);
            if (eventBooking == null)
            {
                return HttpNotFound();
            }
            return View(eventBooking);
        }

        // GET: EventBookings/Create
        public ActionResult Create()
        {
            ViewBag.EventId = new SelectList(db.Events, "EventId", "EventTitle");
            return View();
        }

        // POST: EventBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EventBookingId,EventId,CustomerName,CustomerSurname,Address,IdNumber,PhoneNumber,DateBooked,TicketNumber")] EventBooking eventBooking)
        {
            if (ModelState.IsValid)
            {
                db.EventBookings.Add(eventBooking);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EventId = new SelectList(db.Events, "EventId", "EventTitle", eventBooking.EventId);
            return View(eventBooking);
        }

        // GET: EventBookings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBooking eventBooking = await db.EventBookings.FindAsync(id);
            if (eventBooking == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventId = new SelectList(db.Events, "EventId", "EventTitle", eventBooking.EventId);
            return View(eventBooking);
        }

        // POST: EventBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EventBookingId,EventId,CustomerName,CustomerSurname,Address,IdNumber,PhoneNumber,DateBooked,TicketNumber")] EventBooking eventBooking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventBooking).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EventId = new SelectList(db.Events, "EventId", "EventTitle", eventBooking.EventId);
            return View(eventBooking);
        }

        // GET: EventBookings/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBooking eventBooking = await db.EventBookings.FindAsync(id);
            if (eventBooking == null)
            {
                return HttpNotFound();
            }
            return View(eventBooking);
        }

        // POST: EventBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EventBooking eventBooking = await db.EventBookings.FindAsync(id);
            db.EventBookings.Remove(eventBooking);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
