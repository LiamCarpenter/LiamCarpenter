using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CostImprovementAssistant;

namespace CostImprovementAssistant.Controllers
{
    public class TrustsController : Controller
    {
        private ciaEntities db = new ciaEntities();

        // GET: Trusts
        public async Task<ActionResult> Index()
        {
            return View(await db.Trusts.ToListAsync());
        }

        // GET: Trusts/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trusts trusts = await db.Trusts.FindAsync(id);
            if (trusts == null)
            {
                return HttpNotFound();
            }
            return View(trusts);
        }

        // GET: Trusts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trusts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TrustCode,TrustName")] Trusts trusts)
        {
            if (ModelState.IsValid)
            {
                db.Trusts.Add(trusts);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(trusts);
        }

        // GET: Trusts/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trusts trusts = await db.Trusts.FindAsync(id);
            if (trusts == null)
            {
                return HttpNotFound();
            }
            return View(trusts);
        }

        // POST: Trusts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TrustCode,TrustName")] Trusts trusts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trusts).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(trusts);
        }

        // GET: Trusts/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trusts trusts = await db.Trusts.FindAsync(id);
            if (trusts == null)
            {
                return HttpNotFound();
            }
            return View(trusts);
        }

        // POST: Trusts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Trusts trusts = await db.Trusts.FindAsync(id);
            db.Trusts.Remove(trusts);
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
