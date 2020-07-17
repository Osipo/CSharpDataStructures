using System;
using System.Text;
using System.Collections.Generic;
using CSharpDataStructures.Structures.Trees.Visitors;
using STACK2 = CSharpDataStructures.Structures.Lists.Stack<System.Int32>;
namespace CSharpDataStructures.Structures.Trees {
    ///<summary>
    ///Дерево на основе массива ячеек типа NodeCell<typeparamref name="T"/>.
    ///Реализует интерфейсы ITree<typeparamref name="T"/>
    ///и IPositionalTree<typeparamref name="T"/>
    ///Массив динамический (может расширяться, 
    ///элементы храняться непрерывным блоком).
    ///Можно добавлять элементы на глубину с 1 до N
    ///Можно выбрать родителя нового листового узла среди узлов с предыдущего уровня.
    ///</summary>
    //Uses array of objects : (leftmostchild: int, rightsibling: int, parent: int, value: T, dept: int)
    //Array is dynamic.
    //You can add element to the tree from the 1 to the N dept.
    //You can choose parent of the tree on the dept-1.
    //This parameter is represented as np and it can be from the 0 to the N.
    public class ArrayTree<T> : ITree<T>, IMutableTree<T>, IPositionalTree<T> {
        private NodeCell<T>[] _cellspace;
        private Int32 _count;//
        private Int32 _capacity;
        private Int32 _dept;//->max height of the tree.
        private Int32 _mh;//min height of the tree.
        private Int32 _lastnode;
        private IVisitor<T> _visitor;
        #region .ctors
        
        ///<summary>Создать дерево с корнем на 1000 ячеек.</summary>
        public ArrayTree() : this(1000) {}
        ///<summary>Создать дерево с корнем на <c>capacity</c> ячеек.</summary>
        public ArrayTree(Int32 capacity){
            this._capacity = capacity;
            this._cellspace = new NodeCell<T>[capacity];
            this._count = 1;
            this._dept = 0;
            this._lastnode = 0;
            this._mh = 0;
            this._visitor = new NRVisitor<T>();
            _cellspace[0] = new NodeCell<T>(0){ //root.
                Value = default(T),
                LeftMostChild = -1,
                RightSibling = -1,
                Parent = -1
            };
        }
        
        ///<summary>Закрытый конструктор для создания поддеревьев.</summary>
        private ArrayTree(NodeCell<T>[] cells, Int32 c, Int32 ln, IVisitor<T> v){
            this._capacity = cells.Length * 3;
            this._cellspace = cells;
            this._count = c;
            this._dept = 0;
            this._lastnode = ln;
            this._mh = 0;
            this._visitor = v;
            __ComputeH();
        }
        
        #endregion 
       
      
        #region offset
        private Int32 __MoveChildren(NodeCell<T> n,Int32 i,NodeCell<T>[] nb,Int32 pp, CSharpDataStructures.Structures.Lists.Stack<NodeCell<T>> S, STACK2 S2,Int32 def_i){
            if(S.IsEmpty() && S2.IsEmpty()){
                return def_i;//work is over. stack is empty.
            }
            NodeCell<T> s = n;
            if(s.LeftMostChild == -1){//leaf found.
                S.Pop();
                S2.Pop();
                return def_i;//return last index of the last element of the nb.
            }
            s = _cellspace[s.LeftMostChild];
            i+=1;
            s.Parent = pp;
            n.LeftMostChild = i;
            nb[i] = s;
            S2.Pop();
            S.Pop();
            S.Push(nb[i]);//Pop Parent And Push his children.
            S2.Push(i);
            while(s != null && s.RightSibling != -1){//Align all children from old array in new array.
                i+=1;
                nb[i] = _cellspace[s.RightSibling];
                nb[i-1].RightSibling = i;
                nb[i-1].Parent = pp;
                nb[i].Parent = pp;
                S.Push(nb[i]);
                S2.Push(i);
                s = _cellspace[s.RightSibling];
            }
            return i;
        }
        
        
        private void __Move(){
            NodeCell<T>[] nb = new NodeCell<T>[_capacity];
            nb[0] = _cellspace[0];//ROOT
            Int32 k = 0;
            CSharpDataStructures.Structures.Lists.Stack<NodeCell<T>> S = new CSharpDataStructures.Structures.Lists.Stack<NodeCell<T>>(_capacity);
            STACK2 S2 = new STACK2(_capacity);
            S.Push(nb[0]);
            S2.Push(0);
            k = __MoveChildren(nb[0],0,nb,0,S,S2,k);
            
            
            while(k < _capacity && !S.IsEmpty() && !S2.IsEmpty()){
                Int32 idx = S2.Top();
                NodeCell<T> c = S.Top();
                k = __MoveChildren(c,k,nb,idx,S,S2,k);
                
            }
            for(Int32 ii = 0; ii < nb.Length; ii++){
                if(nb[ii] != null)
                    nb[ii].Idx = ii;
            }
            this._cellspace = null;
            this._cellspace = nb;
            this._lastnode = k;
        }
        #endregion
        
