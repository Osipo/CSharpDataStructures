using System;
using System.Text;
using CSharpDataStructures.Structures.Lists;
using CSharpDataStructures.Structures.Trees;
using CSharpDataStructures.Structures.Trees.Visitors;
namespace CSharpDataStructures {
    class TreeTest {
        public TreeTest(){}
        public void Execute(){
            ArrayTree<String> tree = new ArrayTree<String>();
            IVisitor<String> visitor = new NRVisitor<String>();
            tree.Root().Value = "*";
            tree.Add("+");//dept == 1
            tree.Add("+");
            tree.PrintContent();
            
            tree.Add(2,"a");//dept == 2
            tree.Add(2,"b");
            tree.PrintContent();
            
            tree.Add(2,1,"a");//son of the second element in dept == 1.
            tree.Add(2,1,"c");
            tree.PrintContent();
            Console.WriteLine("PreOrder call");
            visitor.PreOrder(tree);//*+ab+ac
            Console.WriteLine("");
            Console.WriteLine("PostOrder call");
            visitor.PostOrder(tree);//ab+ac+*
            Console.WriteLine("");
            Console.WriteLine("InOrder call");
            visitor.InOrder(tree);//a+b*a+c
            Console.WriteLine("");
            
            Console.WriteLine("");
            tree.Delete("b");
            Console.WriteLine("PreOrder call after delete b");
            visitor.PreOrder(tree);//*+a+ac
            Console.WriteLine("");
            
            tree.PrintContent();
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            
            tree = new ArrayTree<String>();
            tree.Root().Value= "1";
            tree.Add(1,0,"2");//1->2,3,4.
            tree.Add(1,0,"3");
            tree.Add(1,0,"4");
            tree.Add(2,1,"5");//3->5,6.
            tree.Add(2,1,"6");
            tree.Add(2,2,"7");//4-> 7.
            tree.Add(3,0,"8");//5-> 8,9.
            tree.Add(3,0,"9");
            tree.Add(3,1,"10");//6-> 10.
            Console.WriteLine("");
            Console.WriteLine("Post order call");
            visitor.PostOrder(tree);
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Symmetric order call");
            visitor.InOrder(tree);
            
            Console.WriteLine("");
            tree.PrintContent();
            
            Node<String> st = tree.GetNode("5");
            visitor.PostOrder(tree,st);//subtree st of the tree.
            Console.WriteLine("");
            
            visitor.PreOrder(tree,st);
            Console.WriteLine("");
            tree.Delete("5");//delete node with value 5.
            tree.PrintContent();//0 1 2 3 4 5 6 7 8 9 => 0 1 2 3 5 6 9
            Console.WriteLine("");
            visitor.PostOrder(tree);//2 10 6 3 7 4 1.
            Console.WriteLine("");
        }
    }
}