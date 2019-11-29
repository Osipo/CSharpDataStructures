using System;
using System.Collections.Generic;
using CSharpDataStructures.Structures.Sets;
using CSharpDataStructures.Structures.Trees;
namespace CSharpDataStructures {
    class SetTest {
        public SetTest(){}
        
        public void Execute(){
            SortedLinkedList<Int32> SET = new SortedLinkedList<Int32>(new Int32[10]{4,2,7,3,8,11,2,1,13,15});
            Console.WriteLine("\n ----SETS---- \n");
            Console.WriteLine("A = "+SET.ToString());
            
            SortedLinkedList<Int32> B = new SortedLinkedList<Int32>(new Int32[7]{4,2,6,88,77,44,11});
            Console.WriteLine("B = "+B.ToString());
            
            BinarySearchTree<Int32> F1 = new BinarySearchTree<Int32>();
            BinarySearchTree<Int32> F2 = new BinarySearchTree<Int32>();
            
            TwoThreeTree<Int32> BT = new TwoThreeTree<Int32>();
            BT.AddRange(new Int32[10]{4,2,7,3,8,11,2,1,13,15});//NON-RECURSIVE!!
            Console.WriteLine("Count(2-3T) = "+BT.GetCount());
            Console.WriteLine("2-3 TREE: "+BT);
            Console.WriteLine("FIND({0}): {1}",4,BT.Contains(4));
            Console.WriteLine("Height(TREE): {0}",BT.Height);
            
            BT.Delete(11);
            Console.WriteLine("2-3 TREE: "+BT);
            Console.WriteLine("Count(TREE) {0}",BT.Count);
            Console.WriteLine("Height(TREE) {0}",BT.Height);
            Console.WriteLine("Min(TREE) {0}",BT.Min());
            Console.WriteLine("Max(TREE) {0}",BT.Max());
            BT.DeleteMin();//Remove 1.
            Console.WriteLine(BT);
            Console.WriteLine("");
            //2,4,7 (4,7)
            //
            
            F1.AddRange(new Int32[10]{4,2,7,3,8,11,2,1,13,15});//4 27 
            F2.AddRange(new Int32[7]{4,2,6,88,77,44,11});
            
            //F2: 2->4<-6<-88  11->44->77->88
            
            Console.WriteLine("F1 = "+F1.ToString());
            Console.WriteLine("F2 = "+F2.ToString());
            SortedLinkedList<Int32> FFF = F1.ToSortedList();
            Console.WriteLine("F1(S) = "+FFF.ToString());
            
            BinarySearchTree<Int32> F3 = F1.Intersection(F2);
            Console.WriteLine("F1 AND F2 = "+F3.ToString());
            Console.WriteLine("");
            Console.WriteLine("F1 OR F2 = "+F1.Union(F2).ToString());
            Console.WriteLine("");
            Console.WriteLine("F1\\F2 = "+F1.Difference(F2).ToString());
            Console.WriteLine("Min(F1), Max(F1) :: {0},  {1}",F1.Min().ToString(),F1.Max().ToString());
            Console.WriteLine("");
            
            while(F1.GetCount() > 0){
                Console.Write(F1.DeleteMin().ToString()+" ");
            }
            Console.WriteLine("");
            SortedLinkedList<Int32> C;
            
            C = SET.Intersection(B);
            Console.WriteLine("A AND B = "+C.ToString());
            Console.WriteLine("Min(C) = "+C.Min().ToString());
            Console.WriteLine("Max(C) = "+C.Max().ToString());
            Console.WriteLine("Count(C) = "+C.Count);
            
            Console.WriteLine("A OR B = "+SET.Union(B).ToString());
            Console.WriteLine("A\\B = "+SET.Difference(B).ToString());
            Console.WriteLine("B\\A = "+B.Difference(SET).ToString());
            Console.WriteLine("A\\B OR B\\A = "+SET.SymmetricDifference(B).ToString());
            Console.WriteLine("Member(88) = "+SET.SymmetricDifference(B).Contains(88));
            SortedLinkedList<Int32> E = new SortedLinkedList<Int32>();
            Console.WriteLine("Empty set(E) = "+E.ToString());
            
        }
    }
}