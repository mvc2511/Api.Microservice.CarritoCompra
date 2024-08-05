using MediatR;
using Api.Microservice.CarritoCompra.Persistencia;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TiendaServicions.Carrito.Aplicaciones
{
    public class Eliminar
    {
        public class Ejecuta : IRequest<bool>
        {
            public int CarritoSesionId { get; set; }
            public string ProductoId { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, bool>
        {
            private readonly CarritoContexto _context;

            public Manejador(CarritoContexto context)
            {
                _context = context;
            }

            public async Task<bool> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var detalle = await _context.CarritoSesionDetalle
                    .FirstOrDefaultAsync(d => d.CarritoSesionId == request.CarritoSesionId && d.ProductoSeleccionado == request.ProductoId);

                if (detalle == null)
                {
                    return false;
                }

                _context.CarritoSesionDetalle.Remove(detalle);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
        }
    }
}
