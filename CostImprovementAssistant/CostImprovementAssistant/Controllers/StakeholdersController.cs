using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CostImprovementAssistant.Controllers
{
    public class StakeholdersController : Controller
    {
        private ciaEntities db = new ciaEntities();

        // GET: Stakeholders
        public async Task<ActionResult> Index()
        {
            var stakeholders = db.Stakeholders.Include(s => s.CiaForm);
            return View(await stakeholders.ToListAsync());
        }

        // GET: Stakeholders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stakeholders stakeholders = await db.Stakeholders.FindAsync(id);
            if (stakeholders == null)
            {
                return HttpNotFound();
            }
            return View(stakeholders);
        }

        // GET: Stakeholders/Create
        public ActionResult Create()
        {
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName");
            return View();
        }

        // POST: Stakeholders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SchemeID,StakeholdersID,StakeholdersDetail,Impact,TrustCode")] Stakeholders stakeholders)
        {
            if (ModelState.IsValid)
            {
                db.Stakeholders.Add(stakeholders);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", stakeholders.SchemeID);
            return View(stakeholders);
        }

        // GET: Stakeholders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stakeholders stakeholders = await db.Stakeholders.FindAsync(id);
            if (stakeholders == null)
            {
                return HttpNotFound();
            }
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", stakeholders.SchemeID);
            return View(stakeholders);
        }

        // POST: Stakeholders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SchemeID,StakeholdersID,StakeholdersDetail,Impact,TrustCode")] Stakeholders stakeholders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stakeholders).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", stakeholders.SchemeID);
            return View(stakeholders);
        }

        // GET: Stakeholders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stakeholders stakeholders = await db.Stakeholders.FindAsync(id);
            if (stakeholders == null)
            {
                return HttpNotFound();
            }
            return View(stakeholders);
        }

        // POST: Stakeholders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Stakeholders stakeholders = await db.Stakeholders.FindAsync(id);
            db.Stakeholders.Remove(stakeholders);
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
