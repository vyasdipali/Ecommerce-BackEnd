using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class ProductDb
    {

        [Key]
        public int ProductID { get; set; }

        public int SellerID { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string ProductDescription { get; set; } = string.Empty;

        public string ProductImage { get; set; } = string.Empty;

        public int ProductRs { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
