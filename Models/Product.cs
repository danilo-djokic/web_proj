using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CafeManager.Models
{
    
    public class Product
    {
        [Key]
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string ProductName { get; set; } =null!;
        
        [Required]
        public int Price { get; set; }
        [JsonIgnore]
        public ICollection<Table>? Tables {get; set;}
    }
}