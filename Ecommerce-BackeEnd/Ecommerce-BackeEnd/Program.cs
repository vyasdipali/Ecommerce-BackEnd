using DataBase;
using Ecommerce_BackeEnd;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EcommerceDbContext>(options => options.UseSqlServer(connectionString, p => p.MigrationsAssembly("Ecommerce-BackeEnd")));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})

        .AddJwtBearer("JwtBearer", jwtBearerOptions =>
     {
         jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuerSigningKey = true,
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.SecretKey)),
             ValidateIssuer = false,
             //ValidIssuer = "The name of the issuer",  
             ValidateAudience = false,
             //ValidAudience = "The name of the audience",
             ValidateLifetime = true, //validate the expiration and not before values in the token
             ClockSkew = TimeSpan.FromHours(24) //5 minute tolerance for the expiration date
         };
     });

builder.Services.AddSwaggerGen(c =>
{
    var securityScheme = new OpenApiSecurityScheme()
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT" // Optional
    };

    var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "bearerAuth"
                            }
                        },
                        new string[] {}
                    }
                };

    c.AddSecurityDefinition("bearerAuth", securityScheme);
    c.AddSecurityRequirement(securityRequirement);
});



builder.Services.AddCors(b => b.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
}));


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
