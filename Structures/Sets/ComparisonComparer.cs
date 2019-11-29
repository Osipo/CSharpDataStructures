using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
namespace CSharpDataStructures.Structures.Sets{
    class ComparisonComparer<T> : IComparer<T>  {  
        private readonly Comparison<T> _comparison;  

        public ComparisonComparer(Comparison<T> comparison){  
            _comparison = comparison;  
        }  

        public int Compare(T x, T y){
            if(x.Equals(default(T)) || y.Equals(default(T))){
                return 0;
            }
            return _comparison(x, y);  
        }  
    }  
}