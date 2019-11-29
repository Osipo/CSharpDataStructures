using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
namespace CSharpDataStructures.Structures.Lists {
    class ArrayCyclicQueue<T> : ICollection<T>, IEnumerable<T>, IEnumerable {
        private T[] _base;
        private Int32 _front;
        private Int32 _rear;
        private Int32 _capacity;//maxlength + 1
        private Int32 _count;
        
        public ArrayCyclicQueue(Int32 capacity){
            this._base = new T[capacity];
            this._front = 0;
            this._count = 0;
            this._rear = capacity - 1;
            this._capacity = capacity;
        }
        
        public ArrayCyclicQueue(T[] array){
            this._base = array;
            this._capacity = array.Length;
            this._count = array.Length;
            this._front = 0;
            this._rear = _capacity - 1;
        }
      
        
        //MAKENULL
        public void Clear(){
            this._base = null;
            this._base = new T[_capacity];
            this._front = 0;
            this._count = 0;
            this._rear = _capacity - 1; //capacity - 1 = maxlength = LastIndexOfArray
        }
        
        private Int32 __Addone(Int32 i){
            return i % (_capacity - 1) + 1;//(i mod maxlength) + 1 where maxlength is LastIndexOfArray
        }
        
        //EMPTY(Q): BOOLEAN
        public Boolean IsEmpty(){
            if(__Addone(this._rear) == this._front){
                return true;
            }
            else
                return false;
        }
        
        //FRONT
        public T Front(){
            if(IsEmpty()){
                Console.WriteLine("Error. Queue is empty");
                return default(T);
            }
            else
                return this._base[this._front];
        }
        
        public void Enqueue(T item){
            if(__Addone(__Addone(this._rear)) == this._front){
                Console.WriteLine("Error. Queue is filled");//HANDLE dynamic_len
                return;
            }
            else{
                this._rear = __Addone(this._rear);
                this._base[this._rear] = item;
                this._count +=1;
            }
        }
        
        public void Dequeue(){
            if(IsEmpty()){
                Console.WriteLine("Error. Queue is empty");
                return;
            }
            else{
                this._front = __Addone(this._front);
                this._count -=1;
            }
        }
        public void Add(T item){
            Enqueue(item);
        }
        
        public bool Remove(T item){
            if(IsEmpty()){
                Console.WriteLine("Error. Queue is empty");
                return false;
            }
            if(_base[_front].Equals(item)){
                this._count -=1;
                this._front = __Addone(this._front);
                return true;
            }
            return false;
        }
        
        public Boolean Contains(T item){
            Int32 q = _front;
            Int32 c = 0;
            while(q < _capacity && c < _count){//CHECK
                if(_base[q].Equals(item)){
                    return true;
                }
                c+=1;
                q = __Addone(q);
            }
            return false;
        }
        
        public Int32 Count{
            get{
                return _count;
            }
        }
        
        public Boolean IsReadOnly{
            get{
                return false;
            }
        }
        
        //COPY TO ARRAY(array)
        public void CopyTo(T[] array, Int32 arrayIndex){
            if(arrayIndex < 0 || arrayIndex >= array.Length){
                Console.WriteLine("arrayIndex is out of range");
                return;
            }
            for(Int32 i = arrayIndex, j = 0; j < _base.Length && i < array.Length; i++,j++){
                array[i] = this._base[j];
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        
        
        public IEnumerator<T> GetEnumerator(){
            for(Int32 i = 0; i < _capacity; i++){//REPLACE TO __Addone(_front)
                if(_base[i].Equals(default(T))){
                    continue;
                }
                yield return _base[i];
            }
        }
        
        public override String ToString(){
            StringBuilder sb = new StringBuilder();
            sb.Append("^[ ");
            Int32 q = _front;
            Int32 c = 0;
            while(q < _capacity && c < _count){//CHECK
                sb.Append(_base[q].ToString()+" ");
                c+=1;
                q = __Addone(q);
            }
            sb.Append("]$");
            return sb.ToString();
        }
    }
}