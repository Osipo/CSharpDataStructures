using System;
namespace CSharpDataStructures.Structures.Lists {
    public interface IStack<T> {
        void Push(T item);
        void Pop();
        void Clear();
        T Top();
        bool IsEmpty();
        Int32 Count {get;}
    }
}