using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace CSharpDataStructures.Structures.Trees {
    class LinkedBinaryNode<T> : Node<T> {
        public LinkedBinaryNode<T> LeftSon {get;set;}
        public LinkedBinaryNode<T> RightSon {get;set;}
        public Boolean IsLeaf { get{ return (LeftSon == null && RightSon == null);} }//DELETE
        public Boolean IsLeftLeaf { get{ return LeftSon == null;} }//DELETE
        public Boolean IsRightLeaf { get{ return RightSon == null;} }//DELETE
        public Int32 Dept {get; set;}//DELETE.
    }
}