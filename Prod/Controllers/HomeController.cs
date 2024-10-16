using Microsoft.AspNetCore.Mvc;

namespace Web
{
    [Route("")]
    public class HomeController : Controller
    {
        readonly ProductService _productService;

        public HomeController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpGet("/GetAll")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var result = await _productService.GetAllProducts(new EmptyRequest(),null);

                var prodInfos = result.Infos.Select(p => new ProductInfo { Id = p.Id, Name = p.Name, Count = p.Count, Price = p.Price }).ToList();

                return View("GetProducts",prodInfos);
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
                var result = await _productService.GetProduct(productRequest, null);
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
                await _productService.DeleteProduct(new DeleteProductRequest() { Id = id}, null);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody]CreateProductRequest request)
        {
            try
            {
                var response = _productService.CreateProduct(request,null);

                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
            catch (System.Exception ex)
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

  
