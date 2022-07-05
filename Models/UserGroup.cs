using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Models
{
    [Table("UserGroup")]
    public class UserGroup
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string GroupId { get; set; }

        [ForeignKey("UserId")]
        public virtual IdentityUser IdentityUser { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
    }
}
