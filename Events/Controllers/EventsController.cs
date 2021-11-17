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
using System.Net.Mail;
using System.IO;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Drawing;
using System.Text;

namespace Events.Controllers
{
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Events
        public async Task<ActionResult> Index()
        {
            var events = db.Events.Include(m => m.EventCategory);
            return View(await events.ToListAsync());
        }

        public async Task<ActionResult> Book(int id) //Event Booking - id is the Event Id... Booking is stored to the EventBookings Table
        {
            var vFirstName = User.Identity.GetName();          
            var vAddress = User.Identity.GetAddress();          
            var vPhone = User.Identity.GetPhoneNo();          
            string FirstName = vFirstName.ToString();            
            string Address = vAddress.ToString();            
            string Phone = vPhone.ToString();            

            EventBooking eventBooking = new EventBooking();
            eventBooking.CustomerName = FirstName;
            eventBooking.Address = Address;
            eventBooking.PhoneNumber = Phone;
            eventBooking.CustomerName = FirstName;
            eventBooking.DateBooked = DateTime.Now.ToString();

            Event @event = await db.Events.FindAsync(id);
            @event.TicketsRemaining--;

            eventBooking.EventId = @event.EventId;

            if (@event.TicketsRemaining == 0)
            {
                @event.isActive = false;
            }


            string Ticket = "#11" + @event.EventId + FirstName.Substring(0, 2) + DateTime.Now.ToString("FFFFF"); //100000ths of a second makes the ticket unique. (FFFFF)

            eventBooking.TicketNumber = Ticket;

            db.EventBookings.Add(eventBooking);
            await db.SaveChangesAsync();

            MemoryStream msS = new MemoryStream(@event.EventPicture);

            //New Email.
            //Creates a new PDF document
            PdfDocument document = new PdfDocument();
            //Adds page settings
            document.PageSettings.Orientation = PdfPageOrientation.Portrait;
            document.PageSettings.Margins.All = 50;
            //Adds a page to the document
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;
            //Loads the image from disk
            PdfImage image = PdfImage.FromStream(msS);
            RectangleF bounds = new RectangleF(10, 10, 200, 200);
            //Draws the image to the PDF page
            page.Graphics.DrawImage(image, bounds);
            PdfBrush solidBrush = new PdfSolidBrush(new PdfColor(80, 138, 4));
            bounds = new RectangleF(0, bounds.Bottom + 90, graphics.ClientSize.Width, 30);
            //Draws a rectangle to place the heading in that region.
            graphics.DrawRectangle(solidBrush, bounds);
            //Creates a font for adding the heading in the page
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 13);
            //Creates a text element to add the invoice number
            PdfTextElement element = new PdfTextElement("Thank you! You have successfully booked the " + @event.EventName + " Event!", subHeadingFont);
            element.Brush = PdfBrushes.White;
            //Draws the heading on the page
            PdfLayoutResult res = element.Draw(page, new PointF(10, bounds.Top + 8));
            PdfFont timesRoman = new PdfStandardFont(PdfFontFamily.TimesRoman, 12);
            //Creates text elements to add the address and draw it to the page.
            element = new PdfTextElement("This ticket belongs to: " + FirstName, timesRoman);
            element.Brush = new PdfSolidBrush(new PdfColor(16, 36, 7));
            res = element.Draw(page, new PointF(10, res.Bounds.Bottom + 45));            
            element = new PdfTextElement("Event Location: " + @event.EventLocation + ".", timesRoman);
            element.Brush = new PdfSolidBrush(new PdfColor(80, 138, 4));
            res = element.Draw(page, new PointF(10, res.Bounds.Bottom + 15));
            element = new PdfTextElement("Description: " + @event.EventDescription2 + ".", timesRoman);
            element.Brush = new PdfSolidBrush(new PdfColor(80, 138, 4));
            res = element.Draw(page, new PointF(10, res.Bounds.Bottom + 15));
            element = new PdfTextElement("Price: R" + @event.EventPrice, timesRoman);
            element.Brush = new PdfSolidBrush(new PdfColor(80, 138, 4));
            res = element.Draw(page, new PointF(10, res.Bounds.Bottom + 15));

            PdfPen linePen = new PdfPen(new PdfColor(80, 138, 4), 0.70f);
            PointF startPoint = new PointF(0, res.Bounds.Bottom + 3);
            PointF endPoint = new PointF(graphics.ClientSize.Width, res.Bounds.Bottom + 5);
            //Draws a line at the bottom of the address
            graphics.DrawLine(linePen, startPoint, endPoint);

            PdfFont notTimesRoman = new PdfStandardFont(PdfFontFamily.Courier, 16);
            element = new PdfTextElement("Your Ticket Number is" + eventBooking.TicketNumber, notTimesRoman);
            element.Brush = new PdfSolidBrush(new PdfColor(48, 5, 5));
            res = element.Draw(page, new PointF(10, res.Bounds.Bottom + 15));

