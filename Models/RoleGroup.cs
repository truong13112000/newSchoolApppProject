using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Models
{
    [Table("RoleGroup")]
    public class RoleGroup
    {
        [Key]
        public Guid Id { get; set; }
        public string GroupId { get; set; }
        public string RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual IdentityRole IdentityRole { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
    }
}
