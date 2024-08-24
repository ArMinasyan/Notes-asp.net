using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Notes.Models;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("12345678123456781234567812345678"))
//         };
//     });

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DatabaseContext>(ops =>
{
    ops.UseSqlite(builder.Configuration.GetConnectionString("DevDB"));
});
builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

var app = builder.Build();


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