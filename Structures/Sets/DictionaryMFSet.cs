using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSharpDataStructures.Structures.Maps;
using CSharpDataStructures.Structures.Lists;
namespace CSharpDataStructures.Structures.Sets {
    class DictionaryMFSet<T>  {
        
        private LinkedDictionary<Int32,MFSetHEntry> _headers;
        private LinkedDictionary<Int32,MFSetNEntry> _names;
        
        public DictionaryMFSet(){
            _headers = new LinkedDictionary<Int32,MFSetHEntry>();
            _names = new LinkedDictionary<Int32,MFSetNEntry>();
        }
        
        //SETNAME A, ELEM X
        public void Initial(Int32 a, Int32 x){
            _names[x] = new MFSetNEntry(){SetName = a, NextElemC = 0};
            _headers[a] = new MFSetHEntry(){FirstElemC = x, Count = 1};
        }
        
        //SETNAMES A,B
        public void Merge(Int32 a, Int32 b){
            Int32 i = 0;
            if(_headers[a].Count > _headers[b].Count){
                i = _headers[b].FirstElemC;
                while(_names[i].NextElemC != 0){
                    _names[i].SetName = a;
                    i = _names[i].NextElemC;
                }
                _names[i].SetName = a;
                _names[i].NextElemC = _headers[a].FirstElemC;
                _headers[a].FirstElemC = _headers[b].FirstElemC;
                _headers[a].Count = _headers[a].Count + _headers[b].Count;
                //_headers[b].Count = 0;
                //_headers[b].FirstElemC = 0;
                
            }
            else{
                //SWAP(A,B)
                
                i = _headers[a].FirstElemC;
                while(_names[i].NextElemC != 0){
                    _names[i].SetName = b;
                    i = _names[i].NextElemC;
                }
                _names[i].SetName = b;
                _names[i].NextElemC = _headers[b].FirstElemC;
                _headers[b].FirstElemC = _headers[a].FirstElemC;
                _headers[b].Count = _headers[a].Count + _headers[b].Count;
            }
        }
        
        public Int32 Find(Int32 x){
            return _names[x].SetName;
        }
    }
}