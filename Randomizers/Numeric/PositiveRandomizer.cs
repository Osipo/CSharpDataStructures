using System;
namespace CSharpDataStructures.Randomizers.Numeric {
    //Generates Random Sequence of Natural Numbers (1,2,3,...N) where N = B - 1 AND where B is upperBound
    public class PositiveRandomizer {
        private Int32 _b;//returns from 1 to b-1 inclusive.
        private Int32 _seed;//first element of the sequence
        private Int32 _k;
        private Int32 _initial;//random val
        public PositiveRandomizer(): this(100) {}
        public PositiveRandomizer(Int32 ubound){
            this._b = ubound;
            DateTime dt = DateTime.Now;//MAKE UPDATE BY METHOD
            this._initial = /*dt.Month*dt.Day*dt.Hour*dt.Minute**/dt.Second % _b;//CHECK
            //this._seed = (_b/2+1);//REPLACE
            this._seed = _initial > 0 ? _initial/2 + 1 : _initial + 1;
            this._k = 1;
            __ComputeK();
        }
        
        //MAKE PRIVATE OR REMOVE
        public void SetSeed(Int32 seed){
            if(seed >= _b){
                seed = _b - 1;
            }
            else if(seed <= 0){
                seed = 1;
            }
            this._seed = seed;
        }
        
        public void SetUpperBound(Int32 bound){
            this._b = bound;
            this._initial = DateTime.Now.Second % _b;
            this._seed = _initial > 0 ? _initial/2 + 1 : _initial + 1;
            this._k = 1;
            __ComputeK();//k for new upperBound
        }
        
        public Int32 GetUpperBound(){
            return _b;
        }
        
        //O(N^2) where N == UpperBound - 1 IF(_seed IN 1..N/2) AND DELETED(LINES 46-47)
        //O(N^3) where N == UpperBound - 1
        private void __ComputeK(){
            //Int32 di = 1;
            //while(di < _b){
                while(_k < _b){
                    Int32 s = 0;
                    Int32 j = 1;
                    while(j < _b){
                        s+= NextInt();
                        j++;
                    }
                    if(s == __CheckSum()){
                        //_seed = (_b/2)+1;
                        //_seed = _initial + 1;
                        this._seed = _initial > 0 ? _initial/2 + 1 : _initial + 1;
                        return;
                    }
                    _k++;
                }
                //_k = 1;
                //this._seed = this._seed > _b - 2 ? 1 : this._seed + 1;
                //di++;
            //}
            
            //if k is still == upperBound that is the case when it is not possible to create Random sequence.
            //_seed = (_b/2)+1;
            //_seed = _initial + 1;
            //this._seed = _initial > 0 ? _initial : _initial + 1;
        }
        
        private Int32 __CheckSum(){
            return ((_b - 1)*_b)/2; //n*(n+1)/2 OR Sum Of 1 + 2 + 3 + ... + N
        }
        
        
        
        public Int32 NextInt(){
            Int32 res = _seed;
            _seed *=2;
            if(_seed >= _b){
                _seed = (_seed - _b) ^ _k;
            }
            return res;
        }
        
        public Int32 GetK(){
            return _k;
        }
    }
}