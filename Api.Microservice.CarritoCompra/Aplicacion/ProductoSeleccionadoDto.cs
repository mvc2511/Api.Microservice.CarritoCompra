namespace Api.Microservice.CarritoCompra.Aplicacion
{
    public class ProductoSeleccionadoDto
    {
        public string LibroId { get; set; }
        public string TituloLibro { get; set; }
        public string AutorLibro { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public decimal Precio { get; set; }
    }
}
