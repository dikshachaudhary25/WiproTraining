using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.DAL;
using MyWebApp.Models;

namespace MyWebApp.Controllers;

public class PatientsController : Controller
{
    private readonly CRUD _context;
    public PatientsController(CRUD context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        List<Patient> patients = _context.GetPatientsList();

        return View(patients);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Patient p)
    {
        _context.AddPatient(p);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult Edit(int MRN)
    {
        Patient p = _context.GetPatientByMRN(MRN);
        if (p == null)
            return NotFound();
        return View(p);
    }
    [HttpPost]
    public IActionResult Edit(Patient p)
    {
        _context.UpdatePatient(p);
        if (p == null)
            return NotFound();
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult Delete(int MRN)
    {
        Patient p = _context.GetPatientByMRN(MRN);
        if (p == null)
            return NotFound();
        return View(p);
    }
    [HttpPost]
    [ActionName("Delete")]
    public IActionResult DeletePatient(int MRN)
    {
        TempData["Message"]=_context.DeletePatient(MRN);
        return RedirectToAction("Index");
    }
    
}