//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using SchoolProject.Dto;
//using SchoolProject.Models;
//using SchoolProject.ValidateService;
//using System.Reflection;
//using System.Resources;

//namespace SchoolProject.Controllers
//{
//    public class UserEntitiesController : Controller
//    {
//        private readonly DataContext _context;
//        private readonly ResourceManager _rm;
//        private readonly IValidateService _validateServiceClass;

//        public UserEntitiesController(DataContext context,
//                    IValidateService validateServiceClass
//            )
//        {
//            _context = context;
//            _rm = new ResourceManager("SchoolProject.ResourceManager.ResourceLanguage", Assembly.GetExecutingAssembly());
//            _validateServiceClass = validateServiceClass;
//        }

//        // GET: UserEntities
//        public async Task<IActionResult> Index()
//        {
//            var userList = from u in _context.Users.AsNoTracking().Where(e => e.UserName.ToLower() != "admin")
//                           join uD in _context.UserDetail.AsNoTracking() on u.Id equals uD.UserId
//                           join s in _context.SchoolEntitiess.AsNoTracking() on uD.SchoolId equals s.Id into schoolJoin
//                           from s in schoolJoin.DefaultIfEmpty()
//                           join d in _context.DepartmentEntitiess.AsNoTracking() on uD.DepartmentId equals d.Id into departmentJoin
//                           from d in departmentJoin.DefaultIfEmpty()
//                           join c in _context.ClassEntitiess.AsNoTracking() on uD.ClassId equals c.Id into classJoin
//                           from c in classJoin.DefaultIfEmpty()
//                           join uG in _context.UserGroup.AsNoTracking() on u.Id equals uG.UserId into uGJoin
//                           from uG in uGJoin.DefaultIfEmpty()
//                           join r in _context.Group.AsNoTracking() on uG.GroupId equals r.Id into roleJoin
//                           from r in roleJoin.DefaultIfEmpty()
//                           select new UserListDto
//                           {
//                               Id = u.Id,
//                               UserName = u.UserName,
//                               FullName = u.NormalizedUserName,
//                               Email = u.Email,
//                               BirthDay = uD.BirthDay.ToString("dd/MM/yyyy"),
//                               Address = uD.Address,
//                               School = s.SchoolName,
//                               Department = d.DepartmentName,
//                               Class = c.ClassName,
//                               Group = r.Description,
//                           };
//            return userList != null ?
//                          View(await userList.ToListAsync()) :
//                          Problem("UserList is null.");
//        }

//        // GET: UserEntities/Details/5
//        public async Task<IActionResult> Details(string id)
//        {
//            if (id == null || _context.Users == null)
//            {
//                return NotFound();
//            }

//            var userEntities = await _context.Users
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (userEntities == null)
//            {
//                return NotFound();
//            }

//            return View(userEntities);
//        }
//        public IActionResult Cancel()
//        {
//            return RedirectToAction("Index", "Home");
//        }

//        // GET: UserEntities/Create
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: UserEntities/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("UserName,Password,FullName,Email,BirthDay,Address,SchoolId,DepartmentId,ClassId,Role,IsDeleted,DeleterUserId,DeletionTime,LastModificationTime,LastModifierUserId,CreationTime,CreatorUserId,Id")] UserDetail userEntities)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(userEntities);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(userEntities);
//        }

//        // GET: UserEntities/Edit/5
//        public async Task<IActionResult> Edit(string id)
//        {
//            SignUpDto userEntities = new SignUpDto();
//            var user = _context.Users.FirstOrDefault(e => e.Id == id);
//            userEntities.UserName = user.UserName;
//            userEntities.Email = user.Email;
//            userEntities.FullName = user.NormalizedUserName;
//            userEntities.SchoolList = _context.SchoolEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.SchoolName }).ToList();
//            userEntities.DepartmentList = _context.DepartmentEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.DepartmentName }).ToList();
//            userEntities.ClassList = _context.ClassEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.ClassName }).ToList();
//            userEntities.UserDetail = await _context.UserDetail.AsNoTracking().FirstOrDefaultAsync(e => e.UserId == id);
//            userEntities.RoleList = _context.Group.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Description }).ToList();
//            return View(userEntities);
//        }

