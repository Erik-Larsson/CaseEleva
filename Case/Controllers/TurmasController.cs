using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Case;
using PagedList;

namespace Case.Controllers
{
    public class TurmasController : Controller
    {
        private Context db = new Context();

        // GET: Turmas
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
			ViewBag.CurrentSort = sortOrder;
			ViewBag.TurmaSort = String.IsNullOrEmpty(sortOrder) ? "id_escola" : "";
			ViewBag.EscolaSort = String.IsNullOrEmpty(sortOrder) ? "nome_escola" : "";

			if (searchString != null)
			{
				page = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewBag.CurrentFilter = searchString;

			var turma = from s in db.Turma
						select s;
			if (!String.IsNullOrEmpty(searchString))
			{
				turma = turma.Where(s => s.nome_turma.Contains(searchString));
			}
			switch (sortOrder)
			{
				case "nome_escola":
					turma = turma.OrderByDescending(s => s.nome_turma);
					break;
				case "id_escola":
					turma = turma.OrderByDescending(s => s.Escola.nome_escola);
					break;
				default:  // Name ascending 
					turma = turma.OrderBy(s => s.id_turma);
					break;
			}

			int pageSize = 5;
			int pageNumber = (page ?? 1);

			ViewData["Turma"] = turma;
			ViewData["emptyTurma"] = new Turma();
			ViewData["id_escola"] = new SelectList(db.Escola, "id_escola", "nome_escola");
			return View(turma.ToPagedList(pageNumber,pageSize));
		}


        public ActionResult Create()
        {
			ViewBag.id_escola = new SelectList(db.Escola, "id_escola", "nome_escola");
			return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_turma, id_escola,nome_turma")] Turma turma)
        {
            if (ModelState.IsValid)
            {
                db.Turma.Add(turma);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_escola = new SelectList(db.Escola, "id_escola", "nome_escola", turma.id_escola);
            return View(turma);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turma turma = db.Turma.Find(id);
            if (turma == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_escola = new SelectList(db.Escola, "id_escola", "nome_escola", turma.id_escola);
			return PartialView(turma);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_turma,id_escola,nome_turma")] Turma turma)
        {
            if (ModelState.IsValid)
            {
                db.Entry(turma).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_escola = new SelectList(db.Escola, "id_escola", "nome_escola", turma.id_escola);
            return PartialView(turma);
        }

		

		[HttpPost]
		public ActionResult MultipleDelete(string dataJson)
		{
			try
			{
				var listId = new JavaScriptSerializer().Deserialize<List<int>>(dataJson);
				if (listId.Count == 0)
				{
					//return Json(new { success = false, JsonRequestBehavior.AllowGet });
				}
				foreach (var item in listId)
				{
					Turma turma = db.Turma.Find(item);
					db.Turma.Remove(turma);
				}

				db.SaveChanges();
				return RedirectToAction("Index", "Turmas");
				//return Json(new { success = true, JsonRequestBehavior.AllowGet });
			}
			catch (Exception ex)
			{
				throw ex;
			}
			
		}

		public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turma turma = db.Turma.Find(id);
            if (turma == null)
            {
                return HttpNotFound();
            }
            return PartialView(turma);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Turma turma = db.Turma.Find(id);
            db.Turma.Remove(turma);
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
