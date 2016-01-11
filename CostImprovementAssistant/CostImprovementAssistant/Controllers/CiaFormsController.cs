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
    public class CiaFormsController : Controller
    {
        private ciaEntities db = new ciaEntities();

        // GET: CiaForms
        public async Task<ActionResult> Index()
        {
            var ciaForm = db.CiaForm.Include(c => c.BusinessLevel).Include(c => c.BusinessSector).Include(c => c.Category).Include(c => c.QualityImpact).Include(c => c.Department);
            return View(await ciaForm.ToListAsync());
        }

        // GET: CiaForms/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CiaForm ciaForm = await db.CiaForm.FindAsync(id);
            if (ciaForm == null)
            {
                return HttpNotFound();
            }
            return View(ciaForm);
        }

        // GET: CiaForms/Create
        public ActionResult Create()
        {
            ViewBag.BusinessLevelName = new SelectList(db.BusinessLevel, "BusinesslevelName", "BusinesslevelName");
            ViewBag.BusinessSectorName = new SelectList(db.BusinessSector, "BusinessSectorName", "BusinessSectorName");
            ViewBag.CategoryName = new SelectList(db.Category, "CategoryName", "CategoryName");
            ViewBag.SchemeID = new SelectList(db.QualityImpact, "SchemeID", "ImpactDate");
            ViewBag.DepartmentName = new SelectList(db.Department, "DepartmentName", "DepartmentName");
            return View();
        }

        // POST: CiaForms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SchemeID,DateofScheme,SchemeName,CategoryName,DepartmentName,BusinessLevelName,BusinessSectorName,Overview,CurrentState,TargetState,ProjectSupervisor,ProjectManager,BudgetSupervisor,HrSupervisor,ExecutiveOwner,ConfidenceFactor,ImplementationDate,BenefitExpectedInCiaYear,ReccuringAnualBenefit,EnablingCosts,NettCiaYearSavings,PSAuthorisationCode,PSAuthorisationDate,BSAuthorisationCode,BSAuthorisationDate,RiskAdjustedValue,ConfidenceCode,TrustCode,Active,QIRequired")] CiaForm ciaForm)
        {
            if (ModelState.IsValid)
            {
                db.CiaForm.Add(ciaForm);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BusinessLevelName = new SelectList(db.BusinessLevel, "BusinesslevelName", "BusinesslevelName", ciaForm.BusinessLevelName);
            ViewBag.BusinessSectorName = new SelectList(db.BusinessSector, "BusinessSectorName", "BusinessSectorName", ciaForm.BusinessSectorName);
            ViewBag.CategoryName = new SelectList(db.Category, "CategoryName", "CategoryName", ciaForm.CategoryName);
            ViewBag.SchemeID = new SelectList(db.QualityImpact, "SchemeID", "ImpactDate", ciaForm.SchemeID);
            ViewBag.DepartmentName = new SelectList(db.Department, "DepartmentName", "DepartmentName", ciaForm.DepartmentName);
            return View(ciaForm);
        }

        // GET: CiaForms/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CiaForm ciaForm = await db.CiaForm.FindAsync(id);
            if (ciaForm == null)
            {
                return HttpNotFound();
            }
            ViewBag.BusinessLevelName = new SelectList(db.BusinessLevel, "BusinesslevelName", "BusinesslevelName", ciaForm.BusinessLevelName);
            ViewBag.BusinessSectorName = new SelectList(db.BusinessSector, "BusinessSectorName", "BusinessSectorName", ciaForm.BusinessSectorName);
            ViewBag.CategoryName = new SelectList(db.Category, "CategoryName", "CategoryName", ciaForm.CategoryName);
            ViewBag.SchemeID = new SelectList(db.QualityImpact, "SchemeID", "ImpactDate", ciaForm.SchemeID);
            ViewBag.DepartmentName = new SelectList(db.Department, "DepartmentName", "DepartmentName", ciaForm.DepartmentName);
            return View(ciaForm);
        }

        // POST: CiaForms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SchemeID,DateofScheme,SchemeName,CategoryName,DepartmentName,BusinessLevelName,BusinessSectorName,Overview,CurrentState,TargetState,ProjectSupervisor,ProjectManager,BudgetSupervisor,HrSupervisor,ExecutiveOwner,ConfidenceFactor,ImplementationDate,BenefitExpectedInCiaYear,ReccuringAnualBenefit,EnablingCosts,NettCiaYearSavings,PSAuthorisationCode,PSAuthorisationDate,BSAuthorisationCode,BSAuthorisationDate,RiskAdjustedValue,ConfidenceCode,TrustCode,Active,QIRequired")] CiaForm ciaForm)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ciaForm).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BusinessLevelName = new SelectList(db.BusinessLevel, "BusinesslevelName", "BusinesslevelName", ciaForm.BusinessLevelName);
            ViewBag.BusinessSectorName = new SelectList(db.BusinessSector, "BusinessSectorName", "BusinessSectorName", ciaForm.BusinessSectorName);
            ViewBag.CategoryName = new SelectList(db.Category, "CategoryName", "CategoryName", ciaForm.CategoryName);
            ViewBag.SchemeID = new SelectList(db.QualityImpact, "SchemeID", "ImpactDate", ciaForm.SchemeID);
            ViewBag.DepartmentName = new SelectList(db.Department, "DepartmentName", "DepartmentName", ciaForm.DepartmentName);
            return View(ciaForm);
        }

        // GET: CiaForms/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CiaForm ciaForm = await db.CiaForm.FindAsync(id);
            if (ciaForm == null)
            {
                return HttpNotFound();
            }
            return View(ciaForm);
        }

        // POST: CiaForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CiaForm ciaForm = await db.CiaForm.FindAsync(id);
            db.CiaForm.Remove(ciaForm);
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
