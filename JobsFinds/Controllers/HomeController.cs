using JobsFinds.Data;
using JobsFinds.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JobsFinds.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;

        public HomeController(IWebHostEnvironment env, ILogger<HomeController> logger, AppDBContext context, UserManager<User> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        public IActionResult Index()
        {
            List<Job>? Jobs = _context.Job?.OrderBy(m => m.PostingDate).Take(10).ToList();
            ViewBag.Jobs = Jobs;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Jobs()
        {
            int page;
            if(Int32.TryParse(HttpContext.Request.Query["page"].ToString(), out page))
            {
                if(page < 1)
                {
                    page = 1;
                }
            }
            else
            {
                page = 1;
            }
            
            int? totalJobs = _context.Job?.Count();
            int totalPages = (int)(totalJobs / 10);

            if (totalJobs <= 10)
            {
                IQueryable<Job>? jobs = _context.Job?.OrderBy(j => j.PostingDate);
                ViewBag.Jobs = jobs?.ToList();
                ViewBag.Context = _context;
                ViewBag.TotalPages = 1;
                ViewBag.PageNumber = 1;
                return View();
            }
            if(totalJobs / page > 10)
            {
                int start = (page * 10) - 10;
                int end = (page * 10) - 1;
                List<Job>? jobs = _context.Job?.OrderBy(j => j.PostingDate).Skip(start).Take(10).ToList();
                ViewBag.Jobs = jobs;
                ViewBag.Context = _context;
                ViewBag.TotalPages = totalPages;
                ViewBag.PageNumber = page;
                return View();
            }
            else
            {
                return NotFound();
            }
            
        }

        public IActionResult Job(int id)
        {
            Job? job = _context.Job?.Find(id);
            if(job == null)
            {
                return NotFound();
            }
            int? count = _context.JobApplications?.Where(app => app.JobId == id).Where(app => app.StudentId == _userManager.GetUserId(User)).Count();
            if(count > 0)
            {
                ViewBag.alreadyApplied = true;
            }
            else
            {
                ViewBag.alreadyApplied = false;
            }
            ViewBag.Job = job;
            
            ViewBag.Context = _context;
            return View("JobDetails");
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [Authorize(Roles = "Student")]
        public IActionResult Apply(int id)
        {
            int? count = _context.JobApplications?.Where(app => app.JobId == id).Where(app => app.StudentId == _userManager.GetUserId(User)).Count();
            if(count > 0)
            {
                string success = "0";
                string message = "Already applied";
                return Redirect("/" + QueryString.Create("success", success).Add(QueryString.Create("message", message)).ToUriComponent());
            }
            Job? job = _context.Job?.Find(id);
            if (job == null)
            {
                return NotFound();
            }
            ViewBag.Job = job;

            ViewBag.Context = _context;
            return View();
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        public async Task<IActionResult> JobApply(int jobId, string educationLevel, string aboutMe, string residenceCity, string workExperience)
        {
            string success;
            string message;
            if (jobId == 0)
            {
                return NotFound();
            }
            Job? job = _context.Job?.Find(jobId);
            if (job == null)
            {
                return NotFound();
            }
            JobApplication jobApplication = new JobApplication();
            jobApplication.JobId = jobId;
            jobApplication.StudentId = _userManager.GetUserId(User);
            jobApplication.EducationLevel = educationLevel;
            jobApplication.AboutMe = aboutMe;
            jobApplication.ResidenceCity = residenceCity;
            jobApplication.WorkExperience = workExperience;
            jobApplication.CompanyId = job.CompanyId;

            var cv = HttpContext.Request.Form.Files.GetFile("cv");
            var uploads = Path.Combine(_env.WebRootPath, "images");
            if (cv != null)
            {
                var fileName = "cv/" + Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(cv.FileName);
                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                {
                    await cv.CopyToAsync(fileStream);
                    jobApplication.cv = fileName;
                }
            }
            else
            {
                success = "0";
                message = "CV not attached";
                return Redirect(Request.Headers["Referer"].ToString() + QueryString.Create("success", success).Add(QueryString.Create("message", message)).ToUriComponent());
            }

            _context.Add(jobApplication);
            await _context.SaveChangesAsync();

            success = "1";
            message = "Applied for job successfuly";
            return Redirect("/" + QueryString.Create("success", success).Add(QueryString.Create("message", message)).ToUriComponent());

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}