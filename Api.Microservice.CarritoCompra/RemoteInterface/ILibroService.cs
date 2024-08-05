using Api.Microservice.CarritoCompra.RemoteModel;

namespace Api.Microservice.CarritoCompra.RemoteInterface
{
     public interface ILibroService
    {
        Task<(bool resultado, LibroRemote Libro, string ErrorMessage)>
            GetLibro(Guid LibroId);
    }
}
