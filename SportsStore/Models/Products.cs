using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class Products
    {
        [Key]
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Please enter a product Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter a Description")]
        public string Description { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage ="Please enter a positive price")] // https://stackoverflow.com/questions/22126137/decimal-value-is-not-valid-for-price-mvc3
        //[Range(typeof(decimal), "0.01", "100000.00", ErrorMessage = "enter decimal value")]
        //[RegularExpression(@"^\[0-9]{1,6}\.[0-9]{2}$", ErrorMessage = "enter decimal value of format $9.99")]
        public decimal Price { get; set; }
        [Required(ErrorMessage ="Please specify a category")]
        public string Category { get; set; }
        
    }


}
