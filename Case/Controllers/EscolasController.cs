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
    public class EscolasController : Controller
    {
        private Context db = new Context();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
			ViewBag.CurrentSort = sortOrder;
			ViewBag.EscolaSort = String.IsNullOrEmpty(sortOrder) ? "id_escola" : "";
			ViewBag.PaisSort = String.IsNullOrEmpty(sortOrder) ? "pais" : "";
			ViewBag.EstadoSort = String.IsNullOrEmpty(sortOrder) ? "estado" : "";
			ViewBag.CidadeSort = String.IsNullOrEmpty(sortOrder) ? "cidade" : "";
			ViewBag.EnderecoSort = String.IsNullOrEmpty(sortOrder) ? "endereco" : "";

			if (searchString != null)
			{
				page = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewBag.CurrentFilter = searchString;
			var escola = from s in db.Escola
						select s;
			if (!String.IsNullOrEmpty(searchString))
			{
				escola = escola.Where(s => s.nome_escola.Contains(searchString));
			}
			switch (sortOrder)
			{
				case "nome_escola":
					escola = escola.OrderByDescending(s => s.nome_escola);
					break;
				case "pais":
					escola = escola.OrderByDescending(s => s.Pais);
					break;
				case "estado":
					escola = escola.OrderByDescending(s => s.Estado);
					break;
				case "cidade":
					escola = escola.OrderByDescending(s => s.Cidade);
					break;
				case "endereco":
					escola = escola.OrderByDescending(s => s.Endereco);
					break;
				default:  // Name ascending 
					escola = escola.OrderBy(s => s.id_escola);
					break;
			}

			int pageSize = 5;
			int pageNumber = (page ?? 1);

			var esc = new Escola();
			ViewBag.Escola = esc;
			ViewData["Escola"] = esc;
            return View(escola.ToPagedList(pageNumber, pageSize));
        }


		[HttpGet]
		public ActionResult Create()
        {
			var model = new Escola();
			return PartialView(model);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_escola, nome_escola,pais,estado,cidade,endereco")] Escola escola)
        {
            if (ModelState.IsValid)
            {
                db.Escola.Add(escola);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(escola);
        }

		[HttpGet]
		public ActionResult Edit(int? id)
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
			return PartialView(escola);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_escola, nome_escola,pais,estado,cidade,endereco")] Escola escola)
        {
            if (ModelState.IsValid)
            {
                db.Entry(escola).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(escola);
        }

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
            return PartialView(escola);
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

		[HttpPost]
		public ActionResult MultipleDelete(string dataJson)
		{
			try
			{
				var listId = new JavaScriptSerializer().Deserialize<List<int>>(dataJson);
				if (listId.Count == 0)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				foreach (var item in listId)
				{
					Escola escola = db.Escola.Find(item);
					db.Escola.Remove(escola);
				}

				db.SaveChanges();
				return RedirectToAction("Index", "Escolas");
				//return Json(new { success = true, JsonRequestBehavior.AllowGet });
			}
			catch (Exception ex)
			{
				throw ex;
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
