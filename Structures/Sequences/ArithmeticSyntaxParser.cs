using System;
using System.Text;
using System.Collections.Generic;
using CSharpDataStructures.Structures.Lists;
namespace CSharpDataStructures.Structures.Sequences {
    class ArithmeticSyntaxParser : IFormulaParser {
        public ArithmeticSyntaxParser(){
        }
        
        public bool IsOperator(String c)
        {
            switch (c)
            {
                case "^":return true;
                case "*":return true;
                case "/":return true;
                case "+":return true;
                case "-":return true;
                case "(":return true;
                case ")":return true;
                default: return false;
            }
        }
        
        /*Priority of operator.*/
        private int getPriority(String c)
        {
            switch (c)
            {
                case "^": return 3;
                case "*": case "/": return 2;
                case "+": case "-": return 1;
                case "(": case ")": return 0;
                default: return -1;
            }
        }
        
        public LinkedStack<String> GetInput(String[] s){
            LinkedStack<String> ops = new LinkedStack<String>();
            LinkedStack<String> rpn = new LinkedStack<String>();
            for(Int32 i = 0; i < s.Length; i++){
                String tok = s[i];
                if(tok == "("){
                    ops.Push(tok);
                }
                else if(tok == ")"){
                    while(!ops.IsEmpty() && ops.Top() != "("){
                        rpn.Push(ops.Top());
                        ops.Pop();
                    }
                    ops.Pop();
                    /*
                    if(!ops.IsEmpty() && (ops.Top() == "-" || ops.Top() == "-")){
                        rpn.Push(ops.Top());
                        ops.Pop();
                    }*/
                }
                else if(!IsOperator(tok)){//isWord
                    rpn.Push(tok);
                }
                else if(IsOperator(tok)){
                    while(!ops.IsEmpty() && IsOperator(ops.Top()) && getPriority(tok) <= getPriority(ops.Top()) ){
                        rpn.Push(ops.Top());
                        ops.Pop();
                    }
                    ops.Push(tok);
                }
                /*
                else if(tok == "-" || tok == "-"){
                    ops.Push(tok);
                }*/
            }
            while(!ops.IsEmpty()){
                rpn.Push(ops.Top());
                ops.Pop();
            }
            LinkedStack<String> result = new LinkedStack<String>();
            while(!rpn.IsEmpty()){
                result.Push(rpn.Top());
                rpn.Pop();
            }
            return result;
        }
    }
}