using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
namespace CSharpDataStructures.Structures.Lists.Enumerators {
    public interface ISeqEnumerator<T> : IEnumerator<T> {
        bool HasNext();
        T MoveNext();
    }
}