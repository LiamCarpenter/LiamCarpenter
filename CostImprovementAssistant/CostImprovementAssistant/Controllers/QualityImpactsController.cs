using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CostImprovementAssistant.Controllers
{
    public class QualityImpactsController : Controller
    {
        private ciaEntities db = new ciaEntities();

        // GET: QualityImpacts
        public async Task<ActionResult> Index()
        {
            var qualityImpact = db.QualityImpact.Include(q => q.CiaForm);
            return View(await qualityImpact.ToListAsync());
        }

        // GET: QualityImpacts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QualityImpact qualityImpact = await db.QualityImpact.FindAsync(id);
            if (qualityImpact == null)
            {
                return HttpNotFound();
            }
            return View(qualityImpact);
        }

        // GET: QualityImpacts/Create
        public ActionResult Create()
        {
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName");
            return View();
        }

        // POST: QualityImpacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SchemeID,ImpactDate,CompletingQi,Comments,OverallRiskScore,ReviewedBy,Equality,ConsiderationComments,EvidenceComments,ManagerDate,ManagerStatus,ManagerStatusComments,EoDate,EoStatus,EoStatuscomments,TrustCode")] QualityImpact qualityImpact)
        {
            if (ModelState.IsValid)
            {
                db.QualityImpact.Add(qualityImpact);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", qualityImpact.SchemeID);
            return View(qualityImpact);
        }

        // GET: QualityImpacts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QualityImpact qualityImpact = await db.QualityImpact.FindAsync(id);
            if (qualityImpact == null)
            {
                return HttpNotFound();
            }
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", qualityImpact.SchemeID);
            return View(qualityImpact);
        }

        // POST: QualityImpacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SchemeID,ImpactDate,CompletingQi,Comments,OverallRiskScore,ReviewedBy,Equality,ConsiderationComments,EvidenceComments,ManagerDate,ManagerStatus,ManagerStatusComments,EoDate,EoStatus,EoStatuscomments,TrustCode")] QualityImpact qualityImpact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(qualityImpact).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SchemeID = new SelectList(db.CiaForm, "SchemeID", "SchemeName", qualityImpact.SchemeID);
            return View(qualityImpact);
        }

        // GET: QualityImpacts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QualityImpact qualityImpact = await db.QualityImpact.FindAsync(id);
            if (qualityImpact == null)
            {
                return HttpNotFound();
            }
            return View(qualityImpact);
        }

        // POST: QualityImpacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            QualityImpact qualityImpact = await db.QualityImpact.FindAsync(id);
            db.QualityImpact.Remove(qualityImpact);
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
