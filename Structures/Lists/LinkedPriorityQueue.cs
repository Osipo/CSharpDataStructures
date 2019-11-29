using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
namespace CSharpDataStructures.Structures.Lists {
    class LinkedPriorityQueue<T> : LinkedQueue<T>{
        private Func<T,Int32> _p;
        public LinkedPriorityQueue(Func<T,Int32> prior){
            this._p = prior;
        }
        
        public T DeleteMin(){
            ElementType<T> current;
            Int32 lowp = 0;
            ElementType<T> prewinner;
            if(_front.Next == null){
                Console.WriteLine("Error. Queue is empty");
                return default(T);
            }
            lowp = _p(_front.Next.Element);//GET Priority OF FIRST Element
            prewinner = _front;
            current = _front.Next;
            while(current.Next != null){
                if(_p(current.Next.Element) < lowp){
                    prewinner = current;
                    lowp = _p(current.Next.Element);
                }
                current = current.Next;
            }
            T result = prewinner.Next.Element;
            prewinner.Next = prewinner.Next.Next;
            return result;
        }
    }
}