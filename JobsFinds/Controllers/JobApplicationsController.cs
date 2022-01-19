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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace JobsFinds.Controllers
{
    [Authorize(Roles = "Company")]
    public class JobApplicationsController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<User> _userManager;

        public JobApplicationsController(AppDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: JobApplications
        public async Task<IActionResult> Index(int? id)
        {
            User user = await _userManager.GetUserAsync(User);
            List<JobApplication> jobApplications;
            if (id == null)
            {
                jobApplications = _context.JobApplications.Where(app => (app.CompanyId == user.Id && !app.IsRejected)).ToList();
                return View(jobApplications);
            }
            var job = await _context.Job
                .FirstOrDefaultAsync(m => (m.Id == id && m.CompanyId == user.Id));
            if (job == null)
            {
                return NotFound();
            }

            jobApplications = _context.JobApplications.Where(app => (app.CompanyId == user.Id && app.JobId == id && !app.IsRejected)).ToList();

            return View(jobApplications);
        }

        // GET: JobApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            User user = await _userManager.GetUserAsync(User);
            var jobApplication = await _context.JobApplications
                .FirstOrDefaultAsync(m => (m.Id == id && m.CompanyId == user.Id && !m.IsRejected));
            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        
        // GET: JobApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await _userManager.GetUserAsync(User);
            var jobApplication = await _context.JobApplications
                .FirstOrDefaultAsync(m => m.Id == id && m.CompanyId == user.Id);
            if (jobApplication == null)
            {
                return NotFound();
            }

            jobApplication.IsRejected = true;
            _context.Update(jobApplication);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Accept(int ApplicationId, string Message, DateTime InterviewTime, string InterviewLocation)
        {
            User user = await _userManager.GetUserAsync(User);
            var jobApplication = await _context.JobApplications
                .FirstOrDefaultAsync(m => m.Id == ApplicationId && m.CompanyId == user.Id && !m.IsRejected && !m.IsAccepted);
            if (jobApplication == null)
            {
                return NotFound();
            }

            jobApplication.IsAccepted = true;
            _context.Update(jobApplication);

            CompanyResponse companyResponse = new CompanyResponse();
            companyResponse.JobApplicationId = ApplicationId;
            companyResponse.Message = Message;
            companyResponse.InterviewTime = InterviewTime;
            companyResponse.InterviewLocation = InterviewLocation;

            _context.Add(companyResponse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobApplicationExists(int id)
        {
            return _context.JobApplications.Any(e => e.Id == id);
        }
    }
}
