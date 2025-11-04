using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.NSwagClient;

namespace personapi_dotnet.MVC
{
    public class ProductsController : Controller
    {
        private readonly IApiClient _api;
        public ProductsController(IApiClient api) => _api = api;

        // GET: /Products
        public async Task<IActionResult> Index()
        {
            var items = await _api.ProductosApiAllAsync();     // Lista de productos
            return View(items);
        }

        // GET: /Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var item = await _api.ProductosApiGETAsync(id);
            if (item is null) return NotFound();
            return View(item);
        }

        // GET: /Products/Create
        public IActionResult Create() => View(new Profesion());

        // POST: /Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Profesion model)
        {
            if (!ModelState.IsValid) return View(model);
            await _api.ProductosApiPOSTAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _api.ProductosApiGETAsync(id);
            if (item is null) return NotFound();
            return View(item);
        }

        // POST: /Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Profesion model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            await _api.ProductosApiPUTAsync(id, model);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _api.ProductosApiDELETEAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
