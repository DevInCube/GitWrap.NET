using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitML.Configurator.EdgeServerSource.Exceptions
{
    public class GitConnectionException : GitException
    {
        public GitConnectionException() { }
        public GitConnectionException(string msg) : base(msg) { }
    }
}
