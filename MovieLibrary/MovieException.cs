using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary
{
    public class MovieException : Exception
    {
        public MovieException(string errMsg) : base(errMsg)
        {

        }
    }
}
