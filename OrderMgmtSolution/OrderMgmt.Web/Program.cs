using OrderMgmt.Business;
using OrderMgmt.DataAccess;
using OrderMgmt.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Connection string'i config'ten oku
string connStr = builder.Configuration.GetConnectionString("DefaultConnection");

// Repository ve servisleri DI container'a ekle
builder.Services.AddSingleton(new ProductRepository(connStr));
builder.Services.AddSingleton<ProductService>();

builder.Services.AddSingleton(new CustomerRepository(connStr));
builder.Services.AddSingleton<CustomerService>();

builder.Services.AddSingleton(new OrderRepository(connStr));
builder.Services.AddSingleton<EmailService>();
builder.Services.AddSingleton<OrderService>();

builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddSingleton<EmailService>();



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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
