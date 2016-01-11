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
    public class BusinessSectorsController : Controller
    {
        private ciaEntities db = new ciaEntities();

        // GET: BusinessSectors
        public async Task<ActionResult> Index()
        {
            return View(await db.BusinessSector.ToListAsync());
        }

        // GET: BusinessSectors/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessSector businessSector = await db.BusinessSector.FindAsync(id);
            if (businessSector == null)
            {
                return HttpNotFound();
            }
            return View(businessSector);
        }

        // GET: BusinessSectors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BusinessSectors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BusinessSectorName,TargetValue,TrustCode")] BusinessSector businessSector)
        {
            if (ModelState.IsValid)
            {
                db.BusinessSector.Add(businessSector);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(businessSector);
        }

        // GET: BusinessSectors/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessSector businessSector = await db.BusinessSector.FindAsync(id);
            if (businessSector == null)
            {
                return HttpNotFound();
            }
            return View(businessSector);
        }

        // POST: BusinessSectors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BusinessSectorName,TargetValue,TrustCode")] BusinessSector businessSector)
        {
            if (ModelState.IsValid)
            {
                db.Entry(businessSector).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(businessSector);
        }

        // GET: BusinessSectors/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessSector businessSector = await db.BusinessSector.FindAsync(id);
            if (businessSector == null)
            {
                return HttpNotFound();
            }
            return View(businessSector);
        }

        // POST: BusinessSectors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            BusinessSector businessSector = await db.BusinessSector.FindAsync(id);
            db.BusinessSector.Remove(businessSector);
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
