using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public class Identificator
    {
       public TextPosition TextPosition { get; set; }
        public string Name { get; set; }
        public Identificator (TextPosition textPosition, string name)
        {
            this.TextPosition = textPosition;
            this.Name = name;
        }
    }
}
