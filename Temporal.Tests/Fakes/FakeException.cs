using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Temporal.Tests.Fakes
{
    public class FakeException : Exception
    {
        public override string Message
        {
            get { return "hit"; }
        }
    }
}
