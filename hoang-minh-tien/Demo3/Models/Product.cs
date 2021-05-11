using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Demo3.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "Please enter product name")]
        public string ProductName { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Please enter price")]
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedTime { get; set; }

        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public Category Category { get; set; }
    }
}
