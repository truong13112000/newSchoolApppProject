using SchoolProject.Controllers;
using SchoolProject.Dto;
using SchoolProject.Models;

namespace SchoolProject.ValidateService
{
    public class ValidateServiceClass : IValidateService
    {
        private readonly DataContext _context;

        public ValidateServiceClass(DataContext context)
        {
            _context = context;
        }

        public int ValidateSigUp(SignUpDto input)
        {
            var userExisted = _context.Users.FirstOrDefault(e => e.UserName.ToLower().Trim() == input.UserName.ToLower().Trim());
            var countInSchool = _context.UserDetail.Count(e => e.SchoolId == input.UserDetail.SchoolId);
            var countInDepartment = _context.DepartmentEntitiess.Count(e => e.Id == input.UserDetail.DepartmentId);
            var countInClass = _context.ClassEntitiess.Count(e => e.Id == input.UserDetail.ClassId);
            if (_context.SchoolEntitiess.Any(e => e.Id == input.UserDetail.SchoolId && countInSchool == e.Capacity) == true)
            {

                return 1;
            }
            if (_context.DepartmentEntitiess.Any(e => e.Id == input.UserDetail.DepartmentId && countInDepartment == e.Capacity) == true)
            {

                return 2;

            }
            if (_context.ClassEntitiess.Any(e => e.Id == input.UserDetail.ClassId && countInClass == e.Capacity) == true)
            {
                return 3;
            }
            if (_context.Users.Any(e => e.Email == input.Email) == true)
            {
                return 4;
            }
            if (userExisted != null)
            {

                return 5;
            }
            return 0;
        }

        public bool ValidateLogin(LoginDto input)
        {
            var pass = HashCodeAndDecodePassWord.HashCodePassword(input.Password);
            var userExisted = _context.Users.FirstOrDefault(e => (e.UserName.ToLower().Trim() == input.UserName.ToLower().Trim()) && (e.PasswordHash == HashCodeAndDecodePassWord.HashCodePassword(input.Password)));
            if (userExisted == null)
            {
                return false;
            }
            return true;
        }

        public int ValidateCreateClass(ClassEntities classEntities)
        {
            var departmentCapacity = _context.DepartmentEntitiess.FirstOrDefault(e => e.Id == classEntities.DepartmentId).Capacity;
            var classCapacity = _context.ClassEntitiess.Where(e => e.DepartmentId == classEntities.DepartmentId).Sum(e => e.Capacity) + classEntities.Capacity;
            var availability = departmentCapacity - (classCapacity - classEntities.Capacity);
            if (departmentCapacity < classCapacity)
            {
                return 1;
            }
            var existed = _context.ClassEntitiess.FirstOrDefault(e => e.ClassName.ToLower().Trim() == classEntities.ClassName.ToLower().Trim() && e.DepartmentId == classEntities.DepartmentId);
            if (existed != null)
            {
                return 2;
            };
            return 0;
        }

        public bool ValidateCreateManage(ManageEntities manageEntities)
        {
            var manege = _context.ManageEntities.FirstOrDefault(e => ((e.Screen_Name == manageEntities.Screen_Name && e.Display_Name == manageEntities.Display_Name && e.Taget_Name == manageEntities.Taget_Name)
            || (e.Screen_Name == manageEntities.Screen_Name && e.Display_Name == manageEntities.Display_Name && e.Value == manageEntities.Value)));
            if (manege != null)
            {
                return false;
            };
            return true;
        }

        public bool ValidateUpdateManage(Guid key, ManageEntities manageEntities)
        {
            var manege = _context.ManageEntities.Where(e => ((e.Screen_Name == manageEntities.Screen_Name && e.Display_Name == manageEntities.Display_Name && e.Taget_Name == manageEntities.Taget_Name) ||
             (e.Screen_Name == manageEntities.Screen_Name && e.Display_Name == manageEntities.Display_Name && e.Value == manageEntities.Value)) && e.Key != key).FirstOrDefault();
            if (manege != null)
            {
                return false;
            };
            return true;
        }



        public int ValidateUpdateClass(long id, ClassEntities classEntities)
        {
            var departmentCapacity = _context.DepartmentEntitiess.FirstOrDefault(e => e.Id == classEntities.DepartmentId).Capacity;
            var classCapacity = _context.ClassEntitiess.Where(e => e.DepartmentId == classEntities.DepartmentId && e.Id != id).Sum(e => e.Capacity) + classEntities.Capacity;
            var availability = departmentCapacity - (classCapacity - classEntities.Capacity);
            var existed = _context.ClassEntitiess.FirstOrDefault(e => e.ClassName.ToLower().Trim() == classEntities.ClassName.ToLower().Trim() && e.DepartmentId == classEntities.DepartmentId && e.Id != id);
            if (existed != null)
            {
                return 1;
            };
            if (departmentCapacity < classCapacity)
            {
                return 2;
            }
            return 0;
        }

        public bool ValidateCreateDeparment(DepartmentEntities departmentEntities)
        {
            var existed = _context.DepartmentEntitiess.FirstOrDefault(e => e.DepartmentName.ToLower().Trim() == departmentEntities.DepartmentName.ToLower().Trim() && e.SchoolId == departmentEntities.SchoolId);
            if (existed != null)
            {
                return false;
            };
            return true;
        }

        public bool ValidateUpdateDeparment(long id, DepartmentEntities departmentEntities)
        {
            var existed = _context.DepartmentEntitiess.FirstOrDefault(e => e.DepartmentName.ToLower().Trim() == departmentEntities.DepartmentName.ToLower().Trim() && e.SchoolId == departmentEntities.SchoolId && e.Id != id);
            if (existed != null)
            {
                return false;
            };
            return true;
        }

        public bool ValidateCreateSchool(SchoolEntities schoolEntities)
        {
            var check = true;
            var text = System.Text.RegularExpressions.Regex.Replace(schoolEntities.SchoolName, @"\s+", " ").Trim().ToLower();

            foreach (var school in _context.SchoolEntitiess)
            {
                var x = System.Text.RegularExpressions.Regex.Replace(school.SchoolName.ToString(), @"\s+", " ").Trim().ToLower();
                if (x == text)
                {
                    check = false;
                }
            };
            return check;
        }

        public bool ValidateUpdateSchool(long id, SchoolEntities schoolEntities)
        {
            var check = true;
            var text = System.Text.RegularExpressions.Regex.Replace(schoolEntities.SchoolName, @"\s+", " ").Trim().ToLower();

            foreach (var school in _context.SchoolEntitiess)
            {
                var x = System.Text.RegularExpressions.Regex.Replace(school.SchoolName.ToString(), @"\s+", " ").Trim().ToLower();
                if (x == text && school.Id != id)
                {
                    check = false;
                }
            };
            return check;

        }
        public int ValidateUpdateUser(string id, string userName, string email)
        {
            var editUser = _context.Users.FirstOrDefault(e => e.Id == id);
            var existedName = _context.Users.FirstOrDefault(e => e.Id != id && e.UserName.ToLower() == userName);
            if (existedName != null)
            {
                return 1;
            };
            if (_context.Users.Any(e => e.Email == email && e.Id != id) == true)
            {
                return 2;
            }
            return 0;
        }

    }
}
