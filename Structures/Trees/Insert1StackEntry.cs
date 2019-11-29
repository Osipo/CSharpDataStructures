using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CSharpDataStructures.Structures.Trees.Visitors;
namespace CSharpDataStructures.Structures.Trees {
    class Insert1StackEntry<T> {
        private TwoThreeNode<T> _lb;
        private T _l;
        public Insert1StackEntry(TwoThreeNode<T> lowback, T low){
            this._lb = lowback;
            this._l = low;
        }
        
        public T Low {
            get{
                return _l;
            }
        }
        
        public TwoThreeNode<T> LowBack{
            get{
                return _lb;
            }
        }
    }
}