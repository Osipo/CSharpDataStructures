using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
namespace CSharpDataStructures.Structures.Lists {
    //Queue is list where DELETE AND INSERT operations are avaliable only 
    //at the beginning and at the end of the list respectively. (FIFO)
    public class LinkedQueue<T> : ICollection<T>, IEnumerable<T>, IEnumerable{
        protected ElementType<T> _front;//pointer to the first element. (->) Ignored:: Element field.
        protected ElementType<T> _rear;//pointer to the last element. (->)
        protected Int32 _count;
        
        public LinkedQueue(){
            this._count = 0;
            this._front = new ElementType<T>();
            this._front.Next = null;
            this._rear = _front;
        }
        
        //MAKENULL
        public void Clear(){
            this._front = new ElementType<T>();
            this._front.Next = null;
            this._rear = _front;
            this._count = 0;
        }
        
        //EMPTY(Q): BOOLEAN
        public Boolean IsEmpty(){
            if(this._front == this._rear){
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
                return this._front.Next.Element;
        }
        
        //ENQUEUE
        public void Enqueue(T item){
            this._rear.Next = new ElementType<T>();//new(rear.next)
            this._rear = this._rear.Next;//rear = rear.next
            this._rear.Element = item;//rear.element = x
            this._rear.Next = null;//rear.next = nil
            this._count +=1;
        }
        
        //DEQUEUE
        public void Dequeue(){
            if(IsEmpty()){
                Console.WriteLine("Error. Queue is empty");
                return;
            }
            else{
                this._front = this._front.Next; //Q.front = Q.front.next.
                this._count -= 1;
            }
        }
        
        public Int32 Count{
            get{
                return this._count;
            }
        }
        
        public Boolean IsReadOnly{
            get{
                return false;
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
            
            else if(this._front.Next.Element.Equals(item)){
                this._front = this._front.Next; //Q.front = Q.front.next.
                this._count -= 1;
                return true;
            }
            else
                return false;
        }
        
        public bool Contains(T item){
            ElementType<T> f = _front.Next;
            while(f != null){
                if(f.Element.Equals(item)){
                    return true;
                }
                f = f.Next;
            }
            return false;
        }
        
        //COPY TO ARRAY(array)
        public void CopyTo(T[] array, Int32 arrayIndex){
            ElementType<T> f = _front.Next;
            if(arrayIndex < 0 || arrayIndex >= array.Length){
                Console.WriteLine("arrayIndex is out of range");
                return;
            }
            Int32 i = arrayIndex;
            while(f != null && i < array.Length){
                array[i] = f.Element;
                f = f.Next;
                i+=1;
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public IEnumerator<T> GetEnumerator(){
            ElementType<T> p = _front.Next;
            while(p != null){
                yield return p.Element;
                p = p.Next;
            }
        }
        
        public override String ToString(){
            StringBuilder sb = new StringBuilder();
            sb.Append("^[ ");
            ElementType<T> p = _front.Next;
            while(p != null){
                sb.Append(p.Element.ToString()+" ");
                p = p.Next;
            }
            sb.Append("]$");
            return sb.ToString();
        }
    }
}