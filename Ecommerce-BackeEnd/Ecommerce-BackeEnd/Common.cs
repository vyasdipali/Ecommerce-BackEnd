using Ecommerce_BackeEnd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Restaurant_Api
{
    public static class common
    {

        public static string SubscriptionPriceKey => "SubscriptionPrice";

        public static string SubscriptionDescriptionKey => "SubscriptionDescription";

        public static string SubscriptionTitleKey => "SubscriptionTitle";

        public static string SubscriptionPriceDesriptionTextKey => "SubscriptionPriceDesriptionText";

        public static string SubscriptionPriceStateKey => "SubscriptionPriceState";

        public static string SubscriptionPriceDistrictKey => "SubscriptionPriceDistrict";

        public static string SubscriptionPricePincodeKey => "SubscriptionPricePincode";

        public static string SubscriptionPriceFreelancerKey => "SubscriptionPriceFreelancer";





        public static int GetCurrentUser(ControllerBase controllerBase)
        {
            try
            {
                if (controllerBase.User.HasClaim(c => c.Type == "UserID"))
                {
                    var id = controllerBase.User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value ?? "0";
                    var UserId = int.Parse(id);
                    return UserId;
                }
                return 0;
            }
            catch (Exception ex)
            {
                _ = ex;
                return 0;
            }
        }


        public static string GetToken(int Userid)
        {
            string key = Config.SecretKey; //Secret key which will be used later during validation    
            var issuer = Config.issuer;  //normally this will be your site URL    

            var audience = Config.audience;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short       
            var permClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserID", Userid.ToString())
            };

            var token = new JwtSecurityToken(issuer, //Issure    
                            audience,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt_token;
        }


        public static string GetDistKey(int distType)
        {
            switch (distType)
            {
                case 1:
                    return SubscriptionPriceStateKey;
                case 2:
                    return SubscriptionPriceDistrictKey;
                case 3:
                    return SubscriptionPricePincodeKey;
                case 4:
                    return SubscriptionPriceFreelancerKey;
                default:
                    return string.Empty;
            }
        }

        #region EncyptData
        public static string EncyptData(string Text)
        {
            var key = Encoding.ASCII.GetBytes("@@FA$Fierce_Admin##Admin_Panel%%");
            var plainText = Text;
            var encryptedData = EncryptStringToBytes(key, plainText);
            string StrPwdFromByte = ByteArrayToString(encryptedData);
            return StrPwdFromByte;
        }
        #endregion

        #region DecryptData

        public static string DecryptData(string Text)
        {
            var key = Encoding.UTF8.GetBytes("@@FA$Fierce_Admin##Admin_Panel%%");
            Byte[] BytePassword = StringToByteArray(Text);
            var decryptedData = DecryptBytesToString(key, BytePassword);
            return (decryptedData);
        }
        #endregion

        static byte[] EncryptStringToBytes(byte[] key, string plainText)
        {
            byte[] encrypted;
            byte[] iv;

            // Create an RijndaelManaged object with the specified key. 
            using (var aes = CreateAes256Algorithm())
            {
                aes.Key = key;

                iv = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76, 0x12, 0x71, 0x20 };

                // Create a encrytor to perform the stream transform.
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, iv);

                // Create the streams used for encryption. 
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        // convert stream to bytes
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        static RijndaelManaged CreateAes256Algorithm()
        {
            return new RijndaelManaged { KeySize = 256, BlockSize = 128 };
        }

        #region ByteArrayToString
        static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        #endregion

        #region StringToByteArray
        static byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        #endregion

        static string DecryptBytesToString(byte[] key, byte[] data)
        {
            byte[] iv;
            iv = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76, 0x12, 0x71, 0x20 };
            byte[] cipherText = data;

            // Declare the string used to hold the decrypted text. 
            string plaintext;

            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (var aes = CreateAes256Algorithm())
            {
                aes.Key = key;
                aes.IV = iv;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                // Create the streams used for decryption. 
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
        public static async Task<string> UploadFile(string ContentRootPath, string Base64, string FilePath)
        {
            string file = string.Empty;
            if (!string.IsNullOrWhiteSpace(Base64) && !string.IsNullOrWhiteSpace(FilePath))
            {
                var Image = Convert.FromBase64String(Base64);
                string ImageName = DateTime.UtcNow.Ticks + GetFileExtension(Base64);
                string ImagePath = ContentRootPath + FilePath;
                if (!Directory.Exists(ImagePath))
                {
                    Directory.CreateDirectory(ImagePath);
                }
                file = Path.Combine(ImagePath, ImageName);
                if (Image.Length > 0)
                {
                    try
                    {
                        await File.WriteAllBytesAsync(file, Image);
                        file = FilePath + ImageName;
                    }
                    catch (Exception e)
                    {
                        e.ToString();
                    }

                }
            }
            return file;
        }

        public static string GetFileExtension(string base64String)
        {
            var data = base64String.Substring(0, 5).ToUpper();

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return ".png";
                case "/9J/4":
                    return ".jpg";
                case "AAAAG":
                    return ".mp4";
                case "AAAAI":
                    return ".mp4";
                case "JVBER":
                    return ".pdf";
                case "AAABA":
                    return ".ico";
                case "UMFYI":
                    return ".rar";
                case "E1XYD":
                    return ".rtf";
                case "U1PKC":
                    return ".txt";
                case "MQOWM":
                case "77U/M":
                    return ".srt";
                case "SUQZA":
                    return ".mp3";
                default:
                    return string.Empty;
            }
        }

        public static string RemoveFile(string ContentRootPath, string Input)
        {
            string message = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(Input))
                {
                    Input = ContentRootPath + Input;
                    FileInfo oldfile = new FileInfo(Input);
                    if (oldfile.Exists)
                    {
                        oldfile.Delete();
                        message = "File Removed";
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return message;
        }


        public static bool IsBase64String(string s)
        {
            try
            {
                byte[] data = Convert.FromBase64String(s);
                return true;
            }
            catch (Exception ex)
            {
                _ = ex;
                return false;
            }
        }

    }
    public static class Utils
    {
        private static readonly Regex sWhitespace = new Regex(@"\s+");
        public static string ReplaceWhitespace(this string input, string replacement)
        {
            return sWhitespace.Replace(input, replacement);
        }
    }



}

