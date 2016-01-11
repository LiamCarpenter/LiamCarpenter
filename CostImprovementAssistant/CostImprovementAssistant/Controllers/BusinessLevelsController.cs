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
    public class BusinessLevelsController : Controller
    {
        private ciaEntities db = new ciaEntities();

        // GET: BusinessLevels
        public async Task<ActionResult> Index()
        {
            var businessLevel = db.BusinessLevel.Include(b => b.BusinessSector).Include(b => b.Department);
            return View(await businessLevel.ToListAsync());
        }

        // GET: BusinessLevels/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessLevel businessLevel = await db.BusinessLevel.FindAsync(id);
            if (businessLevel == null)
            {
                return HttpNotFound();
            }
            return View(businessLevel);
        }

        // GET: BusinessLevels/Create
        public ActionResult Create()
        {
            ViewBag.BusinessSectorName = new SelectList(db.BusinessSector, "BusinessSectorName", "BusinessSectorName");
            ViewBag.DepartmentName = new SelectList(db.Department, "DepartmentName", "DepartmentName");
            return View();
        }

        // POST: BusinessLevels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BusinesslevelName,BusinessSectorName,DepartmentName,TrustCode")] BusinessLevel businessLevel)
        {
            if (ModelState.IsValid)
            {
                db.BusinessLevel.Add(businessLevel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BusinessSectorName = new SelectList(db.BusinessSector, "BusinessSectorName", "BusinessSectorName", businessLevel.BusinessSectorName);
            ViewBag.DepartmentName = new SelectList(db.Department, "DepartmentName", "DepartmentName", businessLevel.DepartmentName);
            return View(businessLevel);
        }

        // GET: BusinessLevels/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessLevel businessLevel = await db.BusinessLevel.FindAsync(id);
            if (businessLevel == null)
            {
                return HttpNotFound();
            }
            ViewBag.BusinessSectorName = new SelectList(db.BusinessSector, "BusinessSectorName", "BusinessSectorName", businessLevel.BusinessSectorName);
            ViewBag.DepartmentName = new SelectList(db.Department, "DepartmentName", "DepartmentName", businessLevel.DepartmentName);
            return View(businessLevel);
        }

        // POST: BusinessLevels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BusinesslevelName,BusinessSectorName,DepartmentName,TrustCode")] BusinessLevel businessLevel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(businessLevel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BusinessSectorName = new SelectList(db.BusinessSector, "BusinessSectorName", "BusinessSectorName", businessLevel.BusinessSectorName);
            ViewBag.DepartmentName = new SelectList(db.Department, "DepartmentName", "DepartmentName", businessLevel.DepartmentName);
            return View(businessLevel);
        }

        // GET: BusinessLevels/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessLevel businessLevel = await db.BusinessLevel.FindAsync(id);
            if (businessLevel == null)
            {
                return HttpNotFound();
            }
            return View(businessLevel);
        }

        // POST: BusinessLevels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            BusinessLevel businessLevel = await db.BusinessLevel.FindAsync(id);
            db.BusinessLevel.Remove(businessLevel);
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