        private void __CheckCapacity(Int32 dest){
            if(dest >= _capacity){
                NodeCell<T>[] nb = new NodeCell<T>[_capacity * 2];
                this._capacity *= 2;
                for(Int32 i = 0; i < _cellspace.Length; i++){
                    nb[i] = _cellspace[i];
                    if(_cellspace[i] != null)
                        nb[i].Idx = i;
                }
                _cellspace = nb;
            }
        }
        
        //Height of the tree.
        private void __ComputeH(){
            Int32 q = 0;
            this._visitor.PreOrder(this,(n) => __CheckMaxDept(n,ref q));
            this._dept = q;
        }
        
        private void __ComputeMinH(){
            Int32 q = 0;
            this._visitor.PostOrder(this, (n) => __CheckMinDept(n,ref q));
            this._mh = q;
        }
        
        private void __CheckMaxDept(Node<T> n,ref Int32 q){
            Int32 k = ((NodeCell<T>) n).Dept;//REPLACED S1 -> S2.
            q = Math.Max(q,k);
        }
        
        private void __CheckMinDept(Node<T> n,ref Int32 q){
            NodeCell<T> nc = n as NodeCell<T>;
            if(nc.LeftMostChild == -1 && nc.Parent != -1){
                q = q == 0 ? nc.Dept : Math.Min(q,nc.Dept);
            }
        }
        
        
        
        private void __GetParent(Node<T> n, ref Int32 q, Int32 d, ref NodeCell<T> _pr){
            if(_pr != null){
                return;
            }
            q = ((NodeCell<T>) n).Dept;//REPLACED S1 -> S2.
            if(q == d - 1){
                _pr = n as NodeCell<T>;//REPLACED S1 -> S2.   
            }
        }
        
        //Assuming that delete operation iterates from the left end...
        private void __DeleteNode(Node<T> n){
            NodeCell<T> nc = n as NodeCell<T>;//REPLACE S1 -> S2
            NodeCell<T> p = _cellspace[nc.Parent];
            Int32 idx = p.LeftMostChild;
            NodeCell<T> ln = _cellspace[idx];
            //Console.WriteLine(nc.Value);
            if(nc.RightSibling != -1){
                p.LeftMostChild = nc.RightSibling;
            }
            else{
                p.LeftMostChild = -1;
            }
            _cellspace[idx] = null;
            _count -= 1;
        }
        
        //Delete leaf
        private void __DeleteLeaf(Node<T> n){
            NodeCell<T> p = Parent(n) as NodeCell<T>;
            NodeCell<T> c = _cellspace[p.LeftMostChild];
            if(c.Equals(n)){
                Int32 t = p.LeftMostChild;
                _count -=1;
                p.LeftMostChild = c.RightSibling;
                _cellspace[t] = null;
            }
            else{
                while(c.RightSibling != -1 && !_cellspace[c.RightSibling].Equals(n)){
                    c = _cellspace[c.RightSibling];
                }
                Int32 t = c.RightSibling == -1 ? c.Idx : c.RightSibling;
                c.RightSibling = -1;
                _cellspace[t] = null;
                _count -=1;
            }
        }
        
        private void __GetNodeByVal(Node<T> n,T val, ref Node<T> _c){
            if(_c != null){
                return;
            }
            T value = ((NodeCell<T>) n).Value;
            if(val.Equals(value)){
                _c = n;
            }
        }
        
        
        
        #region IMutableTree
        public void Add(T item){
            NodeCell<T> n = new NodeCell<T>(1);
            n.Value = item;
            n.LeftMostChild = -1;
            n.RightSibling = -1;
            Insert2(1,0,n);//add to the root.
        }
        
