using DataBase;
using Ecommerce_BackeEnd.IOModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_BackeEnd.Controllers
{


    [Authorize]
    [Route("[controller]")]
    [Microsoft.AspNetCore.Mvc.ApiController]


    public class User : ControllerBase
    {

        private Microsoft.AspNetCore.Hosting.IHostingEnvironment env;
        private string wwwPath = string.Empty;
        private readonly EcommerceDbContext _context;
        private string contentPath = string.Empty;

        public User(Microsoft.AspNetCore.Hosting.IHostingEnvironment environment, EcommerceDbContext context)
        {
            env = environment;
            _context = context;
            wwwPath = this.env.WebRootPath;
            contentPath = this.env.ContentRootPath;

        }


        [HttpPost]
        [Route("GetAllProduct")]


        public async Task<BaseResponseModel> GetAllProduct()
        {
            var res = new BaseResponseModel();
            try
            {
                var data = await _context.ProductDbs.ToListAsync();
                if (data is not null)
                {
                    res.Status = true;
                    res.Data = data;
                    res.Message = " Product Data Fetch Successfully";
                }
                else
                {
                    res.Status = false;
                    res.Message = " Product  Data Not Found";
                }
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
            }
            return res;
        }


        [Route("GetProductId/{id:int}")]
        [HttpGet]

        public async Task<BaseResponseModel> GetProductId(int id)
        {
            var res = new BaseResponseModel();

            try
            {
                var data = await _context.ProductDbs.Where(x => x.ProductID == id).ToListAsync();

                if (data is not null)
                {
                    res.Status = true;
                    res.Data = data;
                    res.Message = "Data Fetch for Product ID Successfully";
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
    }

    }
