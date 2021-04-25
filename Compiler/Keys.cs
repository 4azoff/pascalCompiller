using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Compiler
{
    public class Keys
    {
        public enum Key : int
        {
            star = 21, slash = 60, equal = 16, comma = 20, semicolon = 14, colon = 5, point = 61, arrow = 62, leftpar = 9, rightpar = 4,
            lbracket = 11, rbracket = 12, flpar = 63, frpar = 64, later = 65, greater = 66, laterequal = 67, greaterequal = 68, latergreater = 69,
            plus = 70, minus = 71, lcomment = 72, rcomment = 73, assign = 51, twopoints = 74, ident = 2, realT = 82, intT = 15, charT = 83, boolT = 84,
            endoffile = 253, eolint = 88, FALSE = 0, TRUE = 1, dosy = 54, ifsy = 56, insy = 22, ofsy = 8, orsy = 23, tosy = 55, andsy = 24, divsy = 25, endsy = 13, forsy = 26, modsy = 27,
            nilsy = 89, notsy = 28, setsy = 29, varsy = 30, casesy = 31, elsesy = 32, filesy = 57, gotosy = 33, onlysy = 90, thensy = 52, typesy = 34, unitsy = 35,
            usessy = 36, withsy = 37, arraysy = 38, beginsy = 17, constsy = 39, labelsy = 40, untilsy = 53, whilesy = 41, downtosy = 58, exportsy = 91, importsy = 92,
            modulesy = 93, packedsy = 42, recordsy = 43, repeatsy = 44, vectorsy = 45, stringsy = 46, forwardsy = 47, processsy = 48,
            programsy = 3, segmentsy = 49, functionsy = 77, separatesy = 78, interfacesy = 79, proceduresy = 80, qualifiedsy = 94,
            implementationsy = 81, intC = 100, charC = 101, realC = 102, boolC = 103
        }

        public static Dictionary<string, int> keyValues = new Dictionary<string, int> {
            {"boolean" , (int)Key.boolT },
            {"TRUE" , (int)Key.TRUE },
            {"FALSE" , (int)Key.FALSE },
            {"real" , (int)Key.realT },
            {"integer" , (int)Key.intT },
            {"char" , (int)Key.charT },
            {"char const" , (int)Key.charC },
            {"real const" , (int)Key.realC },
            {"int const" , (int)Key.intC },
            {"bool const" , (int)Key.boolC },
            { "*",  (int)Key.star },
            { "/",  (int)Key.slash },
            { "=",  (int)Key.equal },
            { ",",  (int)Key.comma },
            { ";",  (int)Key.semicolon },
            { ":",  (int)Key.colon },
            { ".",  (int)Key.point },
            { "^",  (int)Key.arrow },
            { "(",  (int)Key.leftpar },
            { ")",  (int)Key.rightpar },
            { "[",  (int)Key.lbracket },
            { "]",  (int)Key.rbracket },
            { "{",  (int)Key.flpar },
            { "}",  (int)Key.frpar },
            { "<",  (int)Key.later },
            { ">",  (int)Key.greater },
            { "<=",  (int)Key.laterequal },
            { ">=",  (int)Key.greaterequal },
            { "<>",  (int)Key.latergreater },
            { "+",  (int)Key.plus },
            { "-",  (int)Key.minus },
            { "(*",  (int)Key.lcomment},
            { "*)",  (int)Key.rcomment },
            { ":=",  (int)Key.assign },
            { "..",  (int)Key.twopoints },
            { "do",  (int)Key.dosy },
            { "if", (int)Key.ifsy },
            { "in", (int)Key.insy },
            { "of", (int)Key.ofsy },
            { "or", (int)Key.orsy },
            { "to", (int)Key.tosy },
            { "and", (int)Key.andsy },
            { "div", (int)Key.divsy },
            { "end", (int)Key.endsy },
            { "for", (int)Key.forsy },
            { "mod", (int)Key.modsy },
            { "nil", (int)Key.nilsy },
            { "not", (int)Key.notsy },
            { "set", (int)Key.setsy },
            { "var", (int)Key.varsy },
            { "case", (int)Key.casesy },
            { "else", (int)Key.elsesy },
            { "file", (int)Key.filesy },
            { "goto", (int)Key.gotosy },
            { "only", (int)Key.onlysy },
            { "then", (int)Key.thensy },
            { "type", (int)Key.typesy },
            { "unit", (int)Key.unitsy },
            { "uses", (int)Key.usessy },
            { "with", (int)Key.withsy },
            { "array", (int)Key.arraysy },
            { "begin", (int)Key.beginsy },
            { "const", (int)Key.constsy },
            { "label", (int)Key.labelsy },
            { "until", (int)Key.untilsy },
            { "while", (int)Key.whilesy },
            { "downto", (int)Key.downtosy },
            { "export", (int)Key.exportsy },
            { "import", (int)Key.importsy },
            { "module", (int)Key.modulesy },
            { "packed", (int)Key.packedsy },
            { "record", (int)Key.recordsy },
            { "repeat", (int)Key.repeatsy },
            { "vector", (int)Key.vectorsy },
            { "string", (int)Key.stringsy },
            { "forward", (int)Key.forwardsy },
            { "process", (int)Key.processsy },
            { "program", (int)Key.programsy },
            { "segment", (int)Key.segmentsy },
            { "function", (int)Key.functionsy },
            { "separate", (int)Key.separatesy },
            { "interface", (int)Key.interfacesy },
            { "procedure", (int)Key.proceduresy },
            { "qualified", (int)Key.qualifiedsy },
            { "identificator", (int)Key.ident },
            { "implementation", (int)Key.implementationsy }};


        public static Dictionary<string,int> names = new Dictionary<string, int>();
        public static List<Identificator> Identificators = new List<Identificator>();
        public static int searchSym(string name, TextPosition textPosition)
        {
            try
            {
                return keyValues[name];
            }
            catch (KeyNotFoundException ex)
            {
                Identificators.Add(new Identificator(textPosition, name));
                if (!names.ContainsKey(name))
                {
                    names.Add(name, 0);
                    
                }
                return (int)Key.ident;
            }
        }
        public static string searchIdent(TextPosition textPosition)
        {
            foreach (var ident in Identificators)
            {
                if (ident.TextPosition.numberLine == textPosition.numberLine && 
                    ident.TextPosition.position == textPosition.position)
                {
                    return ident.Name;
                }
            }
            return "";

        }
    }
}
