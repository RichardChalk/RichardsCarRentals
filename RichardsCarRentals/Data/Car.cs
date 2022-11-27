using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreCodeFirstTogether.Data
{
    public class Car
    {
        [Key]
        [Required]
        [MaxLength(10)]
        public string LicensePlate { get; set; }


        [Required]
        [MaxLength(100)]
        public string? Make { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Model { get; set; }
        
        [Range(0,100)]
        public int ManufacturingYear { get; set; }

    }
}
