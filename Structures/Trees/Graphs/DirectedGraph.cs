using System;
using CSharpDataStructures.Structures.Trees;
using CSharpDataStructures.Vectors;
namespace CSharpDataStructures.Structures.Trees.Graphs {
    class DirectedGraph<T> {
        private Int32 _n;
        private CSharpDataStructures.Structures.Lists.LinkedList<Node<T>>[] _adj;
        public DirectedGraph(Int32 n){
            this._n = n;
            this._adj = new CSharpDataStructures.Structures.Lists.LinkedList<Node<T>>[n];
            __init();
        }
        
        private void __init(){
            for(Int32 i = 0; i < _adj.Length; i++)
                _adj[i] = new CSharpDataStructures.Structures.Lists.LinkedList<Node<T>>();
            for(Int32 i = 1; i < _adj.Length + 1; i++){
                _adj[i - 1].Add(new GraphNode<T>(){Name = i.ToString(), Weight = Double.MaxValue});
            }
        }
        
        public void Connect(Int32 i, Int32 j,Double w){
            if(i < 1 || i > _n || j < 1 || j > _n){
                Console.WriteLine("Node {0} doesn't exist",(i < 1 || i > _n) ? i : j);
                return;
            }
            ((GraphNode<T>)_adj[j - 1][1]).Weight = w;
            _adj[i - 1].Add(_adj[j - 1][1]);
        }
        
        //Name of the vertex is v
        //Returns the first adjacent vertex with v.
        public GraphNode<T> First(Int32 v){
            if(v < 1 || v > _n){
                Console.WriteLine("Node {0} doesn't exist",v);
                return null;
            }
            else if(_adj[v - 1].Count == 1){
                Console.WriteLine("There are no any adjacent vertex with {0}",v);
                return null;
            }
            return (GraphNode<T>)_adj[v - 1][2];
        }
        
        //Returns the next adjacent vertex with v
        //after adjacent vertex i.
        public GraphNode<T> Next(Int32 v, Int32 i){
            if(v < 1 || v > _n){
                Console.WriteLine("Node {0} doesn't exist",v);
                return null;
            }
            else if(_adj[v - 1].Count <= i || i <= 1 || i > _n){
                Console.WriteLine("There are no any adjacent {1} vertex with {0}",v,i);
                return null;
            }
            return (GraphNode<T>)_adj[v - 1][i + 1];
        }
        
        //Returns the adjacent i-th vertex with v
        public GraphNode<T> Vertex(Int32 v, Int32 i){
            if(v < 1 || v > _n){
                Console.WriteLine("Node {0} doesn't exist",v);
                return null;
            }
            else if(_adj[v - 1].Count < i || i <= 1 || i > _n){
                Console.WriteLine("There are no any adjacent {1} vertex with {0}",v,i);
                return null;
            }
            return (GraphNode<T>)_adj[v - 1][i];
        }
        
        
        //Deikstra Shortest paths from vertex.
        public CSharpDataStructures.Structures.Lists.LinkedList<Int32> DSP(Int32 vertex){
            CSharpDataStructures.Structures.Lists.LinkedList<Int32> paths = new  CSharpDataStructures.Structures.Lists.LinkedList<Int32>();
            Double[] D = new Double[_n];
            Int32[] P = new Int32[_n];
            ArrayHeap<GraphNode<T>> Q = new ArrayHeap<GraphNode<T>>(x => x.Weight);
            for(Int32 i = vertex + 1; i <= _n; i++){
                D[i - 1] = ((GraphNode<T>)_adj[0][i]).Weight;
                Q.Add((GraphNode<T>)_adj[i - 1][1]);
            }
            
            
            for(Int32 i = 1; i < _n; i++){
                GraphNode<T> w = Q.DeleteMin();
                Int32 vi;
                Int32 wi;
                if(Int32.TryParse(w.Name,out wi)){
                     foreach(GraphNode<T> v in Q){
                        if(Int32.TryParse(v.Name,out vi)){
                            if(D[wi - 1] + v.Weight < D[vi - 1]){
                                P[vi - 1] = wi;
                                D[vi - 1] = D[wi - 1] + v.Weight;
                            }
                        }
                    }
                }
            }
            paths.Add(_n - 1);
            for(Int32 j = _n - 1; j > 1; j = P[j]){
                paths.Add(P[j]);
            }
            
            return paths;
        }
        
        public Matrix GetAdjacentMatrix(){
            Matrix A = new Matrix(_n,_n);
            for(Int32 i = 0; i < _n; i++){
                Int32 k = 1;
                GraphNode<T> w = First(i + 1);
                while(w != null){
                    Int32 wi;
                    Int32.TryParse(w.Name,out wi);
                    A[i,wi - 1] = w.Weight;
                    k++;
                    w = Next(i + 1,k);
                }
            }
            return A;
        }
    }
}