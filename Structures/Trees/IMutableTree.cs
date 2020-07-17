using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CSharpDataStructures.Structures.Trees.Visitors;
namespace CSharpDataStructures.Structures.Trees {
    public interface IMutableTree<T> : ITree<T> {
        void Add(T item);
        void Add(Int32 dept, T item);
        void Add(Int32 dept, Int32 np, T item);
        void Delete(T item);
        void Delete(Node<T> n);
    }
}