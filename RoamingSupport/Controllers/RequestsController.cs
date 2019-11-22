using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using RoamingSupport.DAL;
using RoamingSupport.Models;

namespace RoamingSupport.Controllers
{
    public class RequestsController : Controller
    {
        private RoamingSupportContext db = new RoamingSupportContext();

        // GET: Requests
        public ActionResult Index(string searchString, string filter, string destinationName)
        {
            var requests = from r in db.Requests select r;
            var destinationQry = from r in db.Requests select r.Destination;
            
            var destinationDictionary = GetDestinationDictionary(destinationQry.Distinct());
            var filters = GetFilterList();

            ViewData["destinationName"] = new SelectList(destinationDictionary.Keys);
            ViewData["filter"] = new SelectList(filters);

            if (!String.IsNullOrEmpty(searchString))
            {
                switch (filter)
                {
                    case "Причина":
                        requests = requests.Where(r => r.Reason.Contains(searchString));
                        break;
                    case "Номер абонента":
                        requests = requests.Where(r => r.PhoneNumber.Contains(searchString));
                        break;
                    case "Страна":
                        requests = requests.Where(r => r.Country.Contains(searchString));
                        break;
                    case "Регион":
                        requests = requests.Where(r => r.Region.Contains(searchString));
                        break;
                    case "Населённый пункт":
                        requests = requests.Where(r => r.Locality.Contains(searchString));
                        break;
                }
            }

            if (!String.IsNullOrEmpty(destinationName))
            {
                int value = (int)destinationDictionary[destinationName];
                requests = requests.Where(r => r.Destination == (Models.Request.Destinations)value);
            }

            return View(requests.ToList());
        }

        // GET: Requests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // GET: Requests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Requests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Reason,Destination,PhoneNumber,Country,Region,Locality,RequestDateTime")] Request request)
        {
            if (ModelState.IsValid)
            {
                request.RequestDateTime = DateTime.Now;
                db.Requests.Add(request);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(request);
        }

        // GET: Requests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Requests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Reason,Destination,PhoneNumber,Country,Region,Locality,RequestDateTime")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(request);
        }

        // GET: Requests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Request request = db.Requests.Find(id);
            db.Requests.Remove(request);
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

        private static string GetDisplayName(Enum item)
        {
            var type = item.GetType();
            var member = type.GetMember(item.ToString());
            DisplayAttribute displayname = (DisplayAttribute)member[0].GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

            if (displayname != null)
            {
                return displayname.Name;
            }

            return item.ToString();
        }

        private static List<string> GetFilterList()
        {
            return new List<string>
            {
                "Причина",
                "Номер абонента",
                "Страна",
                "Регион",
                "Населённый пункт"
            };
        }

        private static Dictionary<string, Request.Destinations> GetDestinationDictionary(IEnumerable<Request.Destinations> destinations)
        {
            var result = new Dictionary<string, Request.Destinations>();

            foreach (var dest in destinations)
            {
                var destDisplayName = GetDisplayName(dest);
                var destName = dest;

                result.Add(destDisplayName, destName);
            }

            return result;
        }
    }
}
