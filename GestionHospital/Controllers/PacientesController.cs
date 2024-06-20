using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionHospital.Models;
using Microsoft.AspNetCore.Authorization;

namespace GestionHospital.Controllers
{
    [Authorize]
    public class PacientesController : Controller
    {
        private readonly HospitalContext _context;

        public PacientesController(HospitalContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string buscarNombre { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string buscarApellido { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string buscarDNI { get; set; } = string.Empty;

        // GET: Pacientes
        public async Task<IActionResult> Index()
        {
            ViewData["nombre"] = buscarNombre;
            ViewData["apellido"] = buscarApellido;
            ViewData["DNI"] = buscarDNI;
            return View(await _context.Pacientes.
                Where(p => (string.IsNullOrEmpty(buscarNombre) || p.Nombre.ToLower().Contains(buscarNombre.ToLower())) &&
                    (string.IsNullOrEmpty(buscarApellido) || p.Apellido.ToLower().Contains(buscarApellido.ToLower())) &&
                    (string.IsNullOrEmpty(buscarDNI) || p.DNI.ToLower().Contains(buscarDNI.ToLower()))
                ).ToListAsync());
        }

        // GET: Pacientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // GET: Pacientes/Create
        [Authorize(Roles = "Administrador,Recepcion")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pacientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Recepcion")]
        public async Task<IActionResult> Create([Bind("Id,DNI,Nombre,Apellido,FechaNacimiento,Sexo,Telefono,Email")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paciente);
        }

        // GET: Pacientes/Edit/5
        [Authorize(Roles = "Administrador,Recepcion")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }
            return View(paciente);
        }

        // POST: Pacientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrado,Recepcionr")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DNI,Nombre,Apellido,FechaNacimiento,Sexo,Telefono,Email")] Paciente paciente)
        {
            if (id != paciente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteExists(paciente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(paciente);
        }

        // GET: Pacientes/Delete/5
        [Authorize(Roles = "Administrador,Recepcion")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // POST: Pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Recepcion")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacienteExists(int id)
        {
            return _context.Pacientes.Any(e => e.Id == id);
        }
    }
}
