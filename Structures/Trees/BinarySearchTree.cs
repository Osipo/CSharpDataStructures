using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CSharpDataStructures.Structures.Trees.Visitors;
using CSharpDataStructures.Structures.Lists;
using CSharpDataStructures.Structures.Sets;
namespace CSharpDataStructures.Structures.Trees {
    //BTS type for representing SETS
    public class BinarySearchTree<T> : ITree<T>, ISet<T>, IEquatable<BinarySearchTree<T>>, IEnumerable<T>, ICollection<T> {
        private LinkedBinaryNode<T> _r;
        private Int32 _count;
        private Int32 _h;
        private IVisitor<T> _visitor;
        private IComparer<T> _comp;
        
        public BinarySearchTree(IComparer<T> comp){
            this._comp = comp;
            this._count = 0;
            this._h = 0;
            this._r = null;
            this._visitor = new BinaryNRVisitor<T>();//CHECK
        }
        public BinarySearchTree() : this(Comparer<T>.Default) {}
        
        
        
        //INSERT
        private void __Add(T item, LinkedBinaryNode<T> node){
            if(_count == 0){
                _r = new LinkedBinaryNode<T>();
                _r.Dept = 0;
                _r.Value = item;
                _count += 1;
                return;
            }
            while(node != null){
                if(_comp.Compare(item,node.Value) < 0){
                    if(node.IsLeftLeaf){//REPLACE
                        node.LeftSon = new LinkedBinaryNode<T>();
                        node.LeftSon.Value = item;
                        _count += 1;
                        return;
                    }
                    node = node.LeftSon;
                }
                else if(_comp.Compare(item,node.Value) > 0){
                    if(node.IsRightLeaf){//REPLACE
                        node.RightSon = new LinkedBinaryNode<T>();
                        node.RightSon.Value = item;
                        _count += 1;
                        return;
                    }
                    node = node.RightSon;
                }
            }
        }
        
        //MEMBER
        private Boolean __Member(T item,LinkedBinaryNode<T> node){
            if(_count == 0){
                return false;
            }
            while(node != null){
                if(_comp.Compare(item,node.Value) == 0 || node.Value.Equals(item)){
                    return true;
                }
                else if(_comp.Compare(item,node.Value) < 0){
                    node = node.LeftSon;
                }
                else {
                    node = node.RightSon;
                }
            }
            return false;
        }
        
        private T __DeleteMin(LinkedBinaryNode<T> node){
            LinkedBinaryNode<T> p = _r;
            if(_count == 0){
                return default(T);
            }
            while(node.LeftSon != null){
                p = node;
                node = node.LeftSon;
            }
            T m = node.Value;
            p.LeftSon = node.RightSon;
            _count -= 1;
            return m;
        } 
        
        private void __Delete(T item, LinkedBinaryNode<T> node){
            while(node != null){
                if(_comp.Compare(item, node.Value) < 0){
                    node = node.LeftSon; //DELETE(node.LeftSon)
                }
                else if(_comp.Compare(item, node.Value) > 0){
                    node = node.RightSon;//DELETE(node.RightSon)
                }
                else if(node.LeftSon == null && node.RightSon == null){
                    node = null;
                    _count -=1;
                    return;
                }
                else if(node.LeftSon == null){
                    node = node.RightSon;
                    _count -= 1;
                    return;
                }
                else if(node.RightSon == null){
                    node = node.LeftSon;
                    _count -= 1;
                    return;
                }
                else {
                    node.Value = __DeleteMin(node.RightSon);
                    return;
                }
            }
        }
        
        //DO: DELETE(MAKE ptr IN Node) OR REPLACE
        private LinkedBinaryNode<T> __GetParent(LinkedBinaryNode<T> c){
            if(c == null)
                return null;
            LinkedBinaryNode<T> node = _r;
            LinkedBinaryNode<T> p = null;
            if(node != null){
                if(_comp.Compare(c.Value,node.Value) == 0){
                    return null;//PARENT OF ROOT ISNULL
                }
            }
            while(node != null){
                if(_comp.Compare(c.Value,node.Value) < 0){
                    p = node;
                    node = node.LeftSon;
                    if(node.Value.Equals(c.Value)){
                        return p;
                    }
                }
                else{
                    p = node;
                    node = node.RightSon;
                    if(node.Value.Equals(c.Value)){
                        return p;
                    }
                }
            }
            return null;
        }
        