        public void Add(Int32 dept,T item){//HANDLE PARAMS
            NodeCell<T> n = new NodeCell<T>(dept);
            n.Value = item;
            n.LeftMostChild = -1;
            n.RightSibling = -1;
            Insert2(dept,0,n);//add to the first element of the dept-1 yield
        }
        
        public void Add(Int32 dept,Int32 np, T item){//HANDLE PARAMS
            NodeCell<T> n = new NodeCell<T>(dept); //np is cursor to the nodeparent located at the height == dept - 1
            n.Value = item;
            n.LeftMostChild = -1;
            n.RightSibling = -1;
            Insert2(dept,np,n);//Add to the np element of the dept-1 yield.
        }
        
        
        public void Insert2(Int32 dept,Int32 np, NodeCell<T> node){//MAKE PRIVATE
            if(dept <= 0){
                dept = 1;
            }
            if(np < 0){
                np = 0;
            }
            
            Int32 idxp = 0;
            NodeCell<T> parent = null;
            Int32 q = 0;
            this._visitor.PreOrder(this,(n) => __GetParent(n,ref q,dept,ref parent));
            q = 0;
            if(parent == null){
                Console.WriteLine("dept is out of range");
                return;
            }
            //Compute idxp and check np parameter.
            while(parent.RightSibling != -1 && q < np){
                idxp = parent.RightSibling;
                parent = _cellspace[parent.RightSibling];
                q+=1;
            }
            if(q < np){
                Console.WriteLine("parent in the dept {0} is not exists. Parameter np {1} is out of range",dept,q);
                np = q;//IDXP has been computed as the rightmost element in (dept - 1)th level.
                //return;
            }
            if(Parent(parent) == null){
                idxp = 0;//root.
            }
            else if(np == 0){ //get idxp for np == 0
                
                NodeCell<T> cont = (NodeCell<T>) Parent(parent);
                idxp = cont.LeftMostChild;
                /*
                cont = _cellspace[idxp];
                while(cont.RightSibling != -1 && !cont.Equals(parent)){//Compute idxp for np.
                    idxp = cont.RightSibling;
                    cont = _cellspace[idxp];
                }*/
            }
            this._lastnode = this._lastnode + 1;
            this._count += 1;
            __CheckCapacity(_lastnode);
            
            //parent without any child.
            if(parent.LeftMostChild == -1){
                parent.LeftMostChild = _lastnode;//make as the first child.
                node.Parent = idxp;
                node.Idx = _lastnode;
                _cellspace[_lastnode] = node;
            }
            
            //parent with single child.
            else if(_cellspace[parent.LeftMostChild].RightSibling == -1){
                _cellspace[parent.LeftMostChild].RightSibling = _lastnode;//make as a right brother.
                node.Parent = idxp;
                node.Idx = _lastnode;
                _cellspace[_lastnode] = node;
            }
            else{//parent with children.
                NodeCell<T> qq = _cellspace[_cellspace[parent.LeftMostChild].RightSibling];//second son.
                while(qq.RightSibling != -1){//while second son has right brothers...
                    qq = _cellspace[qq.RightSibling];//move to this brother, and check whether it has another right brother.
                }
                node.Parent = idxp;//qq.Parent -> idxp.
                qq.RightSibling = _lastnode;//add new brother to children.
                node.Idx = _lastnode;
                _cellspace[_lastnode] = node;
            }
            __ComputeH();
        }
        
        
        #region IPositionalTree
        //USE INTERFACE ITree<T> for Adding.
        ///<summary>Добавить к указанному узлу дерева новый узел.</summary>
        ///<param name="p">Родитель нового узла</param>
        ///<param name="item">Содержимое нового узла.</param>
        public void AddTo(Node<T> p, T item){
            NodeCell<T> pr = p as NodeCell<T>;
            if(p == null){
                //Console.WriteLine("error");
                return;
            }
            Int32 idxp = 0;
            if(pr.Parent != -1){
                NodeCell<T> cont = (NodeCell<T>) Parent(p);
                idxp = cont.LeftMostChild;
                cont = _cellspace[idxp];
                while(cont.RightSibling != -1 && !cont.Equals(p)){//Compute idxp for np.
                    idxp = cont.RightSibling;
                    cont = _cellspace[idxp];
                }
            }
            this._lastnode = this._lastnode + 1;
            this._count += 1;
            __CheckCapacity(_lastnode);
            
            
            if(pr.LeftMostChild != -1){
                
                NodeCell<T> rb = _cellspace[pr.LeftMostChild];
                while(rb != null && rb.RightSibling != -1){
                    rb = _cellspace[rb.RightSibling];
                }
                rb.RightSibling = _lastnode;
            }
            else{
                pr.LeftMostChild = _lastnode;
            }
            NodeCell<T> n = new NodeCell<T>(pr.Dept + 1);
            n.Value = item;
            n.LeftMostChild = -1;
            n.RightSibling = -1;
            n.Parent = idxp;
            n.Idx = _lastnode;
            _cellspace[_lastnode] = n;
        }
        
