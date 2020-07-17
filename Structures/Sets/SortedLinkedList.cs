using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSharpDataStructures.Structures.Lists;
namespace CSharpDataStructures.Structures.Sets {
    
    ///<summary>Представляет реализацию множества
    ///на основе односвязного линейного списка</summary>
    public class SortedLinkedList<T> : IEquatable<SortedLinkedList<T>>, IEnumerable<T>, ISet<T>, ICollection<T>, IMFSet<T> {
        private ElementType<T> _head;//pointer to the first element. (->) Ignored:: Element field.
        private ElementType<T> _tail;//for adding sorted elements.
        private IComparer<T> _comp;
        private Int32 _count;
        
        
        #region .ctors
        ///<summary>Создать пустое множество с компаратором по умолчанию.</summary>
        public SortedLinkedList() : this(Comparer<T>.Default) {}
        
        ///<summary>Создать пустое множество с указанным компаратором</summary>
        ///<param name="comparer">Компаратор, выполняющие операции упорядочивания
        ///и предоставляющий метод CompareTo(T a, T b)</param>
        public SortedLinkedList(IComparer<T> comparer){
            if(comparer == null){
                this._comp = Comparer<T>.Default;
            }
            else{
                this._comp = comparer;
            }
            this._count = 0;
            this._head = new ElementType<T>();
            this._tail = new ElementType<T>();
            this._tail.Next = null;
        }
        
        //FOR SORTED SEQUENCES O(N)
        //FOR UNSORTED SEQUENCES O(N * LOG(N))
        
        ///<summary>Создать множество с элементами из переданного списка
        ///и указанного компаратора. Если последовательность уже упорядочена
        ///то вставка всех элементов займёт O(N), иначе последовательность
        ///будет отсортирована за O(N LOG N), а после этого все её элементы
        ///будут вставлены за время O(N)</summary>
        public SortedLinkedList(IEnumerable<T> list, IComparer<T> comp) : this(comp) {
            if(list == null){
                return;
            }
            list = list.Distinct();
            if(!__Sorted(list)){
                list = list.OrderBy(x => x,_comp);
                //Console.WriteLine("SORTED LIST");
            }
            foreach(var item in list){//O(N)
                __AddO(item);//O(1)
            }
            
        }
        ///<summary>Создать множество с элементами из переданного списка</summary>
        ///<param name="list">Набор перечислимых элементов, коллекция типа IEnumerable</param>
        public SortedLinkedList(IEnumerable<T> list) : this(list, Comparer<T>.Default){}
        
        
        //Check wheather the IEnumerable<T> has been already sorted.
        private bool __Sorted(IEnumerable<T> list){//REPLACE
            Int32 l = list.Count();
            if(l == 0)
                return true;
            T k = list.ElementAt(0);
            Int32 o = 0;
            for(Int32 i = 1; i < l; i++){
                if(_comp.Compare(k, list.ElementAt(i)) < 0){
                    o = -1;
                    break;
                }
                else if(_comp.Compare(k, list.ElementAt(i)) > 0){
                    o = 1;
                    break;
                }
            }
            //0,1,-1.
            if(o == 0){
                return true;//{C,C,C,...C}
            }
            if(o == 1){
                list = list.Reverse();//{x1 > x2...> x_n} -> {x1 < x2...< x_n}
            }
            
            for (Int32 i = 0; i < l - 1; i++) {
                if (_comp.Compare(list.ElementAt(i), list.ElementAt(i + 1)) > 0) {
                    return false; // It is proven that the sequence is not sorted.
                }
            }
            return true;
        }
        //private method for .ctor(IEnumerable<T> list, IComparer<T> comp)
        private void __AddO(T item){
            if(_count == 0){
                _tail.Element = item;
                _count+=1;
                _head.Next = _tail;
                return;
            }
            ElementType<T> nc = new ElementType<T>();
            nc.Element = item;
            nc.Next = null;
            ElementType<T> p = _tail;
            p.Next = nc;
            _tail = p.Next;
            _count+=1;
        }
        
        #endregion
        
       
        