        public Boolean Contains(T item){
            return __Member(item,_r);//BEGIN WITH ROOT
        }
        
        
        public bool Add(T item){
            if(Contains(item)){
                return false;
            }
           __Add(item,_r);
           return true;
        }
        
        
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
        
        public void AddRange(IEnumerable<T> items){
            if(__Sorted(items)){//FOR SORTED O(N)
                LinkedStack<T> S1 = new LinkedStack<T>();
                LinkedStack<T> S2 = new LinkedStack<T>();
                
                IEnumerator<T> e1 = items.Take(items.Count()/2).GetEnumerator();
                IEnumerator<T> e2 = items.Skip(items.Count()/2).GetEnumerator();
                while(e1.MoveNext() && e2.MoveNext()){
                    S1.Push(e1.Current);
                    S2.Push(e2.Current);
                }
                
                while( !S1.IsEmpty() && !S2.IsEmpty()){
                    Add(S1.Top());
                    Add(S2.Top());
                    S1.Pop();
                    S2.Pop();
                }
                S1 = null;
                S2 = null;
                e1 = null;
                e2 = null;
                return;
            }
            foreach(var item in items){
                Add(item);
            }
        }
       
        public T DeleteMin(){
            return __DeleteMin(_r);
        }
        
        public void Delete(T item){
            if(__Member(item,_r)){
                __Delete(item,_r);
            }
        }
        
        
        #region SetOperators
        
        public BinarySearchTree<T> Intersection(BinarySearchTree<T> B){
            ITree<T> T2 = (ITree<T>) B;
            BinarySearchTree<T> C = new BinarySearchTree<T>(_comp);
            
            LinkedStack<T> S1 = new LinkedStack<T>();//CHECK FOR S = Stack<T> 
            LinkedStack<T> S2 = new LinkedStack<T>();
            
            this._visitor.InOrder(this,(n) => __Nodes(n,ref S1));//SYMMETRIC ORDER (INORDER)
            this._visitor.InOrder(T2,(n) => __Nodes(n,ref S2));
            while( !S1.IsEmpty() && !S2.IsEmpty()){
                if(_comp.Compare(S1.Top(), S2.Top()) == 0){
                    C.Add(S1.Top());
                    S1.Pop();
                    S2.Pop();
                }
                else if(_comp.Compare(S1.Top(), S2.Top()) < 0){
                    S2.Pop();
                }
                else{
                    S1.Pop();
                }
            }
            
            return C;//CHANGE TYPE -> MAKE NEW METHOD
        }
        
        public BinarySearchTree<T> Union(BinarySearchTree<T> B){
            ITree<T> T2 = (ITree<T>) B;
            BinarySearchTree<T> C = new BinarySearchTree<T>(_comp);
            
            LinkedStack<T> S1 = new LinkedStack<T>();//CHECK FOR S = Stack<T> 
            LinkedStack<T> S2 = new LinkedStack<T>();
            
            this._visitor.InOrder(this,(n) => __Nodes(n,ref S1));//SYMMETRIC ORDER (INORDER)
            this._visitor.InOrder(T2,(n) => __Nodes(n,ref S2));
            
            Int32 l1 = S1.Count;
            Int32 l2 = S2.Count;//1,2,3 12,22,44 --  1  12
            LinkedStack<T> S3 = new LinkedStack<T>();
            LinkedStack<T> S4 = new LinkedStack<T>();
            Int32 i1 = 0;
            Int32 i2 = 0;
            while(i1 < l1/2 + 1){
                S3.Push(S1.Top());
                S1.Pop();
                i1++;
            }
            while(i2 < l2/2 + 1){
                S4.Push(S2.Top());
                S2.Pop();
                i2++;
            }
            if(_comp.Compare(S3.Top(), S4.Top()) < 0){
                C.Add(S4.Top());
                S4.Pop();
            }
            else{
                C.Add(S3.Top());
                S3.Pop();
            }
            while( !S1.IsEmpty()){
                C.Add(S1.Top());
                S1.Pop();
            }
            while( !S2.IsEmpty()){
                C.Add(S2.Top());
                S2.Pop();
            }
            while( !S3.IsEmpty()){
                C.Add(S3.Top());
                S3.Pop();
            }
            while( !S4.IsEmpty()){
                C.Add(S4.Top());
                S4.Pop();
            }
            return C;
        }
        
