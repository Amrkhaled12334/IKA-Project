using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Dto_s.Departments
{
    public class UpdatedDepartmentDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="The Name is Required!!")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "The Code is Required!!")]

        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        [Display(Name = "Date Of Creation")]
        public DateOnly CreationDate { get; set; }
    }
}
