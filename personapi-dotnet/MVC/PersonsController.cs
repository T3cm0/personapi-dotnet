using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using personapi_dotnet.NSwagClient;

// Evita choques de nombre con tu Entity Framework Persona
using PersonaDto = personapi_dotnet.NSwagClient.Persona;

namespace personapi_dotnet.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IApiClient _api;
        public PersonsController(IApiClient api) => _api = api;

        // GET: /Persons
        public async Task<IActionResult> Index()
        {
            var personas = await _api.PersonasApiAllAsync();
            return View(personas); // IEnumerable<PersonaDto>
        }

        // GET: /Persons/Details/123
        public async Task<IActionResult> Details(int cc)
        {
            var p = await _api.PersonasApiGETAsync(cc);
            if (p == null) return NotFound();
            return View(p);
        }

        // GET: /Persons/Create
        public IActionResult Create() => View(new PersonaDto());

        // POST: /Persons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonaDto body)
        {
            if (!ModelState.IsValid) return View(body);
            await _api.PersonasApiPOSTAsync(body);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Persons/Edit/123
        public async Task<IActionResult> Edit(int cc)
        {
            var p = await _api.PersonasApiGETAsync(cc);
            if (p == null) return NotFound();
            return View(p);
        }

        // POST: /Persons/Edit/123
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int cc, PersonaDto body)
        {
            if (!ModelState.IsValid) return View(body);
            await _api.PersonasApiPUTAsync(cc, body);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Persons/Delete/123
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int cc)
        {
            await _api.PersonasApiDELETEAsync(cc);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Persons/Search?apellido=rodriguez
        public async Task<IActionResult> Search(string apellido)
        {
            var result = await _api.BuscarAsync(apellido);
            return View("Index", result);
        }
    }
}
