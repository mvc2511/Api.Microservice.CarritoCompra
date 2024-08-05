namespace Api.Microservice.CarritoCompra.Aplicacion
{
    public class CarritoDto
    {
        public int CarritoId {  get; set; }

        public DateTime? FechaCreacionSesion { get; set; }

        public List<CarritoDetalleDto> ListaDeProductos { get; set; }
    }
}