        public BinarySearchTree<T> Difference(BinarySearchTree<T> B){
            ITree<T> T2 = (ITree<T>) B;
            BinarySearchTree<T> C = new BinarySearchTree<T>(_comp);
            
            LinkedStack<T> S1 = new LinkedStack<T>();//CHECK FOR S = Stack<T> 
            LinkedStack<T> S2 = new LinkedStack<T>();
            
            this._visitor.InOrder(this,(n) => __Nodes(n,ref S1));//SYMMETRIC ORDER (INORDER)
            this._visitor.InOrder(T2,(n) => __Nodes(n,ref S2));
            bool flag = true;
            while( !S1.IsEmpty()){
                if(flag && S2.IsEmpty()){
                    flag = false;
                }
                if(flag && _comp.Compare(S1.Top(), S2.Top()) == 0){
                    S1.Pop();
                    S2.Pop();
                }
                else if(flag && _comp.Compare(S1.Top(), S2.Top()) > 0){
                    C.Add(S1.Top());
                    S1.Pop();
                }
                else{
                    if(flag){
                        S2.Pop();
                    }
                    else{
                        C.Add(S1.Top());
                        S1.Pop();
                    }
                }
            }
            
            return C;
        }
        
        public BinarySearchTree<T> SymmetricDifference(BinarySearchTree<T> B){
            BinarySearchTree<T> ANB = Difference(B);//A\B
            BinarySearchTree<T> BNA = B.Difference(this);//B\A
            return ANB.Union(BNA);
        }
        
        public BinarySearchTree<T> Merge(BinarySearchTree<T> B){
            if(Intersection(B)._count != 0){
                return null;//UNDEFINED
            }
            return Union(B);
        }
        
        
        //DESC ORDER
        private void __Nodes(Node<T> a, ref LinkedStack<T> S){
            S.Push(a.Value);
        }
        //ASC ORDER
        private void __Nodes(Node<T> a, ref CSharpDataStructures.Structures.Lists.LinkedList<T> L){
            L.Add(a.Value);
        }
        #endregion
        
        
        #region ISet
        
        public bool Overlaps(IEnumerable<T> other){
            BinarySearchTree<T> B = new BinarySearchTree<T>(_comp);
            B.AddRange(other);
            B = Intersection(B);//A AND B
            
            return B._count != 0;
        }
        
        public void IntersectWith(IEnumerable<T> other){
            BinarySearchTree<T> B = new BinarySearchTree<T>(_comp);
            B.AddRange(other);
            B = Intersection(B);
            if(B._count == 0){
                Clear();
                return;
            }
            Clear();
            AddRange((IEnumerable<T>) B);
            
        }
        
        public void UnionWith(IEnumerable<T> other){
            AddRange(other);
        }
        
        public void ExceptWith(IEnumerable<T> other){
            if(other.Count() == 0){
                return;
            }
            BinarySearchTree<T> B = new BinarySearchTree<T>(_comp);
            B.AddRange(other);
            B = Difference(B);
            Clear();
            AddRange((IEnumerable<T>) B);
        }
        
        public void SymmetricExceptWith(IEnumerable<T> other){
            if(other.Count() == 0){
                return;
            }
            BinarySearchTree<T> B = new BinarySearchTree<T>(_comp);
            B.AddRange(other);
            B = SymmetricDifference(B);
            Clear();
            AddRange((IEnumerable<T>) B);
        }
        
        public bool SetEquals(IEnumerable<T> other){
            if(other == null){
                return false;
            }
            BinarySearchTree<T> B = new BinarySearchTree<T>(_comp);
            B.AddRange(other);
            return Equals(B);
        }
        
