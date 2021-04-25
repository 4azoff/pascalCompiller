using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class ErrorPosition
    {
        public int numberLine { get; private set; }
        public int position { get; private set; }
        public Error error { get; private set; }

        public ErrorPosition(int numberLine, int position, Error error)
        {
            this.numberLine = numberLine;
            this.position = position;
            this.error = error;
        }
    }
}
