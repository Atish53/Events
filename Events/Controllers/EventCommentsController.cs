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
    public class EventCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EventComments
        public async Task<ActionResult> Index()
        {
            return View(await db.EventComments.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(FormCollection form)
        {
            var comment = form["Comment"].ToString();
            var eventId = form["EventId"];
            var rating = int.Parse(form["Rating"]);

            EventComment eventComment = new EventComment()
            {
                EventId = Convert.ToInt32(eventId),
                Comments = comment,
                Rating = rating, 
                ThisDateTime = DateTime.Now
            };

            db.EventComments.Add(eventComment);
            db.SaveChanges();

            return RedirectToAction("Details", "Events", new { id = eventId });
        }

        // GET: EventComments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventComment eventComment = await db.EventComments.FindAsync(id);
            if (eventComment == null)
            {
                return HttpNotFound();
            }
            return View(eventComment);
        }

        // GET: EventComments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CommentId,Comments,ThisDateTime,EventId,Rating")] EventComment eventComment)
        {
            if (ModelState.IsValid)
            {
                db.EventComments.Add(eventComment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(eventComment);
        }

        // GET: EventComments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventComment eventComment = await db.EventComments.FindAsync(id);
            if (eventComment == null)
            {
                return HttpNotFound();
            }
            return View(eventComment);
        }

        // POST: EventComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CommentId,Comments,ThisDateTime,EventId,Rating")] EventComment eventComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventComment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(eventComment);
        }

        // GET: EventComments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventComment eventComment = await db.EventComments.FindAsync(id);
            if (eventComment == null)
            {
                return HttpNotFound();
            }
            return View(eventComment);
        }

        // POST: EventComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EventComment eventComment = await db.EventComments.FindAsync(id);
            db.EventComments.Remove(eventComment);
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
