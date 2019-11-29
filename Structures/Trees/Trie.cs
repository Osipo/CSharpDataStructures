using System;
using System.Collections;
using System.Collections.Generic;
using CSharpDataStructures.Structures.Maps;
using TREE = CSharpDataStructures.Structures.Maps.LinkedDictionary<System.Char,System.Object>; //TRIE
using STACK = CSharpDataStructures.Structures.Lists.LinkedStack<System.Object>;
namespace CSharpDataStructures.Structures.Trees {
    /// <summary>
    /// Представляет нагруженное дерево с двумя операторами словаря:
    /// ADD(key,value), GETVALUEOF(key).
    /// Последний оператор представляет из себя объединение операторов CONTAINS(key) и GETVALUEOF(key)
    /// Cм. метод TryGetValue(key, out value)
    /// </summary>
    class Trie<Leaf> : IEnumerable<Leaf>, IEnumerable where Leaf : class, new() {
        private readonly TREE _tree;
        
        /// <summary>
        /// Создать пустое дерево.
        /// </summary>
        public Trie(){
            _tree = new TREE();//create ROOT.
        }
        
        /// <summary>
        /// Добавить новую пару. Если ключ уже существует - переписать значение.
        /// </summary>
        /// <param name="wordForm">Ключ</param>
        /// <param name="entry">Значение</param>
        public void Add(String wordForm, Leaf entry) {
            // Validation
            if (!(__Is_Valid(wordForm) ))
                return;
            
            TREE node = _tree;//ROOT.
            Int32 tail = wordForm.Length;
            TREE next_node;//CHILD
            
            for(Int32 i = 0; i < tail;i++){
                next_node = (TREE) node.GetValue(wordForm[i]);
                if(next_node != null){
                    node = next_node;
                }
                else{
                    TREE child = new TREE();
                    node.Add(wordForm[i], child);
                    
                    node = child;
                }
            }
            Leaf item = (Leaf) node.GetValue('$');
            
            if (item == null) {
                item = entry;
                node.Add('$', item);
            }
            else{//UPDATE node
                node.OverrideValues = true;
                node.Add('$',entry);
                node.OverrideValues = false;
            }
            
        }
        
        /// <summary>
        /// Получить значение по ключу.
        /// </summary>
        /// <param name="word">Ключ словаря.</param>
        /// <returns>Значение типа IndexEntry, или null, если нет ключа.</returns>
        public Leaf GetValue(String word) {
            if (!(__Is_Valid(word)))
                return null;
            TREE node = _tree;//ROOT
            TREE next_node;//CHILD
            Int32 tail = word.Length;
            
            for(Int32 i = 0; i < tail; i++){
                next_node = (TREE) node.GetValue(word[i]);
                if(next_node != null){
                    node = next_node;
                }
                else{
                    return null;
                }
            }
            Leaf e = (Leaf) node.GetValue('$');
            if (e != null) {
                return e;
            }
            else {
                return null;
            }
        }
        
        /// <summary>
        /// Получить значение по ключу. Полученное значение сохранить в переменной entry.
        /// </summary>
        /// <param name="word">Ключ словаря.</param>
        /// <param name="entry">Переменная, хранящая полученное значение.</param>
        /// <returns>true если есть ключ, иначе - false.</returns>
        public bool TryGetValue(String word, out Leaf entry)
        {
            TREE node = _tree;//ROOT
            TREE next_node;//CHILD
            Int32 tail = word.Length;
            for (Int32 i = 0; i < tail; i++)
            {
                next_node = (TREE)node.GetValue(word[i]);
                if (next_node != null)
                {
                    node = next_node;
                }
                else
                {
                    entry = null;
                    return false;
                }
            }
            Leaf e = (Leaf)node.GetValue('$');
            if (e != null)
            {
                entry = e;
                return true;
            }
            else
            {
                entry = null;
                return false;
            }
        }

        IEnumerator IEnumerable.GetEnumerator(){
            return GetEnumerator();
        }
        
        ///<summary>Возвращает итератор, перечисляющий элементы индексатора
        ///IndexEntry</summary>
        public IEnumerator<Leaf> GetEnumerator(){
            HashSet<Object> hs = new HashSet<Object>();
            CSharpDataStructures.Structures.Lists.LinkedList<Leaf> E = 
                new CSharpDataStructures.Structures.Lists.LinkedList<Leaf>();
            STACK S = new STACK();
            S.Push(_tree);
            while(!S.IsEmpty()){
                Object b = S.Top();
                if(hs.Contains(b)){
                    S.Pop();
                }
                else if(b is Leaf){
                    S.Pop();
                    E.Add((Leaf) b);
                    //yield return (IndexEntry) b;
                }
                else{
                    hs.Add(b);
                    ICollection<Object> children = ((LinkedDictionary<Char,Object>) b).Values;
                    foreach(Object it in children){
                        S.Push(it);
                    }
                }
            }
            return E.GetEnumerator();
        }
        
        //Проверка NULL значений ключа.
        private Boolean __Is_Valid(String line) {
            if (line == null || line.Length < 1)
                return false;
            return true;
        }
    }
}