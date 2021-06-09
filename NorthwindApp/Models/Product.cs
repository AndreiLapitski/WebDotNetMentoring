using System.ComponentModel;
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
        public string ProductName { get; set; }

        [DisplayName("Supplier")]
        public int? SupplierId { get; set; }

        [DisplayName("Category")]
        public int? CategoryId { get; set; }

        public string QuantityPerUnit { get; set; }

        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }


        [NotDisplay]
        public Supplier Supplier { get; set; }

        [NotDisplay]
        public Category Category { get; set; }
    }
}
