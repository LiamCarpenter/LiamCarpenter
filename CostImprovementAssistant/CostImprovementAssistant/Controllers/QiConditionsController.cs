using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CostImprovementAssistant.Controllers
{
    public class QiConditionsController : Controller
    {
        private ciaEntities db = new ciaEntities();

        // GET: QiConditions
        public async Task<ActionResult> Index()
        {
            return View(await db.QiConditions.ToListAsync());
        }

        // GET: QiConditions/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QiConditions qiConditions = await db.QiConditions.FindAsync(id);
            if (qiConditions == null)
            {
                return HttpNotFound();
            }
            return View(qiConditions);
        }

        // GET: QiConditions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QiConditions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ConditionTitle,TrustCode")] QiConditions qiConditions)
        {
            if (ModelState.IsValid)
            {
                db.QiConditions.Add(qiConditions);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(qiConditions);
        }

        // GET: QiConditions/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QiConditions qiConditions = await db.QiConditions.FindAsync(id);
            if (qiConditions == null)
            {
                return HttpNotFound();
            }
            return View(qiConditions);
        }

        // POST: QiConditions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ConditionTitle,TrustCode")] QiConditions qiConditions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(qiConditions).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(qiConditions);
        }

        // GET: QiConditions/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QiConditions qiConditions = await db.QiConditions.FindAsync(id);
            if (qiConditions == null)
            {
                return HttpNotFound();
            }
            return View(qiConditions);
        }

        // POST: QiConditions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            QiConditions qiConditions = await db.QiConditions.FindAsync(id);
            db.QiConditions.Remove(qiConditions);
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
