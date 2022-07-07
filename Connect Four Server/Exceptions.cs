using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_Four_Server
{
    internal class InvalidPlayerAction : Exception
    {
        internal InvalidPlayerAction(string message) : base(message)
        {

        }
    }
}
