using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repositories.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class PersonasApiController : ControllerBase
{
    private readonly IPersonaRepository _repo;
    public PersonasApiController(IPersonaRepository repo) => _repo = repo;

    [HttpGet]
    public Task<List<Persona>> Get() => _repo.GetAllAsync();

    [HttpGet("{cc:int}")]
    public async Task<ActionResult<Persona>> GetById(int cc)
    {
        var p = await _repo.GetByIdAsync(cc);
        return p is null ? NotFound() : p;
    }

    [HttpGet("buscar")]
    public Task<List<Persona>> Buscar([FromQuery] string apellido) =>
        _repo.SearchByApellidoAsync(apellido);

    [HttpPost]
    public async Task<ActionResult<Persona>> Post([FromBody] Persona p)
    {
        var created = await _repo.AddAsync(p);
        return CreatedAtAction(nameof(GetById), new { cc = created.Cc }, created);
    }

    [HttpPut("{cc:int}")]
    public async Task<IActionResult> Put(int cc, [FromBody] Persona p) =>
        await _repo.UpdateAsync(cc, p) ? NoContent() : BadRequest();

    [HttpDelete("{cc:int}")]
    public async Task<IActionResult> Delete(int cc) =>
        await _repo.DeleteAsync(cc) ? NoContent() : NotFound();
}
