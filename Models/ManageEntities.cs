using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Models
{
    public class ManageEntities
    {
        [Key]
        public Guid Key { get; set; }
        public string Screen_Name { get; set; }
        public string Taget_Name { get; set; }
        public string Display_Name { get; set; }
        public float Value { get; set; }
        public DateTime C_Date { get; set; }
        [StringLength(500)]
        public string C_User { get; set; }
        public DateTime? D_Date { get; set; }
        [StringLength(500)]
        public string? D_User { get; set; }
        public bool IsDeleted { get; set; }
    }
}
