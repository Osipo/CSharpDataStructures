using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace CSharpDataStructures.Structures.Trees.Visitors {
    public class BinaryNRVisitor<T> : NRVisitor<T>, IVisitor<T> {
        
        
        //SYMMETRIC BSTREE TRAVERSAL WITHOUT RECURSION
        public override void InOrder(ITree<T> t1, Action<Node<T>> act = null){
            LinkedBinaryNode<T> m1 = t1.Root() as LinkedBinaryNode<T>;
            if(act == null){
                act = (n1) => Console.Write(t1.Value(n1).ToString()+" ");
            }
            CSharpDataStructures.Structures.Lists.Stack<CSharpDataStructures.Structures.Trees.Node<T>> STACK = 
                new CSharpDataStructures.Structures.Lists.Stack<CSharpDataStructures.Structures.Trees.Node<T>>(t1.GetCount()+1);
               
            while(true){
                if(m1 != null){
                    STACK.Push(m1);
                    m1 = m1.LeftSon != null ? t1.LeftMostChild(m1) as LinkedBinaryNode<T> : null;//LEFTMOST_CHILD(node,TREE1)
                }
                else{
                    if(STACK.IsEmpty()){
                        return;
                    }
                    act(STACK.Top());//LABEL(node1) on TREE1 AND TREE2
                    Node<T> c1 = STACK.Top();
                    STACK.Pop();
                    m1 = ((LinkedBinaryNode<T>) c1).IsRightLeaf ? null :  ((LinkedBinaryNode<T>) c1).RightSon;//REPLACE IsRightLeaf
                }
            }
            
        }
    }
}