using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CostImprovementAssistant.Controllers
{
    public class SchemeKpiasController : Controller
    {
        private ciaEntities db = new ciaEntities();

        // GET: SchemeKpias
        public async Task<ActionResult> Index()
        {
            var schemeKpia = db.SchemeKpia.Include(s => s.CiaForm).Include(s => s.KpiaSourcesReporting);
            return View(await schemeKpia.ToListAsync());
        }

        // GET: SchemeKpias/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchemeKpia schemeKpia = await db.SchemeKpia.FindAsync(id);
            if (schemeKpia == null)
            {
                return HttpNotFound();
            }
            return View(schemeKpia);
        }

        // GET: SchemeKpias/Create
        public ActionResult Create()
        {
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName");
            ViewBag.KPIAssurances = new SelectList(db.KpiaSourcesReporting, "KPIAssurance", "KPIAssurance");
            return View();
        }

        // POST: SchemeKpias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SchemeID,KPIAssurances,TrustCode")] SchemeKpia schemeKpia)
        {
            if (ModelState.IsValid)
            {
                db.SchemeKpia.Add(schemeKpia);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", schemeKpia.SchemeID);
            ViewBag.KPIAssurances = new SelectList(db.KpiaSourcesReporting, "KPIAssurance", "KPIAssurance", schemeKpia.KPIAssurances);
            return View(schemeKpia);
        }

        // GET: SchemeKpias/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchemeKpia schemeKpia = await db.SchemeKpia.FindAsync(id);
            if (schemeKpia == null)
            {
                return HttpNotFound();
            }
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", schemeKpia.SchemeID);
            ViewBag.KPIAssurances = new SelectList(db.KpiaSourcesReporting, "KPIAssurance", "KPIAssurance", schemeKpia.KPIAssurances);
            return View(schemeKpia);
        }

        // POST: SchemeKpias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SchemeID,KPIAssurances,TrustCode")] SchemeKpia schemeKpia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schemeKpia).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", schemeKpia.SchemeID);
            ViewBag.KPIAssurances = new SelectList(db.KpiaSourcesReporting, "KPIAssurance", "KPIAssurance", schemeKpia.KPIAssurances);
            return View(schemeKpia);
        }

        // GET: SchemeKpias/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchemeKpia schemeKpia = await db.SchemeKpia.FindAsync(id);
            if (schemeKpia == null)
            {
                return HttpNotFound();
            }
            return View(schemeKpia);
        }

        // POST: SchemeKpias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SchemeKpia schemeKpia = await db.SchemeKpia.FindAsync(id);
            db.SchemeKpia.Remove(schemeKpia);
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
