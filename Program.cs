using Prod.DB;
using Prod.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>();
// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGrpcService<ProductService>();
app.MapGet("/", () => "Hello GRPC");

app.Run();
