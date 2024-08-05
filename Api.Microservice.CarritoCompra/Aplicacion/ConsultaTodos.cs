using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Api.Microservice.CarritoCompra.Persistencia;
using Api.Microservice.CarritoCompra.RemoteInterface;
using Api.Microservice.CarritoCompra.Aplicacion;

namespace TiendaServicions.Carrito.Aplicaciones
{
    public class ConsultaTodos
    {
        public class Ejecuta : IRequest<List<CarritoDto>>
        {
        }

        public class Manejador : IRequestHandler<Ejecuta, List<CarritoDto>>
        {
            private readonly CarritoContexto carritoContexto;
            private readonly ILibroService libroService;

            public Manejador(CarritoContexto _carritoContexto, ILibroService _libroService)
            {
                carritoContexto = _carritoContexto;
                libroService = _libroService;
            }

            public async Task<List<CarritoDto>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritos = await carritoContexto.CarritoSesiones.ToListAsync();

                var listaCarritoDto = new List<CarritoDto>();

                foreach (var carritoSesion in carritos)
                {
                    var carritoSessionDetalle = await carritoContexto.CarritoSesionDetalle
                        .Where(x => x.CarritoSesionId == carritoSesion.CarritoSesionId).ToListAsync();

                    var detallesCarritoDto = new List<CarritoDetalleDto>();

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
                            detallesCarritoDto.Add(carritoDetalle);
                        }
                    }

                    var carritoDto = new CarritoDto
                    {
                        CarritoId = carritoSesion.CarritoSesionId,
                        FechaCreacionSesion = carritoSesion.FechaCreacion,
                        ListaDeProductos = detallesCarritoDto
                    };

                    listaCarritoDto.Add(carritoDto);
                }

                return listaCarritoDto;
            }
        }
    }
}
