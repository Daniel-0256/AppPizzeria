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
        public ActionResult Index()
        {
            var orderSummaries = db.OrderSummaries.Include(o => o.User);
            return View(orderSummaries.ToList());
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
