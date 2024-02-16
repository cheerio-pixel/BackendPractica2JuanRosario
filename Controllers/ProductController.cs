

using System.Security.Claims;
using backend.Authentication;
using backend.Models;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductStatusService _productStatusService;
        private readonly IProductFactory _productFactory;

        public ProductController(IProductStatusService productStatusService, IProductFactory productFactory)
        {
            _productStatusService = productStatusService;
            _productFactory = productFactory;
        }

        [Route("products/status")]
        [HttpGet, Authorize(nameof(Access.VerEstadoProductos))]
        public IActionResult GetProductStatusDetails(bool? filtered)
        {
            string? rol = HttpContext.User.Claims.FirstOrDefault(
                c => c.Type == ClaimTypes.Role
            )?.Value;
            if (rol == null)
            {
                return Forbid();
            }
            IEnumerable<ProductStatus> productStatuses = _productStatusService.PullStatuses(rol);
            if (filtered ?? false)
            {
                productStatuses = productStatuses.Where(p => p.IsEnabled);
            }
            return Ok(productStatuses);
        }

        [Route("products/status")]
        [HttpPut, Authorize(nameof(Access.ModificarEstadoProductos))]
        public IActionResult ModifyProductStatusDetails(List<ProductStatus> productStatuses)
        {
            ResultValue<bool> result = _productStatusService.SaveStatuses(productStatuses);
            return result.Value.HasValue
                ? Ok(result.Value)
                : StatusCode(
                    500,
                    new
                    {
                        result.Errors
                    }
                );
        }

        [Route("product/{type}")]
        [HttpGet, Authorize(nameof(Access.AgregarProducto))]
        public IActionResult GetProductTemplate(TipoProducto type)
        {
            return Ok(_productFactory.CreateTemplate(type));
        }
    }
}