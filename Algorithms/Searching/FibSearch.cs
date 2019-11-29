using System;
namespace CSharpDataStructures.Algorithms.Searching {
    class FibSearch {
        public int min(int x, int y)  
        { 
            return (x <= y)? x : y;  
        } 
        
        
        //O(LOG N)
        //T(N) = c * (1,62^n)
        //For Binary search = 2 * c * log_2 n + O(1)
        //+ unique subsequences.
        //+ operators +/- insteadof division
        public int fibMonaccianSearch(int[] arr, int x){
            int fm2 = 0; //Fib(m - 2)
            int fm1 = 1; //Fib(m - 1)
            int fm = fm2 + fm1;//Fib(m)
            int n = arr.Length;
            while(fm < n){
                fm2 = fm1;
                fm1 = fm;
                fm = fm2 + fm1;
            }
            int offset = -1;
            
            /*Note that we compare arr[fibMm2] with x.  
            When fibM becomes 1, fibMm2 becomes 0 */
            while(fm > 1){
                int i = min(offset + fm2, n - 1);
                if(arr[i] < x){
                    fm = fm1;
                    fm1 = fm2;
                    fm2 = fm - fm1;//move down three elements of fibonacci  F(m - 1) - F(m - 2)
                    offset = i;
                }
                else if(arr[i] > x){
                    fm = fm2;//F(m - 2)
                    fm1 = fm1 - fm2;//F(m - 1) - F(m - 2)
                    fm2 = fm - fm1;//F(m - 2) - (F(m - 1) - F(m - 2))
                }
                else
                    return i;
            }
            
            if(fm1 == 1 && arr[offset + 1] == x)
                return offset + 1;
            return -1;
        }
        
        public void Execute(){
            int []arr = {10, 22, 35, 40, 45, 50,  
                    80, 82, 85, 90, 100}; 
            int x = 85; 
            Console.WriteLine("Found at index: " + 
                fibMonaccianSearch(arr, x)); 
        }
    }
}