using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CostImprovementAssistant.Controllers
{
    public class IssuesController : Controller
    {
        private ciaEntities db = new ciaEntities();

        // GET: Issues
        public async Task<ActionResult> Index()
        {
            var issues = db.Issues.Include(i => i.CiaForm).Include(i => i.Impact1);
            return View(await issues.ToListAsync());
        }

        // GET: Issues/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issues issues = await db.Issues.FindAsync(id);
            if (issues == null)
            {
                return HttpNotFound();
            }
            return View(issues);
        }

        // GET: Issues/Create
        public ActionResult Create()
        {
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName");
            ViewBag.Impact = new SelectList(db.Impact, "ImpactTitle", "ImpactTitle");
            return View();
        }

        // POST: Issues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SchemeID,IssueID,IssueDetail,Impact,TrustCode")] Issues issues)
        {
            if (ModelState.IsValid)
            {
                db.Issues.Add(issues);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", issues.SchemeID);
            ViewBag.Impact = new SelectList(db.Impact, "ImpactTitle", "ImpactTitle", issues.Impact);
            return View(issues);
        }

        // GET: Issues/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issues issues = await db.Issues.FindAsync(id);
            if (issues == null)
            {
                return HttpNotFound();
            }
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", issues.SchemeID);
            ViewBag.Impact = new SelectList(db.Impact, "ImpactTitle", "ImpactTitle", issues.Impact);
            return View(issues);
        }

        // POST: Issues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SchemeID,IssueID,IssueDetail,Impact,TrustCode")] Issues issues)
        {
            if (ModelState.IsValid)
            {
                db.Entry(issues).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", issues.SchemeID);
            ViewBag.Impact = new SelectList(db.Impact, "ImpactTitle", "ImpactTitle", issues.Impact);
            return View(issues);
        }

        // GET: Issues/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issues issues = await db.Issues.FindAsync(id);
            if (issues == null)
            {
                return HttpNotFound();
            }
            return View(issues);
        }

        // POST: Issues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Issues issues = await db.Issues.FindAsync(id);
            db.Issues.Remove(issues);
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
