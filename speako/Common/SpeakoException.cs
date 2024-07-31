using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Common
{
    class SpeakoException : Exception
    {
        public SpeakoException(string message) : base(message)
        {
        }

        public SpeakoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public SpeakoException()
        {
        }
    }
}
