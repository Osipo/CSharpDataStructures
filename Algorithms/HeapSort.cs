using System;
using System.Text;
using System.Collections.Generic;
using CSharpDataStructures.Structures.Trees;
using CSharpDataStructures.Structures.Trees.Visitors;
namespace CSharpDataStructures.Algorithms
{
    public class HeapSort{
        
        public Int32 P(Int32 e){
            return e;
        }
        public void Execute(){
            
            Func<Int32,Int32> f = P;
            
            Console.WriteLine("\n---- Heap Sort(Integers) ----");
            ArrayHeap<Int32> tree = new ArrayHeap<Int32>((it) => it);
            
            //Algorithm O(N*logN)
            Int32[] arrayToS = new Int32[]{3,2,7,1,8,9,4,5,13,2,4,5,7};//for x in L
            for(Int32 i = 0; i < arrayToS.Length; i++){//INSERT(x,S)
                tree.Add(arrayToS[i]);
            }
            
            Console.WriteLine("Sorting...");
            while(tree.GetCount() != 0){//while NOT EMPTY(S)
                Int32 y = tree.DeleteMin();//DELETE(y) and RETURN MIN(y)
                Console.Write(y+" ");
            }
            Console.WriteLine("\n");
        }
        
    }
}