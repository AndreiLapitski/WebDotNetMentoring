using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NorthwindApp.Attributes;

namespace NorthwindApp.Models
{
    [Table("Products")]
    public class Product
    {
        [DisplayName("ID")]
        public int ProductId { get; set; }

        [DisplayName("Name")]
        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }

        [DisplayName("Supplier")]
        [Required]
        public int? SupplierId { get; set; }

        [DisplayName("Category")]
        [Required]
        public int? CategoryId { get; set; }

        [DisplayName("Quantity per unit")]
        [StringLength(20)]
        [Required]
        public string QuantityPerUnit { get; set; }

        [DisplayName("Price")]
        [Range(0, 100_000)]
        [Required]
        public decimal? UnitPrice { get; set; }

        [DisplayName("Units in stock")]
        [Range(0, 100_000)]
        [Required]
        public short? UnitsInStock { get; set; }

        [DisplayName("Units on order")]
        [Range(0, 100_000)]
        [Required]
        public short? UnitsOnOrder { get; set; }

        [DisplayName("Reorder level")]
        [Range(0, 100_000)]
        [Required]
        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }


        [NotDisplay]
        public Supplier Supplier { get; set; }

        [NotDisplay]
        public Category Category { get; set; }
    }
}