//        // POST: UserEntities/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(string id, [Bind("SchoolId,DepartmentId,ClassId,BirthDay,Address,UserId")] UserDetail userDetail, string userName, string fullName, string email,int groupId)
//        {

//            SignUpDto user = new SignUpDto();
//            user.UserName = userName;
//            user.FullName = fullName;
//            user.Email = email;
//            user.UserDetail = userDetail;
//            user.SchoolList = _context.SchoolEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.SchoolName }).ToList();
//            user.DepartmentList = _context.DepartmentEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.DepartmentName }).ToList();
//            user.ClassList = _context.ClassEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.ClassName }).ToList();
//            user.RoleList = _context.Group.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Description }).ToList();

//            var editUser = _context.Users.FirstOrDefault(e => e.Id == id);
//            var editUserDetail = _context.UserDetail.FirstOrDefault(e => e.UserId == editUser.Id);
          
//            if (!ModelState.IsValid)
//            {
//                return View(user);
//            }

//            var tesst = _validateServiceClass?.ValidateUpdateUser(id, userName, email);
//            if (tesst != 0)
//            {
//                switch (tesst)
//                {
//                    case 1:
//                        ViewBag.error = _rm.GetString("UserNameExisted");
//                        break;
//                    case 2:
//                        ViewBag.error = _rm.GetString("EmailExisted");
//                        break;

//                }
//                return View(user);
//            }
//            try
//            {
//                var userGroup = _context.UserGroup.FirstOrDefault(e => e.UserId == editUser.Id);
//                if(userGroup.GroupId  != groupId)
//                {
//                    // Xóa bỏ khi người dùng thay đổi nhóm,
//                    _context.UserGroup.Remove(userGroup);

//                    // Xóa bỏ tất cả các quyền của ng dùng khi thuộc nhóm cũ,
//                    var userRole = _context.UserRoles.Where(e => e.UserId == editUser.Id);
//                    _context.UserRoles.RemoveRange(userRole);

//                    // Thêm mới khi người dùng thay đổi nhóm;
//                    var newMemberGroup = new UserGroup();
//                    newMemberGroup.UserId = editUser.Id;
//                    newMemberGroup.GroupId = groupId;
//                    _context.UserGroup.Add(userGroup);


//                    // Thêm tất cả các quyền mới thuộc nhóm đã chọn;
//                    var roleGroup = _context.RoleGroup.Where(e => e.GroupId == groupId).Select(e => e.RoleId);

//                    var userNewRole = new List<IdentityUserRole<string>>();
//                    var newUserRole = new IdentityUserRole<string>();

//                    foreach (var role in roleGroup)
//                    {
//                        newUserRole.UserId = editUser.Id;
//                        newUserRole.RoleId = role.ToString();
//                        userNewRole.Add(newUserRole);
//                    }
//                    _context.UserRoles.AddRange(userNewRole);
//                }

//                editUser.NormalizedUserName = fullName ;
//                editUser.UserName = userName;
//                editUser.Email = email;

//                editUserDetail.BirthDay = userDetail.BirthDay;
//                editUserDetail.SchoolId = userDetail.SchoolId;
//                editUserDetail.DepartmentId = userDetail.DepartmentId;
//                editUserDetail.ClassId = userDetail.ClassId;

//                _context.Users.Update(editUser);
//                _context.UserDetail.Update(editUserDetail);
//                await _context.SaveChangesAsync();
//                return RedirectToAction("Index");
//            }
//            catch (ArgumentException Exception)
//            {
//                throw Exception;
//            }

//            if (HttpContext.Session.GetInt32("Role") != 0)
//            {
//                return RedirectToAction("Index", "Home");
//            }
//            else
//            {
//                return RedirectToAction("Index");
//            }

//            return RedirectToAction(nameof(Index));
//        }

//        // GET: UserEntities/Delete/5
//        public async Task<IActionResult> Delete(string id)
//        {
//            if (_context.Users == null)
//            {
//                return Problem("Entity set 'DataContext.UserEntitiess'  is null.");
//            }
//            var userEntities = await _context.Users.FindAsync(id);
//            var userEntitiesDetail = _context.UserDetail.FirstOrDefault(e => e.UserId == id);
//            if (userEntities != null)
//            {
//                _context.Users.Remove(userEntities);
//                _context.UserDetail.Remove(userEntitiesDetail);
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//    }
//}
