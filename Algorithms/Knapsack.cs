using System;
using System.Text;
using CSharpDataStructures.Structures.Lists;
namespace CSharpDataStructures.Algorithms {
    internal enum StackStatus { None, Included, Excluded }; //Method's call status.
    // Задача о рюкзаке. Упрощенная. Нужно определить можно ли из набора весов w1..wn составить их сумму равную s > 0.
    internal sealed class Knapsack {
        private Int32[] weights;
        public Knapsack(Int32[] weights){
            this.weights = weights;
        }
        public void SetKnapsack(Int32[] weights){
            this.weights = weights;
        }
       
        //2 recursive calls in body
        public bool KnapsackRR(Int32 s, Int32 i){
            if(s == 0)
                return true;
            else if(s < 0 || i >= weights.Length)
                return false;
            else if(KnapsackR(s - weights[i],i+1)){//s - wi, i+1
                Console.WriteLine(weights[i]);
                return true;
            }
            else
                return KnapsackR(s,i+1);//s, i+1  P(X) calls P(Y) -> P(s,i) calls P(s,i+1) => i = i+1 and goto 1.
        }
        
        //1 recursive call in body
        public bool KnapsackR(Int32 s, Int32 i){
            if(s == 0)// (1)
                return true;
            else if(s < 0 || i >= weights.Length)
                return false;
            while(i < weights.Length){
                if(KnapsackR(s - weights[i],i+1)){//s - wi, i+1{
                    Console.WriteLine(weights[i]);
                    return true;
                }
                i+=1;
            }
            return false;
        }
        
        //Non-Recursive algorithm
        //Print included weights which sum is equal to s.
        public bool KnapsackNR(Int32 s){
            Int32 i = 0;
            Int32 n = weights.Length;
            bool winflag = false;
            Stack<StackStatus> stk = new Stack<StackStatus>(n+3); //CHECK (n^2)
            stk.Push(StackStatus.None);//PUSH(S,none)
            do{
                if(winflag){
                    if(stk.Top() == StackStatus.Included){//TOP(S) == included
                        Console.WriteLine(weights[i]);
                    }
                    i = i - 1;
                    stk.Pop();//POP(S)
                }
                else if(s == 0){
                    winflag = true;
                    i -= 1;
                    stk.Pop();
                }
                else if(((s < 0) && (stk.Top() == StackStatus.None)) || (i >= n) ){
                    i -=1;
                    stk.Pop();
                }
                else if(stk.Top() == StackStatus.None){//current candidate_i
                    s = s - weights[i]; //include candidate_i
                    i = i + 1;
                    stk.Pop();
                    stk.Push(StackStatus.Included);//new address
                    stk.Push(StackStatus.None);
                }
                else if(stk.Top() == StackStatus.Included){
                    s = s + weights[i];//attempt to exclude candidate_i
                    i = i + 1;
                    stk.Pop();
                    stk.Push(StackStatus.Excluded);//new address
                    stk.Push(StackStatus.None);
                }
                else{ //TOP(S) == StackStatus.Excluded
                    stk.Pop();
                    i = i - 1;
                }
            }while(!stk.IsEmpty());//until EMPTY(S)
            return winflag;
        }
        
        
        //Mean algorithm. Выдача монетами суммы s.
        //Веса - монеты разного номинала.
        //Задача выдать монетами сумму s, с min количеством монет.
        //
        public Int32 DecomposeByCoins(Int32 s){
            Int32[] nominal = new Int32[]{7,5,1};
            Int32 i = 0;
            Int32 c = 0;//count of coins
            Int32 t = s;//old value of s.
            if(s <= 0){
                return 0;
            }
            Console.WriteLine("Coins");
            while(s > 0 && i < nominal.Length){
                while(nominal[i] <= s){
                    s -= nominal[i];
                    Console.Write(nominal[i]+" + ");
                    c++;//global value c for whole method.
                }
                i++;
            }
            Console.Write("0  = {0} : {1}\n",t,c);
            return c;
        }
    }
}