using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Web
{
    public class ProductService : CatalogService.CatalogServiceBase
    {
        private readonly ApplicationDbContext _db;

        public ProductService(ApplicationDbContext db)
        {
            _db = db;
        }

        public override async Task<ListReply> GetAllProducts(EmptyRequest request, ServerCallContext context)
        {
            var products = await _db.ProductInfos.AsNoTracking().ToListAsync();

            if (!products.Any())
                throw new RpcException(new Status(StatusCode.NotFound, $"Products Not Found"));

            var listReply = new ListReply(); //создаем коллекцию отправляемую в ответ

            listReply.Infos.AddRange(products.Select(product => 
                    new ProductResponse {
                    Id = product.Id,
                    Name = product.Name,
                    Count = product.Count,
                    Price = product.Price
                })); // добавляем их в коллекцию для ответа

            return listReply;
        }
        public override async Task<ProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            var product = await _db.ProductInfos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id); //ищем обьект по ID

            if (product == null) //если его нет, выкидываем исключение
                throw new RpcException(new Status(StatusCode.NotFound, $"Product with Id:{request.Id} - Not Found"));

            var productResponse = new ProductResponse()
            {
                Id = product.Id,
                Name = product.Name,
                Count = product.Count,
                Price = product.Price
            }; //преобразуем в класс для ответа

            return productResponse;
        }
        public override async Task<ProductResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
        {
            var newProduct = new Product()
            {
                Name = request.Name,
                Price = request.Price,
                Count = request.Count
            };//создаем новый обьект ProductInfo из запроса

            await _db.ProductInfos.AddAsync(newProduct);
            await _db.SaveChangesAsync();

            return new ProductResponse() 
            { 
                Id = newProduct.Id, 
                Name = newProduct.Name, 
                Price = newProduct.Price, 
                Count = newProduct.Count 
            };
        }
        public override async Task<Response> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
        {
            var product = await _db.ProductInfos.FirstOrDefaultAsync(p => p.Id == request.Id);
            if (product == null) //если его нет, выкидываем исключение
                throw new RpcException(new Status(StatusCode.NotFound, $"Product with Id:{request.Id} - Not Found"));

            _db.ProductInfos.Remove(product);
            await _db.SaveChangesAsync();

            return new Response() { Id = product.Id };
        }
        public override async Task<ProductResponse> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
        {
            var product = await _db.ProductInfos.FirstOrDefaultAsync(p => p.Id == request.Id);
            if (product == null) //если его нет, выкидываем исключение
                throw new RpcException(new Status(StatusCode.NotFound, $"Product with Id:{request.Id} - Not Found"));

            //изменяем и сохраняем данные
            product.Name = request.Name;
            product.Price = request.Price;
            product.Count = request.Count;
            await _db.SaveChangesAsync();

            return new ProductResponse()
            {
                Id = product.Id, 
                Name = product.Name, 
                Price = product.Price, 
                Count = product.Count 
            };

        }
    }
}

