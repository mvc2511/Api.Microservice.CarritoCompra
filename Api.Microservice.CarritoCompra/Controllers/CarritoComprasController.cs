using Api.Microservice.CarritoCompra.Aplicacion;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TiendaServicions.Carrito.Aplicaciones;

namespace Api.Microservice.CarritoCompra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoComprasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CarritoComprasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Crear")]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await _mediator.Send(data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarritoDto>> GetCarrito(int id)
        {
            return await _mediator.Send(new Consulta.Ejecuta { CarritoSessionId = id });
        }

        [HttpGet]
        public async Task<ActionResult<List<CarritoDto>>> GetTodos()
        {
            var carritos = await _mediator.Send(new ConsultaTodos.Ejecuta());
            return Ok(carritos);
        }

        [HttpDelete("Eliminar/{carritoSesionId}/{productoId}")]
        public async Task<ActionResult<Unit>> Eliminar(int carritoSesionId, string productoId)
        {
            var resultado = await _mediator.Send(new Eliminar.Ejecuta { CarritoSesionId = carritoSesionId, ProductoId = productoId });

            if (resultado)
            {
                return NoContent(); // 204 No Content
            }

            return BadRequest("No se pudo eliminar el producto del carrito de compras.");
        }

    }
}
