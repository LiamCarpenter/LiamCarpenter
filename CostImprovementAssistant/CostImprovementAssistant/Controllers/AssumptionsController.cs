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
    public class AssumptionsController : Controller
    {
        private ciaEntities db = new ciaEntities();

        // GET: Assumptions
        public async Task<ActionResult> Index()
        {
            var assumptions = db.Assumptions.Include(a => a.Impact1);
            return View(await assumptions.ToListAsync());
        }

        // GET: Assumptions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assumptions assumptions = await db.Assumptions.FindAsync(id);
            if (assumptions == null)
            {
                return HttpNotFound();
            }
            return View(assumptions);
        }

        // GET: Assumptions/Create
        public ActionResult Create()
        {
            ViewBag.Impact = new SelectList(db.Impact, "ImpactTitle", "ImpactTitle");
            return View();
        }

        // POST: Assumptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SchemeID,AssumptionsID,AssumptionsDetail,Impact,TrustCode")] Assumptions assumptions)
        {
            if (ModelState.IsValid)
            {
                db.Assumptions.Add(assumptions);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Impact = new SelectList(db.Impact, "ImpactTitle", "ImpactTitle", assumptions.Impact);
            return View(assumptions);
        }

        // GET: Assumptions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assumptions assumptions = await db.Assumptions.FindAsync(id);
            if (assumptions == null)
            {
                return HttpNotFound();
            }
            ViewBag.Impact = new SelectList(db.Impact, "ImpactTitle", "ImpactTitle", assumptions.Impact);
            return View(assumptions);
        }

        // POST: Assumptions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SchemeID,AssumptionsID,AssumptionsDetail,Impact,TrustCode")] Assumptions assumptions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assumptions).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Impact = new SelectList(db.Impact, "ImpactTitle", "ImpactTitle", assumptions.Impact);
            return View(assumptions);
        }

        // GET: Assumptions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assumptions assumptions = await db.Assumptions.FindAsync(id);
            if (assumptions == null)
            {
                return HttpNotFound();
            }
            return View(assumptions);
        }

        // POST: Assumptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Assumptions assumptions = await db.Assumptions.FindAsync(id);
            db.Assumptions.Remove(assumptions);
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
