using Api.Microservice.CarritoCompra.Persistencia;
using Api.Microservice.CarritoCompra.RemoteInterface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Microservice.CarritoCompra.Aplicacion
{
    public class Consulta
    {
        public class Ejecuta : IRequest<CarritoDto>
        {
            public int CarritoSessionId { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, CarritoDto>
        {
            private readonly CarritoContexto carritoContexto;
            private readonly ILibroService libroService;
            public Manejador(CarritoContexto _carritoContesto, ILibroService _libroService)
            {
                carritoContexto = _carritoContesto;
                libroService = _libroService;
            }

            public async Task<CarritoDto> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritoSesion = await carritoContexto.CarritoSesiones
                    .FirstOrDefaultAsync(x => x.CarritoSesionId ==
                    request.CarritoSessionId);

                var carritoSessionDetalle = await carritoContexto.CarritoSesionDetalle.
                    Where(x => x.CarritoSesionId == request.CarritoSessionId).ToListAsync();

                var listaCarritoDto = new List<CarritoDetalleDto>();

                foreach (var libro in carritoSessionDetalle)
                {
                    var response = await libroService.GetLibro(new System.Guid(libro.ProductoSeleccionado));
                    if (response.resultado)
                    {
                        var objetoLibro = response.Libro;
                        var carritoDetalle = new CarritoDetalleDto()
                        {
                            TituloLibro = objetoLibro.Titulo,
                            FechaPublicacion = objetoLibro.FechaPublicacion,
                            LibroId = objetoLibro.LibreriaMaterialId,
                            Precio = objetoLibro.Precio,
                        };
                        listaCarritoDto.Add(carritoDetalle);
                    }
                }

                var carritoSessionDto = new CarritoDto
                {
                    CarritoId = carritoSesion.CarritoSesionId,
                    FechaCreacionSesion = carritoSesion.FechaCreacion,
                    ListaDeProductos = listaCarritoDto
                };

                return carritoSessionDto;

            }
        }
    }
}