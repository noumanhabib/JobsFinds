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
    [Authorize(Roles="Company")]
    public class JobsController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<User> _userManager;
        public JobsController(AppDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Jobs
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(User);
            IQueryable<Job> currentCompanyList = _context.Job.Where(job => job.CompanyId.Equals(user.Id));
            return View(await currentCompanyList.ToListAsync());
        }

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await _userManager.GetUserAsync(User);

            var job = await _context.Job
                .FirstOrDefaultAsync(m => (m.Id == id && m.CompanyId == user.Id));
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // GET: Jobs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastDate,Title,Position,SalaryRange,AverageYearlySalary,Location,JobNature,Vacancy,WorkCondition,WorkTime,Description,ApplyLink,AdLink")] Job job)
        {
            User user = await _userManager.GetUserAsync(User);
            job.CompanyId = user.Id;
            if (ModelState.IsValid)
            {
                _context.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(job);
        }

        // GET: Jobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            User user = await _userManager.GetUserAsync(User);
            var job = await _context.Job
                .FirstOrDefaultAsync(m => (m.Id == id && m.CompanyId == user.Id));
            if (job == null)
            {
                return NotFound();
            }
            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PostingDate,LastDate,Title,Position,SalaryRange,AverageYearlySalary,Location,JobNature,Vacancy,WorkCondition,WorkTime,Description,ApplyLink,AdLink")] Job job)
        {
            User user = await _userManager.GetUserAsync(User);
            if (id != job.Id || job.CompanyId != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(job.Id))
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
            return View(job);
        }

        // GET: Jobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await _userManager.GetUserAsync(User);

            var job = await _context.Job
                .FirstOrDefaultAsync(m => (m.Id == id && m.CompanyId == user.Id));
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            User user = await _userManager.GetUserAsync(User);
            var job = await _context.Job
                .FirstOrDefaultAsync(m => (m.Id == id && m.CompanyId == user.Id));
            _context.Job.Remove(job);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobExists(int id)
        {
            return _context.Job.Any(e => e.Id == id);
        }

    }
}
