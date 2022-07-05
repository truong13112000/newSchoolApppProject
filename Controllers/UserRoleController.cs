//using Microsoft.AspNetCore.Mvc;
//using SchoolProject.Dto.Role;
//using SchoolProject.Models;
//using SchoolProject.ValidateService;
//using System.Reflection;
//using System.Resources;
//using X.PagedList;

//namespace SchoolProject.Controllers
//{
//    public class UserRoleController : Controller
//    {
//        private readonly DataContext _context;
//        private readonly ResourceManager _rm;
//        private readonly IValidateService _validateServiceClass;

//        public UserRoleController(DataContext context,
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
//            // var result = _context.Roles.Where(e => e.Name.Contains(name) || string.IsNullOrEmpty(name)).ToList();

//            var result = from userRole in _context.UserRoles
//                         join user in _context.UserEntitiess on userRole.UserId equals user.Id.ToString()
//                         join role in _context.Roles on userRole.RoleId equals role.Id into roleJoin
//                         from role in roleJoin.DefaultIfEmpty()
//                         select new UserRole
//                         {
//                             UserId = user.Id,
//                             UserName = user.UserName,
//                             RoleName = role.Name,
//                         };
//            //  roles = _roleManager.Roles.ToList();
//            int pageSize = 5;
//            int pageNumber = (page ?? 1);
//            return View(result.ToPagedList(pageNumber, pageSize));
//        }
//    }
//}
