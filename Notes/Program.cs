using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.OpenApi.Models;
using System.Security.AccessControl;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Notes;
using Notes.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("12345678123456781234567812345678")),
            ValidIssuer = builder.Configuration["JwtSettings:ValidIssuer"],
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = false
        };
    });

services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

services.AddHttpContextAccessor();
services.AddControllersWithViews();
services.AddDbContext<DatabaseContext>(ops =>
{
    ops.UseSqlite(builder.Configuration.GetConnectionString("DevDB"));
});
services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

services.AddEndpointsApiExplorer();
services.AddControllers();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    dbContext.Database.EnsureCreated();
    var user = dbContext.Users.SingleOrDefault(user => user.Username == "admin");
    if (user is null)
    {
        dbContext.Users.Add(new UserModel
        {
            Username = "admin",
            Password = "$2a$12$njGx9EUcQ9Iu.Ms4jgM1yepvmjVnYK99NsrJAZ1zr4BmJBSTjUlFe"
        });
        dbContext.SaveChanges();
    }
}


app.Run();