using Api.Microservice.CarritoCompra.Modelo;
using Microsoft.EntityFrameworkCore;

namespace Api.Microservice.CarritoCompra.Persistencia
{
    public class CarritoContexto : DbContext
    {
        public CarritoContexto(DbContextOptions<CarritoContexto> options)
            : base(options)
        {

        }
        public DbSet<CarritoSesion > CarritoSesiones {  get; set; }

        public DbSet<CarritoSesionDetalle> CarritoSesionDetalle { get; set; }
    }
}
