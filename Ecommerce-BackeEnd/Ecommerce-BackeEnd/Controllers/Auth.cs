using DataBase;
using Ecommerce_BackeEnd.IOModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Restaurant_Api;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce_BackeEnd.Controllers
{
    [Authorize]
    [Route("api/")]
    [ApiController]
    public class Auth : ControllerBase

    {

        private Microsoft.AspNetCore.Hosting.IHostingEnvironment env;
        private string wwwPath = string.Empty;
        private readonly EcommerceDbContext _context;
        private string contentPath = string.Empty;


        public Auth(Microsoft.AspNetCore.Hosting.IHostingEnvironment environment, EcommerceDbContext context)
        {
            env = environment;
            _context = context;
            wwwPath = this.env.WebRootPath;
            contentPath = this.env.ContentRootPath;
        }

        string GetToken(int Userid)
        {
            string key = Config.SecretKey;//Secret key which will be used later during validation    
            var issuer = Config.issuer;  //normally this will be your site URL    

            var audience = Config.audience;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("UserID", Userid.ToString()));

            var token = new JwtSecurityToken(issuer, //Issure    
                            audience,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt_token;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("SignUp")]

        public async Task<BaseResponseModel> SignUp(SignUpInputModel InputData)
        {
            var res = new BaseResponseModel();

            try
            {
                if (InputData != null)
                {

                    var userMaster = _context.UserMasters.FirstOrDefault(x => x.PhoneNo.ToLower() == InputData.PhoneNo.Trim().ToLower());
                    if (userMaster != null)
                    {
                        res.Status = false;
                        res.Message = "User Already Exist !! Please LogIn";
                        res.Data = InputData;
                        return res;
                    }
                    if (_context.UserMasters.Any(n => n.Email == InputData.Email))
                    {
                        res.Status = false;
                        res.Message = "Entered email is already linked with another account. Please enter a different email";
                        return res;
                    }
                    var InputModel = JsonConvert.DeserializeObject<UserMaster>(JsonConvert.SerializeObject(InputData)) ?? new();
                    InputModel.Email = InputData.Email;
                    InputModel.Password = common.EncyptData(InputData.Password);
                    InputModel.UserName = InputData.UserName;
                    InputModel.PhoneNo = InputData.PhoneNo;
                    InputModel.CreatedAt = DateTime.Now;
                    _context.UserMasters.Add(InputModel);
                    _context.SaveChanges();

                    var UserData = _context.UserMasters.FirstOrDefault(b => b.Email == InputData.Email);
                    if (UserData != null)
                    {
                        var LoginResponse = JsonConvert.DeserializeObject<LoginResponseModelDemo>(JsonConvert.SerializeObject(UserData)) ?? new();
                        LoginResponse.Token = GetToken(UserData.UserID);

                        res.Status = true;
                        res.Message = "Registration successful";
                        res.Data = LoginResponse;
                    }
                    else
                    {
                        res.Status = false;
                        res.Message = "Details not found !!";
                    }
                }
                else
                {
                    res.Status = false;
                    res.Message = "Registratioin Details not found !!";
                }

            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
                res.Data = new object();
            }
            return res;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]

        public async Task<BaseResponseModel> Login(LoginInputModelDemo InputData)
        {
            var resp = new BaseResponseModel();

            try
            {
                if (InputData != null)
                {
                    var data = await _context.UserMasters.FirstOrDefaultAsync(x => x.PhoneNo.ToLower() == InputData.PhoneNo.Trim().ToLower());
                    if (data is not null)
                    {
                        InputData.Password = common.EncyptData(InputData.Password);
                        var temp = common.DecryptData(data.Password);
                        if (data.PhoneNo == InputData.PhoneNo)
                        {
                            if (data != null && data.Password == InputData.Password)
                            {

                                var loginRespData = JsonConvert.DeserializeObject<LoginResponseModelDemo>(JsonConvert.SerializeObject(data)) ?? new();
                                loginRespData.Token = GetToken(loginRespData.UserID);
                                if (loginRespData.Token != null)
                                {
                                    resp.Status = true;
                                    resp.Message = "User login Successfully!!";
                                    resp.Data = loginRespData;
                                }
                                else
                                {
                                    resp.Status = false;
                                    resp.Message = "User login faild!!";
                                    resp.Data = new object();
                                }

                            }
                            else
                            {
                                resp.Status = false;
                                resp.Message = "Invalid Password";
                                resp.Data = new object();
                            }
                        }
                        else
                        {
                            resp.Status = false;
                            resp.Message = "User Not Found";
                            resp.Data = new object();
                        }
                    }
                    else
                    {
                        resp.Status = false;
                        resp.Message = "Requested phone number does not exists, Please sign up.";
                        resp.Data = new object();
                    }
                }
                else
                {
                    resp.Status = false;
                    resp.Message = "Invalid Input !!";
                    resp.Data = new object();
                }
            }
            catch (Exception ex)
            {
                resp.Status = false;
                resp.Message = ex.Message; ;
                resp.Data = new object();
            }
            return resp;
        }

    }
}
