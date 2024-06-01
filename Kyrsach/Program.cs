using Kyrsach.Components;
using Kyrsach.Initializer;
using Kyrsach.Models;
using Kyrsach.Services;
using Kyrsach.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Dropbox.Api;
using DocumentFormat.OpenXml.InkML;

namespace Kyrsach
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            var dropboxToken = builder.Configuration.GetConnectionString("AccessToken");
         

            builder.Services.AddDbContext<SerovaContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddScoped<IDropboxService, DropboxService>();
            builder.Services.AddScoped(_ => new DropboxClient(dropboxToken));

            builder.Services.AddScoped<Initializer.IDbInitializer, DbInitializer>();
            builder.Services.AddTransient<ListUserViewComponent>();
            builder.Services.AddSignalR().AddHubOptions<Chat>(options =>
            {
                options.EnableDetailedErrors = true;
            });
            builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>(); ;
            builder.Services.AddAuthentication();
            builder.Services.AddIdentity<UserModel, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.ClaimsIdentity.UserIdClaimType = "id";
            })
               .AddEntityFrameworkStores<SerovaContext>();
            // Add services to the container.
            //builder.Services.AddHttpClient<IUserService, UserService>();
            //builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<DropboxClient>(_ =>
            {
                var appKey = builder.Configuration.GetValue<string>("Dropbox:AppKey");
                var appSecret = builder.Configuration.GetValue<string>("Dropbox:AppSecret");
                var accessToken = builder.Configuration.GetValue<string>("Dropbox:AccessToken");

                return new DropboxClient(appKey, appSecret, accessToken);
            });
            //    builder.Services.AddIdentity<UserWithRole, IdentityRole>()
            //.AddEntityFrameworkStores<SerovaContext>()
            //.AddDefaultTokenProviders();
            //    builder.Services.AddScoped<UserManager<UserWithRole>>();
            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = "oidc";
            //}).AddCookie(options =>
            //{
            //    options.Cookie.HttpOnly = true;
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            //    options.SlidingExpiration = true;
            //}).AddOpenIdConnect("oidc", options =>
            //{
            //    options.Authority = builder.Configuration["ServiceUrls:IdentityAPI"];
            //    options.GetClaimsFromUserInfoEndpoint = true;
            //    options.ClientId = "kyrsach";
            //    options.ClientSecret = "secret";
            //    options.ResponseType = "code";
            //    options.ClaimActions.MapJsonKey("role", "role", "role");
            //    options.ClaimActions.MapJsonKey("sub", "sub", "sub");
            //    options.TokenValidationParameters.NameClaimType = "name";
            //    options.TokenValidationParameters.RoleClaimType = "role";
            //    options.Scope.Add("kyrsach");
            //    options.SaveTokens = true;
            //});


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            SeedDataBase();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            //app.MapHub<ServiceHub>("/Service");
            app.MapHub<Chat>("/chatHub");
            app.Run();

            async Task SeedDataBase()
            {
                using (var scope = app.Services.CreateScope())
                {
                    var dbInitializer = scope.ServiceProvider.GetRequiredService<Initializer.IDbInitializer>();
                    await dbInitializer.Initialize();
                }
            }
        }
    }
}


