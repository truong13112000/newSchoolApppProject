using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Models;
using SchoolProject.ValidateService;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;
using X.PagedList;

namespace SchoolProject.Controllers
{
    public class RoleManageController : Controller
    {
        private readonly DataContext _context;
        private readonly ResourceManager _rm;
        private readonly IValidateService _validateServiceClass;

        public RoleManageController(DataContext context,
            IValidateService validateServiceClass
            )
        {
            _context = context;
            _rm = new ResourceManager("SchoolProject.ResourceManager.ResourceLanguage", Assembly.GetExecutingAssembly());
            _validateServiceClass = validateServiceClass;
        }


        public ViewResult Index(string sortOrder, string currentFilter, string name, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (name != null)
            {
                page = 1;
            }
            else
            {
                name = currentFilter;
            }

            ViewBag.CurrentFilter = name;
            var result = _context.Roles.Where(e => e.Name.Contains(name) || string.IsNullOrEmpty(name)).ToList();
          //  roles = _roleManager.Roles.ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(result.ToPagedList(pageNumber, pageSize));
        }

        public IActionResult Create()
        {
            return View();
        }
        public class InputModel
        {
            public string ID { set; get; }

            [Required(ErrorMessage = "Phải nhập tên role")]
            [Display(Name = "Tên của Role")]
            [StringLength(100, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 3)]
            public string Name { set; get; }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] InputModel identityRole)
        {
            var exit = _context.Roles.FirstOrDefault(e => e.Name == identityRole.Name);
            if (exit != null) {
                ViewBag.Error = "Role is existed!";
                return View();
            }
            else
            {
                var newRole = new Microsoft.AspNetCore.Identity.IdentityRole(identityRole.Name);
                _context.Roles.Add(newRole);
                await _context.SaveChangesAsync();
                ViewBag.success = _rm.GetString("RoleCreateSuccess");
                return RedirectToAction(nameof(Index));
                return View(identityRole);
            }
            return View(identityRole);
        }

        // GET: DepartmentEntities/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.ManageEntities == null)
            {
                return NotFound();
            }

            var role = _context.Roles.FirstOrDefault(e => e.Id == id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string Id, [Bind("Name")] InputModel identityRole)
        {
            var exit = _context.Roles.FirstOrDefault(e => e.Id == Id);
            var exited = _context.Roles.FirstOrDefault(e => e.Name == identityRole.Name && e.Id != Id);
            if (exited != null)
            {
                ViewBag.Error = "Role is existed!";
                return View();
            }
            else
            {

                try
                {
                    exit.Name = identityRole.Name;
                    _context.Update(exit);
                    await _context.SaveChangesAsync();
                    ViewBag.success = _rm.GetString("RoleUpdateSuccess");
                }
                catch (DbUpdateConcurrencyException exception)
                {
                    throw exception;
                }
                return View(exit);
            }
            return View(exit);
        }
        public async Task<IActionResult> Delete(string Id)
        {
            if (_context.Roles == null)
            {
                return Problem(" Role  is null.");
            }
            var role = await _context.Roles.FindAsync(Id);
            if (role != null)
            {
                _context.Roles.Remove(role);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> UserRole(Guid key)
        {
            return RedirectToAction("Index", "UserRole");
        }
    }
}
