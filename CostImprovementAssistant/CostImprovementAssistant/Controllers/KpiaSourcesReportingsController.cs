using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CostImprovementAssistant.Controllers
{
    public class KpiaSourcesReportingsController : Controller
    {
        private ciaEntities db = new ciaEntities();

        // GET: KpiaSourcesReportings
        public async Task<ActionResult> Index()
        {
            return View(await db.KpiaSourcesReporting.ToListAsync());
        }

        // GET: KpiaSourcesReportings/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KpiaSourcesReporting kpiaSourcesReporting = await db.KpiaSourcesReporting.FindAsync(id);
            if (kpiaSourcesReporting == null)
            {
                return HttpNotFound();
            }
            return View(kpiaSourcesReporting);
        }

        // GET: KpiaSourcesReportings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KpiaSourcesReportings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "KPIAssurance,TrustCode")] KpiaSourcesReporting kpiaSourcesReporting)
        {
            if (ModelState.IsValid)
            {
                db.KpiaSourcesReporting.Add(kpiaSourcesReporting);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(kpiaSourcesReporting);
        }

        // GET: KpiaSourcesReportings/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KpiaSourcesReporting kpiaSourcesReporting = await db.KpiaSourcesReporting.FindAsync(id);
            if (kpiaSourcesReporting == null)
            {
                return HttpNotFound();
            }
            return View(kpiaSourcesReporting);
        }

        // POST: KpiaSourcesReportings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "KPIAssurance,TrustCode")] KpiaSourcesReporting kpiaSourcesReporting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kpiaSourcesReporting).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(kpiaSourcesReporting);
        }

        // GET: KpiaSourcesReportings/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KpiaSourcesReporting kpiaSourcesReporting = await db.KpiaSourcesReporting.FindAsync(id);
            if (kpiaSourcesReporting == null)
            {
                return HttpNotFound();
            }
            return View(kpiaSourcesReporting);
        }

        // POST: KpiaSourcesReportings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            KpiaSourcesReporting kpiaSourcesReporting = await db.KpiaSourcesReporting.FindAsync(id);
            db.KpiaSourcesReporting.Remove(kpiaSourcesReporting);
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
