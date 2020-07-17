using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace CSharpDataStructures.Structures.Trees {
    public class NodeCell<T> : Node<T>{
        private Int32 _dept;
       
        public NodeCell(Int32 d){
            this._dept = d;
        }
        public Int32 LeftMostChild {get;set;}//LEFT_MOST_CHILD
        public Int32 RightSibling {get;set;}//RIGHT_SIBLING.
        public Int32 Parent {get;set;}//PARENT.
        public Int32 Idx {get;set;}
        public Int32 Dept {get {return _dept;} set {_dept = value;}}
        
        
        public override bool Equals(Object ob){
            NodeCell<T> B = ob as NodeCell<T>;
            if(ob == null)
                return false;
            if(this.Dept != B.Dept)
                return false;
            if(this.LeftMostChild != B.LeftMostChild)
                return false;
            if(this.RightSibling != B.RightSibling)
                return false;
            if(this.Parent != B.Parent)
                return false;
            return true;
        }
    }
}