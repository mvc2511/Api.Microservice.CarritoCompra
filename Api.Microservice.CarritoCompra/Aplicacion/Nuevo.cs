using Api.Microservice.CarritoCompra.Modelo;
using Api.Microservice.CarritoCompra.Persistencia;
using MediatR;

namespace Api.Microservice.CarritoCompra.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public DateTime FechaCreacionSesion { get; set; }
            public List<string> ProductoLista { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CarritoContexto _context;
            public Manejador(CarritoContexto context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritoSesion = new CarritoSesion
                {
                    FechaCreacion = request.FechaCreacionSesion,
                };

                _context.CarritoSesiones.Add(carritoSesion);
                var result = await _context.SaveChangesAsync();
                if (result == 0)
                {
                    throw new Exception("No se pudo insertar en el carrito de compras");
                }
                int id = carritoSesion.CarritoSesionId;

                foreach (var p in request.ProductoLista)
                {
                    var detalleSesion = new CarritoSesionDetalle
                    {
                        FechaCreacion = DateTime.Now,
                        CarritoSesionId = id,
                        ProductoSeleccionado = p
                    };
                    _context.CarritoSesionDetalle.Add(detalleSesion);
                }

                var value = await _context.SaveChangesAsync();

                if (value > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo insertar el detalle del carrito de compras");
            }
        }
    }
}