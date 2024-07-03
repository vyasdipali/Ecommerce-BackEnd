namespace Ecommerce_BackeEnd.IOModel
{
    public class ProductInput
    {
        public int ProductID { get; set; }

        public int SellerID { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string ProductDescription { get; set; } = string.Empty;

        public string ProductImage { get; set; } = string.Empty;

        public int ProductRs { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
