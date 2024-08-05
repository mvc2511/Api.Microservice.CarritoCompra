namespace Api.Microservice.CarritoCompra.Aplicacion
{
    public class CarritoDetalleDto
    {
        public Guid? LibroId { get; set; }
        public string TituloLibro { get; set; }
        public string AutorLibro { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public double Precio { get; set; }
        public byte[] Imagenes { get; set; }
    }
}