        //INSERT(x,A): O(N) (Union also O(N))
        /// <summary>
        /// Вставка элемента в множество. 
        /// </summary>
        /// <param name="item">Элемент, которого нет в множестве</param>
        /// <returns>true если вставлен новый элемент, иначе false.</returns>
        public bool Add(T item){
            if(item == null)
                return false;
            ElementType<T> newcell, p;
            p = _head;
            
            if(_count == 0){
                _tail.Element = item;
                _count+=1;
                _head.Next = _tail;
                return true;
            }
            while(p.Next != null){
                if(p.Next.Element.Equals(item)){
                    return false;
                }
                if(_comp.Compare(p.Next.Element, item) > 0){
                    break;
                }
                p = p.Next;
            }
            newcell = new ElementType<T>();
            newcell.Element = item;
            newcell.Next = p.Next;
            if(p.Next == null){
                _tail = newcell;
            }
            p.Next = newcell;
            this._count+=1;
            return true;
        }
        
        /// <summary>
        /// Наследуется от ICollection. Вызывает метод вставки bool Add(T item)
        /// </summary>
        /// <param name="item">Новый элемент множества.</param>
        void ICollection<T>.Add(T item){
            bool f = Add(item);
        }
        
        //DELETE(x,A): O(N)
        /// <summary>
        /// Удалить указанный элемент из множества
        /// </summary>
        /// <param name="item">Элемент множества.</param>
        /// <returns>true если элемент удалён, иначе false (нет такого элемента в множестве)</returns>
        public bool Remove(T item){
            if(item == null)
                return false;
            ElementType<T> p = _head;
            while(p.Next != null){
                if(p.Next.Element.Equals(item)){
                    p.Next = p.Next.Next;
                    _count -=1;
                    return true;
                }
            }
            return false;
        }
        
        //MAKENULL
        /// <summary>
        /// Сделать множество пустым.
        /// </summary>
        public void Clear(){
            this._count = 0;
            this._head.Next = null;
            this._tail = null;
            this._tail = new ElementType<T>();
            this._tail.Next = null;
        }
        
        /// <summary>
        /// Индекс элемента в множестве, отсчитываемый с 1.
        /// </summary>
        /// <param name="item">Элемент множества</param>
        /// <returns>Индекс элемента, если он есть в множестве, иначе -1.</returns>
        public Int32 IndexOf(T item){
            ElementType<T> p = _head.Next;
            Int32 idx = 1;
            while(p != null){
                if(p.Element.Equals(item)){
                    return idx;
                }
                idx+=1;
                p = p.Next;
            }
            return -1;
        }
        
        //RETRIEVE.
        /// <summary>
        /// Возвращает элемент множества по указанному индексу.
        /// </summary>
        /// <param name="index">Индекс элемента множества.</param>
        /// <returns>Либо элемент множества, либо значение по умолчанию для типа T.</returns>
        public T this[Int32 index]{
            get{
                ElementType<T> b = _head;
                Int32 q = -1;//for Start = 0 q => -1.
                while(b.Next != null && q < index){
                    q+=1;
                    b = b.Next;
                }
                return b.Element;
            }
        }
        
        
        //RETRIEVE(RANGE) where RANGE IS [l,r] (0,0) returns this[0]
        /// <summary>
        /// Получить подмножество элементов множества, начиная с индекса l
        /// и заканчивая индексом r включительно. (Индексы отсчитываются с нуля).
        /// </summary>
        /// <param name="l">Левая граница, отсчитываемая с нуля.</param>
        /// <param name="r">Правая граница, отсчитываемая с нуля.</param>
        /// <returns>Подмножество элементов, или null, если границы указаны неверно.</returns>
        public SortedLinkedList<T> GetRange(Int32 l,Int32 r){
            if(l >= _count || l < 0){
                Console.WriteLine("left bound is out of range");
                return null;
            }
            if(r >= _count || r < 0 || r < l){
                Console.WriteLine("right bound is out of range");
                return null;
            }
            
            ElementType<T> b = _head;
            Int32 i = -1;
            SortedLinkedList<T> R = new SortedLinkedList<T>(_comp);
            while(i < l){
                b = b.Next;
                i++;
            }
            
            
            while(i <= r){
                R.__AddO(b.Element);
                b = b.Next;
                i++;
            }
            return R;
        }
        
        //MEMBER(x,L)
        /// <summary>
        /// Проверить, что указанный элемент содержится в множестве.
        /// </summary>
        /// <param name="item">Элемент.</param>
        /// <returns>true если элемент есть в множестве, иначе false.</returns>
        public Boolean Contains(T item){
            return IndexOf(item) != -1;
        }
        
