using System;
namespace CSharpDataStructures.Structures.Sequences {
    public static class Naturals  {
        private const int FIRST = 1;
        private static int c = 0;
        
        public static int Next {
            get{
                c = c + FIRST;
                return c;
            }
        }
        
        
        public static int First{
            get{
                return FIRST;
            }
        }
    }
}