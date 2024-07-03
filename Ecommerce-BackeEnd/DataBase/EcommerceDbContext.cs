using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class EcommerceDbContext : DbContext


    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> option) : base(option) { }

        public DbSet<UserMaster> UserMasters { get; set; }

        public DbSet<AddSellerDb> AddSellers { get; set; }


        public DbSet<ProductDb> ProductDbs { get; set; }


    }


}
