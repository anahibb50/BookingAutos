using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Autos.Business.Exceptions
{
    public class ValidationException : BusinessException
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException()
            : base("Se han producido uno o más errores de validación.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        // Este es el que ya tenías
        public ValidationException(IDictionary<string, string[]> errors) : this()
        {
            Errors = errors;
        }

        // ============================================================
        // AGREGA ESTE CONSTRUCTOR: Recibe la lista y quita el rojo
        // ============================================================
        public ValidationException(List<string> errorList) : this()
        {
            // Metemos tu lista en el diccionario bajo una llave genérica
            Errors.Add("General", errorList.ToArray());
        }
    }
}
