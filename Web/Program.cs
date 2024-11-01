using Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddDbContext<ApplicationDbContext>();

var app = builder.Build();

app.MapGrpcService<ProductService>();

app.UseExceptionHandler("/Error");

app.Run();
