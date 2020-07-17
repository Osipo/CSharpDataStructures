using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace CSharpDataStructures.Structures.Trees {
    public class TwoThreeNode<T> : Node<T> {
        public TwoThreeNode<T> FirstSon {get;set;}
        public TwoThreeNode<T> SecondSon {get;set;}
        public TwoThreeNode<T> ThirdSon {get;set;}
        public TwoThreeNode<T> Parent {get;set;}
        public T LowOfSecond {get;set;}
        public T LowOfThird {get;set;}
    }
}