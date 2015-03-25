using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitML.Configurator.EdgeServerSource.Exceptions
{
    public class GitAccessException : GitException
    {

        public GitAccessException() { }
        public GitAccessException(string msg) : base(msg) { }

        public GitAccessException(string message, Exception inner) : base(message, inner) { }
    }
}
