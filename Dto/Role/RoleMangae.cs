using Microsoft.AspNetCore.Identity;

namespace SchoolProject.Dto.Role
{
    public class RoleMangae
    {
        public string ScreenName { get; set; }
        public List<IdentityRole> NeWGroup { get; set; }
    }
}
