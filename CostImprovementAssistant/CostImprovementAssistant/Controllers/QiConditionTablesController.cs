using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CostImprovementAssistant.Controllers
{
    public class QiConditionTablesController : Controller
    {
        private ciaEntities db = new ciaEntities();

        // GET: QiConditionTables
        public async Task<ActionResult> Index()
        {
            var qiConditionTable = db.QiConditionTable.Include(q => q.QiConditions);
            return View(await qiConditionTable.ToListAsync());
        }

        // GET: QiConditionTables/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QiConditionTable qiConditionTable = await db.QiConditionTable.FindAsync(id);
            if (qiConditionTable == null)
            {
                return HttpNotFound();
            }
            return View(qiConditionTable);
        }

        // GET: QiConditionTables/Create
        public ActionResult Create()
        {
            ViewBag.ConditionTitle = new SelectList(db.QiConditions, "ConditionTitle", "ConditionTitle");
            return View();
        }

        // POST: QiConditionTables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SchemeID,ConditionTitle,ConditionDetail,Mittigation,Consiquence,Likelyhood,Score,TrustCode")] QiConditionTable qiConditionTable)
        {
            if (ModelState.IsValid)
            {
                db.QiConditionTable.Add(qiConditionTable);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ConditionTitle = new SelectList(db.QiConditions, "ConditionTitle", "ConditionTitle", qiConditionTable.ConditionTitle);
            return View(qiConditionTable);
        }

        // GET: QiConditionTables/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QiConditionTable qiConditionTable = await db.QiConditionTable.FindAsync(id);
            if (qiConditionTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.ConditionTitle = new SelectList(db.QiConditions, "ConditionTitle", "ConditionTitle", qiConditionTable.ConditionTitle);
            return View(qiConditionTable);
        }

        // POST: QiConditionTables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SchemeID,ConditionTitle,ConditionDetail,Mittigation,Consiquence,Likelyhood,Score,TrustCode")] QiConditionTable qiConditionTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(qiConditionTable).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ConditionTitle = new SelectList(db.QiConditions, "ConditionTitle", "ConditionTitle", qiConditionTable.ConditionTitle);
            return View(qiConditionTable);
        }

        // GET: QiConditionTables/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QiConditionTable qiConditionTable = await db.QiConditionTable.FindAsync(id);
            if (qiConditionTable == null)
            {
                return HttpNotFound();
            }
            return View(qiConditionTable);
        }

        // POST: QiConditionTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            QiConditionTable qiConditionTable = await db.QiConditionTable.FindAsync(id);
            db.QiConditionTable.Remove(qiConditionTable);
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
