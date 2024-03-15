using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AppPizzeria.Models;

namespace AppPizzeria.Controllers
{
    public class OrderSummariesController : Controller
    {
        private DBContext db = new DBContext();

        // GET: OrderSummaries
        [Authorize]
        public ActionResult Index()
        {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            OrderSummary orderSummary = db.OrderSummaries
                                          .Include(o => o.User)
                                          .FirstOrDefault(o => o.UserId == userId && o.State == "Non Evaso");

            if (orderSummary != null)
            {
                var orderItems = db.OrderItems.Include(o => o.Product).Where(oi => oi.OrderSummaryId == orderSummary.OrderSummaryId).ToList();
                ViewBag.Somma = orderItems.Sum(oi => oi.ItemPrice);

                return View(orderSummary);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }



        [HttpPost]
        [Authorize]
        public ActionResult ConfermaOrdine(OrderSummary summary)
        {
            if (summary == null)
            {
                return View("Error"); // Gestire il caso di riepilogo ordine nullo
            }

            var userId = Convert.ToInt32(HttpContext.User.Identity.Name); // Assicurati che questo sia il modo corretto in cui recuperi l'ID utente

            if (userId <= 0)
            {
                return View("Error", new { message = "Impossibile recuperare l'ID dell'utente valido." });
            }

            var existingOrderSummary = db.OrderSummaries.FirstOrDefault(o => o.UserId == userId && o.State == "Non Evaso");

            if (existingOrderSummary != null)
            {
                decimal sommaPrezzo = db.OrderItems.Where(oi => oi.OrderSummaryId == existingOrderSummary.OrderSummaryId).Sum(o => o.ItemPrice);
                
                existingOrderSummary = new OrderSummary
                {
                    UserId = userId,
                    State = "Evaso",
                    OrderDate = summary.OrderDate,
                    OrderAddress = summary.OrderAddress,
                    Note = summary.Note,
                    TotalPrice = sommaPrezzo,
                };
                db.OrderSummaries.Add(existingOrderSummary);
                db.SaveChanges(); // Salva prima per ottenere un OrderSummaryId se necessario
            }

            // Reindirizza alla pagina Index di OrderSummaries
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult FinalizeOrder(int OrderSummaryId, string OrderAddress, string OrderDate, string Note)
        {
            if (OrderSummaryId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Identificativo ordine non valido");
            }
            OrderSummary orderSummary = db.OrderSummaries.Find(OrderSummaryId);
            if (orderSummary == null)
            {
                return HttpNotFound("Ordine non trovato");
            }

            if (!string.IsNullOrEmpty(OrderAddress) && !string.IsNullOrEmpty(Note))
            {
                // Aggiorna i campi dell'ordine con i nuovi valori forniti dall'utente.
                orderSummary.OrderAddress = OrderAddress;
                orderSummary.OrderDate = OrderDate;
                orderSummary.Note = Note;
                orderSummary.State = "Evaso";

                // Salva le modifiche nel database
                db.Entry(orderSummary).State = EntityState.Modified;
                db.SaveChanges();

                // Reindirizza l'utente a una pagina di conferma o alla pagina di riepilogo degli ordini
                return RedirectToAction("Index");
            }
            else
            {
                // Gestisci il caso in cui l'indirizzo o le note siano vuoti
                // Ritorna alla vista con un messaggio di errore
                ModelState.AddModelError("", "Indirizzo di consegna e note sono obbligatori.");
                return View(orderSummary); // Assicurati che questa vista esista o adatta secondo il tuo caso
            }
        }



        // GET: OrderSummaries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderSummary orderSummary = db.OrderSummaries.Find(id);
            if (orderSummary == null)
            {
                return HttpNotFound();
            }
            return View(orderSummary);
        }

        // GET: OrderSummaries/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email");
            return View();
        }

        // POST: OrderSummaries/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderSummaryId,UserId,OrderDate,OrderAddress,Note,TotalPrice,State")] OrderSummary orderSummary)
        {
            if (ModelState.IsValid)
            {
                db.OrderSummaries.Add(orderSummary);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", orderSummary.UserId);
            return View(orderSummary);
        }

        // GET: OrderSummaries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderSummary orderSummary = db.OrderSummaries.Find(id);
            if (orderSummary == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", orderSummary.UserId);
            return View(orderSummary);
        }

        // POST: OrderSummaries/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderSummaryId,UserId,OrderDate,OrderAddress,Note,TotalPrice,State")] OrderSummary orderSummary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderSummary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", orderSummary.UserId);
            return View(orderSummary);
        }

        // GET: OrderSummaries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderSummary orderSummary = db.OrderSummaries.Find(id);
            if (orderSummary == null)
            {
                return HttpNotFound();
            }
            return View(orderSummary);
        }

        // POST: OrderSummaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderSummary orderSummary = db.OrderSummaries.Find(id);
            db.OrderSummaries.Remove(orderSummary);
            db.SaveChanges();
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
