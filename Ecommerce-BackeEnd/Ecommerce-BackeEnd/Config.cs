namespace Ecommerce_BackeEnd
{
    public class Config
    {

        public static string SecretKey = "ECOMMERCEBACKEEND@0123654987_ismjsasdf";

        public static string issuer = "https://localhost:7256";
        public static string audience = "http://localhost:4201";
    }

    public class FolderPath
    {
        public static string CommonPath = "/Helper/";
        public static string ProductImage = CommonPath + "ProductImage/";

    }
}
