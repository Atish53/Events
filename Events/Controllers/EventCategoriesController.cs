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
    public class EventCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EventCategories
        public async Task<ActionResult> Index()
        {
            return View(await db.EventCategories.ToListAsync());
        }

        // GET: EventCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventCategory eventCategory = await db.EventCategories.FindAsync(id);
            if (eventCategory == null)
            {
                return HttpNotFound();
            }
            return View(eventCategory);
        }

        // GET: EventCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EventCategoryId,EventCategoryName")] EventCategory eventCategory)
        {
            if (ModelState.IsValid)
            {
                db.EventCategories.Add(eventCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(eventCategory);
        }

        // GET: EventCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventCategory eventCategory = await db.EventCategories.FindAsync(id);
            if (eventCategory == null)
            {
                return HttpNotFound();
            }
            return View(eventCategory);
        }

        // POST: EventCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EventCategoryId,EventCategoryName")] EventCategory eventCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(eventCategory);
        }

        // GET: EventCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventCategory eventCategory = await db.EventCategories.FindAsync(id);
            if (eventCategory == null)
            {
                return HttpNotFound();
            }
            return View(eventCategory);
        }

        // POST: EventCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EventCategory eventCategory = await db.EventCategories.FindAsync(id);
            db.EventCategories.Remove(eventCategory);
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
