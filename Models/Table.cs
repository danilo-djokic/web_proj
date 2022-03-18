using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace CafeManager.Models
{
    public class Table
    {
        [Key]
        [Required]
        public int Id { get; set; }
        
        [Required]
        public int NoOfSeats { get; set; }
        
        public bool Occupied { get; set; } = false;

        [ForeignKey("AdminId")]
        public int AdminId{ get; set;}

        public ICollection<Product>? Products{ get; set;}

    }
}