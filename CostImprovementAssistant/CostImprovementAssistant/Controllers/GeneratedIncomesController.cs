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
    public class GeneratedIncomesController : Controller
    {
        private ciaEntities db = new ciaEntities();

        // GET: GeneratedIncomes
        public async Task<ActionResult> Index()
        {
            var generatedIncome = db.GeneratedIncome.Include(g => g.CiaForm);
            return View(await generatedIncome.ToListAsync());
        }

        // GET: GeneratedIncomes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeneratedIncome generatedIncome = await db.GeneratedIncome.FindAsync(id);
            if (generatedIncome == null)
            {
                return HttpNotFound();
            }
            return View(generatedIncome);
        }

        // GET: GeneratedIncomes/Create
        public ActionResult Create()
        {
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName");
            return View();
        }

        // POST: GeneratedIncomes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SchemeID,IncomeDescription,IncomeYear1SavingValueTotal,April,May,June,July,August,September,October,November,December,January,February,March,ReccurentSavingValue,ActualIncomeYear1Savings,ActualApril,ActualMay,ActualJune,ActualJuly,ActualAugust,ActualSeptember,ActualOctober,ActualNovember,ActualDecember,ActualJanuary,ActualFebruary,ActualMarch,ActualReccurentSavings,TrustCode")] GeneratedIncome generatedIncome)
        {
            if (ModelState.IsValid)
            {
                db.GeneratedIncome.Add(generatedIncome);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", generatedIncome.SchemeID);
            return View(generatedIncome);
        }

        // GET: GeneratedIncomes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeneratedIncome generatedIncome = await db.GeneratedIncome.FindAsync(id);
            if (generatedIncome == null)
            {
                return HttpNotFound();
            }
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", generatedIncome.SchemeID);
            return View(generatedIncome);
        }

        // POST: GeneratedIncomes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SchemeID,IncomeDescription,IncomeYear1SavingValueTotal,April,May,June,July,August,September,October,November,December,January,February,March,ReccurentSavingValue,ActualIncomeYear1Savings,ActualApril,ActualMay,ActualJune,ActualJuly,ActualAugust,ActualSeptember,ActualOctober,ActualNovember,ActualDecember,ActualJanuary,ActualFebruary,ActualMarch,ActualReccurentSavings,TrustCode")] GeneratedIncome generatedIncome)
        {
            if (ModelState.IsValid)
            {
                db.Entry(generatedIncome).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", generatedIncome.SchemeID);
            return View(generatedIncome);
        }

        // GET: GeneratedIncomes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeneratedIncome generatedIncome = await db.GeneratedIncome.FindAsync(id);
            if (generatedIncome == null)
            {
                return HttpNotFound();
            }
            return View(generatedIncome);
        }

        // POST: GeneratedIncomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            GeneratedIncome generatedIncome = await db.GeneratedIncome.FindAsync(id);
            db.GeneratedIncome.Remove(generatedIncome);
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
