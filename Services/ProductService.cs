using Grpc.Core;
using Prod.DB;
using Prod.Entity;

namespace Prod.Services
{
    public class ProductService : CatalogService.CatalogServiceBase
    {
        //Условное хранилище/Бд
        private ApplicationDbContext _db = null!;

        public override Task<ListReply> GetAllProducts(EmptyRequest request, ServerCallContext context)
        {
            var listReply = new ListReply(); //создаем коллекцию отправляемую в ответ

            var productsCollection = _db.ProductInfos.Select(product =>
            new ProductResponse { Id = product.Id, Name = product.Name, Count = product.Count, Price = product.Price });
            //преобразуем обьекты из хранилища в обьекты требуемые для ответа
            
            listReply.Infos.AddRange(productsCollection); // добавляем их в коллекцию для ответа
            
            return Task.FromResult(listReply);

            //добавить обработку исключений если коллекция пуста или нет доступа
        }
        public override Task<ProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            var product = _db.ProductInfos.FirstOrDefault(p => p.Id == request.Id); //ищем обьект по ID

            if (product == null) //если его нет, выкидываем исключение
                throw new RpcException(new Status(StatusCode.NotFound, $"Product with Id - {request.Id}: Not Found"));

            var productResponse = new ProductResponse() 
            { Id = product.Id,Name=product.Name,
              Count=product.Count,
              Price=product.Price}; //преобразуем в класс для ответа

            return Task.FromResult(productResponse);
        }
        public override Task<ProductResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
        {
            var newProduct = new ProductInfo() {Name=request.Name, Price=request.Price,Count=request.Count};//создаем новый обьект с параметрами из запроса

            if(_db.ProductInfos.Contains(newProduct))
                throw new RpcException(new Status(StatusCode.InvalidArgument,$"{newProduct} - уже есть в БД"));

            _db.ProductInfos.Add(newProduct);
            _db.SaveChangesAsync();

            //добавляем его в хранилище/БД
            //подумать о том как не добавлять уже существующие товары
            return Task.FromResult(new ProductResponse() { Id = newProduct.Id,Name=newProduct.Name,Price=newProduct.Price,Count=newProduct.Count});
        }
        public override Task<Response> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
        {
            var product = _db.ProductInfos.FirstOrDefault(p => p.Id == request.Id);
            if (product == null) //если его нет, выкидываем исключение
                throw new RpcException(new Status(StatusCode.NotFound, $"Product with Id - {request.Id}: Not Found"));

            _db.ProductInfos.Remove(product); //удаляем если нашли
            _db.SaveChangesAsync();

            return Task.FromResult(new Response() { Id = product.Id});
        }
        public override Task<ProductResponse> UpdateProduct(CreateProductRequest request, ServerCallContext context)
        {
            var product = _db.ProductInfos.FirstOrDefault(p => p.Name == request.Name);
            if (product == null) //если его нет, выкидываем исключение
                throw new RpcException(new Status(StatusCode.NotFound, $"Product with Id - {request.Name}: Not Found"));

            //изменяем данные
            product.Name = request.Name;
            product.Price = request.Price;
            product.Count = request.Count;
            _db.SaveChangesAsync();

            return Task.FromResult(new ProductResponse() 
            { Id = product.Id, Name=product.Name, Price=product.Price, Count=product.Count});
        }
    }
}
