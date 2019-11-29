using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
namespace CSharpDataStructures.Structures.Lists {
    class DoubleElementType<T> {
        public T Element {get;set;}
        public DoubleElementType<T> Next {get; set;}
        public DoubleElementType<T> Previous {get;set;}
    }
}