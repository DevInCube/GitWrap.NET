using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitML.Configurator.EdgeServerSource.Exceptions
{
    public abstract class GitException : Exception
    {
        public GitException() { }
        public GitException(string msg) : base(msg) { }
        public GitException(string msg, Exception inner) : base(msg, inner) { }
    }
}
