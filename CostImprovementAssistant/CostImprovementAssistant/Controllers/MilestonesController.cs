using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CostImprovementAssistant.Controllers
{
    public class MilestonesController : Controller
    {
        private ciaEntities db = new ciaEntities();

        // GET: Milestones
        public async Task<ActionResult> Index()
        {
            var milestones = db.Milestones.Include(m => m.CiaForm);
            return View(await milestones.ToListAsync());
        }

        // GET: Milestones/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Milestones milestones = await db.Milestones.FindAsync(id);
            if (milestones == null)
            {
                return HttpNotFound();
            }
            return View(milestones);
        }

        // GET: Milestones/Create
        public ActionResult Create()
        {
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName");
            return View();
        }

        // POST: Milestones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SchemeID,MilestonesID,MilestonesDetail,ExpectedCompletionDate,RevisedDate,RevisionReason,DateOFCompletion,TrustCode")] Milestones milestones)
        {
            if (ModelState.IsValid)
            {
                db.Milestones.Add(milestones);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", milestones.SchemeID);
            return View(milestones);
        }

        // GET: Milestones/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Milestones milestones = await db.Milestones.FindAsync(id);
            if (milestones == null)
            {
                return HttpNotFound();
            }
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", milestones.SchemeID);
            return View(milestones);
        }

        // POST: Milestones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SchemeID,MilestonesID,MilestonesDetail,ExpectedCompletionDate,RevisedDate,RevisionReason,DateOFCompletion,TrustCode")] Milestones milestones)
        {
            if (ModelState.IsValid)
            {
                db.Entry(milestones).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", milestones.SchemeID);
            return View(milestones);
        }

        // GET: Milestones/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Milestones milestones = await db.Milestones.FindAsync(id);
            if (milestones == null)
            {
                return HttpNotFound();
            }
            return View(milestones);
        }

        // POST: Milestones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Milestones milestones = await db.Milestones.FindAsync(id);
            db.Milestones.Remove(milestones);
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
