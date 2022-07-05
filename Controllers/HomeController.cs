
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using SchoolProject.Dto;
//using SchoolProject.Models;
//using SchoolProject.ValidateService;
//using System.Reflection;
//using System.Resources;
//using System.Security.Claims;

//namespace SchoolProject.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;
//        private readonly DataContext _context;
//        private readonly ResourceManager _rm;
//        private readonly SignInManager<IdentityUser> _signInManager;
//        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _userManager;
//        private readonly IValidateService _validateServiceClass;

//        public HomeController(ILogger<HomeController> logger,
//            DataContext context,
//            IValidateService validateServiceClass,
//           Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager,
//            SignInManager<IdentityUser> signInManager
//        )
//        {
//            _logger = logger;
//            _context = context;
//            _rm = new ResourceManager("SchoolProject.ResourceManager.ResourceLanguage", Assembly.GetExecutingAssembly());
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _validateServiceClass = validateServiceClass;
//        }


//        public ActionResult Index()
//        {
//            ViewBag.success = _rm.GetString("LoginSuccess");
//            return View();
//        }

//        public ActionResult SignUp()
//        {
//            SignUpDto newUser = new SignUpDto();
//            newUser.SchoolList = _context.SchoolEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.SchoolName }).ToList();
//            newUser.DepartmentList = _context.DepartmentEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.DepartmentName }).ToList();
//            newUser.ClassList = _context.ClassEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.ClassName }).ToList();
//            newUser.RoleList = _context.Group.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Description }).ToList();
//            return View(newUser);
//        }


//        [HttpPost]
//        public async Task<ActionResult> SignUp(SignUpDto input ,string returnUrl = null)
//        {
//            returnUrl ??= Url.Content("~/");
//            SignUpDto newUser = new SignUpDto();
//            newUser.SchoolList = _context.SchoolEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.SchoolName }).ToList();
//            newUser.DepartmentList = _context.DepartmentEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.DepartmentName }).ToList();
//            newUser.ClassList = _context.ClassEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.ClassName }).ToList();
//            newUser.RoleList = _context.Group.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Description }).ToList();
//            var tesst = _validateServiceClass?.ValidateSigUp(input);
//            if (tesst != 0)
//            {
//                switch (tesst)
//                {
//                    case 1:
//                        ViewBag.error = _rm.GetString("MaxSchool");
//                        break;
//                    case 2:
//                        ViewBag.error = _rm.GetString("MaxDepartment");
//                        break;
//                    case 3:
//                        ViewBag.error = _rm.GetString("MaxClass");
//                        break;
//                    case 4:
//                        ViewBag.errorEmail = _rm.GetString("EmailExisted");
//                        break;
//                    case 5:
//                        ViewBag.error = _rm.GetString("AccountExisted");
//                        break;
//                }
//                return View(newUser);
//            }
//            else
//            {
//                var newMemberGroup = new UserGroup();
//                var newMemberDetail = new UserDetail();
//                var newMember = new IdentityUser();

//                newMember.UserName = input.UserName;
//                newMember.PasswordHash = HashCodeAndDecodePassWord.HashCodePassword(input.Password);
//                newMember.NormalizedUserName = input.FullName;
//                newMember.Email = input.Email;
//                _context.Users.AddAsync(newMember);

//                newMemberDetail.UserId = newMember.Id;
//                newMemberDetail.BirthDay = input.UserDetail.BirthDay;
//                newMemberDetail.Address = input.UserDetail.Address;
//                newMemberDetail.SchoolId = input.UserDetail.SchoolId;
//                newMemberDetail.DepartmentId = input.UserDetail.DepartmentId;
//                newMemberDetail.ClassId = input.UserDetail.ClassId;
//                _context.UserDetail.AddAsync(newMemberDetail);

//                newMemberGroup.GroupId = input.GroupId;
//                newMemberGroup.UserId = newMember.Id;
//                _context.UserGroup.AddAsync(newMemberGroup);

//                var roleIdList = from a in _context.Roles
//                                 join b in _context.RoleGroup.Where(e => e.GroupId.ToString() == input.GroupId.ToString()) on a.Id equals b.RoleId.ToString()
//                                 select a.Id;
//                var userRole = new IdentityUserRole<string>();
//                foreach (var roleId in roleIdList)
//                {
//                    userRole.RoleId = roleId;
//                    userRole.UserId = newMember.Id;
//                    _context.UserRoles.Add(userRole);
//                }
//                _context.SaveChangesAsync();
               
//                var user =  new IdentityUser { UserName = input.UserName, Email = input.Email };
//                var result = await  _userManager.CreateAsync(user, input.Password);
//                if (result.Succeeded)
//                {

//                    await _signInManager.SignInAsync(user, isPersistent: false);
//                    return LocalRedirect(returnUrl);
//                }
//                foreach (var error in result.Errors)
//                {
//                    ModelState.AddModelError(string.Empty, error.Description);
//                }
//                return RedirectToAction("LogIn");
//            }
//        }

//        public ActionResult LogIn()
//        {
//            return View();
//        }

//        public ActionResult ChangeSignUp()
//        {
//            return Redirect("SignUp");
//        }
//        public async Task<IActionResult> LogOut()
//        {
//            await _signInManager.SignOutAsync();
//            HttpContext.Session.Clear();
//            return RedirectToAction("LogIn");
//        }
//        public ActionResult Cancel()
//        {
//            return RedirectToAction("LogIn");
//        }

//        public ActionResult EditInfo()
//        {
//            var id = HttpContext.Session.GetString("Id").ToString();
//            return RedirectToAction("Edit", "UserEntities", new { id = id });
//        }

//        [HttpPost]
//        public async Task<ActionResult> LogIn(LoginDto input,string returnUrl = null)
//        {
//            returnUrl = returnUrl ?? Url.Content("~/");
//            ViewData["ReturnUrl"] = returnUrl;
//            var userExisted = _context.Users.FirstOrDefault(e => (e.UserName.ToLower().Trim() == input.UserName.ToLower().Trim()) && (e.PasswordHash == HashCodeAndDecodePassWord.HashCodePassword(input.Password)));
//            if (_validateServiceClass?.ValidateLogin(input) == false)
//            {
//                ViewBag.error = _rm.GetString("LoginFail");
//                return View();
//            }
//            else
//            {
//                var user = new IdentityUser { UserName = input.UserName, Email = userExisted.Email };
//                await _signInManager.SignInAsync(userExisted, isPersistent: true);

//                HttpContext.Session.SetString("UserName", userExisted.UserName);
//                HttpContext.Session.SetString("Id", userExisted.Id);
//                HttpContext.Session.SetInt32("Role", 0);
//                return RedirectToAction("Index");
//            }
//        }
//    }
//}