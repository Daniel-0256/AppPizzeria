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
    public class OrderItemsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: OrderItems
        [Authorize]
        public ActionResult Index()
        {
            var orderItems = db.OrderItems.Include(o => o.OrderSummary).Include(o => o.Product);
            return View(orderItems.ToList());
        }

   
        [HttpPost]
        [Authorize]
        public ActionResult LinkCarrello(int id)
        {
            // Trova il prodotto desiderato
            var product = db.Products.Find(id);
            if (product == null)
            {
                // Gestisci il caso in cui il prodotto non viene trovato
                return RedirectToAction("Index", "Home");
            }

            // Controlla se l'utente ha già un carrello attivo
            var userId = HttpContext.User.Identity.Name;
            var cart = db.OrderSummaries.FirstOrDefault(o => o.UserId.ToString() == userId && o.State == "Non Evaso");
            if (cart == null)
            {
                // se non c'è, crea un nuovo riepilogo d'ordine (carrello)
                cart = new OrderSummary
                {
                    UserId = Convert.ToInt32(userId), // sostituisci con il metodo appropriato per ottenere l'id utente
                    State = "non evaso"
                };
                db.OrderSummaries.Add(cart);
                db.SaveChanges(); // salva per ottenere un ordersummaryid
            }

            // Aggiungi il nuovo OrderItem al carrello
            OrderItem orderItem = new OrderItem
            {
                ProductId = product.ProductId,
                OrderSummaryId = cart.OrderSummaryId,
                ItemPrice = product.ProductPrice,
                Quantity = 1 // Puoi impostare una quantità di default o passarla come parametro
            };
            db.OrderItems.Add(orderItem);
            db.SaveChanges();

            // Reindirizza l'utente alla vista del carrello
            return RedirectToAction("Index");
        }

        // GET: OrderItems/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = db.OrderItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            return View(orderItem);
        }

        // GET: OrderItems/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.OrderSummaryId = new SelectList(db.OrderSummaries, "OrderSummaryId", "OrderDate");
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName");
            return View();
        }

        // POST: OrderItems/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "OrderItemId,OrderSummaryId,ProductId,Quantity,ItemPrice")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                db.OrderItems.Add(orderItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderSummaryId = new SelectList(db.OrderSummaries, "OrderSummaryId", "OrderDate", orderItem.OrderSummaryId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", orderItem.ProductId);
            return View(orderItem);
        }

        // GET: OrderItems/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = db.OrderItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderSummaryId = new SelectList(db.OrderSummaries, "OrderSummaryId", "OrderDate", orderItem.OrderSummaryId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", orderItem.ProductId);
            return View(orderItem);
        }

        // POST: OrderItems/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int OrderItemId, int Quantity)
        {
            var orderItem = db.OrderItems.Find(OrderItemId);
            var product = db.Products.FirstOrDefault(o => o.ProductId == orderItem.ProductId);
            if (orderItem == null)
            {
                return HttpNotFound();
            }

            if (Quantity > 0) // Aggiungi qui ulteriori controlli se necessario
            {
                orderItem.Quantity = Quantity;
                orderItem.ItemPrice = (decimal)Quantity * product.ProductPrice;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Se arrivi qui, c'è stato un problema
            ViewBag.OrderSummaryId = new SelectList(db.OrderSummaries, "OrderSummaryId", "OrderDate", orderItem.OrderSummaryId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", orderItem.ProductId);
            return View(orderItem);
        }


        // GET: OrderItems/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = db.OrderItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            return View(orderItem);
        }

        // POST: OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderItem orderItem = db.OrderItems.Find(id);
            db.OrderItems.Remove(orderItem);
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
