using Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<ProductService>();

builder.Services.AddGrpc();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();


var app = builder.Build();

app.UseExceptionHandler("/Error");

app.UseStaticFiles();
app.MapControllers();

app.Run();
