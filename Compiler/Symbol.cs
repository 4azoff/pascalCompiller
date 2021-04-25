using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class Symbol
    {
        public int code;
        public int numberLine;
        public int numberPosition;

        public Symbol(int code, int numberLine, int numberPosition)
        {
            this.code = code;
            this.numberLine = numberLine;
            this.numberPosition = numberPosition;
        }
    }
}
