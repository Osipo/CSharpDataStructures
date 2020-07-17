using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
namespace CSharpDataStructures.Structures.Lists {
    //Stack is list where DELETE AND INSERT operations are avaliable only at the beginning of the list. (LIFO)
    public class Stack<T> : ICollection<T>, IStack<T>, IEnumerable<T>, IEnumerable {
        private Int32 _top;//pointer to the top of the stack.
        private Int32 _count;//count
        private Int32 _maxlength;//capacity.
        private T[] _base;
        
        public Stack(T[] array){
            this._base = array;
            this._maxlength = array.Length;
            this._count = array.Length;
            this._top = 0;
        }
      
        public Stack(Int32 capacity){
            this._base = new T[capacity];
            this._maxlength = capacity;
            this._count = 0;
            this._top = capacity;//empty.
        }
        
        //EMPTY(S): BOOLEAN
        public Boolean IsEmpty(){
            return (this._top == _maxlength); //top > lastIndexOfArray
        }
        
        //TOP
        public T Top(){
            if(IsEmpty()){
                Console.WriteLine("Error. Stack is empty");
                return default(T);
            }
            else return _base[_top];
        }
        
        //POP
        public void Pop(){
            if(IsEmpty()){
                Console.WriteLine("Error. Stack is empty");
                return;
            }
            else{
                _base[_top] = default(T);
                this._count -= 1;
                this._top += 1;//move to the end...
            }
        }
        
        //PUSH
        public void Push(T item){
            if(_top == 0){
                Console.WriteLine("Error. Stack is filled");
                return;
            }
            else{
                this._top -= 1;//move to the begining...
                this._count += 1;
                _base[_top] = item;
            }
        }
        
        public void Add(T item){
            Push(item);
        }
        
        public bool Remove(T item){
            if(IsEmpty()){
                Console.WriteLine("Error. Stack is empty");
                return false;
            }
            else if(_base[_top].Equals(item)){
                _base[_top] = default(T);
                _top += 1;
                return true;
            }
            else
                return false;
        }
        
        public bool Contains(T item){
            Int32 q = _top;
            while(q < _maxlength){
                if(_base[q].Equals(item)){
                    return true;
                }
                q+=1;
            }
            return false;
        }
        
        public Boolean IsReadOnly{
            get{  return false;  }
        }
        
        public Int32 Count{
            get{
                return this._count;
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public IEnumerator<T> GetEnumerator(){
            for(Int32 i = _top; i < this._maxlength; i++){
                yield return _base[i];
            }
        }
        
        //MAKENULL
        public void Clear(){
            this._base = null;
            this._base = new T[_maxlength];
            this._count = 0;
            this._top  = _maxlength;//empty.
        }
        
        public void CopyTo(T[] array, Int32 arrayIndex){
            if(arrayIndex < 0 || arrayIndex >= array.Length){
                Console.WriteLine("arrayIndex is out of range");
                return;
            }
            for(Int32 i = arrayIndex; i < _base.Length && i < array.Length; i++){
                array[i] = this._base[i];
            }
        }
        
        public override String ToString(){
            StringBuilder sb = new StringBuilder();
            sb.Append("$[ ");
            for(Int32 i = _top; i < _maxlength; i++){
                sb.Append(_base[i].ToString()+" ");
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}