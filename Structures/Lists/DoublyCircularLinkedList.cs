using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using CSharpDataStructures.Structures.Lists.Enumerators;
namespace CSharpDataStructures.Structures.Lists {
    public class DoublyCircularLinkedList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable{
        private DoubleElementType<T> _head;//pointer to the first element. (->) Ignored:: Element field.
        private DoubleElementType<T> _tail;//tail
        
        private Int32 _count;
        
        public DoublyCircularLinkedList(){
            this._count = 0;
            this._head = new DoubleElementType<T>();
            this._tail = new DoubleElementType<T>();
            this._tail.Next = _head;
            this._head.Previous = _tail;
            this._head.Next = _tail;
            this._tail.Previous = _head;
        }
        
        public Int32 Count {
            get{
                return _count;
            }
        }
        
        //EMPTY(L): BOOLEAN
        public Boolean IsEmpty(){
            return _count == 0;
        }
        
        public Boolean IsReadOnly{
            get{
                return false;
            }
        }
        
        //0 pointer to the _head. So pos must be [1..count]
        public void Insert(Int32 p,T item){
            Int32 np = p;
            if(p <= 0){
                np = (_count + 1) - Math.Abs(p);
            }
            else if(Math.Abs(p) > _count + 1)
            {
                np = Math.Abs(p) - _count + 1;
            }
            #region _tail
            //if(IsEmpty)
            if(Count == 0){
                this._tail.Element = item;
                _count+=1;
                return;
            }
            //Append.
            else if(np == Count + 1){//or p == 0
                _tail.Next = new DoubleElementType<T>();
                _tail.Next.Element = item;
                DoubleElementType<T> pr = _tail;
                _tail = _tail.Next;
                _tail.Next = _head;
                _tail.Previous = pr;
                _head.Previous = _tail;
                _count+=1;
                return;
            }
            #endregion
            Int32 q = 1;
            DoubleElementType<T> elem = new DoubleElementType<T>();
            elem.Element = item;
            DoubleElementType<T> temp = _head;//_head pos = 0.
            while(q < p){//move to p.
                temp = temp.Next;
                q+=1;
            }
            DoubleElementType<T> pp = temp;
            temp = temp.Next;//temp = p.next -> after last -> head
            
            pp.Next = elem;//p.next.element = x -> last -> next -> inserted_elem
            elem.Next = temp;//p.next.next = temp -> inserted_elem -> head.
            elem.Previous = pp;
            temp.Previous = elem;
            this._count += 1;
        }
        
        public void Add(T item){
            Insert(Count+1,item);//to the end.
        }
        
        
        //COPY TO ARRAY(array)
        public void CopyTo(T[] array, Int32 arrayIndex){
            DoubleElementType<T> first = _head.Next;
            if(arrayIndex < 0 || arrayIndex >= array.Length){
                Console.WriteLine("arrayIndex is out of range");
                return;
            }
            Int32 i = arrayIndex;
            while(first != _head && i < array.Length){//THROW ArgumentException IF(array.Length - arrayIndex < Count)
                array[i] = first.Element;
                first = first.Next;
                i+=1;
            }
        }
        
        //RETRIEVE.
        public T this[Int32 index]{
            get{
                DoubleElementType<T> b = _head;
                Int32 q = 0;
                while(b.Next != _head && q < index){
                    q+=1;
                    b = b.Next;
                }
                return b.Element;
            }
            set{
                DoubleElementType<T> b = _head;
                Int32 q = 0;
                while(b.Next != _head && q < index){
                    q+=1;
                    b = b.Next;
                }
                b.Element = value;
            }
        }
       
       
        public void RemoveAt(Int32 p){
            Int32 q = 1;
            if(Count == 0)
                return;
            DoubleElementType<T> pp = _head;
            while(q < p && pp.Next != _head){
                pp = pp.Next;
                q+=1;
            }
            pp.Previous.Next = pp.Next;
            pp.Next.Previous = pp.Previous;
            
            this._count -=1;
        }
        
        //DELETE
        public Boolean Remove(T item){
            DoubleElementType<T> p = _head;
            while(p.Next != _head){
                if(p.Next.Element.Equals(item)){
                    //p.Next = p.Next.Next;
                    p.Previous.Next = p.Next;
                    p.Next.Previous = p.Previous;
                    _count -=1;
                    return true;
                }
                p = p.Next;
            }
            return false;
        }
        
        //MAKENULL
        public void Clear(){
            this._count = 0;
            this._head = new DoubleElementType<T>();
            this._tail = new DoubleElementType<T>();
            this._tail.Next = _head;
            this._head.Previous = _tail;
            this._head.Next = _tail;
            this._tail.Previous = _head;
        }
        
        //TODO: Make traverse into two ways left->right, right->left.
        public Int32 IndexOf(T item){
            DoubleElementType<T> p = _head.Next;
            Int32 idx = 1;
            while(p != _head){
                if(p.Element.Equals(item)){
                    return idx;
                }
                idx+=1;
                p = p.Next;
            }
            return -1;
        }
        
        public Boolean Contains(T item){
            if(IndexOf(item) != -1)
                return true;
            return false;
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public IEnumerator<T> GetEnumerator(){
            return new LinkedListEnumerator<T>(this);
        }
        
        public IListEnumerator<T> GetIterator(){
            return new LinkedListEnumerator<T>(this);
        }
        
        private class LinkedListEnumerator<T> : IListEnumerator<T> {
            
            private T _c;
            private Int32 _idx;
            private DoubleElementType<T> _p;
            private DoublyCircularLinkedList<T> l;
            
            public LinkedListEnumerator(DoublyCircularLinkedList<T> l){
                this.l = l;
                this._p = l._head;
                this._idx = 1;
                _c = default(T);
            }
            public void Dispose(){}
            
            bool IEnumerator.MoveNext(){
                bool r;
                if((r = HasNext()) == true){
                    _c = MoveNext();
                }
                return r;
            }
            
            public bool HasNext(){
                return _idx < l._count + 1;
            }
            
            public T MoveNext(){
                if(HasNext()){
                    _idx++;
                    _p = _p.Next;
                    _c = _p.Element;
                    return _c;
                }
                _c = default(T);
                return _c;
            }
            
            public void Reset(){
                _idx = 1;
            }
            
            public void SetForward(){
                _p = l._head;
            }
            
            public void SetBack(){
                _p = l._tail;
            }
            
            public T Current {
                get{
                    return _c;
                }
            }
            
            object IEnumerator.Current {
                get{
                    return Current;
                }
            }
            
            
            public bool HasPrevious(){
                return _idx > 1;
            }
            
            public T MovePrevious(){
                if(HasPrevious()){
                    _idx--;
                    _p = _p.Previous;
                    _c = _p.Element;
                    return _c;
                }
                _c = default(T);
                return _c;
            }
        }
        
        public override String ToString(){
            StringBuilder sb = new StringBuilder();
            sb.Append("[ ");
            DoubleElementType<T> p = _head.Next;
            while(p != _head){
                sb.Append(p.Element.ToString()+" ");
                p = p.Next;
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}