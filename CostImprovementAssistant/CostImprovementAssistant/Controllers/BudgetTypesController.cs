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
    public class BudgetTypesController : Controller
    {
        private ciaEntities db = new ciaEntities();

        // GET: BudgetTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.BudgetType.ToListAsync());
        }

        // GET: BudgetTypes/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetType budgetType = await db.BudgetType.FindAsync(id);
            if (budgetType == null)
            {
                return HttpNotFound();
            }
            return View(budgetType);
        }

        // GET: BudgetTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BudgetTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BudgetTypeName,BudgetStatus,TrustCode")] BudgetType budgetType)
        {
            if (ModelState.IsValid)
            {
                db.BudgetType.Add(budgetType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(budgetType);
        }

        // GET: BudgetTypes/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetType budgetType = await db.BudgetType.FindAsync(id);
            if (budgetType == null)
            {
                return HttpNotFound();
            }
            return View(budgetType);
        }

        // POST: BudgetTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BudgetTypeName,BudgetStatus,TrustCode")] BudgetType budgetType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budgetType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(budgetType);
        }

        // GET: BudgetTypes/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetType budgetType = await db.BudgetType.FindAsync(id);
            if (budgetType == null)
            {
                return HttpNotFound();
            }
            return View(budgetType);
        }

        // POST: BudgetTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            BudgetType budgetType = await db.BudgetType.FindAsync(id);
            db.BudgetType.Remove(budgetType);
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