            linePen = new PdfPen(new PdfColor(80, 138, 4), 0.70f);
            startPoint = new PointF(0, res.Bounds.Bottom + 3);
            endPoint = new PointF(graphics.ClientSize.Width, res.Bounds.Bottom + 5);
            //Draws a line at the bottom of the address
            graphics.DrawLine(linePen, startPoint, endPoint);


            MemoryStream outputStream = new MemoryStream();
            document.Save(outputStream);
            outputStream.Position = 0;

            var invoicePdf = new System.Net.Mail.Attachment(outputStream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            string docname = "Invoice.pdf";
            invoicePdf.ContentDisposition.FileName = docname;

            MailMessage mail = new MailMessage();
            string emailTo = User.Identity.Name;
            MailAddress from = new MailAddress("21642835@dut4life.ac.za");
            mail.From = from;
            mail.Subject = "Your e-Ticket for event: " + @event.EventName;
            mail.Body = "Dear " + FirstName + ", find your invoice in the attached PDF document.";
            mail.To.Add(emailTo);
            mail.Attachments.Add(invoicePdf);
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp-mail.outlook.com";
            smtp.EnableSsl = true;
            NetworkCredential networkCredential = new NetworkCredential("21642835@dut4life.ac.za", "$$Dut980514");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = networkCredential;
            smtp.Port = 587;
            smtp.Send(mail);
            //Clean-up.
            //Close the document.
            document.Close(true);
            //Dispose of email.
            mail.Dispose();
            try
            {
                // Retrieve required values for the PayFast Merchant
                string name = "ParadiseTravels Event: " + @event.EventName;
                string description = "This is a once-off and non-refundable payment. ";

                string site = "https://sandbox.payfast.co.za/eng/process";
                string merchant_id = "";
                string merchant_key = "";

                string paymentMode = System.Configuration.ConfigurationManager.AppSettings["PaymentMode"];

                if (paymentMode == "test")
                {
                    site = "https://sandbox.payfast.co.za/eng/process?";
                    merchant_id = "10000100";
                    merchant_key = "46f0cd694581a";
                }

                // Build the query string for payment site

                StringBuilder str = new StringBuilder();
                str.Append("merchant_id=" + HttpUtility.UrlEncode(merchant_id));
                str.Append("&merchant_key=" + HttpUtility.UrlEncode(merchant_key));
                str.Append("&return_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_ReturnURL"]));
                str.Append("&cancel_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_CancelURL"]));
                str.Append("&notify_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_NotifyURL"]));

                str.Append("&m_payment_id=" + HttpUtility.UrlEncode(eventBooking.EventBookingId.ToString()));
                str.Append("&amount=" + HttpUtility.UrlEncode(@event.EventPrice.ToString()));
                str.Append("&item_name=" + HttpUtility.UrlEncode(name));
                str.Append("&item_description=" + HttpUtility.UrlEncode(description));

                // Redirect to PayFast
                return Redirect(site + str.ToString());
            }
            catch (Exception)
            {
                throw;
            }

        }


        // GET: Events/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await db.Events.FindAsync(id);
            if (@event == null)
            {
                return HttpNotFound();
            }

            ViewBag.EventId = id.Value;

            var comments = db.EventComments.Where(d => d.EventId.Equals(id.Value)).ToList();
            ViewBag.Comments = comments;

            var ratings = db.EventComments.Where(d => d.EventId.Equals(id.Value)).ToList();
            if (ratings.Count() > 0)
            {
                var ratingSum = ratings.Sum(d => d.Rating.Value);
                ViewBag.RatingSum = ratingSum;
                var ratingCount = ratings.Count();
                ViewBag.RatingCount = ratingCount;
            }
            else
            {
                ViewBag.RatingSum = 0;
                ViewBag.RatingCount = 0;
            }

            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.EventCategoryId = new SelectList(db.EventCategories, "EventCategoryId", "EventCategoryName");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EventId,EventTitle,EventPicture,EventName,EventDescription1,EventDescription2,EventLocation,StartDate,EndDate,EventPrice,TicketsRemaining,EventCategoryId,isActive")] Event @event ,HttpPostedFileBase img_upload)
        {
            byte[] data;
            data = new byte[img_upload.ContentLength];
            img_upload.InputStream.Read(data, 0, img_upload.ContentLength);
            @event.EventPicture = data;
            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EventCategoryId = new SelectList(db.EventCategories, "EventCategoryId", "EventCategoryName", @event.EventCategoryId);
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await db.Events.FindAsync(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventCategoryId = new SelectList(db.EventCategories, "EventCategoryId", "EventCategoryName", @event.EventCategoryId);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EventId,EventTitle,EventPicture,EventName,EventDescription1,EventDescription2,EventLocation,StartDate,EndDate,EventPrice,TicketsRemaining,EventCategoryId,isActive")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EventCategoryId = new SelectList(db.EventCategories, "EventCategoryId", "EventCategoryName", @event.EventCategoryId);
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await db.Events.FindAsync(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Event @event = await db.Events.FindAsync(id);
            db.Events.Remove(@event);
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
