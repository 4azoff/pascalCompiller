namespace Compiler
{
    class Litera
    {
        public char ch { get; private set; }
        public int numberLine;
        public int numberPosition;

        public Litera(char ch, int numberLine, int numberPosition)
        {
            this.ch = ch;
            this.numberLine = numberLine;
            this.numberPosition = numberPosition;
        }
    }
}
