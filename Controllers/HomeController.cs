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

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet("/GetAll")]
        public async Task<ActionResult<ListReply>> GetAllProducts()
        {
            try
            {
                var result = await _productService.GetAllProducts(new EmptyRequest(),null);

                var prodInfos = result.Infos.Select(p => new ProductInfo { Id = p.Id, Name = p.Name, Count = p.Count, Price = p.Price }).ToList();

                return View("Index",prodInfos);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("/Get")]
        public async Task<ActionResult> GetById(int id)
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

        [HttpDelete("Home/Delete")]
        public async Task<ActionResult> DeleteProduct(int id)
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

    }
}

  
