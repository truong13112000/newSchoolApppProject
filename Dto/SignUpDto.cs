using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Models;

namespace SchoolProject.Dto
{
    public class SignUpDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int GroupId { get; set; }
        public UserDetail UserDetail { get; set; }
        public IEnumerable<SelectListItem>? SchoolList { get; set; }
        public IEnumerable<SelectListItem>? DepartmentList { get; set; }
        public IEnumerable<SelectListItem>? ClassList { get; set; }
        public IEnumerable<SelectListItem>? RoleList { get; set; }
    }
}
