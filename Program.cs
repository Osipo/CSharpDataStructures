using System;
using System.Collections.Generic;
using System.Reflection;
using CSharpDataStructures.Counters;
using CSharpDataStructures.Vectors;
using CSharpDataStructures.Structures.Sequences;
using CSharpDataStructures.Structures.Lists;
using CSharpDataStructures.Structures.Maps;
using CSharpDataStructures.Algorithms;
using CSharpDataStructures.Algorithms.Nums;
using CSharpDataStructures.Algorithms.Searching;
using CSharpDataStructures.Algorithms.Trees;
using CSharpDataStructures.Randomizers.Numeric;
using LIST = CSharpDataStructures.Structures.Lists.LinkedList<System.Int32>;
using DLIST = CSharpDataStructures.Structures.Lists.DoublyCircularLinkedList<System.Int32>;
using CSharpDataStructures.Structures.Lists.Enumerators;
using CSharpDataStructures.Structures.Trees.Graphs;
namespace CSharpDataStructures
{
   
    class Program
    {
        static void Main(string[] args)
        {
            //Counter<T> -> Counter<String>
            //Console.WriteLine("Hello World!");
            List<String> labels = new List<String>(){"aba","ba","cc","ba","a","b","b","cc","d"};
            Console.Write("[");
            foreach(var Item in labels){
                Console.Write(Item+" ");
            }
            Console.WriteLine("]");
            
            Counter<String> counter = new Counter<String>(labels);
            
            List<(String e,Int32 c)> tuples = counter.MostCommon(5);
            List<Int32> values = counter.Values();
            
            tuples = counter.MostCommon(6); //testing InvalidOperationException
            
            foreach(var tuple in tuples){
                Console.WriteLine("({0} : {1})",tuple.e,tuple.c);
            }
            foreach(Int32 i in values){
                Console.WriteLine("{0}",i);
            }
            
            Console.WriteLine("");
            /*
            //Matrix.
            Matrix A = new Matrix(
                new Double[][]{
                        new Double[]{2,3,4,1,5},
                        new Double[]{2,3,1,-2,-5},
                        new Double[]{-3,-4,1,-2,4},
                        new Double[]{0,0,0,0,0},
                        new Double[]{1,2,3,4,5}
                        });
            Console.WriteLine(A);
            
            A.ToArray()[0][0] = 233; //copy of _base.
            Console.WriteLine("");
            Console.WriteLine("A*(-1)");
            A.ScalarMul(-1);
            Console.WriteLine("");
            Console.WriteLine(A);
            
            Console.WriteLine("A[4,2] = "+A[4,2]);
            Console.WriteLine("Rows: {0}, Columns: {1}",A.GetRowsCount(),A.GetColumnsCount());
            Console.WriteLine("");
            
            Matrix E = Matrix.GetEMatrix(5,5);
            Console.WriteLine(E);
            Console.WriteLine("Rows: {0}, Columns: {1}",E.GetRowsCount(),E.GetColumnsCount());
            
            Console.WriteLine("");
            Console.WriteLine(E.Transpose());
            Console.WriteLine("Rows: {0}, Columns: {1}",E.GetRowsCount(),E.GetColumnsCount());
            Console.WriteLine("");
            Console.WriteLine(E.Transpose());
            
            Console.WriteLine(A.Mull(E));   
            Console.WriteLine(A);
            Console.WriteLine("Squared A : {0}",A.Squared);
            Matrix B = new Matrix(A,3,0);
            Console.WriteLine("Matrix B");
            Console.WriteLine("");
            Console.WriteLine(B);
            Console.WriteLine("det B = {0}",B.Determinant); //-247.
            Console.WriteLine("F: det B = {0}",B.MatrixDeterminant());
            Matrix C = new Matrix(A,1,2);
            Console.WriteLine(C);
            Console.WriteLine("det C = {0}",C.Determinant);
            Console.WriteLine("Special: {0}",C.IsSpecial);
            
            Double[][] ___ = new Double[][]{
                new Double[]{2,3,1},
                new Double[]{1,7,4},
                new Double[]{6,5,11}
            };
            
            Matrix D = new Matrix(___);
            Console.WriteLine("Matrix D");
            Console.WriteLine(D);
            Console.WriteLine("det D = {0}",D.Determinant);
            Console.WriteLine("");
            D.SwapRows(0,1);
            Console.WriteLine(D);
            Console.WriteLine("det D = {0}",D.Determinant);
            //Matrix Dm = D.GetInverse();
            //Console.WriteLine(Dm);
            Console.WriteLine("");
            Matrix F = new Matrix(C,2);
            Console.WriteLine(F);
            Console.WriteLine("");
            Console.WriteLine(D);
            Console.WriteLine("Rank D = {0}",D.Rank);
            Console.WriteLine("\nF");
            Console.WriteLine(F);
            Console.WriteLine("Rank F = {0}",F.Rank);
            
            Matrix F2 = new Matrix(new Double[][]{
                new Double[]{1,1,6,12},
                new Double[]{2,2,0.5,1},
                new Double[]{3,7,-5,-1},
                new Double[]{1,8,1,2}
            });
            Console.WriteLine("");
            Console.WriteLine(F2);
            Console.WriteLine("Det F2 = {0}",F2.Determinant);
            Console.WriteLine("Rank F2 = {0}",F2.Rank);
            Console.WriteLine(F2);
            Console.WriteLine(E);
            Console.WriteLine(E.Rank);
            Console.WriteLine("");
            Console.WriteLine(E.ToXml());
            Console.WriteLine(F2.ToXml());
            //Console.WriteLine(F2*F2.GetInverse());
            //Console.WriteLine("Rank F = {0}",F.Rank);
            //Console.WriteLine(Dm * D); //A^-1 * A = E.
            
            
            //ArrayList,LinkedList,Queue test.
            AListTest moduleC1 = new AListTest();
            moduleC1.Execute();
            
            
            //Matrix IEnumerable test.
            Console.WriteLine("Matrix F2");
            foreach(Double item in F2){ //IEnumerable<Double>
                Console.Write(item+" ");
            }
            */
            //Knapsack test.
            /*
            Int32[] ws = new Int32[]{1,3,2,6,5,7,7,9,9,3,3,2,3};
            Knapsack knapsack = new Knapsack(ws);
            Console.WriteLine("");
            bool rr1 = knapsack.KnapsackRR(10,0);//1,3,6 -> 10.
            Console.WriteLine("");
            bool rr2 = knapsack.KnapsackR(10,0);
            Console.WriteLine("");
            bool rr3 = knapsack.KnapsackNR(10);
            Console.WriteLine("");
            Console.WriteLine("Results: {0}, {1}, {2}",rr1,rr2,rr3);
            knapsack.DecomposeByCoins(24);
            knapsack.DecomposeByCoins(27);
           */
            
            
            Console.WriteLine("");
            TreeTest trt = new TreeTest();
            trt.Execute();//*+ab+ac
            
            SetTest ssss = new SetTest();
            ssss.Execute();
            
            HeapSort PQ = new HeapSort();
            PQ.Execute();
            Console.WriteLine("Random PositiveNums Generator");
            PositiveRandomizer randG = new PositiveRandomizer(8);//b = 8 => 1..7
            Console.WriteLine("K = {0}",randG.GetK());
            for(Int32 i = 1; i < 8; i++){
                Console.WriteLine("d{0} = {1}",i,randG.NextInt());
            }
            Console.WriteLine("");
            
            //FibSearch fibo = new FibSearch();
            //fibo.Execute();
            //GFG algo = new GFG();
            //algo.Execute();
            
            
            ArrayMap<String,Double> pr = new ArrayMap<String,Double>(10);
            pr.Add("c",3d);
            Sequence S1 = new ParamsSequence("( n + 1 ) * c ^ 2",pr);//(n+1)*3^2        (n+1)*n^2
            
            LinkedStack<String> S1_e = S1.Parsed;
            foreach(String it in S1_e){
                Console.WriteLine(it);
            }
            Console.WriteLine("Sequence: (n+1)*3^2");
            for(UInt32 i = 1; i < 101; i++){
                Console.WriteLine("{0} = {1}",i,S1[i]);
            }
          
            //TreeAlgo TA = new TreeAlgo();
            //TA.Execute();
          
            
            LIST nums = new LIST();
            nums.Add(2);
            nums.Add(1);
            nums.Add(3);
            nums.Add(5);
            Console.WriteLine(nums);
            nums.RemoveAt(4);
            nums.Insert(1,44);
            Console.WriteLine("Append");
            Console.WriteLine(nums);
            
            DLIST nums2 = new DLIST();
            nums2.Add(2);
            nums2.Add(1);
            nums2.Add(3);
            Console.WriteLine(nums2);
            nums2.RemoveAt(3);
            nums2.Insert(1,44);
            Console.WriteLine("Append");
            Console.WriteLine(nums2);
            
            
            
            foreach(Int32 item2 in nums2){
                Console.WriteLine("Item: "+item2);
            }
            Console.WriteLine("");
            foreach(Int32 item1 in nums){
                Console.WriteLine("LItem: "+item1);
            }
            Console.WriteLine(nums2);
            nums2.Insert(2,10);
            nums2.Insert(4,55);
            Console.WriteLine(nums2);
            IListEnumerator<Int32> nums2_iter = nums2.GetIterator();
            DirectedGraph<String> GR = new DirectedGraph<String>(5);
            GR.Connect(1,2,10d);
            GR.Connect(1,5,100d);
            GR.Connect(1,4,30d);
            GR.Connect(2,3,50d);
            GR.Connect(3,5,10d);
            GR.Connect(4,3,20d);
            GR.Connect(4,5,60d);
            CSharpDataStructures.Structures.Lists.LinkedList<Int32> PGR = GR.DSP(1);
            Console.WriteLine(PGR);
            LinkedStack<Int32> PRGS = new LinkedStack<Int32>();
            foreach(Int32 i in PGR)
                PRGS.Push(i);
            Console.Write("Directed Graph:: Shortest Path from 1st vertex\n{ ");
            while(!PRGS.IsEmpty()){
                Console.Write(PRGS.Top());
                Console.Write(" ");
                PRGS.Pop();
            }
            Console.WriteLine("}");
            Console.WriteLine("Adjacent Matrix of Graph\n");
            Console.WriteLine(GR.GetAdjacentMatrix().ToString());
        }
    }
}