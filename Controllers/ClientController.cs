
using backend.Authentication;
using backend.DTO;
using backend.Models;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IProductService _productService;
        private readonly IProductStatusService _productStatusService;
        private readonly ILogger<ClientController> _logger;

        public ClientController(IClientService clientService, IProductService productService,
                                IProductStatusService productStatusService,
                                ILogger<ClientController> logger)
        {
            _clientService = clientService;
            _productService = productService;
            _productStatusService = productStatusService;
            _logger = logger;
        }

        [Authorize(nameof(Access.MostrarClientes))]
        [HttpGet, Route("clients/search/")]
        public IActionResult SearchClient(string? name)
        {
            _logger.LogInformation("Se inicio una busqueda con los parametros: name=" + name );
            return Ok(_clientService.Search(name).Select(client =>
            new ClientDTO(client.Id, client.Name, client.Surname, client.Telefono, client.Address, client.Email)
            ));
        }

        [Authorize(nameof(Access.MostrarClientes))]
        [HttpGet, Route("client/{id}")]
        public IActionResult GetClientDetails(Guid id)
        {
            Client? client = _clientService.GetClient(id);
            if (client == null)
            {
                return NotFound(new ErrorDTO(ErrorDTO.Errors.NotFound, "No se pudo encontrar cliente con ese id."));
            }
            _logger.LogInformation("Revisando los detalles del cliente de id " + id);
            return Ok(new ClientProfileDTO(client.Name, client.Surname,
                client.Telefono, client.Address, client.Email, "",
                client.Productos.Select(p => p.ToProductoDTO())
            ));
        }

        [Authorize(nameof(Access.EditarCliente))]
        [HttpPut, Route("client/{id}")]
        public IActionResult SaveClient(Guid id, ClientProfileDTO client)
        {
            _logger.LogInformation("Se estan editando los detalles del cliente de id " + id);
            return Ok(_clientService.SaveClient(id, client));
        }

        [Authorize(nameof(Access.BorrarCliente))]
        [HttpDelete, Route("client/{id}")]
        public IActionResult DeleteClient(Guid id)
        {
            _logger.LogInformation("Se mando una peticion para borrar el cliente de id " + id);
            return Ok(_clientService.DeleteClient(id));
        }

        [Authorize(nameof(Access.CrearCliente))]
        [HttpPost, Route("client/")]
        public IActionResult CreateClient(ClientProfileDTO client)
        {
            ResultValue<Guid> result = _clientService.CreateClient(client);
            if (result.Value.HasValue) {
                _logger.LogInformation("Se esta creando un nuevo cliente con id " + result.Value);
            } else {
                _logger.LogWarning("Ocurrio un problema al crear un cliente");
            }
            return result.Value.HasValue
                ? Ok(result.Value)
                : Conflict(
                    new
                    {
                        result.Errors
                    }
                );
        }

        [Authorize(nameof(Access.MostrarProductos))]
        [HttpGet, Route("client/{id}/product/{productId}")]
        public IActionResult SeeProductDetails(Guid id, Guid productId) {
            Result<Producto> result = _productService.ShowProductOfClient(id, productId);
            _logger.LogInformation("Se estan revisando los detalles de los productos del cliente de id " + id + " el producto de id " + productId);
            return result.Value == null ?
                NotFound(new { result.Errors }) :
                Ok(result.Value);
        }

        [Authorize(nameof(Access.AgregarProducto))]
        [Authorize(nameof(Access.EditarCliente))]
        [HttpPost, Route("client/{id}/product")]
        public IActionResult AddProduct(Guid id, Producto product)
        {
            if (!_productStatusService.CanCreateOfType(product.Kind))
            {
                return UnprocessableEntity(new ErrorDTO(
                    error: ErrorDTO.Errors.UnprocessableEntity,
                    "No es posible registrar un nuevo recurso de este tipo por que ha sido deshabilitado por otro rol."
                ));
            }

            _logger.LogInformation("Se esta añadiendo un nuevo producto con id " + product.Id);
            ResultValue<Guid> resultValue = _productService.AddProductToClient(id, product);
            return resultValue.Value.HasValue
                ? Ok(new { resultValue.Value })
                : NotFound(new
                {
                    resultValue.Errors
                });
        }

        [Authorize(nameof(Access.BorrarProducto))]
        [Authorize(nameof(Access.EditarCliente))]
        [HttpDelete, Route("client/{id}/product/{productId}")]
        public IActionResult DeleteProduct(Guid id, Guid productId)
        {
            _logger.LogInformation("Se mandado una petecion de borrar un producto con id " + productId);
            // Should handle this empty promises better, but can't find how.
            _productService.DeleteProductFromClient(id, productId);
            return Ok();
        }

        [Authorize(nameof(Access.EditarProducto))]
        [Authorize(nameof(Access.EditarCliente))]
        [HttpPut, Route("client/{id}/product/")]
        public IActionResult UpdateProduct(Guid id, [FromBody] Producto product)
        {
            ResultValue<bool> resultValue = _productService.UpdateProductFromClient(id, product);
            _logger.LogInformation("Se esta intentanto actualizar un producto con id " + product.Id);
            return resultValue.Value ?? false ? Ok(new { resultValue.Value }) : Conflict(new { resultValue.Errors });
        }
    }
}