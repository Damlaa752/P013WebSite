using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using P013WebSite.Data;
using P013WebSite.Entities;
using P013WebSite.Tools;

namespace P013WebSite.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class ProductsController : Controller
    {
        private readonly DatabaseContext _databaseContext; // _databaseContext i boş olarak ekledik, sağ klik yapıp ampule tıklayıp açılan menüden generate constructor diyerek DI(Dependency Injection) işlemini yapıyoruz.

        public ProductsController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext; // context i kurcu metootta doldurduk
        }

        // GET: ProductsController
        public ActionResult Index()
        {
           // return View(_databaseContext.Products.ToList()); // var model oluşturmadan direk sayfaya modeli bu şekilde de yollayabiliyoruz
            return View(_databaseContext.Products.Include(c=> c.Category).ToList()); // Products tablosundaki kayıtlara EntityFrameworkCore un Include metoduyl Ktegorilerini de dahil ettik, böylece sql deki join işlemi yapılmış oldu.
        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductsController/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_databaseContext.Categories.ToList(), "Id", "Name");
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Product collection, IFormFile? Image)
        {
            try
            {
                collection.Image = await FileHelper.FileLoaderAsync(Image);
                await _databaseContext.Products.AddAsync(collection); //asenkron metotlar çağırılırken mutlaka başına await anahtar kelimesini yazıyoruz!
                await _databaseContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.CategoryId = new SelectList(_databaseContext.Categories.ToList(), "Id", "Name");
                return View();
            }
        }

        // GET: ProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.CategoryId = new SelectList(_databaseContext.Categories.ToList(), "Id", "Name");
            return View(_databaseContext.Products.Find(id));
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Product collection, IFormFile? Image) // Bir senkron metodun içerisinde asenkron bir metot çağırılırsa ilgili senkron metot da asenkrona çevrilmelidir! Bu işlem için içerdeki asenkron metodun üzerine gelip ampulun çıkmasını bekliyoruz ve gelen menüden make method async seçeneğine tıklayıp hata nın giderilmesini sağlıyoruz.
        {
            try
            {
                if (Image is not null)
                {
                    collection.Image = await FileHelper.FileLoaderAsync(Image);
                }
                _databaseContext.Products.Update(collection);
                _databaseContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.CategoryId = new SelectList(_databaseContext.Categories.ToList(), "Id", "Name");
                return View();
            }
        }

        // GET: ProductsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_databaseContext.Products.Include(c => c.Category).FirstOrDefault(p => p.Id == id));
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Product collection)
        {
            try
            {
                _databaseContext.Products.Remove(collection);
                _databaseContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
