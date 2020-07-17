using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
namespace CSharpDataStructures.Structures.Lists {
    public class ArrayList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable {
        private T[] _base;
        private Int32 _last;//pointer to the last element of the list. (index)
        private Int32 _capacity;
        
        public ArrayList(T[] array){
            this._base = array;
            this._capacity = array.Length;
            this._last = array.Length - 1;
        }
        
        public ArrayList(){
            this._base = new T[1000];
            this._capacity = 1000;
            this._last = -1;
        }
        
        public ArrayList(IEnumerable<T> seq){
            this._base = new T[seq.Count()*2];
            this._capacity = seq.Count()*2;
            this._last = seq.Count() - 1;
            foreach(var item in seq){
                Add(item);
            }
        }
        
        public ArrayList(Int32 capacity){
            this._capacity = capacity;
            this._base = new T[capacity];
            this._last = -1;
        }
        
        public Boolean IsEmpty(){
            return (this._last + 1 == 0);
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
        
        //INSERT (dynamic space.)
        public void Insert(Int32 index, T item){
            Int32 q = index;
            if(Count == 0){
                _base[0] = item;
                _last+=1;
                return;
            }
            if(this._last >= _base.Length - 1){ //HANDLE ERROR
            
                T[] nr = new T[_capacity*2];
                for(Int32 i = 0; i < _base.Length; i++){
                    nr[i] = _base[i];
                }
                nr[_base.Length] = item;
                this._last = _base.Length;
                this._base = null;
                this._base = nr;
                this._capacity = _capacity*2;
                
                //Console.WriteLine("Not enough space.");
                return;
            }
            else if(index > _last + 1 || index < 0){
                Console.WriteLine("This position isn't existed in the list");//HANDLE ERROR
                return;
            }
            
            else if(index > _last){
                _base[index] = item;
                this._last += 1;
                return;
            }
            for(Int32 i = _last; i <= index; i--){
                _base[i+1] = _base[i]; //move others to the right.
            }
            this._last+=1;
            _base[index] = item;
        }
        
        public void Add(T item){
            
            Int32 q = _last;
            Insert(q+1,item);
        }
        
        
        //DELETE
        public void RemoveAt(Int32 index){
            if(index > this._last || index < 0){
                Console.WriteLine("This position isn't existed in the list");//HANDLE ERROR
                return;
            }
            this._last -= 1;
            for(Int32 i = index; i <= _last; i++){
                _base[i] = _base[i+1]; //move others to the left.
            }
            
        }
        
        public Boolean Remove(T item){
            for(Int32 i = 0; i <= _last; i++){
                if(_base[i].Equals(item)){
                    RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        
        //MAKENULL
        public void Clear(){
            this._base = null;
            _base = new T[_capacity];
            this._last = -1;
        }
        
        //LOCATE
        public Int32 IndexOf(T item){
            for(Int32 i = 0; i <= _last; i++){
                if(_base[i].Equals(item))
                    return i;
            }
            return -1;
        }
        
        public Boolean Contains(T item){
            return IndexOf(item) != -1;
        }
        
        //RETRIEVE
        public T this[Int32 index]{
            get{
                return _base[index];
            }
            set{
                _base[index] = value;
            }
        }
        
        //END
        public Int32 Count {
            get{
                return this._last + 1;
            }
        }
        
       
        
        public Boolean IsReadOnly {
            get{ return false; }
        }
      
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public IEnumerator<T> GetEnumerator(){
            for(Int32 i = 0; i <= this._last; i++){
                yield return _base[i];
            }
        }
        
        //FIRST position.
        public Int32 First {
            get{
                if(Count == 0)
                    return -1;
                return 0;
            }
        }
        
        public T FirstElement {
            get{
                return this[0];
            }
        }
        
        //Remove All Duplicates.
        public void Purge(){
            Int32 p = this.First;
            Int32 q = 0;
            while(p != this.Count){ //!= END
                q = p + 1;//NEXT(p)
                while(q != this.Count){//!= END
                    if(this[p].Equals(this[q])) //RETRIVE and same funcs.
                        RemoveAt(q);
                    else
                        q+=1;//NEXT
                }
                p+=1;//NEXT
            }
        }
        
        public override String ToString(){
            StringBuilder sb = new StringBuilder();
            sb.Append("[ ");
            for(Int32 i = 0; i <= _last; i++){
                sb.Append(_base[i]+" ");
            }
            sb.Append("]");
            sb.Append("\n");
            return sb.ToString();
        }
        
        
    }
}