        ///<summary>Получить последнего сына указанного узла дерева.</summary>
        public Node<T> RightMostChild(Node<T> n){
            NodeCell<T> c = n as NodeCell<T>;
            if(c == null || c.LeftMostChild == -1)
                return null;
            c = _cellspace[c.LeftMostChild];
            while(c.RightSibling != -1){
                c = _cellspace[c.RightSibling];
            }
            return c;
        }
        
        ///<summary>Получить список детей указанного узла дерева.</summary>
        public IList<Node<T>> GetChildren(Node<T> n){
            /*
            Node<T> c = LeftMostChild(n);
            List<Node<T>> l = new List<Node<T>>();
            while(c != null){
                l.Add(c);
                c = RightSibling(c);
            }*/
            
            NodeCell<T> c = n as NodeCell<T>;
            
            List<Node<T>> l = new List<Node<T>>();
            if(c.LeftMostChild == -1){
                return l;
            }
            c = _cellspace[c.LeftMostChild];
            l.Add(c);
            while(c.RightSibling != -1){
                c = _cellspace[c.RightSibling];
                l.Add(c);
            }
            return l;
        }
        
        ///<summary>Вызвать посетителя для обхода всего дерева
        ///в указаном режиме order(VisitorMode:: PRE,POST IN)</summary>
        public void Visit(VisitorMode order, Action<Node<T>> act){
            switch(order){
                case VisitorMode.PRE:
                    _visitor.PreOrder(this,act);
                    break;
                case VisitorMode.POST:
                    _visitor.PostOrder(this,act);
                    break;
                case VisitorMode.IN:
                    _visitor.InOrder(this,act);
                    break;
                default:
                    break;
            }
        }
        
        ///<summary>Получить поддерево с корнем n узла дерева</summary>
        ///<param name="n">Корень поддерева. Является узлом дерева.</param>
        //ERROR IN ARRAYS.
        public IPositionalTree<T> GetSubTree(Node<T> n){
            NodeCell<T> nc = n as NodeCell<T>;
            List<Int32> idxs = new List<Int32>();
            _visitor.PreOrder(this,n, (p) => __GetSubTreeIdxs(p, ref idxs));
            NodeCell<T>[] ar = new NodeCell<T>[_count];
            for(Int32 i = 0; i < idxs.Count; i++){
                ar[idxs[i]] = _cellspace[idxs[i]];
            }
            Int32 ln = idxs[idxs.Count - 1];
            Int32 co = idxs.Count;
            Int32 offset = ar[idxs[0]].Dept;
            for(Int32 i = 0; i < idxs.Count; i++){
                ar[idxs[i]].Dept = ar[idxs[i]].Dept - offset;
            }
            
            ar[0] = ar[idxs[0]];//move sub_root to the root position.
            ar[0].Parent = -1;
            ar[idxs[0]].Parent = -1;
            IList<Node<T>> rc = GetChildren(ar[0]);
            for(Int32 ii = 0; ii < rc.Count; ii++){
                ((NodeCell<T>) rc[ii]).Parent = 0;
            }
            return new ArrayTree<T>(ar,co,ln,_visitor);
        }
        
        
        ///<summary>Сохранить в списке idxs индексы узлов 
        ///в массиве ячеек дерева (_cellspace)</summary>
        private void __GetSubTreeIdxs(Node<T> p, ref List<Int32> idxs){
            NodeCell<T> c = p as NodeCell<T>;
            idxs.Add(c.Idx);
        }
        
        #endregion
        
        //Delete all nodes with Parent n.
        public void Delete(Node<T> n){
            _visitor.PostOrder(this,n,(x) => __DeleteNode(x));//delete node with all children.
            __ComputeH();
            __Move();
        }
        
