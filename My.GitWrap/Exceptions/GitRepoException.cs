using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitML.Configurator.EdgeServerSource.Exceptions
{
    public class GitRepoException : GitException
    {

         public GitRepoException() { }
         public GitRepoException(string msg) : base(msg) { }
         public GitRepoException(string msg, Exception inner) : base(msg, inner) { }
    }
}
