

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Models
{
    [Table("UserDetail")]
    public class UserDetail
    {
        [Key]
        public string UserId { get; set; }
        [MinimumAge(10, 50)]
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }
        public long? SchoolId { get; set; }
        public long? DepartmentId { get; set; }
        public long? ClassId { get; set; }
        public int? Role { get; set; }
    }
}
