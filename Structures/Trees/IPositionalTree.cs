using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CSharpDataStructures.Structures.Trees.Visitors;
namespace CSharpDataStructures.Structures.Trees {
    interface IPositionalTree<T> : ITree<T> {
        void AddTo(Node<T> n, T item);
        Node<T> RightMostChild(Node<T> n);
        IList<Node<T>> GetChildren(Node<T> n);
        void Visit(VisitorMode order, Action<Node<T>> act);
        IPositionalTree<T> GetSubTree(Node<T> n);
    }
}