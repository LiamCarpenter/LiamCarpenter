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
    public class BreakdownOfEnablingCostsController : Controller
    {
        private ciaEntities db = new ciaEntities();

        // GET: BreakdownOfEnablingCosts
        public async Task<ActionResult> Index()
        {
            var breakdownOfEnablingCosts = db.BreakdownOfEnablingCosts.Include(b => b.CiaForm).Include(b => b.BudgetType);
            return View(await breakdownOfEnablingCosts.ToListAsync());
        }

        // GET: BreakdownOfEnablingCosts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BreakdownOfEnablingCosts breakdownOfEnablingCosts = await db.BreakdownOfEnablingCosts.FindAsync(id);
            if (breakdownOfEnablingCosts == null)
            {
                return HttpNotFound();
            }
            return View(breakdownOfEnablingCosts);
        }

        // GET: BreakdownOfEnablingCosts/Create
        public ActionResult Create()
        {
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName");
            ViewBag.BudgetTypeNameA = new SelectList(db.BudgetType, "BudgetTypeName", "BudgetStatus");
            return View();
        }

        // POST: BreakdownOfEnablingCosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SchemeID,BudgetTypeNameA,BudgetTypeNameB,WTE,Costs,TrustCode")] BreakdownOfEnablingCosts breakdownOfEnablingCosts)
        {
            if (ModelState.IsValid)
            {
                db.BreakdownOfEnablingCosts.Add(breakdownOfEnablingCosts);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", breakdownOfEnablingCosts.SchemeID);
            ViewBag.BudgetTypeNameA = new SelectList(db.BudgetType, "BudgetTypeName", "BudgetStatus", breakdownOfEnablingCosts.BudgetTypeNameA);
            return View(breakdownOfEnablingCosts);
        }

        // GET: BreakdownOfEnablingCosts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BreakdownOfEnablingCosts breakdownOfEnablingCosts = await db.BreakdownOfEnablingCosts.FindAsync(id);
            if (breakdownOfEnablingCosts == null)
            {
                return HttpNotFound();
            }
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", breakdownOfEnablingCosts.SchemeID);
            ViewBag.BudgetTypeNameA = new SelectList(db.BudgetType, "BudgetTypeName", "BudgetStatus", breakdownOfEnablingCosts.BudgetTypeNameA);
            return View(breakdownOfEnablingCosts);
        }

        // POST: BreakdownOfEnablingCosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SchemeID,BudgetTypeNameA,BudgetTypeNameB,WTE,Costs,TrustCode")] BreakdownOfEnablingCosts breakdownOfEnablingCosts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(breakdownOfEnablingCosts).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", breakdownOfEnablingCosts.SchemeID);
            ViewBag.BudgetTypeNameA = new SelectList(db.BudgetType, "BudgetTypeName", "BudgetStatus", breakdownOfEnablingCosts.BudgetTypeNameA);
            return View(breakdownOfEnablingCosts);
        }

        // GET: BreakdownOfEnablingCosts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BreakdownOfEnablingCosts breakdownOfEnablingCosts = await db.BreakdownOfEnablingCosts.FindAsync(id);
            if (breakdownOfEnablingCosts == null)
            {
                return HttpNotFound();
            }
            return View(breakdownOfEnablingCosts);
        }

        // POST: BreakdownOfEnablingCosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BreakdownOfEnablingCosts breakdownOfEnablingCosts = await db.BreakdownOfEnablingCosts.FindAsync(id);
            db.BreakdownOfEnablingCosts.Remove(breakdownOfEnablingCosts);
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
