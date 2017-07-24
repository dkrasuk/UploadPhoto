using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UploadPhoto.Controllers
{
    public class ImageGaleryController : Controller
    {
        // GET: ImageGalery
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Gallery()
        {
            List<ImageGalary> all = new List<ImageGalary>();
            using (DatabasePhotoEntities dc = new DatabasePhotoEntities()) 
            {
                all = dc.ImageGalary.ToList();
            }
            return View(all);
        }
        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Upload(ImageGalary IG)
        {
            if (IG.File.ContentLength > (2*1024*1024))
            {
                ModelState.AddModelError("CustomError", "File  size must be less than 2 Mb");
                return View();
            }

            if (!(IG.File.ContentType == "image/jpeg" || IG.File.ContentType == "image/gif"))
            {
                ModelState.AddModelError("CustomError", "File type allowed : jpeg and gif");
                return View();
            }
            IG.FileName = IG.File.FileName;
            IG.ImageSize = IG.File.ContentLength;
            byte[] data = new byte[IG.File.ContentLength];
            IG.File.InputStream.Read(data, 0, IG.File.ContentLength);
            IG.ImageDate = data;
            using (DatabasePhotoEntities db = new DatabasePhotoEntities() )
            {
                db.ImageGalary.Add(IG);
                db.SaveChangesAsync();
            }

            return RedirectToAction("Gallery");
        }

    }
}