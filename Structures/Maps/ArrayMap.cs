using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
namespace CSharpDataStructures.Structures.Maps {
    class ArrayMap<T,R> : IEnumerable<R>, IEnumerable {
        private Int32 _count;
        private T[] _domain;
        private R[] _range;
        private Int32 _capacity;
        #region .ctors
        public ArrayMap() : this(1000){
            
        }
        
        public ArrayMap(Int32 capacity){
            this._capacity = capacity;
            this._domain = new T[capacity];
            this._range = new R[capacity];
            this._count = 0;
        }
        
        public ArrayMap(T[] domain, R[] range){
            if(domain.Length > range.Length){
                T[] nd = new T[domain.Length - (domain.Length - range.Length)];
                Int32 t1 = 0;
                while(t1 < nd.Length){
                    nd[t1] = domain[t1];
                    t1+=1;
                }
                this._domain = nd;
                this._range = range;
                this._capacity = nd.Length;
                this._count = nd.Length;
            }
            else{
                this._domain = domain;
                this._range = range;
                this._capacity = domain.Length;
                this._count = _capacity;
            }
        }
        #endregion
        
        //MAKENULL
        public void Clear(){
            this._domain = null;
            this._range = null;
            this._domain = new T[_capacity];
            this._range = new R[_capacity];
            this._count = 0;
        }
        
        
        //Get index of Key for RETRIEVING...
        public Int32 IndexOfKey(T key){
            Int32 q = 0;
            if(typeof(T).IsClass && key == null)
                return -1;
            while(q < _domain.Length){
                if((typeof(T).IsClass && _domain[q] == null)){
                    q++;
                    continue;
                }
                if(_domain[q].Equals(key)){
                    return q;
                }
                q++;
            }
            return -1;
        }
        
        public bool ContainsKey(T key){
            return IndexOfKey(key) != -1;
        }
        
        //ASSIGN(M,k,v)
        public void Add(T key,R value){
            Int32 idx = IndexOfKey(key);
            if(idx == -1){//new key...
                if(_count == _capacity){//Arrays are filled.
                    T[] _nd = new T[_capacity * 2];
                    R[] _nr = new R[_capacity * 2];
                    Int32 q = 0;
                    while(q < _domain.Length){
                        _nd[q] = _domain[q];
                        _nr[q] = _range[q];
                        q+=1;
                    }
                    this._domain = _nd;
                    this._range = _nr;
                    _domain[_count] = key;
                    _range[_count] = value;
                    _count +=1;
                    this._capacity = _capacity * 2;
                    return;
                }
                _domain[_count] = key;
                _range[_count] = value;//Append new pair KV.
                _count+=1;
                return;
            }//set new value by key...
            else{
                _range[idx] = value;
            }
        }
        
        //RETRIEVE(M,k)
        public R GetValue(T key){
            Int32 idx = IndexOfKey(key);
            if(idx == -1){
                return default(R);
            }
            return _range[idx];
        }
        
        public R this[T key]{
            get{
                return GetValue(key);
            }
            set{
                Add(key,value);//value -> R value.
            }
        }
        
        //COMPUTE(M,k,v)
        public bool TryGetValue(T key,out R value){
            Int32 idx = IndexOfKey(key);
            if(idx == -1){
                value = default(R);
                return false;
            }
            value = _range[idx];
            return true;
        }
        
        public bool Remove(T key){
            Int32 idx = IndexOfKey(key);
            if(idx == -1){
                return false;
            }
            this._domain[idx] = default(T);
            this._range[idx] = default(R);
            this._count -=1;
            return true;
        }
        
        //EMPTY(M): BOOLEAN
        public Boolean IsEmpty(){
            return _count == 0;
        }
        
        public Boolean IsReadOnly{
            get{
                return false;
            }
        }
        
        public Int32 Count{
            get{
                return _count;
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator(){
            return GetEnumerator();
        }
        
        public IEnumerator<R> GetEnumerator(){
            for(Int32 q = 0; q < _count; q++){
                if(_range[q].Equals(default(R)))
                    continue;
                yield return _range[q];
            }
        }
        
        public T[] Keys {
            get{
                T[] keys = new T[_count];
                for(Int32 q = 0; q < _count; q++){
                    keys[q] = _domain[q];
                }
                return keys;
            }
        }
        
        public R[] Values {
            get{
                R[] vals = new R[_count];
                for(Int32 q = 0; q < _count; q++){
                    vals[q] = _range[q];
                }
                return vals;
            }
        }
    }
}