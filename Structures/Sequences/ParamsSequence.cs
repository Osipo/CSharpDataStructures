using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CSharpDataStructures.Structures.Lists;
using CSharpDataStructures.Structures.Maps;
namespace CSharpDataStructures.Structures.Sequences {
    class ParamsSequence : Sequence {
        private ArrayMap<String,Double> _prms;
        public ParamsSequence(String expr, IFormulaParser p, ArrayMap<String,Double> paramList) : base(expr, p){
            this._prms = paramList;
        }
        
        public ParamsSequence(String expr, ArrayMap<String,Double> paramList) : base(expr, new ArithmeticSyntaxParser()){
            this._prms = paramList;
        }
        
        public ParamsSequence(String expr) : base(expr, new ArithmeticSyntaxParser()) {}
        
        public override Double GetElement(UInt32 num){
            if(num == 0)
                num = 1;
            Double res = 1d;
            String[] fa = _parsedExpr.ToArray();
            Int32 i = 0;
            while(i < fa.Length){
                Double k;
                if(!Double.TryParse(fa[i],out k) && !(_par.IsOperator(fa[i])) && (fa[i].ToLower() == "n" || _prms == null || _prms.Count == 0)){//IF IT IS NOT A NUMBER AND OPERATOR AND SEQUENCE_NUMBER
                    fa[i] = num+",0";
                }
                else if(!Double.TryParse(fa[i],out k) && !(_par.IsOperator(fa[i])) ){
                    fa[i] = _prms.GetValue(fa[i]).ToString();
                }
                i++;
            }
            return base.__GetByFormula(fa);
        }
    }
}