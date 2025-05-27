using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ProfileSample.DAL;
using ProfileSample.Models;

namespace ProfileSample.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            using (var context = new ProfileSampleEntities())
            {
                var images = await context
                    .ImgSources
                    .AsNoTracking()
                    .Take(20)
                    .Select(img => new ImageModel
                    {
                        Id = img.Id,
                        Name = img.Name,
                    }).ToListAsync();

                return View(images);
            }
        }

        public async Task<ActionResult> Image(int id)
        {
            using (var context = new ProfileSampleEntities())
            {
                var image = await context
                    .ImgSources
                    .AsNoTracking()
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (image == null)
                {
                    return HttpNotFound();
                }

                return File(image.Data, "image/jpg");
            }
        }

        public ActionResult Convert()
        {
            var files = Directory.GetFiles(Server.MapPath("~/Content/Img"), "*.jpg");

            using (var context = new ProfileSampleEntities())
            {
                foreach (var file in files)
                {
                    using (var stream = new FileStream(file, FileMode.Open))
                    {
                        byte[] buff = new byte[stream.Length];

                        stream.Read(buff, 0, (int)stream.Length);

                        var entity = new ImgSource()
                        {
                            Name = Path.GetFileName(file),
                            Data = buff,
                        };

                        context.ImgSources.Add(entity);
                    }
                }

                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}