using System;
using System.Collections;
using System.Collections.Generic;
namespace CSharpDataStructures.Structures.Sets {
    public interface IMFSet<T> {
        ISet<T> Merge(ISet<T> A, ISet<T> B);
        bool Contains(T item);
        T First();
        T Next();
        Int32 Count {get;}
    }
}