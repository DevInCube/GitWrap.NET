using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitML.Configurator.EdgeServerSource.Exceptions
{
    public class GitSyncException : GitException
    {

        public GitSyncException() { }
        public GitSyncException(string msg) : base(msg) { }

        public GitSyncException(string message, Exception inner) : base(message, inner) { }
    }
}
