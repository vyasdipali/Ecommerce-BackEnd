namespace Ecommerce_BackeEnd.IOModel
{
    public class AddSellerInput
    {
        public int SellerID { get; set; }
        public string SellerName { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public string SellerPhoneNo { get; set; } = string.Empty;
        public string SellerEmail { get; set; } = string.Empty;
        public string AadharCardNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }


    public class UpdateSellerInput
    {
        public int SellerID { get; set; }
        public string SellerName { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public string SellerPhoneNo { get; set; } = string.Empty;
        public string SellerEmail { get; set; } = string.Empty;
        public string AadharCardNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
