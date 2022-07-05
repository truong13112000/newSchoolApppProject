using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Models
{
    [Table("UserGroup")]
    public class UserGroup
    {
        public string UserId { get; set; }
        public int GroupId { get; set; }

        [ForeignKey("UserId")]
        public virtual IdentityUser IdentityUser { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
    }
}
