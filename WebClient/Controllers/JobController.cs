using Microsoft.AspNetCore.Mvc;
using SharedLibrary.DTOs;
using WebClient.Services;

namespace WebClient.Controllers
{
    public class JobController(IHttpClientApi _api) : Controller
    {
        private const string AccountsPath = "api/job";

        public async Task<IActionResult> Index()
        {
            var accounts = await _api.GetListAsync<JobReadDto>(AccountsPath)
                           ?? Enumerable.Empty<JobReadDto>();
            return View(accounts);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var account = await _api.GetAsync<JobReadDto>($"{AccountsPath}/{id}");
            if (account == null) return NotFound();
            return View(account);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobCreateDto model)
        {
            if (!ModelState.IsValid) return View(model);

            var (created, problem) = await _api.PostAsync<JobCreateDto, JobReadDto>(AccountsPath, model);
            if (problem is not null && problem.Errors is not null)
            {
                foreach (var (key, value) in problem.Errors)
                {
                    ModelState.AddModelError(key, string.Join(" ", value));
                }
                return View(model);
            }

            if (created is null)
            {
                ModelState.AddModelError(string.Empty, "Create failed.");
                return View(model);
            }

            TempData["Success"] = "Created account successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var account = await _api.GetAsync<JobReadDto>($"{AccountsPath}/{id}");
            if (account is null) return NotFound();

            var vm = new JobUpdateDto { Title = account.Title, Company = account.Company, Description = account.Description, Experience = account.Experience, Location = account.Location, Salary = account.Salary };
            ViewBag.Id = id;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JobUpdateDto model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Id = id;
                return View(model);
            }

            var (success, problem) = await _api.PutAsync($"{AccountsPath}/{id}", model);
            if (problem is not null && problem.Errors is not null)
            {
                foreach (var (key, value) in problem.Errors)
                {
                    ModelState.AddModelError(key, string.Join(" ", value));
                }
                ViewBag.Id = id;
                return View(model);
            }

            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Update failed.");
                ViewBag.Id = id;
                return View(model);
            }

            TempData["Success"] = "Updated account successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var account = await _api.GetAsync<JobReadDto>($"{AccountsPath}/{id}");
            if (account is null) return NotFound();
            return View(account);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ok = await _api.DeleteAsync($"{AccountsPath}/{id}");
            if (!ok)
            {
                TempData["Error"] = "Delete failed.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            TempData["Success"] = "Deleted account successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
