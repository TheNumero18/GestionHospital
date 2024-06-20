using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionHospital.Models;
using Microsoft.AspNetCore.Authorization;

namespace GestionHospital.Controllers
{
    [Authorize]
    public class MedicosController : Controller
    {
        private readonly HospitalContext _context;

        public MedicosController(HospitalContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string buscarNombre { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string buscarApellido { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string buscarEspecialidad { get; set; } = string.Empty;

        // GET: Medicos
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Medicos.ToListAsync());
            ViewData["nombre"] = buscarNombre;
            ViewData["apellido"] = buscarApellido;
            ViewData["especialidad"] = buscarEspecialidad;
            return View(await _context.Medicos.
                Where(p => (string.IsNullOrEmpty(buscarNombre) || p.Nombre.ToLower().Contains(buscarNombre.ToLower())) &&
                    (string.IsNullOrEmpty(buscarApellido) || p.Apellido.ToLower().Contains(buscarApellido.ToLower())) &&
                    (string.IsNullOrEmpty(buscarEspecialidad) || p.Especialidad.ToLower().Contains(buscarEspecialidad.ToLower()))
                ).ToListAsync());
        }

        // GET: Medicos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medico = await _context.Medicos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

        [Authorize(Roles = "Administrador")]
        // GET: Medicos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Medicos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([Bind("Id,DNI,Nombre,Apellido,FechaNacimiento,Sexo,Domicilio,Telefono,Email,Especialidad")] Medico medico)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medico);
        }

        // GET: Medicos/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null)
            {
                return NotFound();
            }
            return View(medico);
        }

        // POST: Medicos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DNI,Nombre,Apellido,FechaNacimiento,Sexo,Domicilio,Telefono,Email,Especialidad")] Medico medico)
        {
            if (id != medico.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicoExists(medico.Id))
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
            return View(medico);
        }

        // GET: Medicos/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medico = await _context.Medicos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

        // POST: Medicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico != null)
            {
                _context.Medicos.Remove(medico);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicoExists(int id)
        {
            return _context.Medicos.Any(e => e.Id == id);
        }
    }
}
