using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class AddSellerDb
    {
        [Key]
        public int SellerID { get; set; }

        public string SellerName { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public string SellerPhoneNo { get; set; } = string.Empty;
        public string SellerEmail { get; set; } = string.Empty;
        public string AadharCardNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
