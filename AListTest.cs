using System;
using System.Text;
using System.Linq;
using IE = System.Collections.Generic.IEnumerable<System.Int32>;
using E = System.Collections.Generic.IEnumerator<System.Int32>;
using CSharpDataStructures.Structures.Lists;
namespace CSharpDataStructures {
    internal sealed class AListTest {
        public AListTest(){}
        
        //Intersect without sorting...
        //Args: IEnumerable<Int32> li1, IEnumerable<Int32> li2
        private LinkedList<Int32> Intersect(IE li1, IE li2){
            E en = li1.GetEnumerator();
            E en2 = li2.GetEnumerator();
            LinkedList<Int32> answer = new LinkedList<Int32>();
            while(en.MoveNext() && en2.MoveNext()){
                M1: Int32 p1 = en.Current;
                Int32 p2 = en2.Current;
                if(p1 == p2){
                    answer.Add(p1);
                    continue;
                }
                else if(p1 < p2){
                    en.MoveNext();
                    goto M1;//MoveNext is already called.
                }
                else{
                    en2.MoveNext();
                    goto M1;//MoveNext is already called.
                }
            }
            return  answer;
        }
        
        public void Execute(){
            Double[] _d = new Double[]{2d,4d,6d,1d,3d,5d,7d,3d,5d,10d};
            ArrayList<Double> data = new ArrayList<Double>(_d); //CSharpDataStructures.Structures.Lists;
            foreach(Double elem in data){
                Console.Write(elem);
                Console.Write("  ");
            }
            data.Clear();
            data.Add(2d);
            data.Add(5d);
            data.Add(6d);
            data.Add(2d);
            data.Add(7d);
            data.Add(5d);//256275 -> 2567
            Console.Write("\n");
            Console.WriteLine(data.Count);
            Console.WriteLine(data);
            data.Purge();
            Console.WriteLine(data);
            
            LinkedList<string> linkedList = new LinkedList<string>();
            // добавление элементов
            linkedList.Add("Tom");
            linkedList.Add("Alice");
            linkedList.Add("Bob");
            linkedList.Add("Sam");
             
            // выводим элементы
            foreach(var item in linkedList)
            {
                Console.WriteLine(item);
            }
            // удаляем элемент
            linkedList.Remove("Alice");
           
            foreach (var item in linkedList)
            {
                Console.WriteLine(item);
            }
            // проверяем наличие элемента
            bool isPresent = linkedList.Contains("Sam");
            Console.WriteLine(isPresent == true ? "Sam присутствует" : "Sam отсутствует");
            Console.WriteLine(linkedList.Count);
            Console.WriteLine(linkedList);
            Console.WriteLine(linkedList[1]);//Tom.
            linkedList.Insert(3,"Duke");//Tom Bob->Duke->Sam
            Console.WriteLine(linkedList);
            
            LinkedQueue<Int32> queue = new LinkedQueue<Int32>();
            queue.Clear();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Dequeue();
            Console.WriteLine(queue);
            Console.WriteLine(queue.Front().ToString());
            
            //Linq methods.
            LinkedList<Int32> li1 = new LinkedList<Int32>();
            LinkedList<Int32> li2 = new LinkedList<Int32>();
            for(Int32 i = 0; i < 10; i++){
                li1.Add(i);
                li2.Add(i*3);
                if(i % 2 == 0){
                    li2.Add(i);
                }
            }
            
            Console.WriteLine("Li1: "+li1);
            Console.WriteLine("Li2: "+li2);
            IE li3 = ((IE) li1).Intersect(li2); //IEnumerable<Int32>
            Console.WriteLine("Li3 = Li1 AND Li2");
            foreach(Int32 iii in li3){
                Console.Write(iii+" ");
            }
            Console.WriteLine("Li4 = Li3");
            LinkedList<Int32> li4 = Intersect(li1,li2);
            IE li5 = (IE)li4;
            LinkedList<Int32> li6 = li5 as LinkedList<Int32>;//li4 WAS LinkedList. li5 is IEnumerable. Can Cast it.
            Console.WriteLine(li6 == null);
            Console.WriteLine("Li4");
            Console.WriteLine(li6);
            foreach(Int32 iiii in li4){
                Console.Write(iiii+" ");
            }
            
        }
    }
}