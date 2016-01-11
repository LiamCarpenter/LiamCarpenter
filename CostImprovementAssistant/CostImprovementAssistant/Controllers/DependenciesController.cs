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
    public class DependenciesController : Controller
    {
        private ciaEntities db = new ciaEntities();

        // GET: Dependencies
        public async Task<ActionResult> Index()
        {
            var dependencies = db.Dependencies.Include(d => d.Impact1);
            return View(await dependencies.ToListAsync());
        }

        // GET: Dependencies/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dependencies dependencies = await db.Dependencies.FindAsync(id);
            if (dependencies == null)
            {
                return HttpNotFound();
            }
            return View(dependencies);
        }

        // GET: Dependencies/Create
        public ActionResult Create()
        {
            ViewBag.Impact = new SelectList(db.Impact, "ImpactTitle", "ImpactTitle");
            return View();
        }

        // POST: Dependencies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SchemeID,DependenciesID,DependenciesDetail,Impact,TrustCode")] Dependencies dependencies)
        {
            if (ModelState.IsValid)
            {
                db.Dependencies.Add(dependencies);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Impact = new SelectList(db.Impact, "ImpactTitle", "ImpactTitle", dependencies.Impact);
            return View(dependencies);
        }

        // GET: Dependencies/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dependencies dependencies = await db.Dependencies.FindAsync(id);
            if (dependencies == null)
            {
                return HttpNotFound();
            }
            ViewBag.Impact = new SelectList(db.Impact, "ImpactTitle", "ImpactTitle", dependencies.Impact);
            return View(dependencies);
        }

        // POST: Dependencies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SchemeID,DependenciesID,DependenciesDetail,Impact,TrustCode")] Dependencies dependencies)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dependencies).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Impact = new SelectList(db.Impact, "ImpactTitle", "ImpactTitle", dependencies.Impact);
            return View(dependencies);
        }

        // GET: Dependencies/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dependencies dependencies = await db.Dependencies.FindAsync(id);
            if (dependencies == null)
            {
                return HttpNotFound();
            }
            return View(dependencies);
        }

        // POST: Dependencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Dependencies dependencies = await db.Dependencies.FindAsync(id);
            db.Dependencies.Remove(dependencies);
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
