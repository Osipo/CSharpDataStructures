using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
namespace CSharpDataStructures.Structures.Lists.Enumerators {
    public interface IListEnumerator<T> : ISeqEnumerator<T> {
        bool HasPrevious();
        T MovePrevious();
        void SetForward();
        void SetBack();
    }
}