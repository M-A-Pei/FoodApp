using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FoodApp.Data;
using FoodApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version("10.4.32"))
    )
);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "YourSuperSecureKey12345!")
            )
        };

         // Specify that the token should come from the cookie (not the Authorization header)
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var cookie = context.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(cookie))
                {
                    context.Token = cookie;
                }
                return Task.CompletedTask;
            }
        };
    });


builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings")
);
// JWT Configuration from appsettings.json
var jwtKey = builder.Configuration["Jwt:Key"] ?? "YourSuperSecureKey12345!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "localhost";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "localhost";

// Register the JwtService
builder.Services.AddSingleton(new JwtService(jwtKey, jwtIssuer, jwtAudience));

builder.Services.AddTransient<IUploadService, UploadService>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

// Configure HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
