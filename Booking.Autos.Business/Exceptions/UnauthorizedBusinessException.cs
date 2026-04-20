using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Autos.Business.Exceptions
{
    public class UnauthorizedBusinessException : BusinessException
    {
        public UnauthorizedBusinessException()
            : base("No está autorizado para realizar esta acción o sus credenciales son inválidas.") { }

        public UnauthorizedBusinessException(string message)
            : base(message) { }

        // Útil para especificar si es por login fallido o token expirado
        public UnauthorizedBusinessException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
