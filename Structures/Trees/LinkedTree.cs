using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using CSharpDataStructures.Structures.Trees.Visitors;
namespace CSharpDataStructures.Structures.Trees {
    ///<summary>
    ///Дерево на основе связанных списков сыновей узлов.
    ///Реализует интерфейсы ITree<typeparamref name="T"/>
    ///и IPositionalTree<typeparamref name="T"/>
    ///</summary>
    public class LinkedTree<T> : ITree<T>, IPositionalTree<T> {
        private Int32 _count;
        private LinkedNode<T> _r;
        private IVisitor<T> _visitor;
        
        #region .ctors
        ///<summary>Создать пустое дерево (с корнем без содержимого)</summary>
        public LinkedTree(){
            _r = new LinkedNode<T>();
            _visitor = new NRVisitor<T>();
            _count = 1;
        }
        ///<summary>Создать дерево с указанным корнем.</summary>
        public LinkedTree(LinkedNode<T> n){
            n.Parent = null;
            _r = n;
            _visitor = new NRVisitor<T>();
            _count = 0;
            __ComputeC(ref _count);
        }
        
        //Compute new count for new sub_tree.
        private void __ComputeC(ref Int32 nc){
            HashSet<Node<T>> hs = new HashSet<Node<T>>();
            CSharpDataStructures.Structures.Lists.LinkedStack<Node<T>> STACK =
                new CSharpDataStructures.Structures.Lists.LinkedStack<Node<T>>();
            Node<T> n;
            STACK.Push(Root());
            while(!STACK.IsEmpty()){
                n = STACK.Top();
                if(hs.Contains(n)) {
                    STACK.Pop();
                } else {
                    hs.Add(n);
                    nc++;
                    IList<Node<T>> children = GetChildren(n);
                    for(Int32 c = children.Count - 1; c >= 0; c--){
                        STACK.Push(children[c]);
                    }
                }
            }
        }
        
        private void __GetCountOf(Node<T> n, ref Int32 c){
            c+=1;
        }
        
        #endregion
        
        ///<summary>Добавить новый узел с содержимым item
        ///к самому левому листу дерева.
        ///</summary>
        public void Add(T item){
            LinkedNode<T> n = _r;
            while(n.Children.Count != 0){
                n = n.Children[0];//LEFTMOST_CHILD
            }
            AddTo(n,item);
        }
        
        ///<summary>Добавить новый узел с содержимым item
        ///к указанному узлу дерева.
        ///</summary>
        public void AddTo(Node<T> n, T item){
            LinkedNode<T> p = n as LinkedNode<T>;
            LinkedNode<T> it = new LinkedNode<T>();
            it.Value = item;
            it.Parent = p;
            _count+= 1;
            p.Children.Add(it);
        }
        
        ///<summary>Получить последнего сына указанного узла.</summary>
        public Node<T> RightMostChild(Node<T> n){
            LinkedNode<T> c = n as LinkedNode<T>;
            if(c == null || c.Children.Count == 0)
                return null;
            return c.Children[c.Children.Count - 1];
        }
        
        ///<summary>Получить список детей указанного узла.</summary>
        public IList<Node<T>> GetChildren(Node<T> n){
            LinkedNode<T> c = n as LinkedNode<T>;
            List<Node<T>> l = new List<Node<T>>();
            for(Int32 i = 0; i < c.Children.Count; i++){
                l.Add(c.Children[i]);
            }
            return l;
        }
       
        ///<summary>Пройтись по всему дереву в указанном порядке order.</summary>
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
        
        
        ///<summary>Получить поддерево с корнем n указанного узла дерева.</summary>
        public IPositionalTree<T> GetSubTree(Node<T> n){
            LinkedNode<T> ln = n as LinkedNode<T>;
            return new LinkedTree<T>(ln);
        }
        
        #region ITree
        
        ///<summary>Корень дерева. (LinkedNode<typeparamref name="T"/>)</summary>
        public Node<T> Root(){
            return _r;
        }
        
        ///<summary>Содержимое узла (LinkedNode<typeparamref name="T"/>)</summary>
        public T Value(Node<T> node){
            return node.Value;
        }
        
        ///<summary>Родитель узла. (LinkedNode<typeparamref name="T"/>)</summary>
        public Node<T> Parent(Node<T> node){
            LinkedNode<T> np = node as LinkedNode<T>;
            if(np == null || np.Parent == null){
                return null;
            }
            return np.Parent;
        }
        ///<summary>Первый сын узла (LinkedNode<typeparamref name="T"/>)</summary>
        public Node<T> LeftMostChild(Node<T> node){
            LinkedNode<T> np = node as LinkedNode<T>;
            if(np == null || np.Children == null || np.Children.Count == 0){
                return null;
            }
            return np.Children[0];
        }
        
        ///<summary>Правый брат узла (LinkedNode<typeparamref name="T"/>)</summary>
        public Node<T> RightSibling(Node<T> node){
            LinkedNode<T> np = node as LinkedNode<T>;
            if(np == null || np.Parent == null){
                return null;
            }
            LinkedNode<T> parent = np.Parent;
            List<LinkedNode<T>> c = parent.Children;
            Int32 i = 0;
            while(!c[i].Equals(np)){
                i++;
            }
            if(i >= c.Count - 1)
                return null;
            return c[i+1];
        }
        
        ///<summary>Количество узлов</summary>
        public Int32 GetCount(){
            return this._count;
        }
        
        ///<summary>Удалить все узлы дерева. (Корень сохранится, но не будет ничего содержать)</summary>
        public void Clear(){
            _r = null;
            _r = new LinkedNode<T>();
            _count = 0;
        }
        
        ///<summary>Задать нового посетителя этого дерева.</summary>
        public void SetVisitor(IVisitor<T> visitor){
            this._visitor = visitor;
        }
        #endregion
        
        ///<summary>Преобразовать в строку, со скобками.
        ///Скобки указывают вложенность узлов. Сама строка окружена фигурными скобками.</summary>
        public override String ToString(){
            StringBuilder sb = new StringBuilder();
            HashSet<Node<T>> hs = new HashSet<Node<T>>();
            CSharpDataStructures.Structures.Lists.LinkedStack<Node<T>> STACK =
                new CSharpDataStructures.Structures.Lists.LinkedStack<Node<T>>();
            Node<T> n;
            STACK.Push(Root());
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
        
        ///<summary>Количество узлов</summary>
        public Int32 Count {
            get{
                return _count;
            }
        }
    }
}