using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class AppException:Exception
    {
        private readonly ApiResultStatusCode _statusCode;

        public AppException(string message, ApiResultStatusCode statusCode) : base(message)
        {
            _statusCode = statusCode;
        }
    }
}
