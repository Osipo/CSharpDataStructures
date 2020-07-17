using System;
using IE = System.Collections.Generic.IEnumerable<System.String>;
using System.Text;
using System.Globalization;
using CSharpDataStructures.Structures.Lists;
using CSharpDataStructures.Structures.Sets;
using CSharpDataStructures.Structures.Trees;
using CSharpDataStructures.Structures.Trees.Visitors;
namespace CSharpDataStructures.Algorithms.Trees{
    public class TreeAlgo{
        public TreeAlgo(){}
        
        public void Execute(){
            
            CultureInfo cul = new CultureInfo("en-GB");
            StringComparer comp = StringComparer.Create(cul,true);
            TwoThreeTree<String> T1 = new TwoThreeTree<String>(comp);//2-3 TREE
            ArrayList<String> V = new ArrayList<String>();// ARRAY LIST AS VECTOR.
            SortedLinkedList<String> SET;//SET.
            
            
            ArrayTree<String> TS = new ArrayTree<String>();//SOURCE TREE.
            TS.Root().Value = "a";//ROOT_VALUE
            TS.Add("b");//DEPTH = 1, PARENT = ROOT.
            TS.Add("c");
            
            TS.Add(2,0,"d");//DEPTH = 2, PARENT = FIRST_CHILD(ROOT)
            TS.Add(2,0,"e");/*
            Int32 h = TS.MinHeight + 1;//MAX_DEPTH OF THE NODES
            Console.WriteLine(h);
            IVisitor<String> v = new NRVisitor<String>();
            ArrayList<ArrayList<String>> VS = new ArrayList<ArrayList<String>>();
            
            
            //PROCESS PATHS
            v.PreOrder(TS, (n) => __AddAllPathsFromRootToLeaves(n, ref V));
            PrintPathsFromVector(ref V, VS, h);
            for(Int32 i = 0; i < VS.Count; i++){
                ArrayList<String> VS_i = VS[i];
                AddFromList(ref VS_i, ref T1);
            }
            
            SET = (SortedLinkedList<String>)T1;
            Console.WriteLine(SET);*/
            /*
            TS.Clear();
            T1.Clear();
            TS.Root().Value = "a";//ROOT_VALUE
            TS.Add("b");//DEPTH = 1, PARENT = ROOT.
            TS.Add("c");
            /*
            TS.Add(2,0,"d");//DEPTH = 2, PARENT = FIRST_CHILD(ROOT)
            TS.Add(2,0,"e");
            TS.Add(3,1,"g");//TO e
            TS.Add(4,0,"i");
            h = TS.MinHeight + 1;
            Console.WriteLine(h);
            VS.Clear();
            V.Clear();
            //PROCESS PATHS
            /*
            v.PreOrder(TS, (n) => __AddAllPathsFromRootToLeaves(n, ref V));
            PrintPathsFromVector(ref V, VS, h);
            for(Int32 i = 0; i < VS.Count; i++){
                ArrayList<String> VS_i = VS[i];
                AddFromList(ref VS_i, ref T1);
            }
            
            SortedLinkedList<String> SET2 = (SortedLinkedList<String>)T1;
            Console.WriteLine(SET2);
            Console.WriteLine(SET.Intersection(SET2));*/
        }
        
        
        //O(N^3) OR O(L*D*D) WHERE D IS Height of the tree AND L IS Count(Leaves)
        private void AddFromList(ref ArrayList<String> V, ref TwoThreeTree<String> TREE){
            StringBuilder sb = new StringBuilder();
            Int32 l = 0;
            while(l < V.Count){//O(H*H) where H is the count of the V.
                for(Int32 i = 0; i < V.Count; i++){
                    if(i+l < V.Count){
                        for(Int32 j = i,c = 0; j < V.Count && c <= l; j++,c++){
                            sb.Append(V[j]);
                        }
                    }
                    else
                        continue;
                    //if(sb.Length != 0){
                        TREE.Add(sb.ToString());
                        //Console.WriteLine("{0} : {1}",sb.ToString(),TREE.Add(sb.ToString()));
                        sb.Length = 0;
                    //}
                }
                l++;
            }
        }
        
        
        //O(N^2)
        private void PrintPathsFromVector(ref ArrayList<String> v, ArrayList<ArrayList<String>> VS, Int32 h){//ref TwoThreeTree<String> TREE){
            ArrayList<String> L = new ArrayList<String>();
            for(Int32 i = 0; i < v.Count; i++){
                if(v[i] == "$"){
                    Console.WriteLine(L);
                    //if(L.Count >= h){
                    //    VS.Add(new ArrayList<String>((IE) L));
                    //}
                    //AddFromList(ref L, ref TREE);//REPLACE.
                    L.RemoveAt(L.Count - 1);
                    continue;
                }
                L.Add(v[i]);
            }
        }
        
        //O(N)
        private void __AddAllPathsFromRootToLeaves(Node<String> n, ref ArrayList<String> v){
            NodeCell<String> c = n as NodeCell<String>;
            v.Add(c.Value);
            if(c.LeftMostChild == -1){
                v.Add("$");
                if(c.RightSibling == -1)
                    v.Add("$");
            }
        }
        
        
        //Util function. Prints elements of vector v from the idx position.
        private void PrintVector(ArrayList<String> v, Int32 idx){
            Console.Write("[");
            for(Int32 j = idx; j < v.Count; j++)
                Console.Write(" "+v[j]);
            Console.Write("]\n");
        }
        
        //Print all paths (subpaths) IN TREE which the value_of_nodes in path is equal to k.
        //
        //Parameter n -> root of the SUB_TREE.
        //Parameter v -> name of the path. CONCATENATION_OF_ALL_VALUES OF NODES IN THE PATH.
        private void __PrintKPathUtil(Node<String> n, ref ArrayList<String> v, ref Int32 k){
            NodeCell<String> c = n as NodeCell<String>;
            v.Add(c.Value);
            Console.WriteLine("{0} : {1}",c.Dept,c.LeftMostChild);
            if(c.LeftMostChild == -1){
                StringBuilder sb = new StringBuilder();
                for(Int32 j = v.Count - 1; j >= 0; j--){
                    sb.Append(v[j]);//f += v[j]
                    if(sb.Length == k){
                        PrintVector(v,j);
                    }
                }
                v.RemoveAt(v.Count - 1);
            }
        }
    }
}