using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using NorthwindApp.Attributes;

namespace NorthwindApp.Models
{
    [Table("Categories")]
    public class Category
    {
        [DisplayName("ID")]
        public int CategoryId { get; set; }

        [DisplayName("Name")]
        public string CategoryName { get; set; }

        public string Description { get; set; }

        [NotDisplay]
        public byte[] Picture { get; set; }


        [NotDisplay]
        public ICollection<Product> Products { get; set; }
    }
}
