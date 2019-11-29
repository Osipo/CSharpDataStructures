using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CSharpDataStructures.Structures.Lists;
namespace CSharpDataStructures.Structures.Sequences {
    class Sequence {
        protected IFormulaParser _par;
        protected String _expr;
        protected LinkedStack<String> _parsedExpr;
        public Sequence(String expr, IFormulaParser p){
            this._par = p;
            this._expr = expr;
            this._parsedExpr = _par.GetInput(expr.ToLower().Split(new Char[]{' ' , ',', '.', ';', '-', ':','?','!','\"'},StringSplitOptions.RemoveEmptyEntries));
        }
        public Sequence(String expr) : this(expr, new ArithmeticSyntaxParser()) {}
        
        public Double this[UInt32 idx]{
            get{
                if(idx == 0)
                    idx = 1;
                return GetElement(idx);
            }
        }
        
        public String Expression {
            get{
                return _expr;
            }
        }
        
        public LinkedStack<String> Parsed {
            get{
                return _parsedExpr;
            }
        }
        
        protected Double __GetByFormula(String[] e){
            Int32 i = 0;
            LinkedStack<Double> r = new LinkedStack<Double>();
            while(i < e.Length){
                if(_par.IsOperator(e[i])){
                    Double o1 = r.Top();
                    r.Pop();
                    Double o2 = r.Top();
                    r.Pop();
                    
                    switch(e[i]){
                        case "+":{
                            r.Push(o1 + o2);
                            break;
                        }
                        case "-":{
                            r.Push(o1 - o2);
                            break;
                        }
                        case "*":{
                            r.Push(o1 * o2);
                            break;
                        }
                        case "/":{
                            r.Push(o1 / o2);
                            break;
                        }
                        case "^":{
                            r.Push(Math.Pow(o2,o1));
                            break;
                        }
                        default:{break;}
                    }
                }
                else{
                    r.Push(Convert.ToDouble(e[i]));
                }
                i++;
            }
            return r.Top();
        }
        
        public virtual Double GetElement(UInt32 num){
            if(num == 0)
                num = 1;
            Double res = 1d;
            String[] fa = _parsedExpr.ToArray();
            Int32 i = 0;
            while(i < fa.Length){
                Double k;
                if(!Double.TryParse(fa[i],out k) && !(_par.IsOperator(fa[i]))){//IF IT IS NOT A NUMBER AND OPERATOR
                    fa[i] = num+",0";
                }
                i++;
            }
            return __GetByFormula(fa);
        }
    }
}