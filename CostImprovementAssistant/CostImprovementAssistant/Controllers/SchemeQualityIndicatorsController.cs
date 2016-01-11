using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CostImprovementAssistant.Controllers
{
    public class SchemeQualityIndicatorsController : Controller
    {
        private ciaEntities db = new ciaEntities();

        // GET: SchemeQualityIndicators
        public async Task<ActionResult> Index()
        {
            var schemeQualityIndicators = db.SchemeQualityIndicators.Include(s => s.CiaForm);
            return View(await schemeQualityIndicators.ToListAsync());
        }

        // GET: SchemeQualityIndicators/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchemeQualityIndicators schemeQualityIndicators = await db.SchemeQualityIndicators.FindAsync(id);
            if (schemeQualityIndicators == null)
            {
                return HttpNotFound();
            }
            return View(schemeQualityIndicators);
        }

        // GET: SchemeQualityIndicators/Create
        public ActionResult Create()
        {
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName");
            return View();
        }

        // POST: SchemeQualityIndicators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SchemeID,QualityIndicatorsTitle,TrustCode")] SchemeQualityIndicators schemeQualityIndicators)
        {
            if (ModelState.IsValid)
            {
                db.SchemeQualityIndicators.Add(schemeQualityIndicators);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", schemeQualityIndicators.SchemeID);
            return View(schemeQualityIndicators);
        }

        // GET: SchemeQualityIndicators/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchemeQualityIndicators schemeQualityIndicators = await db.SchemeQualityIndicators.FindAsync(id);
            if (schemeQualityIndicators == null)
            {
                return HttpNotFound();
            }
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", schemeQualityIndicators.SchemeID);
            return View(schemeQualityIndicators);
        }

        // POST: SchemeQualityIndicators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SchemeID,QualityIndicatorsTitle,TrustCode")] SchemeQualityIndicators schemeQualityIndicators)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schemeQualityIndicators).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", schemeQualityIndicators.SchemeID);
            return View(schemeQualityIndicators);
        }

        // GET: SchemeQualityIndicators/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchemeQualityIndicators schemeQualityIndicators = await db.SchemeQualityIndicators.FindAsync(id);
            if (schemeQualityIndicators == null)
            {
                return HttpNotFound();
            }
            return View(schemeQualityIndicators);
        }

        // POST: SchemeQualityIndicators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SchemeQualityIndicators schemeQualityIndicators = await db.SchemeQualityIndicators.FindAsync(id);
            db.SchemeQualityIndicators.Remove(schemeQualityIndicators);
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
