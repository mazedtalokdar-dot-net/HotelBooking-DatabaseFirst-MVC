using _1291163_MVC_Project.Models;
using _1291163_MVC_Project.Models.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _1291163_MVC_Project.Controllers
{
    public class GuestsController : Controller
    {
        private MSDBContext db = new MSDBContext();

        public ActionResult Index(int? page)
        {
            int pageSize = 6;
            int pageNumber = page ?? 1;

            var data = db.guests
                         .OrderByDescending(x => x.guestsId)
                         .ToPagedList(pageNumber, pageSize);

            return View(data);
        }
        public ActionResult Details(int? id)
        {
            var data = db.guests
                         .FirstOrDefault(x => x.guestsId == id);
                         

            return View(data);
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult AddNewRoomRow(int? id)
        {
            ViewBag.rooms = new SelectList(db.rooms.ToList(), "roomId", "roomName", (id != null) ? id.ToString() : "");
            return PartialView("_addNewRoom");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GuestVM guestVM, int[] roomId)
        {
            if (ModelState.IsValid)
            {
                guests g = new guests()
                // Guest (Master)

                {
                    guestsName = guestVM.guestsName,
                    bookingDate = guestVM.bookingDate,
                    numberOfGueist = guestVM.numberOfGueist,
                    coupleStatus = guestVM.coupleStatus

                };

                // Image Upload 
                HttpPostedFileBase file = guestVM.pictureFile;
                if (file != null && file.ContentLength > 0)
                {
                    string filePath = Path.Combine("/Images/",
                        DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName));

                    file.SaveAs(Server.MapPath(filePath));
                    g.picture = filePath;
                }

                // If no room selected, show error
                if (roomId == null || roomId.Length == 0)
                {
                    ModelState.AddModelError("", "Please select at least one room.");
                    return PartialView("_error");
                }

                foreach (var rid in roomId)
                {
                    bookingEntry be = new bookingEntry()
                    {
                        guests = g,   
                        roomId = rid
                    };
                    db.bookingEntries.Add(be);
                }

                db.SaveChanges();
                return PartialView("_success");
            }

            return PartialView("_error");
        }
        public ActionResult Edit(int? id)
        {
            if (id == null) return HttpNotFound();

            // Guest master
            guests g = db.guests.FirstOrDefault(x => x.guestsId == id);
            if (g == null) return HttpNotFound();


            var guestRooms = db.bookingEntries
                              .Where(x => x.guestsId == g.guestsId)
                              .ToList();

            // VM mapping
            GuestVM guestVM = new GuestVM()
            {
                guestsId = g.guestsId,
                guestsName = g.guestsName,
                bookingDate = g.bookingDate,
                numberOfGueist = g.numberOfGueist,
                picture = g.picture,
                coupleStatus = g.coupleStatus
            };

            // keep previous image if new not uploaded
            Session["imPath"] = g.picture;

            // Selected Rooms list
            if (guestRooms.Count > 0)
            {
                foreach (var item in guestRooms)
                {
                    guestVM.RoomList.Add(item.roomId);
                }
            }

            return View(guestVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GuestVM guestVM, int[] roomId)
        {
            if (ModelState.IsValid)
            {
                guests g = new guests()
                {
                    guestsId = guestVM.guestsId,
                    guestsName = guestVM.guestsName,
                    bookingDate = guestVM.bookingDate,
                    numberOfGueist = guestVM.numberOfGueist,
                    coupleStatus = guestVM.coupleStatus
                };

                // Image handling
                HttpPostedFileBase file = guestVM.pictureFile;
                var oldPic = guestVM.picture; // hidden field from view

                if (file != null && file.ContentLength > 0)
                {

                    string folderPath = Server.MapPath("~/Images");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string fileName = DateTime.Now.Ticks + Path.GetExtension(file.FileName);
                    string savePath = Path.Combine(folderPath, fileName);

                    file.SaveAs(savePath);
                    g.picture = "/Images/" + fileName;
                }
                else
                {
                    g.picture = oldPic;
                }

                // Remove old room bookings
                var oldEntries = db.bookingEntries
                                  .Where(x => x.guestsId == g.guestsId)
                                  .ToList();

                foreach (var be in oldEntries)
                {
                    db.bookingEntries.Remove(be);
                }

                // Add new room bookings
                if (roomId != null && roomId.Length > 0)
                {
                    foreach (var rid in roomId)
                    {
                        bookingEntry be = new bookingEntry()
                        {
                            guestsId = g.guestsId,
                            roomId = rid
                        };
                        db.bookingEntries.Add(be);
                    }
                }
                else
                {
                   
                    ModelState.AddModelError("", "Please select at least one room.");
                    return PartialView("_error");
                }

                db.Entry(g).State = EntityState.Modified;
                db.SaveChanges();

                return PartialView("_success");
            }

            return PartialView("_error");
        }
        public ActionResult Delete(int? id)
        {
            if (id == null) return HttpNotFound();

            var g = db.guests.FirstOrDefault(x => x.guestsId == id);
            if (g == null) return HttpNotFound();


            var guestRooms = db.bookingEntries
                               .Where(x => x.guestsId == g.guestsId)
                               .ToList();

            GuestVM vm = new GuestVM()
            {
                guestsId = g.guestsId,
                guestsName = g.guestsName,
                bookingDate = g.bookingDate,
                numberOfGueist = g.numberOfGueist,
                picture = g.picture,
                coupleStatus = g.coupleStatus
            };

            
            foreach (var be in guestRooms)
            {
                vm.RoomList.Add(be.roomId);

                if (be.room != null)
                    vm.RoomNames.Add(be.room.roomName);
            }

            return View(vm);
        }


        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            guests g = db.guests.Find(id);
            if (g == null)
            {
                return HttpNotFound();
            }

            var roomEntries = db.bookingEntries
                                .Where(x => x.guestsId == g.guestsId)
                                .ToList();

            foreach (var be in roomEntries)
            {
                db.bookingEntries.Remove(be);
            }

            // Delete guest
            db.Entry(g).State = EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}