        public bool IsSubsetOf(IEnumerable<T> other){
            if(other.Count() == 0 && _count == 0){
                return true;
            }
            else if(other.Count() == 0 && _count != 0){
                return false;
            }
            BinarySearchTree<T> B = new BinarySearchTree<T>(_comp);
            B.AddRange(other);
            B = Difference(B);//A\B.
            Int32 c = B._count;
            if(c == 0){
                return true;
            }
            return false;
        }
        
        public bool IsProperSubsetOf(IEnumerable<T> other){
            if(other.Count() == 0){
                return false;
            }
            BinarySearchTree<T> B = new BinarySearchTree<T>(_comp);
            B.AddRange(other);
            BinarySearchTree<T> C = Difference(B);//A\B.
            Int32 c = C._count;
            if(c == 0 && B._count > _count){
                return true;
            }
            return false;
        }
        
        public bool IsSupersetOf(IEnumerable<T> other){
            if(other.Count() == 0){
                return true;
            }
            BinarySearchTree<T> B = new BinarySearchTree<T>(_comp);
            B.AddRange(other);
            BinarySearchTree<T> C = B.Difference(this);//B\A.
            Int32 c = C._count;
            if(c == 0){
                return true;
            }
            return false;
        }
        
        public bool IsProperSupersetOf(IEnumerable<T> other){
            if(other.Count() == 0 && _count == 0){
                return false;
            }
            else if(other.Count() == 0){
                return true;
            }
            BinarySearchTree<T> B = new BinarySearchTree<T>(_comp);
            B.AddRange(other);
            BinarySearchTree<T> C = B.Difference(this);//B\A.
            Int32 c = C._count;
            if(c == 0 && B._count < _count){
                return true;
            }
            return false;
        }
        
        #endregion
        
        #region ITree
        public Node<T> Root(){
            return _r;
        }
        
        public T Value(Node<T> node){
            return node.Value;
        }
        
        
        public Node<T> Parent(Node<T> node){
            LinkedBinaryNode<T> np = node as LinkedBinaryNode<T>;
            if(np == null){//OR np.Parent
                return null;
            }
            //return np.Parent;
            return __GetParent(np);//OR np.Parent
        }
        
        public Node<T> LeftMostChild(Node<T> node){
            LinkedBinaryNode<T> np = node as LinkedBinaryNode<T>;
            if(np == null){
                return null;
            }
            return np.LeftSon;
        }
        
        
        public Node<T> RightSibling(Node<T> node){
            LinkedBinaryNode<T> np = node as LinkedBinaryNode<T>;
            //np = np.Parent;
            np = __GetParent(np);//OR np.Parent;
            if(np == null || np.RightSon == null || np.RightSon.Value.Equals(node.Value)){//ROOT HAS NO ONE BROTHERS.
                return null;
            }
            return np.RightSon;
        }
        
        public Int32 GetCount(){
            return this._count;
        }
        
        public void SetVisitor(IVisitor<T> visitor){
            this._visitor = visitor;
        }
        
        //MAKENULL
        public void Clear(){
            this._count = 0;
            this._h = 0;
            this._r = null;
        }
        
        #endregion
        
        
        public Boolean Equals(BinarySearchTree<T> B){
            if(B == null)
                return false;
            if(Intersection(B)._count == _count)
                return true;
            else
                return false;
        }
        
        public override Boolean Equals(Object obj){
            if(obj == null)
                return false;
            BinarySearchTree<T> B = obj as BinarySearchTree<T>;
            if(B == null)
                return false;
            else
                return Equals(B);
        }
        
        public override String ToString(){
            StringBuilder sb = new StringBuilder();
            sb.Append("{ ");
            if(_count == 0){
                sb.Append("}");
                return sb.ToString();
            }
            this._visitor.InOrder(this,(n) => __PrintVal(n,ref sb));//sorted.
            sb.Append("}");
            return sb.ToString();
        }
        
        private void __PrintVal(Node<T> n,ref StringBuilder sb){
            sb.Append(n.Value.ToString()+" ");
        }
        
        
        
       
        
        public T Min(){
            LinkedBinaryNode<T> p = _r;
            LinkedBinaryNode<T> node = _r;
            while(node.LeftSon != null){
                p = node;
                node = node.LeftSon;
            }
            T m = node.Value;
            return m;
        }
        
