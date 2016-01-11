using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CostImprovementAssistant.Controllers
{
    public class SchemeRisksController : Controller
    {
        private ciaEntities db = new ciaEntities();

        // GET: SchemeRisks
        public async Task<ActionResult> Index()
        {
            var schemeRisks = db.SchemeRisks.Include(s => s.CiaForm);
            return View(await schemeRisks.ToListAsync());
        }

        // GET: SchemeRisks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchemeRisks schemeRisks = await db.SchemeRisks.FindAsync(id);
            if (schemeRisks == null)
            {
                return HttpNotFound();
            }
            return View(schemeRisks);
        }

        // GET: SchemeRisks/Create
        public ActionResult Create()
        {
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName");
            return View();
        }

        // POST: SchemeRisks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SchemeID,RiskID,SchemeRiskDetail,Likelyhood,Impact,Risk,TrustCode")] SchemeRisks schemeRisks)
        {
            if (ModelState.IsValid)
            {
                db.SchemeRisks.Add(schemeRisks);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", schemeRisks.SchemeID);
            return View(schemeRisks);
        }

        // GET: SchemeRisks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchemeRisks schemeRisks = await db.SchemeRisks.FindAsync(id);
            if (schemeRisks == null)
            {
                return HttpNotFound();
            }
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", schemeRisks.SchemeID);
            return View(schemeRisks);
        }

        // POST: SchemeRisks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SchemeID,RiskID,SchemeRiskDetail,Likelyhood,Impact,Risk,TrustCode")] SchemeRisks schemeRisks)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schemeRisks).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", schemeRisks.SchemeID);
            return View(schemeRisks);
        }

        // GET: SchemeRisks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchemeRisks schemeRisks = await db.SchemeRisks.FindAsync(id);
            if (schemeRisks == null)
            {
                return HttpNotFound();
            }
            return View(schemeRisks);
        }

        // POST: SchemeRisks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SchemeRisks schemeRisks = await db.SchemeRisks.FindAsync(id);
            db.SchemeRisks.Remove(schemeRisks);
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
