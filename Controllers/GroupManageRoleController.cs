//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using SchoolProject.Dto;
//using SchoolProject.Dto.Role;
//using SchoolProject.Models;
//using SchoolProject.ValidateService;
//using System.ComponentModel.DataAnnotations;
//using System.Data.Entity.Infrastructure;
//using System.Reflection;
//using System.Resources;
//using X.PagedList;

//namespace SchoolProject.Controllers
//{
//    public class GroupManageRoleController : Controller
//    {

//        private readonly DataContext _context;
//        private readonly ResourceManager _rm;
//        private readonly IValidateService _validateServiceClass;

//        public GroupManageRoleController(DataContext context,
//            IValidateService validateServiceClass
//            )
//        {
//            _context = context;
//            _rm = new ResourceManager("SchoolProject.ResourceManager.ResourceLanguage", Assembly.GetExecutingAssembly());
//            _validateServiceClass = validateServiceClass;
//        }


//        public ViewResult Index(string sortOrder, string currentFilter, string name, int? page)
//        {
//            ViewBag.CurrentSort = sortOrder;

//            if (name != null)
//            {
//                page = 1;
//            }
//            else
//            {
//                name = currentFilter;
//            }

//            ViewBag.CurrentFilter = name;
//            var result = _context.Group.Where(e => e.Name.Contains(name) || string.IsNullOrEmpty(name)).ToList();
//            //  roles = _roleManager.Roles.ToList();
//            int pageSize = 5;
//            int pageNumber = (page ?? 1);
//            return View(result.ToPagedList(pageNumber, pageSize));
//        }

//        public IActionResult Create()
//        {
//            return View();
//        }
//        public class InputModel
//        {
//            public string ID { set; get; }

//            [Required(ErrorMessage = "Phải nhập tên role")]
//            [Display(Name = "Tên của Role")]
//            [StringLength(100, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 3)]
//            public string Name { set; get; }
//            [StringLength(100, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 3)]
//            public string Description { set; get; }
//        }



//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Name,Description")] InputModel identityRole)
//        {
//            var exit = _context.Group.FirstOrDefault(e => e.Name == identityRole.Name);
//            if (exit != null)
//            {
//                ViewBag.Error = "Role is existed!";
//                return View();
//            }
//            else
//            {
//                var newRole = new Group();
//                newRole.Name = identityRole.Name;
//                newRole.Description = identityRole.Description;
//                _context.Group.Add(newRole);
//                await _context.SaveChangesAsync();
//                ViewBag.success = _rm.GetString("RoleCreateSuccess");
//                return RedirectToAction(nameof(Index));
//                return View(identityRole);
//            }
//            return View(identityRole);
//        }

//        // GET: DepartmentEntities/Edit/5
//        public async Task<IActionResult> Edit(string id)
//        {
//            if (id == null || _context.ManageEntities == null)
//            {
//                return NotFound();
//            }

//            var role = _context.Group.FirstOrDefault(e => e.Id.ToString() == id);
//            if (role == null)
//            {
//                return NotFound();
//            }
//            return View(role);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(string Id, [Bind("Name,Description")] InputModel identityRole)
//        {
//            var exit = _context.Group.FirstOrDefault(e => e.Id.ToString() == Id);
//            var exited = _context.Group.FirstOrDefault(e => e.Name == identityRole.Name && e.Id.ToString() != Id);
//            if (exited != null)
//            {
//                ViewBag.Error = "Role is existed!";
//                return View();
//            }
//            else
//            {

//                try
//                {
//                    exit.Name = identityRole.Name;
//                    exit.Description = identityRole.Description;
//                    _context.Update(exit);
//                    await _context.SaveChangesAsync();
//                    ViewBag.success = _rm.GetString("RoleUpdateSuccess");
//                }
//                catch (DbUpdateConcurrencyException exception)
//                {
//                    throw exception;
//                }
//                return View(exit);
//            }
//            return View(exit);
//        }
//        public async Task<IActionResult> Delete(string Id)
//        {
//            if (_context.Roles == null)
//            {
//                return Problem(" Role  is null.");
//            }
//            var role = await _context.Group.FindAsync(Id);
//            if (role != null)
//            {
//                _context.Group.Remove(role);
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }
//        public async Task<IActionResult> UserRole(Guid key)
//        {
//            return RedirectToAction("Index", "UserRole");
//        }


//        //public async Task<IActionResult> EditRole(string id)
//        //{
//        //    var result = (from a in _context.Roles
//        //                  group a by a.NormalizedName into agroup
//        //                  select new RoleMangae
//        //                  {
//        //                      ScreenName = agroup.Key,
//        //                      NeWGroup = agroup.ToList(),
//        //                  });
//        //    return View(result.ToList());
//        //}

//        public IActionResult EditRole()
//        {
//            List<TreeViewNode> nodes = new List<TreeViewNode>();
//            TreeViewNode newnode = new TreeViewNode();
//            var result = (from a in _context.Roles
//                          group a by a.NormalizedName into agroup
//                          select new RoleMangae
//                          {
//                              ScreenName = agroup.Key,
//                              NeWGroup = agroup.ToList(),
//                          });
//            //Loop and add the Parent Nodes.
//            foreach (var r in result)
//            {
//                nodes.Add(new TreeViewNode { id = r.ScreenName, parent = "#", text = r.ScreenName });
//                foreach (var child in r.NeWGroup)
//                {
//                    nodes.Add(new TreeViewNode { id = child.NormalizedName + "-" + child.Id, parent = child.NormalizedName , text = child.Name });
//                }
//            }
//            ViewBag.Json = JsonConvert.SerializeObject(nodes);
//            return View();
//        }

//        [HttpPost]
//        public IActionResult EditRole(string selectedItems)
//        {
//            List<TreeViewNode> items = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);
//            return RedirectToAction("Index","GroupManageRole");
//        }
//    }
//}
