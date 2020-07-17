using System;
using System.Text;
using System.Collections.Generic;
using CSharpDataStructures.Structures.Lists;
namespace CSharpDataStructures.Structures.Sequences {
    public interface IFormulaParser {
        bool IsOperator(String c);
        LinkedStack<String> GetInput(String[] expr);
    }
}