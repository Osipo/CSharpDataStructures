using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace CSharpDataStructures.Structures.Trees {
    public class LinkedNode<T> : Node<T> {
        private List<LinkedNode<T>> _children;
        public LinkedNode(){
            _children = new List<LinkedNode<T>>();
        }
        public LinkedNode<T> Parent {get;set;}
        public List<LinkedNode<T>> Children {
            get{
                return _children;
            }
            set{
                _children = value;
            }
        }
    }
}