using Logging_Serilog.Models;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Logging_Serilog.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Add(Doctor doctor)
        {
           await _doctorService.AddDoctorAsync(doctor);
            return View();
        }

    }
}
