
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PhoneShop.DI.Account;
using PhoneShop.DI.Banner;
using PhoneShop.DI.Category;
using PhoneShop.DI.DeliveryProcess;
using PhoneShop.DI.DI_User.Banner_User;
using PhoneShop.DI.DI_User.Category_User;
using PhoneShop.DI.DI_User.Evaluate_Product_User;
using PhoneShop.DI.DI_User.ImageProduct_User;
using PhoneShop.DI.DI_User.MyAddress_User;
using PhoneShop.DI.DI_User.Order_User;
using PhoneShop.DI.DI_User.PaymentResponses;
using PhoneShop.DI.DI_User.Product_User;
using PhoneShop.DI.DI_User.ReviewProduct_User;
using PhoneShop.DI.DI_User.SupportContentData;
using PhoneShop.DI.DI_User.SupportDirectoryData;
using PhoneShop.DI.DI_User.Voucher_User;
using PhoneShop.DI.ImageProduct;
using PhoneShop.DI.Introduce;
using PhoneShop.DI.Order;
using PhoneShop.DI.Product;
using PhoneShop.DI.Role;
using PhoneShop.DI.Specification;
using PhoneShop.DI.SupportContent;
using PhoneShop.DI.SupportDirectory;
using PhoneShop.DI.Voucher;
using PhoneShop.Extension.CollaborativeFiltering;
using PhoneShop.Libraries;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using PhoneShop.Services;
using PhoneShop.Services.ChatHub;
using Stripe;
using System;
using System.Configuration;
using Twilio.Clients;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container. //tu load trang khi save .cshtml
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();



builder.WebHost.ConfigureAppConfiguration((hostingContext, config) =>
{
    // ... cấu hình khác

    if (hostingContext.HostingEnvironment.IsDevelopment())
    {
        builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
        //services.AddSession();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();

        builder.Services.AddScoped<ISupportDirectoryRepository, SupportDirectoryRepository>();

        builder.Services.AddScoped<ISupportContentRepository, SupportContentRepository>();

        builder.Services.AddScoped<IImageProductRepository, ImageProductRepository>();
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IBannerRepository, BannerRepository>();

        builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
        builder.Services.AddScoped<IIntroduceRepository, IntroduceRepository>();
        
        builder.Services.AddScoped<IDeliveryProcessRepository, DeliveryProcessRepository>();



        //user
        builder.Services.AddScoped<IProduct_UserRepository, Product_UserRepository>();
        builder.Services.AddScoped<ISupportDirectory_Repository,SupportDirectory_Repository> ();
        builder.Services.AddScoped<ISupportContent_Repository, SupportContent_Repository>();
        builder.Services.AddScoped<IBanner_UserRepository, Banner_UserRepository>();
        
        builder.Services.AddScoped<ISpecificationRepository, SpecificationRepository>();

        builder.Services.AddScoped<IVnPayService, VnPayService>();

        builder.Services.AddScoped<IPaymentResponse_Repository,PaymentResponses_Repository >();

        builder.Services.AddScoped<ICategory_UserRepository, Category_UserRepository>();

        builder.Services.AddScoped<IImageProduct_UserRepository, ImageProduct_UserRepository>();

        builder.Services.AddScoped<IReviewProduct_UserRepository, ReviewProduct_UserRepository>();


        builder.Services.AddScoped<IOrder_UserRepository, Order_UserRepository>();

        builder.Services.AddScoped<IVoucher_UserRepository, Voucher_UserRepository>();

        builder.Services.AddScoped<IMyAddress_UserRepository, MyAddress_UserRepository>();

        builder.Services.AddScoped<IEvaluate_ProductRepository, Evaluate_ProductRepository>();

        //thuat toan goi y
        builder.Services.AddScoped<CollaborativeFilteringService>();




        builder.Services.AddScoped<EmailService>();

        //api
        builder.Services.AddHttpClient();


        //API
        // Trong phương thức ConfigureServices của Startup.cs
        //builder.Services.AddDistributedRedisCache(options =>
        //{
        //    options.Configuration = "localhost"; // Thay đổi thành địa chỉ Redis của bạn
        //    options.InstanceName = "SampleInstance";
        //});

    }
});


builder.Services.AddControllersWithViews().AddNewtonsoftJson();

//cau hinh xac thuc sdt
builder.Services.ConfigureOptions<ConfigureTwilioSettings>();
// Đăng ký TwilioClient
builder.Services.AddScoped(provider =>
{
    var twilioSettings = provider.GetRequiredService<IOptions<TwilioSettings>>().Value;
    return new TwilioRestClient(twilioSettings.AccountSid, twilioSettings.AuthToken);
});

builder.Services.AddTransient<SmsService>();

//chat real time
builder.Services.AddSignalR();





builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});
builder.Services.AddResponseCaching();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(p =>
              {
                  p.Cookie.Name = "UserLoginCookie";
                 
                  p.ExpireTimeSpan = TimeSpan.FromHours(1);
                  p.LoginPath = "/Login.html";
              
              });

//connect db
builder.Services.AddDbContext<ShopPhoneDbContext>(options =>
{
    options.UseSqlServer("Server=LAPTOP-LH4CD5VL;Database=PhoneShop_Core6;Trusted_Connection=True;Connect Timeout=60;MultipleActiveResultSets=True");
});






var app = builder.Build();

//Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // Cấu hình middleware xử lý ngoại lệ
    app.UseExceptionHandler("/error/404");

    // Bật cơ chế HSTS
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();  // Kích hoạt Middleware Session

app.UseAuthentication();

app.UseAuthorization();


app.UseHttpsRedirection();

app.UseResponseCaching();

// Middleware để xử lý yêu cầu không xử lý được
app.Use(async (context, next) =>
{
    await next(); // Cho middleware tiếp theo xử lý yêu cầu

    // Nếu không tìm thấy trang
    if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
    {
        // Chuyển hướng đến trang lỗi tùy chỉnh 
        context.Response.Redirect("/error/pageNotFound");
    }
   
});


app.UseEndpoints(endpoints =>
{

    endpoints.MapHub<ChatHub>("/chathub");


    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute(
   name: "default",
   pattern: "{controller=Home}/{action=Index}/{id?}");

    // Ánh xạ trang lỗi 404 tùy chỉnh

    endpoints.MapControllerRoute(
        name: "demoroute",
        pattern: "demothoi/{Title}",
        defaults: new { controller = "ProductController", action = "demoget" });



});



app.Run();