        //COPY TO ARRAY(array)
        /// <summary>
        /// Копирует элементы множества в указанный массив array,
        /// начиная с индекса массива arrayIndex.
        /// </summary>
        /// <param name="array">Массив, куда надо вставить элементы.</param>
        /// <param name="arrayIndex">Индекс массива, с которого надо начинать вставку.</param>
        public void CopyTo(T[] array, Int32 arrayIndex){
            ElementType<T> first = _head.Next;
            if(arrayIndex < 0 || arrayIndex >= array.Length){
                Console.WriteLine("arrayIndex is out of range");
                return;
            }
            Int32 i = arrayIndex;
            while(first != null && i < array.Length){//THROW ArgumentException IF(array.Length - arrayIndex < Count)
                array[i] = first.Element;
                first = first.Next;
                i+=1;
            }
        }
        
        
        #region operators
        /// <summary>
        /// Возвращает пересечение текущего множества с указанным.
        /// Типы множеств - SortedLinkedList, т.е. используется реализация списков.
        /// </summary>
        /// <param name="B">Второе множество</param>
        /// <returns>Новое множество элементов. (может быть пустым)</returns>
        public SortedLinkedList<T> Intersection(SortedLinkedList<T> B){//MAKE PARAM_TYPE COMMON
            SortedLinkedList<T> sll = new SortedLinkedList<T>(_comp);//C.
            if(B.Count == 0){//B = Empty.
                return sll;
            }
            __Intersection(_head,B._head,out sll._head,ref sll._tail,ref sll._count);
            return sll;
        }

        /// <summary>
        /// Возвращает объединение текущего множества с указанным.
        /// Типы множеств - SortedLinkedList, т.е. используется реализация списков.
        /// </summary>
        /// <param name="B">Второе множество</param>
        /// <returns>Новое множество элементов. (может быть пустым)</returns>
        public SortedLinkedList<T> Union(SortedLinkedList<T> B){
            SortedLinkedList<T> sll = new SortedLinkedList<T>(_comp);//C.
            __Union(_head,B._head,out sll._head,ref sll._tail,ref sll._count);
            return sll;
        }

        /// <summary>
        /// Возвращает разность текущего множества с указанным.
        /// Типы множеств - SortedLinkedList, т.е. используется реализация списков.
        /// </summary>
        /// <param name="B">Второе множество</param>
        /// <returns>Новое множество элементов. (может быть пустым)</returns>
        public SortedLinkedList<T> Difference(SortedLinkedList<T> B){
            SortedLinkedList<T> sll = new SortedLinkedList<T>(_comp);//C.
            __Difference(_head,B._head,out sll._head,ref sll._tail,ref sll._count);
            return sll;
        }

        /// <summary>
        /// Возвращает слияние уникальных элементов текущего множества с указанным.
        /// Типы множеств - SortedLinkedList, т.е. используется реализация списков.
        /// </summary>
        /// <param name="B">Второе множество</param>
        /// <returns>Новое множество элементов, если пересечение исходных множеств было пустым, иначе null.</returns>
        public SortedLinkedList<T> Merge(SortedLinkedList<T> B){
            SortedLinkedList<T> C = Intersection(B);
            if(C.Count != 0){
                return null;
            }
            SortedLinkedList<T> R = Union(B);
            return R;
        }

        /// <summary>
        /// Возвращает геометрическую разность (элементы, не входящие в пересечение) текущего множества с указанным.
        /// Типы множеств - SortedLinkedList, т.е. используется реализация списков.
        /// </summary>
        /// <param name="B">Второе множество</param>
        /// <returns>Новое множество элементов. (может быть пустым)</returns>
        public SortedLinkedList<T> SymmetricDifference(SortedLinkedList<T> B){
            SortedLinkedList<T> ANB = Difference(B);//A\B
            SortedLinkedList<T> BNA = B.Difference(this);//B\A
            return ANB.Union(BNA);
        }
        
        private void __Intersection(ElementType<T> ahead,ElementType<T> bhead, out ElementType<T> pc,ref ElementType<T> t, ref Int32 c){
            ElementType<T> acurrent = ahead.Next;
            ElementType<T> bcurrent = bhead.Next;
            pc = new ElementType<T>();
            ElementType<T> ccurrent = pc;
            while(acurrent != null && bcurrent != null){
                if(acurrent.Element.Equals(bcurrent.Element)){
                    ccurrent.Next = new ElementType<T>();//3 ROWS
                    ccurrent = ccurrent.Next;
                    ccurrent.Element = acurrent.Element;
                    c+=1;
                    acurrent = acurrent.Next;
                    bcurrent = bcurrent.Next;
                }
                else if(_comp.Compare(acurrent.Element,bcurrent.Element) < 0){
                    acurrent = acurrent.Next;
                }
                else{
                    bcurrent = bcurrent.Next;
                }
            }
            ccurrent.Next = null;
            t = ccurrent;
        }
        
