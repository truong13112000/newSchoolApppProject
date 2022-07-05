using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Models;
using SchoolProject.ValidateService;
using System.Reflection;
using System.Resources;
using X.PagedList;

namespace SchoolProject.Controllers
{
    public class ManageController : Controller
    {
        private readonly DataContext _context;
        private readonly ResourceManager _rm;
        private readonly IValidateService _validateServiceClass;


        public ManageController(DataContext context, IValidateService validateServiceClass)
        {
            _context = context;
            _rm = new ResourceManager("SchoolProject.ResourceManager.ResourceLanguage", Assembly.GetExecutingAssembly());
            _validateServiceClass = validateServiceClass;
        }

        // GET: DepartmentEntities
        public ViewResult Index(string sortOrder, string currentFilter, string screenName, string targetName, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (screenName != null)
            {
                page = 1;
            }
            else
            {
                screenName = currentFilter;
            }

            ViewBag.CurrentFilter = screenName;

            var manageEntities = from s in _context.ManageEntities
                           select s;
 
                manageEntities = manageEntities.OrderBy(e => e.Screen_Name).ThenBy(e => e.Taget_Name).Where(s => (s.Screen_Name.Contains(screenName) || string.IsNullOrEmpty(screenName))
                                       &&( s.Taget_Name.Contains(targetName) || string.IsNullOrEmpty(targetName)));
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(manageEntities.ToPagedList(pageNumber, pageSize));
        }


        // GET: DepartmentEntities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DepartmentEntities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Screen_Name,Taget_Name,Display_Name,Value,Key")] ManageEntities manageEntities)
        {
            if (!_validateServiceClass.ValidateCreateManage(manageEntities))
            {
                ViewBag.error = _rm.GetString("DepartmentExisted");
                return View();
            }
            else
            {
                manageEntities.C_Date = DateTime.Now;
                manageEntities.C_User = HttpContext.Session.GetString("UserName");
                manageEntities.Key = new Guid();
                _context.Add(manageEntities);
                await _context.SaveChangesAsync();
                ViewBag.success = _rm.GetString("CreateDepartment");
                return RedirectToAction(nameof(Index));

            }
            return View(manageEntities);
        }

        // GET: DepartmentEntities/Edit/5
        public async Task<IActionResult> Edit(Guid key)
        {
            if (key == null || _context.ManageEntities == null)
            {
                return NotFound();
            }

            var departmentEntities = _context.ManageEntities.FirstOrDefault(e => e.Key == key);
            if (departmentEntities == null)
            {
                return NotFound();
            }
            return View(departmentEntities);
        }

        // POST: DepartmentEntities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid key, [Bind("Screen_Name,Taget_Name,Display_Name,Value,Key")] ManageEntities manageEntities)
        {
            var exit =  _context.ManageEntities.FirstOrDefault(e => e.Key == key);
            if (!_validateServiceClass.ValidateUpdateManage(key, manageEntities))
            {
                ViewBag.error = _rm.GetString("DepartmentExisted");
                return View();
            }
            else
            {

                try
                {
                    exit.Screen_Name = manageEntities.Screen_Name;
                    exit.Display_Name = manageEntities.Display_Name;
                    exit.Taget_Name = manageEntities.Taget_Name;
                    exit.Value = manageEntities.Value;
                    _context.Update(exit);
                    await _context.SaveChangesAsync();
                    ViewBag.success = _rm.GetString("UpdateDepartment");
                }
                catch (DbUpdateConcurrencyException exception)
                {
                    throw exception;
                }
                return View(exit);
            }
            return View(exit);
        }

        // GET: DepartmentEntities/Delete/5
        public async Task<IActionResult> Delete(Guid? key)
        {
            if (_context.ManageEntities == null)
            {
                return Problem(" Class  is null.");
            }
            var manageEntities = await _context.ManageEntities.FindAsync(key);
            if (manageEntities != null)
            {
                manageEntities.IsDeleted = true;
                _context.ManageEntities.Remove(manageEntities);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
