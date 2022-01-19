#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobsFinds.Data;
using JobsFinds.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace JobsFinds.Controllers
{
    [Authorize(Roles = "Student")]
    public class CompanyResponseController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<User> _userManager;


        public CompanyResponseController(AppDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: CompanyResponse
        public async Task<IActionResult> Index()
        {
            string UserId = _userManager.GetUserId(User);

            List<CompanyResponse> List = await _context.CompanyResponses.ToListAsync();
            List<CompanyResponse> FiltterList = new List<CompanyResponse>();
            List.ForEach( action =>
            {
                JobApplication jobApp =  _context.JobApplications.Find(action.JobApplicationId);
                if(jobApp.StudentId == UserId)
                {
                    FiltterList.Add(action);
                }
            });

            return View(FiltterList);
        }

        // GET: CompanyResponse/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyResponse = await _context.CompanyResponses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyResponse == null)
            {
                return NotFound();
            }
            string UserId = _userManager.GetUserId(User);

            JobApplication jobApp = await _context.JobApplications.FindAsync(companyResponse.JobApplicationId);
            if(jobApp.StudentId != UserId)
            {
                return NotFound();
            }

            return View(companyResponse);
        }

        

        // GET: CompanyResponse/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyResponse = await _context.CompanyResponses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyResponse == null)
            {
                return NotFound();
            }

            return View(companyResponse);
        }

        private bool CompanyResponseExists(int id)
        {
            return _context.CompanyResponses.Any(e => e.Id == id);
        }
    }
}
