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

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
			ViewBag.CurrentSort = sortOrder;
			ViewBag.TurmaSort = String.IsNullOrEmpty(sortOrder) ? "id_escola" : "";
			ViewBag.EscolaSort = String.IsNullOrEmpty(sortOrder) ? "nome_escola" : "";
			ViewBag.QtdAlunosSort = String.IsNullOrEmpty(sortOrder) ? "Quantidade_Alunos" : "";
			ViewBag.QtdProfessoresSort = String.IsNullOrEmpty(sortOrder) ? "Quantidade_Professores" : "";

			if (searchString != null)
				page = 1;			
			else			
				searchString = currentFilter;
			

			ViewBag.CurrentFilter = searchString;

			var turma = from s in db.Turma
						select s;

			if (!String.IsNullOrEmpty(searchString))
				turma = turma.Where(s => s.nome_turma.Contains(searchString));
			
			switch (sortOrder)
			{
				case "nome_escola":
					turma = turma.OrderBy(s => s.nome_turma);
					break;
				case "id_escola":
					turma = turma.OrderBy(s => s.Escola.nome_escola);
					break;
				case "Quantidade_Alunos":
					turma = turma.OrderBy(s => s.Quantidade_Alunos);
					break;
				case "Quantidade_Professores":
					turma = turma.OrderBy(s => s.Quantidade_Professores);
					break;
				default: 
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
        public ActionResult Create([Bind(Include = "id_turma, id_escola,nome_turma,Quantidade_Alunos, Quantidade_Professores")] Turma turma)
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
        public ActionResult Edit([Bind(Include = "id_turma,id_escola,nome_turma,Quantidade_Alunos, Quantidade_Professores")] Turma turma)
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
					return Json(new { retorno = false, JsonRequestBehavior.AllowGet });
				}
				foreach (var item in listId)
				{
					Turma turma = db.Turma.Find(item);
					db.Turma.Remove(turma);
				}

				db.SaveChanges();
				return Json(new { retorno = true, JsonRequestBehavior.AllowGet });
			}
			catch (Exception ex)
			{
				return Json(new { retorno = false, JsonRequestBehavior.AllowGet });
			}
			
		}

		public ActionResult Delete(int? id)
        {
			try
			{ 
				if (id == null)
				{
					return RedirectToAction("Index");
				}
				Turma turma = db.Turma.Find(id);
				if (turma == null)
				{
					return HttpNotFound();
				}
				return PartialView(turma);
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index");
			}
		}

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			try
			{
				Turma turma = db.Turma.Find(id);
				db.Turma.Remove(turma);
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index");
			}
			
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
