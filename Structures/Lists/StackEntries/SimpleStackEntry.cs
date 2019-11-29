using System;
namespace CSharpDataStructures.Structures.Lists.StackEntries {
    class SimpleStackEntry<T> {
        private T _value;
        private bool _isRight;
        private bool _isLeaf;
        private bool _isCenterL;
        public SimpleStackEntry(T v, bool r, bool l, bool c){
            this._value = v;
            this._isRight = r;
            this._isLeaf = l;
            this._isCenterL = c;
        }
        
        public T Value(){
            return _value;
        }
        public bool IsRight(){
            return _isRight;
        }
        public bool IsLeaf(){
            return _isLeaf;
        }
        public bool IsInter(){
            return _isCenterL;
        }
    }
}