using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionHospital.Models;
using GestionHospital.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace GestionHospital.Controllers
{
    [Authorize(Roles = "Medico")]
    public class DiagnosticoPacientesController : Controller
    {
        private readonly HospitalContext _context;

        public DiagnosticoPacientesController(HospitalContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string buscarPaciente { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string buscarMedico { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string buscarFecha { get; set; } = string.Empty;

        // GET: DiagnosticoPacientes
        public async Task<IActionResult> Index()
        {
            //var hospitalContext = _context.DiagnosticoPacientes
            //    .Include(d => d.Medico)
            //    .Include(d => d.Paciente)
            //    .Select(x=> new DiagnosticoPacienteDTO()
            //    {
            //        Id = x.Id,
            //        Fecha = x.Fecha,
            //        Diagnostico = x.Diagnostico,
            //        NombrePaciente = x.Paciente!.Apellido + ", " + x.Paciente.Nombre,
            //        NombreMedico = x.Medico!.Apellido + ", " + x.Medico.Nombre
            //    });
            //return View(await hospitalContext.ToListAsync());

            ViewData["paciente"] = buscarPaciente;
            ViewData["medico"] = buscarMedico;
            ViewData["fecha"] = buscarFecha;
            string[] fechaBuscada = string.IsNullOrEmpty(buscarFecha)? new string[3] : buscarFecha.Split('-');
            return View(await _context.DiagnosticoPacientes
                .Include(d => d.Medico)
                .Include(d => d.Paciente).
                Where(p => (string.IsNullOrEmpty(buscarPaciente) || (p.Paciente!.Apellido + ", " + p.Paciente.Nombre).ToLower().Contains(buscarPaciente.ToLower())) &&
                    (string.IsNullOrEmpty(buscarMedico) || (p.Medico!.Apellido + ", " +p.Medico.Nombre).ToLower().Contains(buscarMedico.ToLower())) &&
                    (string.IsNullOrEmpty(buscarFecha) || p.Fecha.Date.Equals(new DateTime(int.Parse(fechaBuscada[0]), int.Parse(fechaBuscada[1]), int.Parse(fechaBuscada[2])))))
                .Select(x => new DiagnosticoPacienteDTO()
                {
                    Id = x.Id,
                    Fecha = x.Fecha,
                    Diagnostico = x.Diagnostico,
                    NombrePaciente = x.Paciente!.Apellido + ", " + x.Paciente.Nombre,
                    NombreMedico = x.Medico!.Apellido + ", " + x.Medico.Nombre
                }).ToListAsync());
        }

        // GET: DiagnosticoPacientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosticoPaciente = await _context.DiagnosticoPacientes
                .Include(d => d.Medico)
                .Include(d => d.Paciente)
                .Where(m => m.Id == id).Select(x => new DiagnosticoPacienteDTO()
                {
                    Id = x.Id,
                    Fecha = x.Fecha,
                    Diagnostico = x.Diagnostico,
                    NombrePaciente = x.Paciente!.Apellido + ", " + x.Paciente.Nombre,
                    NombreMedico = x.Medico!.Apellido + ", " + x.Medico.Nombre
                }).FirstOrDefaultAsync();
            if (diagnosticoPaciente == null)
            {
                return NotFound();
            }

            return View(diagnosticoPaciente);
        }

        // GET: DiagnosticoPacientes/Create
        [Authorize(Roles = "Medico")]
        public IActionResult Create()
        {
            //ViewData["MedicoId"] = new SelectList(_context.Medicos, "Id", "Id");
            //ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Id");
            ViewData["Medico"] = new SelectList(_context.Medicos.Select(x => new { x.Id, NombreCompleto = x.Apellido + ", " + x.Nombre }).AsEnumerable(), "Id", "NombreCompleto");
            ViewData["Paciente"] = new SelectList(_context.Pacientes.Select(x => new { x.Id, NombreCompleto = x.Apellido + ", " + x.Nombre }).AsEnumerable(), "Id", "NombreCompleto");
            return View();
        }

        // POST: DiagnosticoPacientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Medico")]
        public async Task<IActionResult> Create([Bind("Id,Fecha,Diagnostico,PacienteId,MedicoId")] DiagnosticoPaciente diagnosticoPaciente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(diagnosticoPaciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Medico"] = new SelectList(_context.Medicos.Select(x => new { x.Id, NombreCompleto = x.Apellido + ", " + x.Nombre }).AsEnumerable(), "Id", "NombreCompleto", diagnosticoPaciente.MedicoId);
            ViewData["Paciente"] = new SelectList(_context.Pacientes.Select(x => new { x.Id, NombreCompleto = x.Apellido + ", " + x.Nombre }).AsEnumerable(), "Id", "NombreCompleto", diagnosticoPaciente.PacienteId);
            return View(diagnosticoPaciente);
        }

        // GET: DiagnosticoPacientes/Edit/5
        [Authorize(Roles = "Medico")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosticoPaciente = await _context.DiagnosticoPacientes.FindAsync(id);
            if (diagnosticoPaciente == null)
            {
                return NotFound();
            }
            ViewData["Medico"] = new SelectList(_context.Medicos.Select(x => new { x.Id, NombreCompleto = x.Apellido + ", " + x.Nombre }).AsEnumerable(), "Id", "NombreCompleto", diagnosticoPaciente.MedicoId);
            ViewData["Paciente"] = new SelectList(_context.Pacientes.Select(x => new { x.Id, NombreCompleto = x.Apellido + ", " + x.Nombre }).AsEnumerable(), "Id", "NombreCompleto", diagnosticoPaciente.PacienteId);
            return View(diagnosticoPaciente);
        }

        // POST: DiagnosticoPacientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Medico")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,Diagnostico,PacienteId,MedicoId")] DiagnosticoPaciente diagnosticoPaciente)
        {
            if (id != diagnosticoPaciente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diagnosticoPaciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiagnosticoPacienteExists(diagnosticoPaciente.Id))
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
            ViewData["MedicoId"] = new SelectList(_context.Medicos.Select(x => new { x.Id, NombreCompleto = x.Apellido + ", " + x.Nombre }).AsEnumerable(), "Id", "NombreCompleto", diagnosticoPaciente.MedicoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes.Select(x => new { x.Id, NombreCompleto = x.Apellido + ", " + x.Nombre }).AsEnumerable(), "Id", "NombreCompleto", diagnosticoPaciente.PacienteId);
            return View(diagnosticoPaciente);
        }

        // GET: DiagnosticoPacientes/Delete/5
        [Authorize(Roles = "Medico")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosticoPaciente = await _context.DiagnosticoPacientes
                .Include(d => d.Medico)
                .Include(d => d.Paciente)
                .Where(m => m.Id == id).Select(x => new DiagnosticoPacienteDTO()
                {
                    Id = x.Id,
                    Fecha = x.Fecha,
                    Diagnostico = x.Diagnostico,
                    NombrePaciente = x.Paciente!.Apellido + ", " + x.Paciente.Nombre,
                    NombreMedico = x.Medico!.Apellido + ", " + x.Medico.Nombre
                }).FirstOrDefaultAsync();
            if (diagnosticoPaciente == null)
            {
                return NotFound();
            }

            return View(diagnosticoPaciente);
        }

        // POST: DiagnosticoPacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Medico")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var diagnosticoPaciente = await _context.DiagnosticoPacientes.FindAsync(id);
            if (diagnosticoPaciente != null)
            {
                _context.DiagnosticoPacientes.Remove(diagnosticoPaciente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiagnosticoPacienteExists(int id)
        {
            return _context.DiagnosticoPacientes.Any(e => e.Id == id);
        }
    }
}
