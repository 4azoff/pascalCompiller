using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public class Error
    {
        public string code { get; private set; }
        public string description { get; private set; }

        public Error(string code, string description)
        {
            this.code = code;
            this.description = description;
        }
    }
}