        private void __Union(ElementType<T> ahead,ElementType<T> bhead, out ElementType<T> pc,ref ElementType<T> t,ref Int32 c){
            ElementType<T> acurrent = ahead.Next;
            ElementType<T> bcurrent = bhead.Next;
            pc = new ElementType<T>();
            bool flag = true;
            ElementType<T> ccurrent = pc;
            while(acurrent != null || bcurrent != null){
                if(flag && (acurrent == null || bcurrent == null)){
                    flag = false;
                }
                if(flag && acurrent.Element.Equals(bcurrent.Element)){
                    ccurrent.Next = new ElementType<T>();//3 ROWS
                    ccurrent = ccurrent.Next;
                    ccurrent.Element = acurrent.Element;
                    c+=1;
                    acurrent = acurrent.Next;
                    bcurrent = bcurrent.Next;
                }
                else if(flag && _comp.Compare(acurrent.Element,bcurrent.Element) < 0){
                    ccurrent.Next = new ElementType<T>();//3 ROWS
                    ccurrent = ccurrent.Next;
                    ccurrent.Element = acurrent.Element;
                    c+=1;
                    acurrent = acurrent.Next;
                }
                else if(bcurrent == null){
                    ccurrent.Next = new ElementType<T>();
                    ccurrent = ccurrent.Next;
                    ccurrent.Element = acurrent.Element;
                    c+=1;
                    acurrent = acurrent.Next;
                }
                else{
                    ccurrent.Next = new ElementType<T>();//3 ROWS
                    ccurrent = ccurrent.Next;
                    ccurrent.Element = bcurrent.Element;
                    c+=1;
                    bcurrent = bcurrent.Next;
                }
            }
            ccurrent.Next = null;
            t = ccurrent;
        }
        
        private void __Difference(ElementType<T> ahead,ElementType<T> bhead, out ElementType<T> pc, ref ElementType<T> t,ref Int32 c){
            ElementType<T> acurrent = ahead.Next;
            ElementType<T> bcurrent = bhead.Next;
            pc = new ElementType<T>();
            bool flag = true;
            ElementType<T> ccurrent = pc;
            while(acurrent != null){
                if(flag && bcurrent == null)
                    flag = false;
                
                if(flag && acurrent.Element.Equals(bcurrent.Element)){
                    acurrent = acurrent.Next;
                    bcurrent = bcurrent.Next;
                }
                else if(flag && _comp.Compare(acurrent.Element,bcurrent.Element) < 0){
                    ccurrent.Next = new ElementType<T>();//3 ROWS
                    ccurrent = ccurrent.Next;
                    ccurrent.Element = acurrent.Element;
                    c+=1;
                    acurrent = acurrent.Next;
                }
                else{
                    if(flag)
                        bcurrent = bcurrent.Next;
                    else{
                        ccurrent.Next = new ElementType<T>();//3 ROWS
                        ccurrent = ccurrent.Next;
                        ccurrent.Element = acurrent.Element;
                        c+=1;
                        acurrent = acurrent.Next;
                    }
                }
            }
            ccurrent.Next = null;
            t = ccurrent;
        }
        #endregion
        
        /// <summary>
        /// Получить минимальный элемент множества.
        /// </summary>
        /// <returns>Первый элемент списка.</returns>
        public T Min(){
            return _head.Next.Element;
        }

        /// <summary>
        /// Получить максимальный элемент множества.
        /// </summary>
        /// <returns>Последний элемент списка.</returns>
        public T Max(){
            return _tail.Element;
        }
        
        public T DeleteMin(){
            if(_count == 0)
                return default(T);
            T val = _head.Next.Element;
            _head.Next = _head.Next.Next;
            return val;
        }
        
        /// <summary>
        /// Количество элементов множества.
        /// </summary>
        public Int32 Count {
            get{
                return _count;
            }
        }
        
        /// <summary>
        /// Текущее множество доступно для записи новых элементов (false).
        /// </summary>
        public bool IsReadOnly{
            get{
                return false;
            }
        }
        
