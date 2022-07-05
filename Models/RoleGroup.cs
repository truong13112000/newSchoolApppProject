using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Models
{
    [Table("RoleGroup")]
    public class RoleGroup
    {
        public  int GroupId { get; set; }   
        public string RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual IdentityRole IdentityRole { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
    }
}