        public T Max(){
            LinkedBinaryNode<T> p = _r;
            LinkedBinaryNode<T> node = _r;
            while(node.RightSon != null){
                p = node;
                node = node.RightSon;
            }
            T m = node.Value;
            return m;
        }
        
        
        //Height of the tree.
        
        private void __ComputeH(){
            Int32 q = 0;
            this._visitor.PreOrder(this,(n) => __CheckDept(n,ref q));
            this._h = q;
        }
        
        //REPLACE
        private void __CheckDept(Node<T> n,ref Int32 q){
            Int32 k = ((LinkedBinaryNode<T>) n).Dept;//REPLACED S1 -> S2.
            q = Math.Max(q,k);
        }
        
        public Int32 Height{
            get{
                return _h;
            }
        }
        
        #region ICollection
        public Int32 Count{
            get{
                return _count;
            }
        }
        
        public bool IsReadOnly{
            get{
                return false;
            }
        }
        
        
        void ICollection<T>.Add(T item){
            bool f = Add(item);
        }
        
        public bool Remove(T item){
            if(__Member(item,_r)){
                __Delete(item,_r);
                return true;
            }
            return false;
        }
        
        public void CopyTo(T[] array,Int32 arrayIndex){
            CSharpDataStructures.Structures.Lists.LinkedList<T> L = new CSharpDataStructures.Structures.Lists.LinkedList<T>();
            this._visitor.InOrder(this,(n) => __Nodes(n,ref L));//SYMMETRIC ORDER (INORDER)
            L.CopyTo(array,arrayIndex);
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public IEnumerator<T> GetEnumerator(){
            CSharpDataStructures.Structures.Lists.LinkedList<T> L = new CSharpDataStructures.Structures.Lists.LinkedList<T>();
            this._visitor.InOrder(this,(n) => __Nodes(n,ref L));//SYMMETRIC ORDER (INORDER)
            return L.GetEnumerator();
        }
        #endregion
        //O(N)
        public SortedLinkedList<T> ToSortedList(){
            CSharpDataStructures.Structures.Lists.LinkedList<T> L = new CSharpDataStructures.Structures.Lists.LinkedList<T>();
            this._visitor.InOrder(this,(n) => __Nodes(n,ref L));//SYMMETRIC ORDER (INORDER)
            return new SortedLinkedList<T>((IEnumerable<T>) L,_comp);
        }
        
        #region Convertations ops
        
        public static implicit operator SortedLinkedList<T>(BinarySearchTree<T> A){
            CSharpDataStructures.Structures.Lists.LinkedList<T> L = new CSharpDataStructures.Structures.Lists.LinkedList<T>();
            A._visitor.InOrder(A,(n) => A.__Nodes(n,ref L));//SYMMETRIC ORDER (INORDER)
            return new SortedLinkedList<T>((IEnumerable<T>) L,A._comp);
        }
        
        public static implicit operator BinarySearchTree<T>(SortedLinkedList<T> L){
            BinarySearchTree<T> TREE = new BinarySearchTree<T>();
            if(L.Count == 0){
                return TREE;
            }
            else if(L.Count < 4){
                T r = L[1];
                TREE.Add(r);
                TREE.Add(L[0]);
                TREE.Add(L[2]);
                return TREE;
            }
            SortedLinkedList<T> L1 = L.GetRange(0,L.Count/2 - 1);//0..1 -> 2..3
            SortedLinkedList<T> L2 = L.GetRange(L.Count/2,L.Count - 1);//R = last_elem
            
            LinkedStack<T> S1 = new LinkedStack<T>();
            LinkedStack<T> S2 = new LinkedStack<T>();
            IEnumerator<T> e1 = L1.GetEnumerator();
            IEnumerator<T> e2 = L2.GetEnumerator();
            while(e1.MoveNext() && e2.MoveNext()){
                S1.Push(e1.Current);
                S2.Push(e2.Current);
            }
            
            while( !S1.IsEmpty() && !S2.IsEmpty()){
                TREE.Add(S1.Top());
                TREE.Add(S2.Top());
                S1.Pop();
                S2.Pop();
            }
            S1 = null;
            S2 = null;
            e1 = null;
            e2 = null;
            L1 = null;
            L2 = null;
            return TREE;
        }
        
        #endregion
    }
}