        #region ISET
        /*
        public bool Add(T item){
            Int32 t = _count;
            Add(item);
            if(_count > t)
                return true;
            return false;
        }*/
        
        /// <summary>
        /// Определяет, содержит ли множества общие элементы.
        /// </summary>
        /// <param name="other">Другое множество на основе набора элементов.</param>
        /// <returns>true - если да, иначе false.</returns>
        public bool Overlaps(IEnumerable<T> other){
            SortedLinkedList<T> B = new SortedLinkedList<T>(other,_comp);
            SortedLinkedList<T> sll = new SortedLinkedList<T>(_comp);//C.
            __Intersection(_head,B._head,out sll._head,ref sll._tail,ref sll._count);
            return (sll.Count != 0);
        }

        /// <summary>
        /// Оставляет в текущем множестве только те элементы,
        /// которые есть в другом указанном множестве.
        /// </summary>
        /// <param name="other">Другое множество на основе набора элементов.</param>
        public void IntersectWith(IEnumerable<T> other){
            SortedLinkedList<T> B = new SortedLinkedList<T>(other,_comp);
            if(B.Count == 0){
                Clear();
                return;
            }
            ElementType<T> _nh;
            ElementType<T> _nt = new ElementType<T>();
            Int32 c = 0;
            __Intersection(_head,B._head,out _nh, ref _nt,ref c);
            this._count = 0;
            this._head = _nh;
            this._tail = _nt;
            this._count = c;
        }

        /// <summary>
        /// Добавляет в текущее множестве элементы из другого
        /// указанного множества.
        /// </summary>
        /// <param name="other">Другое множество на основе набора элементов.</param>
        public void UnionWith(IEnumerable<T> other){
            SortedLinkedList<T> B = new SortedLinkedList<T>(other,_comp);
            if(B.Count == 0){
                return;
            }
            ElementType<T> _nh;
            ElementType<T> _nt = new ElementType<T>();
            Int32 c = 0;
            __Union(_head,B._head,out _nh, ref _nt,ref c);
            this._count = 0;
            this._head = _nh;
            this._tail = _nt;
            this._count = c;
        }

        /// <summary>
        /// Оставляет в текущем множестве только те элементы,
        /// которых нет в другом указанном множестве.
        /// </summary>
        /// <param name="other">Другое множество на основе набора элементов.</param>
        public void ExceptWith(IEnumerable<T> other){
            SortedLinkedList<T> B = new SortedLinkedList<T>(other,_comp);
            if(B.Count == 0){
                return;
            }
            ElementType<T> _nh;
            ElementType<T> _nt = new ElementType<T>();
            Int32 c = 0;
            __Difference(_head,B._head,out _nh, ref _nt,ref c);
            this._count = 0;
            this._head = _nh;
            this._tail = _nt;
            this._count = c;
        }

        /// <summary>
        /// Оставляет в текущем множестве только те элементы,
        /// которые не содержатся одновременно в двух множествах (текущем и указанном)
        /// </summary>
        /// <param name="other">Другое множество на основе набора элементов.</param>
        public void SymmetricExceptWith(IEnumerable<T> other){
            SortedLinkedList<T> B = new SortedLinkedList<T>(other,_comp);
            if(B.Count == 0){
                return;
            }
            B = B.Difference(this);// B\A
            ExceptWith(other);// A\B
            
            ElementType<T> _nh;
            ElementType<T> _nt = new ElementType<T>();
            Int32 c = 0;
            
            __Union(_head,B._head,out _nh, ref _nt, ref c);
            this._count = c;
            this._head = _nh;
            this._tail = _nt;
        }

        /// <summary>
        /// Определяет, равны ли множества (текущее и указанное), т.е. содержат одни и те же элементы.
        /// </summary>
        /// <param name="other">Другое множество на основе набора элементов.</param>
        public bool SetEquals(IEnumerable<T> other){
            if(other == null){
                return false;
            }
            SortedLinkedList<T> B = new SortedLinkedList<T>(other,_comp);
            return Equals(B);
        }

        /// <summary>
        /// Определяет, является ли текущее множество подмножеством указанного.
        /// </summary>
        /// <param name="other">Другое множество на основе набора элементов.</param>
        public bool IsSubsetOf(IEnumerable<T> other){
            if(other.Count() == 0 && _count == 0){
                return true;
            }
            else if(other.Count() == 0 && _count != 0){
                return false;
            }
            SortedLinkedList<T> B = new SortedLinkedList<T>(other,_comp);
            ElementType<T> _nh;
            ElementType<T> _nt = new ElementType<T>();
            Int32 c = 0;
            __Difference(_head,B._head,out _nh, ref _nt,ref c);//A\B
            if(c == 0){
                return true;
            }
            return false;
        }

