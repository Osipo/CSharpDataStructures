using System;
namespace CSharpDataStructures.Algorithms.Nums {
    class GFG {
        public int Fact(int N){
            Int32 r = 1;
            while(N > 0){
                r = r * N;
                N--;
            }
            return r;
        }
        
        public double A(int N, int K){
            Int32 r = 1;
            Int32 t = N - K + 1;
            while(t <= N){
                r = r * t;
                t++;
            }
            return r;
        }
        
        public double C(int N,int K){
            Int32 r = 1;
            Int32 b = 1;
            Int32 t1 = N - K + 1;
            while(t1 <= N){
                r = r * t1;
                t1++;
            }
            while(K > 0){
                b = b * K;
                K--;
            }
            return r/b;
        }
        
        bool checkFact(int N, int countP, int P){
            int cF = 0;
            if(P == 2 || P == 3)
                cF++;
            int d = P;
            while(N / d != 0){
                cF += N / d;
                d = d * d;
            }
            if(cF >= countP)
                return true;
            else
                return false;
        }
        
        //Is Divisible(n! ON SumOfSquares(N)) 
        public bool check(int N){
            int countP = 0;
            int ssqr = (N + 1) * (2 * N + 1);//Formula after removing N and 6
            for(int i = 2; i <= Math.Sqrt(ssqr); i++){
                int flag = 0;
                while(ssqr % i == 0){
                    flag = 1;
                    countP++;
                    ssqr /= i;
                }
                
                
                if(flag == 1){ 
                    if(!checkFact(N - 1, countP, i))  
                        return false;  
                    countP = 0;  
                }
                
            }
            if(ssqr != 1){
                if(!checkFact(N - 1, 1, ssqr))
                    return false;
            }
            return true;
        }
        
        
        //FIND SUM OF XOR OF ALL ELEMENTS OF ALL SUB-ARRAYS OF THE arr.
        //O(N)
        public int FindXORSumOfAllSubs(Int32[] arr){
            Int32 n = arr.Length;
            Int32 s = 0;
            Int32 m = 1;
            for(Int32 i = 0; i < 30; i++){
                Int32 c_odd = 0;//count of sub-arrays with odd number of elements.
                
                // variable to check the status 
                // of the odd-even count while 
                // calculating c_odd 
                bool odd = false;
                
                
                // loop to calculate initial 
                // value of c_odd 
                for(Int32 j = 0; j < n; j++){
                    if((arr[j] & (1 << i)) > 0)
                        odd = (!odd);
                    if(odd)
                        c_odd++;
                }
                for(Int32 j = 0; j < n; j++){
                    s += (m * c_odd);
                    if((arr[j] & (1 << i)) > 0)
                        c_odd = (n - j - c_odd);
                }
                m *= 2;
            }
            return s;
        }
        
        public void Execute(){
            int N = 7;
            Console.WriteLine("N = {0}",N);
            Console.WriteLine("Fact(N) = {0}",Fact(N));
            Console.WriteLine("SumOfSquares(N) = {0}",N*(N+1)*(2*N+1)/6);
            if(check(N))
                Console.WriteLine("Yes");
            else
                Console.WriteLine("No");
        }
    }
}