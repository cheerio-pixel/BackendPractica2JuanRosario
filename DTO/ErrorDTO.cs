
namespace backend.DTO
{
    public class ErrorDTO
    {
        public static class Errors
        {
            public const string NotAuthorized = "No autorizado";
            public const string NotFound = "No se encontro";
            public const string BadRequest = "Datos invalidos";
            public const string UnprocessableEntity = "No es posible procesar";

        }
        public string Error { get; set; }
        public string Message { get; set; }

        public ErrorDTO(string error, string message)
        {
            Error = error;
            Message = message;
        }
    }
}