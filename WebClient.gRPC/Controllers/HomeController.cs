using Microsoft.AspNetCore.Mvc;

namespace WebClient.gRPC
{

    [Route("")]
    public class HomeController : Controller
    {
        private readonly CatalogService.CatalogServiceClient _catalogServiceClient;
        public HomeController(CatalogService.CatalogServiceClient catalogService) 
        {
            _catalogServiceClient = catalogService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/GetAll")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var result = await _catalogServiceClient.GetAllProductsAsync(new EmptyRequest());

                var prodInfos = result.Infos.Select(p => new ProductViewModel() { Id = p.Id,Name=p.Name,Price=p.Price,Count=p.Count}).ToList();

                return Ok(prodInfos);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("/Get")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var productRequest = new GetProductRequest { Id = id };
                var result = await _catalogServiceClient.GetProductAsync(productRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("/Delete")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _catalogServiceClient.DeleteProductAsync(new DeleteProductRequest() {Id = id});
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpPost("/Create")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductViewModel product)
        {
            try
            {
                //var createRequest = new CreateProductRequest() { Name = product.Name, Price = product.Price,Count = product.Count};
                //var response = _catalogServiceClient.CreateProductAsync(createRequest);

                return View("CreateView");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Route("Error")]
        public IActionResult Error(string message)
        {
            var errorViewModel = new ErrorViewModel()
            {
                RequestId = HttpContext.TraceIdentifier,
                Message = message
            };

            return View(errorViewModel);
        }

    }
}


  
