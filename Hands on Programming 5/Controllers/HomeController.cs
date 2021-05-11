using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hands_on_Programming_5.Controllers
{
    public class HomeController : Controller
    {
        VolunteerDBEntities db = new VolunteerDBEntities();
        // GET: Index
        public ActionResult Index(string searchVolunteer, string sortOrder, int? page, string currentFilter)
        {
            try
            {


                List<VolunteerList> volunteerLists = db.VolunteerLists.ToList();
                ViewBag.NoSearch = false;

                if (searchVolunteer != null)
                {
                    page = 1;
                }
                else
                {
                    searchVolunteer = currentFilter;
                }

                ViewBag.CurrentFilter = searchVolunteer;
                volunteerLists = Sorting(volunteerLists, sortOrder, searchVolunteer);
                int pageSize = 10;
                int pageNumber = (page ?? 1);



                return View(volunteerLists.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                TempData["ToastrMsg"] = string.Format("showToastrMsg('{0}','{1}')", "error", ex.InnerException.Message.ToString());
                return RedirectToAction("Index", "Home");
            }
        }

        private List<VolunteerList> Sorting(List<VolunteerList> volunteerLists, string sortOrder, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                volunteerLists = volunteerLists.Where(x => x.Firstname.ToLower().Contains(searchString.ToLower())).ToList();

                if (volunteerLists.Count == 0)
                {
                    ViewBag.SearchMessage = "No data found.";
                    ViewBag.NoSearch = true;
                }
            }
            ViewBag.CurrentSort = sortOrder;
            ViewBag.Lastname = String.IsNullOrEmpty(sortOrder) ? "Lastname_Desc" : "";

            switch (sortOrder)
            {
                case "Lastname_Desc":
                    volunteerLists = volunteerLists.OrderByDescending(x => x.Lastname).ToList();
                    break;

                default:
                    volunteerLists = volunteerLists.OrderBy(x => x.Lastname).ToList();
                    break;
            }
            return volunteerLists;
        }

        private void LoadData()
        {
            var gender = new SelectList(db.lib_sex.ToList().OrderBy(m => m.sex_id), "sex_id", "name");
            ViewData["gender"] = gender;

            var region = new SelectList(db.lib_regions.ToList().OrderBy(m => m.region_code), "region_code", "region_name");
            ViewData["region"] = region;

        }

        private static int CalculateAge(DateTime dateOfBirth)
        {

            int age = DateTime.Now.Year - dateOfBirth.Year;

            if (DateTime.Now.Month < dateOfBirth.Month || (DateTime.Now.Month == dateOfBirth.Month && DateTime.Now.Day < dateOfBirth.Day))
                age--;

            return age;
        }

        // GET: Home/New
        [HttpGet]
        public ActionResult New()
        {
            LoadData();
            return View();
        }

        // POST: Home/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(VolunteerList volunteer)
        {

            try
            {

                LoadData();

                if (ModelState.IsValid)
                {

                    VolunteerList newVolunteer = new VolunteerList()
                    {
                        Firstname = volunteer.Firstname,
                        Lastname = volunteer.Lastname,
                        Age = CalculateAge((DateTime)volunteer.Birthdate),
                        Birthdate = volunteer.Birthdate,
                        sex_id = volunteer.sex_id,
                        region_code = volunteer.region_code

                    };


                    db.VolunteerLists.Add(newVolunteer);
                    db.SaveChanges();


                    TempData["ToastrMsg"] = "showToastrMsg('success','Record saved.')";

                    return RedirectToAction("Index", "Home");


                }
            }
            catch (Exception ex)
            {

                TempData["ToastrMsg"] = string.Format("showToastrMsg('{0}','{1}')", "error", ex.Message.ToString());
            }


            return View(volunteer);
        }

        // GET: Home/Edit
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpNotFoundResult();
            }

            VolunteerList volunteerList = db.VolunteerLists.Find(id);
            if (volunteerList == null)
            {
                return new HttpNotFoundResult();
            }

            LoadData();

            return View(volunteerList);


        }

        // POST: Home/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VolunteerList volunteer)
        {

            try
            {
                LoadData();
                VolunteerList updateVolunteer = db.VolunteerLists.Find(volunteer.VolunteerId);

                if (updateVolunteer == null)
                {
                    return new HttpNotFoundResult();
                }


                if (ModelState.IsValid)
                {

                    updateVolunteer.Firstname = volunteer.Firstname;
                    updateVolunteer.Lastname = volunteer.Lastname;
                    updateVolunteer.Age = CalculateAge((DateTime)volunteer.Birthdate);
                    updateVolunteer.Birthdate = volunteer.Birthdate;
                    updateVolunteer.sex_id = volunteer.sex_id;
                    updateVolunteer.region_code = volunteer.region_code;



                    db.Entry(updateVolunteer).State = EntityState.Modified;
                    db.SaveChanges();


                    TempData["ToastrMsg"] = "showToastrMsg('success','Record updated.')";

                    return RedirectToAction("Index", "Home");


                }
            }
            catch (Exception ex)
            {

                TempData["ToastrMsg"] = string.Format("showToastrMsg('{0}','{1}')", "error", ex.Message.ToString());
            }


            return View(volunteer);
        }

        // GET: Home/Delete
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpNotFoundResult();
            }

            VolunteerList volunteerList = db.VolunteerLists.Find(id);
            if (volunteerList == null)
            {
                return new HttpNotFoundResult();
            }

            LoadData();

            return View(volunteerList);


        }

        // POST: Home/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(VolunteerList volunteer)
        {
            VolunteerList deleteVolunteer = db.VolunteerLists.Find(volunteer.VolunteerId);

            try
            {

                db.VolunteerLists.Remove(deleteVolunteer);
                db.SaveChanges();

                TempData["ToastrMsg"] = "showToastrMsg('success','Record deleted.')";
                return RedirectToAction("Index", "Home");
            }

            catch (Exception ex)
            {

                TempData["ToastrMsg"] = string.Format("showToastrMsg('{0}','{1}')", "warning", "Unable to delete. Try again, and if the problem persists contact your system administrator." + ex.Message);
                return View(deleteVolunteer);
            }



        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}