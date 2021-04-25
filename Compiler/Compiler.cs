using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Compiler
{
    public struct TextPosition
    {
        public int numberLine;
        public int position;
    }

    class Compiler
    {
        List<Error> errors = new List<Error>();
        List<ErrorPosition> errorsPositions = new List<ErrorPosition>();
        bool ident = true;
        bool progIdent = true;
        Dictionary<int, string> keysVal = new Dictionary<int, string>();
        List<string> allLines = new List<string>();
        List<Symbol> tokens = new List<Symbol>();
        string sourceFilePath;
        string resultFilePath;
        string buf;
        Litera litera;
        TextPosition textPosition;
        //Dictionary<string, int> identTable = new Dictionary<string, int>();
        List<string> identsToType = new List<string>();
        Dictionary<string, string> constTable = new Dictionary<string, string>();
        bool emptyBuf = true;
        StreamReader sr;    
        StreamWriter sw;
        string errorsPath = "Errors.txt";
        int maxint = 32767;
        int minint = -32768;
        double maxreal = 1.7 * Math.Pow(10, 38);
        double minreal = 2.9 * Math.Pow(10, -39);
        Symbol symbol;

        public Compiler(string sourceFilePath, string resultFilePath, List<Error> errors)
        {
            this.sourceFilePath = sourceFilePath;
            this.resultFilePath = resultFilePath;
            this.errors = errors;
            foreach (KeyValuePair<string, int> key in Keys.keyValues){
                keysVal.Add(key.Value, key.Key);
            }
        }

        private void Error(string code, TextPosition pos)
        {
            var index = errors.FindIndex((o) => o.code == code);
            Error error = errors[index];
            ErrorPosition errorPos = new ErrorPosition(pos.numberLine, pos.position, error);
            errorsPositions.Add(errorPos);
        }

        private void ListLine()
        {
            int numline = textPosition.numberLine;
            sw.WriteLine("{0,4}  {1}", numline, buf);
        }

        private void ListErrors()
        {
            for(int i = 0; i < errorsPositions.Count; i++)
            {
                if(errorsPositions[i].numberLine == textPosition.numberLine)
                {
                    string numbError = (i + 1).ToString();
                    numbError = numbError.PadLeft(2, '0');
                    sw.WriteLine("**{0}**{1," + errorsPositions[i].position + "}^ ошибка  код {2}",
                        numbError, "", errorsPositions[i].error.code);
                    sw.WriteLine("****** {0}", errorsPositions[i].error.description);
                }
            }
        }

        private bool HasErrors()
        {
            int numLine = textPosition.numberLine;
            var index = errorsPositions.FindIndex((o) => o.numberLine == numLine);
            if (index != -1)
                return true;
            else
                return false;
        }

        private Litera NextCh()
        {
            if (buf.Length == 0 || textPosition.position == buf.Length - 1)
            {
                ListLine();
                if (HasErrors())
                    ListErrors();

                if (!sr.EndOfStream)
                {
                    buf = sr.ReadLine();
                    textPosition.numberLine++;
                    textPosition.position = 0;
                }
                else
                {
                    emptyBuf = true;
                    litera = null;
                    return litera;
                }
            }
            else
                textPosition.position++;

            if (buf.Length > 0)
                litera = new Litera(buf[textPosition.position], textPosition.numberLine, textPosition.position);
            else
                litera = null;

            return litera;
        }

        private bool isLetter(char ch)
        {
            if (ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z' || ch == '_')
                return true;
            else return false;
        }

        private bool isDigit(char ch)
        {
            if (ch >= '0' && ch <= '9')
                return true;
            else return false;
        }

        private Symbol NextSym()
        {
            int symbol = -1;
            TextPosition token;
            while (litera.ch == ' ' && !emptyBuf)
                NextCh();
            if (emptyBuf)
                return null;
            token.numberLine = textPosition.numberLine;
            token.position = textPosition.position;

            if (isLetter(litera.ch))
            {
                string name = "";
                while (isDigit(litera.ch) || isLetter(litera.ch))
                {
                    name += litera.ch;
                    NextCh();
                }

                symbol = Keys.searchSym(name, token);
                
                
            }
            else if(isDigit(litera.ch))
            {
                string digit = "";
                double x = 0;
                while (isDigit(litera.ch) || litera.ch == '.')
                {
                    digit += litera.ch;
                    NextCh();
                }
                try
                {
                    x = Convert.ToDouble(digit.Replace('.',','));
                    if (x % 1 == 0)
                    {
                        symbol = (int)Keys.Key.intC;
                        if (x > maxint) Error("203", token);
                       
                    }
                    else
                    {
                        symbol = (int)Keys.Key.realC;
                        if (x > maxreal) Error("207", token);
                        else if (x < minreal) Error("206", token);
                    }
                }
                catch
                {
                    Error("201", token);
                }
                
            }
            else
                switch (litera.ch)
                {
                    case '\'':
                        string constant = "";
                        NextCh();
                        while (litera.ch != '\'')
                        {
                            constant += litera.ch;
                            NextCh();
                        }
                        var arr = constant.ToCharArray();
                        if (arr.Length == 0 || arr.Length > 1)
                            Error("75", token);
                        else
                        {
                            symbol = (int)Keys.Key.charC;
                        }
                        NextCh();
                        break;
                    case ':' : NextCh();
                        if (litera.ch == '=')
                        {
                            symbol = (int)Keys.Key.assign;
                            NextCh();
                        }
                        else
                            symbol = (int)Keys.Key.colon;
                        break;
                    case '<': NextCh();
                        if (litera.ch == '=')
                        {
                            symbol = (int)Keys.Key.laterequal;
                            
                            NextCh();
                        }
                        else if (litera.ch == '>')
                        {
                            symbol = (int)Keys.Key.latergreater;
                            NextCh();
                        }
                        else
                            symbol = (int)Keys.Key.later;
                        break;
                    case '>': NextCh();
                        if (litera.ch == '=')
                        {
                            symbol = (int)Keys.Key.greaterequal;
                            NextCh();
                        }
                        else
                            symbol = (int)Keys.Key.greater;
                        break;
                    case '+':
                        symbol = (int)Keys.Key.plus;
                        NextCh();
                        break;
                    case '-': 
                        symbol = (int)Keys.Key.minus;
                        NextCh();
                        break;
                    case ';':
                        symbol = (int)Keys.Key.semicolon;
                        NextCh();
                        break;
                    case '^':
                        symbol = (int)Keys.Key.arrow;
                        NextCh();
                        break;
                    case '[':
                        symbol = (int)Keys.Key.lbracket;
                        NextCh();
                        break;
                    case ']':
                        symbol = (int)Keys.Key.rbracket;
                        NextCh();
                        break;
                    case '{':
                        symbol = (int)Keys.Key.flpar;
                        NextCh();
                        break;
                    case '}':
                        symbol = (int)Keys.Key.frpar;
                        NextCh();
                        break;
                    case '=':
                        symbol = (int)Keys.Key.equal;
                        NextCh();
                        break;
                    case '/':
                        symbol = (int)Keys.Key.slash;
                        NextCh();
                        break;
                    case '.':
                        NextCh();
                        if (litera == null)
                        {
                            symbol = (int)Keys.Key.point;
                            break;
                        }
                        if (litera.ch == '.')
                        {
                            symbol = (int)Keys.Key.twopoints;
                            NextCh();
                        }
                        else
                            symbol = (int)Keys.Key.point;
                        break;
                    case ',':
                        symbol = (int)Keys.Key.comma;
                        NextCh();
                        break;
                    case '*':
                        NextCh();
                        if (litera.ch == ')')
                        {
                            symbol = (int)Keys.Key.rcomment;
                            NextCh();
                        }
                        else
                            symbol = (int)Keys.Key.star;
                        break;
                    case '(':
                        NextCh();
                        if (litera.ch == '*')
                        {
                            symbol = (int)Keys.Key.lcomment;
                            NextCh();
                        }
                        else
                            symbol = (int)Keys.Key.leftpar;
                        break;
                    case ')':
                        symbol = (int)Keys.Key.rightpar;
                        NextCh();
                        break;
                    default: Error("6", token);
                        NextCh();
                        break;
                       
                }
            
            var simv = new Symbol(symbol, token.numberLine, token.position);
            //if (symbol != -1)
            //    Console.WriteLine("(" + token.numberLine + ", " + token.position + ")" + symbol + "  -  " + keysVal[symbol]);
            return simv;
        }

        public void Start()
        {
            sr = new StreamReader(sourceFilePath);
            sw = new StreamWriter(resultFilePath);
            
            //ReadErrorsTable();
            textPosition.numberLine = 1;
            textPosition.position = -1;
            List<Litera> literas = new List<Litera>();
            

            if (!sr.EndOfStream)
            {
                buf = sr.ReadLine();
                allLines.Add(buf);
                emptyBuf = false;
                NextCh();
            }
            //while (!emptyBuf)
            //    litera = NextCh();
            
            while (!emptyBuf)
            {
                symbol = NextSym();
                if (symbol.code != -1)
                    tokens.Add(symbol); 
            }
            programmCheck(tokens);

          
            
            sr.Close();
            sw.Close();
           
            WriteNames();
            writeErrors();
        }

        void writeErrors()
        {
            using (StreamWriter sw = new StreamWriter("Errors.txt"))
            {
                foreach (var error in errorsPositions)
                {                   
                  
                    sw.WriteLine("(" + error.numberLine + ", " + error.position + ") ошибка  код " +
                        error.error.code +  " - " + error.error.description);
                    
                }
            }
        }

        

        void programmCheck (List<Symbol> symbols)
        {
            int i = 0;
            ident = false;
            accept(symbols[i], (int)Keys.Key.programsy, "3", ref i);
            progIdent = true;
            accept(symbols[i], (int)Keys.Key.ident, "2", ref i);
            progIdent = false;
            accept(symbols[i], (int)Keys.Key.semicolon, "14", ref i);
           
            blockCheck(symbols, ref i);

            accept(symbols[i], (int)Keys.Key.point, "61", ref i);

        }

        void blockCheck (List<Symbol> symbols, ref int i)
        {
            ident = true;
            constPart(symbols, ref i);
            varPart(symbols, ref i);
            ident = false;            
            statementCheck(symbols, ref i);
        }

        void statementCheck(List<Symbol> symbols, ref int i)
        {
            accept(symbols[i], (int)Keys.Key.beginsy, "17", ref i);
            while (symbols[i].code == (int)Keys.Key.ident)
            {
                TextPosition tx;
                tx.numberLine = symbols[i].numberLine;
                tx.position = symbols[i].numberPosition;
                string nm = Keys.searchIdent(tx);
                int type = Keys.names[nm];
                accept(symbols[i], (int)Keys.Key.ident, "2", ref i);
                accept(symbols[i], (int)Keys.Key.assign, "51", ref i);
                int type_op = mathOperCheck(symbols, ref i);
                if (type != type_op)
                    Error("328", tx);
                accept(symbols[i], (int)Keys.Key.semicolon, "14", ref i);

            }
            accept(symbols[i], (int)Keys.Key.endsy, "13", ref i);
        }

        int mathOperCheck(List<Symbol> symbols, ref int i)
        {
            textPosition.numberLine = symbols[i].numberLine;
            textPosition.position = symbols[i].numberPosition;
            int type = 0;
           
            switch (symbols[i].code)
            {
                case (int)Keys.Key.ident:
                    TextPosition tx;
                    tx.numberLine = symbols[i].numberLine;
                    tx.position = symbols[i].numberPosition;
                    string nm = Keys.searchIdent(tx);
                    type = Keys.names[nm];
                    accept(symbols[i], (int)Keys.Key.ident, "2", ref i);
                    if (symbols[i].code == (int)Keys.Key.semicolon)
                    {
                        return type;
                    }
                    break;
                case (int)Keys.Key.intC:
                    i++;
                    type = (int)Keys.Key.intT;
                    if (symbols[i].code == (int)Keys.Key.semicolon)
                    {
                        return (int)Keys.Key.intT;
                    }
                    break;
                case (int)Keys.Key.realC:
                    i++;
                    type = (int)Keys.Key.realT;
                    if (symbols[i].code == (int)Keys.Key.semicolon)
                    {
                        return (int)Keys.Key.realT;
                    }
                    break;
                //case (int)Keys.Key.boolC:

                //    break;
                //case (int)Keys.Key.charC:

                //    break;
                case (int)Keys.Key.leftpar:
                    i++;
                    type = mathOperCheck(symbols, ref i);
                    accept(symbols[i], (int)Keys.Key.rightpar, "4", ref i);
                    if (symbols[i].code == (int)Keys.Key.semicolon)
                    {
                        return type;
                    }
                    break;
                default:
                    Error("335", textPosition);
                    return 0;
                    break;
            }
            textPosition.numberLine = symbols[i].numberLine;
            textPosition.position = symbols[i].numberPosition;
            int type_post = 0;
            switch (symbols[i].code)
            {
                case (int)Keys.Key.plus:
                    i++;
                    type_post = mathOperCheck(symbols, ref i);
                    if (type != type_post)
                        Error("328", textPosition);
                    break;
                case (int)Keys.Key.minus:
                    i++;
                    type_post = mathOperCheck(symbols, ref i);
                    if (type != type_post)
                        Error("328", textPosition);
                    break;
                case (int)Keys.Key.slash:
                    i++;
                    type_post = mathOperCheck(symbols, ref i);
                    if (type != type_post)
                        Error("328", textPosition);
                    break;
                case (int)Keys.Key.star:
                    i++;
                    type_post = mathOperCheck(symbols, ref i);
                    if (type != type_post)
                        Error("328", textPosition);
                    break;
                case (int)Keys.Key.rightpar:                    
                    return type;
                    break;
                //case (int)Keys.Key.boolC:

                //    break;
                //case (int)Keys.Key.charC:

                //    break;

                default:
                    Error("336", textPosition);
                    return 0;
                    break;
            }
            return type_post;


        }



        void varPart(List<Symbol> symbols, ref int i)
        {
            if (symbols[i].code == (int)Keys.Key.varsy)
            {
                accept(symbols[i], (int)Keys.Key.varsy, "0", ref i);
                do
                {
                    varDeclaration(symbols, ref i);
                    accept(symbols[i], (int)Keys.Key.semicolon, "14", ref i);


                } while (symbols[i].code == (int)Keys.Key.ident);

            }
        }
        void constPart(List<Symbol> symbols, ref int i)
        {
            if (symbols[i].code == (int)Keys.Key.constsy)
            {
                accept(symbols[i], (int)Keys.Key.constsy, "0", ref i);
                do
                {
                    constDeclaration(symbols, ref i);
                    accept(symbols[i], (int)Keys.Key.semicolon, "14", ref i);


                } while (symbols[i].code == (int)Keys.Key.ident);

            }
        }

        void varDeclaration(List<Symbol> symbols, ref int i)
        {
            accept(symbols[i], (int)Keys.Key.ident, "2", ref i);
            while(symbols[i].code == (int)Keys.Key.comma)
            {
                i++;
                accept(symbols[i], (int)Keys.Key.ident, "2", ref i);
            }
            accept(symbols[i], (int)Keys.Key.colon, "5", ref i);
            type(symbols, ref i);

        }
        void constDeclaration(List<Symbol> symbols, ref int i)
        {
            accept(symbols[i], (int)Keys.Key.ident, "2", ref i);

            accept(symbols[i], (int)Keys.Key.equal, "16", ref i);

            constValue(symbols, ref i);           
            

        }

        void type(List<Symbol> symbols, ref int i)
        {
            textPosition.numberLine = symbols[i].numberLine;
            textPosition.position = symbols[i].numberPosition;
            if (symbols[i].code != (int)Keys.Key.realT && symbols[i].code != (int)Keys.Key.charT &&
                symbols[i].code != (int)Keys.Key.intT && symbols[i].code != (int)Keys.Key.boolT)
            {
                Error("331", textPosition);
                identsToType.Clear();
            }
            else
            {
                foreach (var cons in identsToType)
                {
                    Keys.names[cons] = symbols[i].code;
                }
                identsToType.Clear();
                
            }

            i++;

        }

        void constValue(List<Symbol> symbols, ref int i)
        {
            textPosition.numberLine = symbols[i].numberLine;
            textPosition.position = symbols[i].numberPosition;
            if (symbols[i].code != (int)Keys.Key.realC && symbols[i].code != (int)Keys.Key.intC && symbols[i].code != (int)Keys.Key.boolC)
            {
                Error("334", textPosition);
                identsToType.Clear();
            }
            else
            {
                int code = symbols[i].code;
                switch (code)
                {
                    case (int)Keys.Key.intC:                        
                        code = (int)Keys.Key.intT;                      
                        break;
                    case (int)Keys.Key.realC:                       
                        code = (int)Keys.Key.realT;                       
                        break;
                    case (int)Keys.Key.boolC:
                        code = (int)Keys.Key.boolT;
                        break;
                    case (int)Keys.Key.charC:
                        code = (int)Keys.Key.charT;
                        break;
                }
                foreach (var cons in identsToType)
                {
                    Keys.names[cons] = code ;
                }
                identsToType.Clear();
                i++;

            }
            

        }

        void accept(Symbol symbol, int key, string errorCode, ref int i)
        {

            textPosition.numberLine = symbol.numberLine;
            textPosition.position = symbol.numberPosition;
            if (symbol.code != key)
            {
                Error(errorCode, textPosition);
            }
            else
            {
                if (key == (int)Keys.Key.ident)
                {
                    string name = Keys.searchIdent(textPosition);
                    if (progIdent)
                        Keys.names[name] = (int)Keys.Key.programsy;
                    if (ident)
                    {
                        
                        if (Keys.names[name] != 0 || identsToType.Contains(name))
                            Error("101", textPosition);
                        else
                            identsToType.Add(name);
                    }
                    else
                    {
                        if (Keys.names[name] == 0)
                            Error("104", textPosition);

                    }
                    
                }
                
                i++;
            }
                
        }

        private void WriteNames()
        {
            using(StreamWriter sw = new StreamWriter("names.txt"))
            {
                foreach (var name in tokens)
                    sw.WriteLine("(" + name.numberLine + ", " + name.numberPosition + ")" + name.code + "  -  " + keysVal[name.code]);
            }
        }

        private void ReadErrorsTable()
        {
            using (StreamReader sr = new StreamReader("Errors.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    var arr = line.Split(' ');
                    TextPosition textPosition;
                    textPosition.numberLine = Convert.ToInt32(arr[0]);
                    textPosition.position = Convert.ToInt32(arr[1]);
                    Error(arr[2], textPosition);
                }
            }
        }
    }
}
