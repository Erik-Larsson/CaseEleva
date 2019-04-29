using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Case;

namespace Case.Controllers
{
    public class EscolasController : Controller
    {
        private Context db = new Context();

        // GET: Escolas
        public ActionResult Index()
        {
			var esc = new Escola();
			ViewBag.Escola = esc;
			ViewData["Escola"] = esc;
            return View(db.Escola.ToList());
        }

        // GET: Escolas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Escola escola = db.Escola.Find(id);
            if (escola == null)
            {
                return HttpNotFound();
            }
            return View(escola);
        }

        // GET: Escolas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Escolas/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "nome_escola")] Escola escola)
        {
            if (ModelState.IsValid)
            {
                db.Escola.Add(escola);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(escola);
        }

		// GET: Escolas/Edit/5
		[HttpGet]
		public ActionResult Edit(int? id)
        {
			var model = new Escola();
			return PartialView(model);
			//if (id == null)
			//{
			//    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			//}
			//Escola escola = db.Escola.Find(id);
			//if (escola == null)
			//{
			//    return HttpNotFound();
			//}
			//return View(escola);
		}

        // POST: Escolas/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "nome_escola")] Escola escola)
        {
            if (ModelState.IsValid)
            {
                db.Entry(escola).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(escola);
        }

        // GET: Escolas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Escola escola = db.Escola.Find(id);
            if (escola == null)
            {
                return HttpNotFound();
            }
            return View(escola);
        }

        // POST: Escolas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Escola escola = db.Escola.Find(id);
            db.Escola.Remove(escola);
            db.SaveChanges();
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
