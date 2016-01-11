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
    public class BreakdownOfSavingsController : Controller
    {
        private ciaEntities db = new ciaEntities();

        // GET: BreakdownOfSavings
        public async Task<ActionResult> Index()
        {
            var breakdownOfSavings = db.BreakdownOfSavings.Include(b => b.BudgetType).Include(b => b.BudgetType1).Include(b => b.CiaForm);
            return View(await breakdownOfSavings.ToListAsync());
        }

        // GET: BreakdownOfSavings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BreakdownOfSavings breakdownOfSavings = await db.BreakdownOfSavings.FindAsync(id);
            if (breakdownOfSavings == null)
            {
                return HttpNotFound();
            }
            return View(breakdownOfSavings);
        }

        // GET: BreakdownOfSavings/Create
        public ActionResult Create()
        {
            ViewBag.BudgetTypeNameB = new SelectList(db.BudgetType, "BudgetTypeName", "BudgetStatus");
            ViewBag.BudgetTypeNameA = new SelectList(db.BudgetType, "BudgetTypeName", "BudgetStatus");
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName");
            return View();
        }

        // POST: BreakdownOfSavings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SchemeID,BudgetTypeNameA,BudgetTypeNameB,WTE,Year1Saving,ReccurentSavingValue,TrustCode")] BreakdownOfSavings breakdownOfSavings)
        {
            if (ModelState.IsValid)
            {
                db.BreakdownOfSavings.Add(breakdownOfSavings);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BudgetTypeNameB = new SelectList(db.BudgetType, "BudgetTypeName", "BudgetStatus", breakdownOfSavings.BudgetTypeNameB);
            ViewBag.BudgetTypeNameA = new SelectList(db.BudgetType, "BudgetTypeName", "BudgetStatus", breakdownOfSavings.BudgetTypeNameA);
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", breakdownOfSavings.SchemeID);
            return View(breakdownOfSavings);
        }

        // GET: BreakdownOfSavings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BreakdownOfSavings breakdownOfSavings = await db.BreakdownOfSavings.FindAsync(id);
            if (breakdownOfSavings == null)
            {
                return HttpNotFound();
            }
            ViewBag.BudgetTypeNameB = new SelectList(db.BudgetType, "BudgetTypeName", "BudgetStatus", breakdownOfSavings.BudgetTypeNameB);
            ViewBag.BudgetTypeNameA = new SelectList(db.BudgetType, "BudgetTypeName", "BudgetStatus", breakdownOfSavings.BudgetTypeNameA);
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", breakdownOfSavings.SchemeID);
            return View(breakdownOfSavings);
        }

        // POST: BreakdownOfSavings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SchemeID,BudgetTypeNameA,BudgetTypeNameB,WTE,Year1Saving,ReccurentSavingValue,TrustCode")] BreakdownOfSavings breakdownOfSavings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(breakdownOfSavings).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BudgetTypeNameB = new SelectList(db.BudgetType, "BudgetTypeName", "BudgetStatus", breakdownOfSavings.BudgetTypeNameB);
            ViewBag.BudgetTypeNameA = new SelectList(db.BudgetType, "BudgetTypeName", "BudgetStatus", breakdownOfSavings.BudgetTypeNameA);
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", breakdownOfSavings.SchemeID);
            return View(breakdownOfSavings);
        }

        // GET: BreakdownOfSavings/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BreakdownOfSavings breakdownOfSavings = await db.BreakdownOfSavings.FindAsync(id);
            if (breakdownOfSavings == null)
            {
                return HttpNotFound();
            }
            return View(breakdownOfSavings);
        }

        // POST: BreakdownOfSavings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BreakdownOfSavings breakdownOfSavings = await db.BreakdownOfSavings.FindAsync(id);
            db.BreakdownOfSavings.Remove(breakdownOfSavings);
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
