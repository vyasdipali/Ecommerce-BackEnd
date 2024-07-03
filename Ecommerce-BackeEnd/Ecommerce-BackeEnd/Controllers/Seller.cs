using DataBase;
using Ecommerce_BackeEnd.IOModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Api;

namespace Ecommerce_BackeEnd.Controllers
{

    [Authorize]
    [Route("[controller]")]
    [Microsoft.AspNetCore.Mvc.ApiController]

    public class Seller : ControllerBase
    {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment env;
        private string wwwPath = string.Empty;
        private readonly EcommerceDbContext _context;
        private string contentPath = string.Empty;

        public Seller(Microsoft.AspNetCore.Hosting.IHostingEnvironment environment, EcommerceDbContext context)
        {
            env = environment;
            _context = context;
            wwwPath = this.env.WebRootPath;
            contentPath = this.env.ContentRootPath;

        }

        [HttpPost]
        [Route("Add")]
        public async Task<BaseResponseModel> Add(AddSellerInput InputData)
        {
            var resp = new BaseResponseModel();
            try
            {
                   if (InputData != null)
                    {
                        var input = new AddSellerDb();
                        input.SellerID = InputData.SellerID;
                    input.SellerName = InputData.SellerName;
                    input.BusinessName = InputData.BusinessName;
                    input.SellerPhoneNo = InputData.SellerPhoneNo;
                    input.SellerEmail = InputData.SellerEmail;
                    input.AadharCardNumber = InputData.AadharCardNumber;
                    input.CreatedAt = DateTime.Now;

                    await _context.AddSellers.AddAsync(input);
                        await _context.SaveChangesAsync();

                        resp.Data = input;
                        resp.Status = true;
                        resp.Message = "Seller Added successfully";
                    }
                    else
                    {
                        resp.Status = false;
                        resp.Message = "Seller Data not provided";
                        resp.Data = new object();
                    }
             
            }
            catch (Exception ex)
            {
                resp.Status = false;
                resp.Message = ex.Message;
                resp.Data = new object();
            }
            return resp;
        }



        [Route("GetSellerData")]
        [HttpGet]
        public async Task<BaseResponseModel> GetSellerData()
        {
            var res = new BaseResponseModel();
            try
            {
                var data = await _context.AddSellers.ToListAsync();
                if (data is not null)
                {
                    res.Status = true;
                    res.Data = data;
                    res.Message = " Seller Data Fetch Successfully";
                }
                else
                {
                    res.Status = false;
                    res.Message = " Seller  Data Not Found";
                }
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
            }
            return res;
        }
        [HttpPost]
        [Route("SellerUpdate")]


        public async Task<BaseResponseModel> SellerUpdate(UpdateSellerInput InputData)
        {
            var resp = new BaseResponseModel();
            try
            {
              
                    if (InputData != null)
                    {
                        var existingSeller = await _context.AddSellers.FirstOrDefaultAsync(x => x.SellerID == InputData.SellerID);
                        if (existingSeller != null)
                        {
                        existingSeller.SellerName = InputData.SellerName;
                        existingSeller.BusinessName = InputData.BusinessName;
                        existingSeller.SellerPhoneNo = InputData.SellerPhoneNo;
                        existingSeller.SellerEmail = InputData.SellerEmail;
                        existingSeller.AadharCardNumber = InputData.AadharCardNumber;
                     


                            _context.AddSellers.Update(existingSeller);
                            await _context.SaveChangesAsync();

                            resp.Data = existingSeller;
                            resp.Status = true;
                            resp.Message = "Seller Data Updated successfully";
                        }
                        else
                        {
                            resp.Status = false;
                            resp.Message = "Seller not found";
                        }
                    }
                    else
                    {
                        resp.Status = false;
                        resp.Message = "Seller Data not provided";
                        resp.Data = new object();
                    }
               
            }
            catch (Exception ex)
            {
                resp.Status = false;
                resp.Message = ex.Message;
                resp.Data = new object();
            }
            return resp;
        }



        [HttpPost]
        [Route("DeletSellerID")]

        public async Task<ActionResult<BaseResponseModel>> DeletSellerID([FromQuery] int SellerID)
        {
            var res = new BaseResponseModel();

            try
            {
                var Seller = await _context.AddSellers.FirstOrDefaultAsync(c => c.SellerID == SellerID);

                if (Seller == null)
                {
                    res.Status = false;
                    res.Message = "Seller not found.";
                    return NotFound(res);
                }

                _context.AddSellers.Remove(Seller);

                await _context.SaveChangesAsync();

                res.Status = true;
                res.Message = " Seller removed successfully";
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
                res.Data = null;

            }
            return res;
        }

        [HttpPost]
        [Route("AddProduct")]


        public async Task<BaseResponseModel> AddProduct(ProductInput InputData)
        {
            var resp = new BaseResponseModel();

            try
            {
                if (InputData != null)
                {
                    var input = new ProductDb();
                    input.ProductID = InputData.ProductID;
                    input.SellerID = InputData.SellerID;
                    input.ProductName = InputData.ProductName;
                    input.ProductDescription = InputData.ProductDescription;
                    input.ProductRs = InputData.ProductRs;

                    input.ProductImage = await common.UploadFile(env.ContentRootPath, InputData.ProductImage, FolderPath.ProductImage);
                    input.CreatedAt = DateTime.Now;
                    await _context.ProductDbs.AddAsync(input);
                    await _context.SaveChangesAsync();

                    resp.Data = input;
                    resp.Status = true;
                    resp.Message = "Product Add successfully";
                }
                else
                {
                    resp.Status = false;
                    resp.Message = "Product not save";
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


        [Route("GetProductForSellerID/{id:int}")]
        [HttpGet]

        public async Task<BaseResponseModel> GetProductForSellerID(int id)
        {
            var res = new BaseResponseModel();

            try
            {
                var data = await _context.ProductDbs.Where(x => x.SellerID == id).ToListAsync();

                if (data is not null)
                {
                    res.Status = true;
                    res.Data = data;
                    res.Message = "Data Fetch Successfully";
                }
                else
                {
                    res.Status = false;
                    res.Message = "Data Not Found";
                }
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
            }
            return res;
        }

        [HttpPost]
        [Route("ProductDelet")]

        public async Task<ActionResult<BaseResponseModel>> ProductDelet([FromQuery] int ProductID)
        {
            var res = new BaseResponseModel();

            try
            {
                var Product = await _context.ProductDbs.FirstOrDefaultAsync(c => c.ProductID == ProductID);

                if (Product == null)
                {
                    res.Status = false;
                    res.Message = "Product not found.";
                    return NotFound(res);
                }

                _context.ProductDbs.Remove(Product);

                await _context.SaveChangesAsync();

                res.Status = true;
                res.Message = " Product removed successfully";
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
                res.Data = null;

            }
            return res;
        }
    }




}
