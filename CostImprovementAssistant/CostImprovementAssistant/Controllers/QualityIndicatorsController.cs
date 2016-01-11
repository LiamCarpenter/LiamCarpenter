using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CostImprovementAssistant.Controllers
{
    public class QualityIndicatorsController : Controller
    {
        private ciaEntities db = new ciaEntities();

        // GET: QualityIndicators
        public async Task<ActionResult> Index()
        {
            return View(await db.QualityIndicators.ToListAsync());
        }

        // GET: QualityIndicators/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QualityIndicators qualityIndicators = await db.QualityIndicators.FindAsync(id);
            if (qualityIndicators == null)
            {
                return HttpNotFound();
            }
            return View(qualityIndicators);
        }

        // GET: QualityIndicators/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QualityIndicators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "QualityIndicatorsTitle")] QualityIndicators qualityIndicators)
        {
            if (ModelState.IsValid)
            {
                db.QualityIndicators.Add(qualityIndicators);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(qualityIndicators);
        }

        // GET: QualityIndicators/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QualityIndicators qualityIndicators = await db.QualityIndicators.FindAsync(id);
            if (qualityIndicators == null)
            {
                return HttpNotFound();
            }
            return View(qualityIndicators);
        }

        // POST: QualityIndicators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "QualityIndicatorsTitle")] QualityIndicators qualityIndicators)
        {
            if (ModelState.IsValid)
            {
                db.Entry(qualityIndicators).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(qualityIndicators);
        }

        // GET: QualityIndicators/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QualityIndicators qualityIndicators = await db.QualityIndicators.FindAsync(id);
            if (qualityIndicators == null)
            {
                return HttpNotFound();
            }
            return View(qualityIndicators);
        }

        // POST: QualityIndicators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            QualityIndicators qualityIndicators = await db.QualityIndicators.FindAsync(id);
            db.QualityIndicators.Remove(qualityIndicators);
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
