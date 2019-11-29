using System;
using CSharpDataStructures.Structures.Trees;
namespace CSharpDataStructures.Structures.Trees.Graphs {
    class GraphNode<T> : Node<T> {
        public String Name{
            get;set;
        }
        
        public Double Weight{
            get;set;
        }
    }
}