        public void Delete(T item){//Delete first node with item and remove its children.
            Node<T> wflag = null;
            _visitor.PreOrder(this,(n) => __GetNodeByVal(n,item,ref wflag));//Contains(T item)
            if(wflag == null){
                Console.WriteLine("Not found");
                return;
            }
            if(LeftMostChild(wflag) == null){
                __DeleteLeaf(wflag);
                __ComputeH();
                __Move();
            }
            else{
                Delete(wflag);
            }
        }
        #endregion
        
        public Node<T> GetNode(T val){
            Node<T> wflag = null;
            _visitor.PreOrder(this,(n) => __GetNodeByVal(n,val,ref wflag));
            return wflag;
        }
        
        #region ITree
        ///<summary>Корень дерева</summary>
        public Node<T> Root(){
            return _cellspace[0];
        }
        
        ///<summary>Содержимое указанного узла</summary>
        public T Value(Node<T> node){
            return node.Value;
        }
        
        ///<summary>Родитель узла. (NodeCell<typeparamref name="T"/>)</summary>
        public Node<T> Parent(Node<T> node){
            NodeCell<T> np = node as NodeCell<T>;
            if(np == null || np.Parent == -1){
                return null;
            }
            return _cellspace[np.Parent];
        }
        
        ///<summary>Первый сын узла (NodeCell<typeparamref name="T"/>)</summary>
        public Node<T> LeftMostChild(Node<T> node){
            NodeCell<T> np = node as NodeCell<T>;
            if(np == null || np.LeftMostChild == -1){
                return null;
            }
            return _cellspace[np.LeftMostChild];
        }
        
        ///<summary>Правый брат узла (NodeCell<typeparamref name="T"/>)</summary>
        public Node<T> RightSibling(Node<T> node){
            NodeCell<T> np = node as NodeCell<T>;
            if(np == null || np.RightSibling == -1){
                return null;
            }
            return _cellspace[np.RightSibling];
        }
        
        ///<summary>Количество узлов дерева</summary>
        public Int32 GetCount(){
            return this._count;
        }
        
        ///<summary>Задать нового посетителя этого дерева.</summary>
        public void SetVisitor(IVisitor<T> visitor){
            this._visitor = visitor;
        }
        
        //MAKENULL
        ///<summary>Удалить все узлы дерева. (Корень сохранится, но не будет ничего содержать)</summary>
        public void Clear(){
            for(Int32 i = 1; i < _cellspace.Length; i++){
                _cellspace[i] = null;
            }
            this._count = 1;
            this._dept = 0;
            this._lastnode = 0;
            _cellspace[0] = new NodeCell<T>(0){ //root.
                Value = default(T),
                LeftMostChild = -1,
                RightSibling = -1,
                Parent = -1
            };
        }
        
        #endregion      
        #region ArrayTree
        public Int32 MinHeight{
            get{
                __ComputeMinH();
                return _mh;
            }
        }
        
        public void PrintContent(){
            for(Int32 i = 0; i < _cellspace.Length;i++){
                if(_cellspace[i] == null)
                    continue;
                Console.Write(_cellspace[i].Value+" : {0}; ",i);
            }
            Console.WriteLine("");
        }
        
        ///<summary>Преобразовать в строку, со скобками.
        ///Скобки указывают вложенность узлов. Сама строка окружена фигурными скобками.</summary>
        public override String ToString(){
            StringBuilder sb = new StringBuilder();
            HashSet<Node<T>> hs = new HashSet<Node<T>>();
            CSharpDataStructures.Structures.Lists.LinkedStack<Node<T>> STACK =
                new CSharpDataStructures.Structures.Lists.LinkedStack<Node<T>>();
            Node<T> n;
            STACK.Push(Root());//_cellspace[0]
            while(!STACK.IsEmpty()){
                n = STACK.Top();
                if(hs.Contains(n)) {
                    STACK.Pop();
                    sb.Append("}");
                } else {
                    hs.Add(n);
                    sb.Append("{" + n.Value.ToString());
                    
                    IList<Node<T>> children = GetChildren(n);
                    for(Int32 c = children.Count - 1; c >= 0; c--){
                        STACK.Push(children[c]);
                    }
                }
            }
            return sb.ToString();
        }
        
        
        
        public Int32 Height{
            get{
                return _dept;
            }
        }
        #endregion
    }
}