        /// <summary>
        /// Определяет, является ли текущее множество истинным подмножеством указанного (т.е. второе != первому множеству).
        /// </summary>
        /// <param name="other">Другое множество на основе набора элементов.</param>
        public bool IsProperSubsetOf(IEnumerable<T> other){
            if(other.Count() == 0){
                return false;
            }
            SortedLinkedList<T> B = new SortedLinkedList<T>(other,_comp);
            ElementType<T> _nh;
            ElementType<T> _nt = new ElementType<T>();
            Int32 c = 0;
            __Difference(_head,B._head,out _nh, ref _nt,ref c);
            if(c == 0 && B._count > _count){
                return true;
            }
            return false;
        }

        /// <summary>
        /// Определяет, является ли текущее множество надмножеством указанного.
        /// (Т.е. содержит ли в себе текущее набор элементов other).
        /// </summary>
        /// <param name="other">Другое множество на основе набора элементов.</param>
        public bool IsSupersetOf(IEnumerable<T> other){
            if(other.Count() == 0){
                return true;
            }
            SortedLinkedList<T> B = new SortedLinkedList<T>(other,_comp);
            ElementType<T> _nh;
            ElementType<T> _nt = new ElementType<T>();
            Int32 c = 0;
            __Difference(B._head,_head,out _nh, ref _nt,ref c);//B\A
            if(c == 0){
                return true;
            }
            return false;
        }

        /// <summary>
        /// Определяет, является ли текущее множество истинным надмножеством указанного.
        /// (Т.е. содержит ли в себе текущее набор элементов other и не является равным other).
        /// </summary>
        /// <param name="other">Другое множество на основе набора элементов.</param>
        public bool IsProperSupersetOf(IEnumerable<T> other){
            if(other.Count() == 0 && _count == 0){
                return false;
            }
            else if(other.Count() == 0){
                return true;
            }
            SortedLinkedList<T> B = new SortedLinkedList<T>(other,_comp);
            ElementType<T> _nh;
            ElementType<T> _nt = new ElementType<T>();
            Int32 c = 0;
            __Difference(B._head,_head,out _nh, ref _nt,ref c);//B\A
            if(c == 0 && _count > B._count){
                return true;
            }
            return false;
        }
        
        #endregion
        
        #region IMFSet
        public ISet<T> Merge(ISet<T> A, ISet<T> B){
            if(A.Overlaps(B)){
                return null;
            }
            ISet<T> C = new SortedLinkedList<T>();
            C.UnionWith(A);
            C.UnionWith(B);
            return C;
        }
        
        
        //Min
        public T First(){
            return _head.Next.Element;
        }
        
        public T Next(){
            return _count > 1 ? _head.Next.Next.Element : default(T);
        }
        
        #endregion
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        /// <summary>
        /// Получить итератор множества.
        /// </summary>
        /// <returns>Перечислитель элементов множества.</returns>
        public IEnumerator<T> GetEnumerator(){
            ElementType<T> p = _head.Next;
            while(p != null){
                yield return p.Element;
                p = p.Next;
            }
        }
        
        //EQUALS(A,B)
        /// <summary>
        /// Проверяет, что множества содержат одинаковые элементы.
        /// </summary>
        /// <param name="B">Второе множество.</param>
        /// <returns>true если множества равны, иначе false.</returns>
        public bool Equals(SortedLinkedList<T> B){
            if(B == null)
                return false;
            if(Intersection(B)._count == _count)
                return true;
            else
                return false;
        }
        
        ///<summary>Наследуется от Object.
        ///Определяет, эквиваленты ли два множества.</summary>
        public override bool Equals(Object obj){
            if(obj == null)
                return false;
            SortedLinkedList<T> list = obj as SortedLinkedList<T>;
            if(list == null)
                return false;
            else
                return Equals(list);
        }
        
        ///<summary>Строкове представление множества вида:
        ///{ 1 2 3 4 5 }</summary>
        public override String ToString(){
            StringBuilder sb = new StringBuilder();
            sb.Append("{ ");
            ElementType<T> p = _head.Next;
            while(p != null){
                sb.Append(p.Element.ToString()+" ");
                p = p.Next;
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}