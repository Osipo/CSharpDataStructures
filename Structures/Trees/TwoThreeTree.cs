using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CSharpDataStructures.Structures.Trees.Visitors;
using CSharpDataStructures.Structures.Lists;
using CSharpDataStructures.Structures.Sets;
namespace CSharpDataStructures.Structures.Trees {
    //2-3 TREE FOR REPRESENTING SETS
    public class TwoThreeTree<T> : ITree<T>, ISet<T>, ICollection<T>, IEnumerable<T>, IEquatable<TwoThreeTree<T>> {
        private TwoThreeNode<T> _r;
        private Int32 _count;
        private Int32 _h;
        private IVisitor<T> _visitor;
        private IComparer<T> _comp;
        
        public TwoThreeTree(IComparer<T> comp){
            this._comp = comp;
            this._count = 0;
            this._h = 0;
            this._r = null;
            this._visitor = new NRVisitor<T>();//CHECK
        }
        
        public TwoThreeTree() : this(Comparer<T>.Default) {}
        
        private void __Insert2(TwoThreeNode<T> node, T item,ref TwoThreeNode<T> pnew, ref T low,ref LinkedStack<Int32> SC, ref Boolean added,ref LinkedStack<TwoThreeNode<T>> S){
            TwoThreeNode<T> pback;//for pnew
            TwoThreeNode<T> w = node;//__CreateNewNode(node,null);
            Int32 child = 1;
            pnew = null;
            while(w.FirstSon != null){
                if(_comp.Compare(item, w.LowOfSecond) < 0){
                    child = 1;
                    SC.Push(child);
                    S.Push(w);
                    w = w.FirstSon;
                }
                else if(w.ThirdSon == null || _comp.Compare(item, w.LowOfSecond) < 0){
                    child = 2;
                    SC.Push(child);
                    S.Push(w);
                    w = w.SecondSon;
                }
                else{
                    child = 3;
                    SC.Push(child);
                    S.Push(w);
                    w = w.ThirdSon;
                }
            }
            if(w.FirstSon == null){//leaf
                if(_comp.Compare(w.Value, item) != 0){//NOT EQUALS
                added = true;
                _count += 1;
                    pnew = new TwoThreeNode<T>();
                    if(_comp.Compare(w.Value, item) < 0){
                        pnew.Value = item;
                        low = item;
                        return;
                    }
                    else{
                        pnew.Value = w.Value;
                        w.Value = item;
                        low = pnew.Value;
                        return;
                    }
                }
            }
        }
        //NON-RECURSIVE.
        private void __Insert1(TwoThreeNode<T> node, T item,ref TwoThreeNode<T> pnew, ref T low,ref Boolean added){
            
            TwoThreeNode<T> pback = null;
            T lowback = default(T);
            LinkedStack<TwoThreeNode<T>> STACK = new LinkedStack<TwoThreeNode<T>>();
            LinkedStack<Insert1StackEntry<T>> S2 = new LinkedStack<Insert1StackEntry<T>>();
            LinkedStack<Int32> S3 = new LinkedStack<Int32>();

            
            //move down to the leaf. return leaf as pback and its minValue as lowback.
            //also return path from the root to the parent of the leaf. (PARENT(pback))
            //path includes parameter child which is contained in the S3.
            //parent is the Top of the STACK.
            //child option is the Top of the S3.
            __Insert2(node,item,ref pback,ref lowback,ref S3,ref added,ref STACK);
            
            
            S2.Push(new Insert1StackEntry<T>(pback,lowback));//pback lowback - first.
            
            
            
            //AFTER DESCENT.
            while(!STACK.IsEmpty() && !S3.IsEmpty() && !S2.IsEmpty()){
                Int32 child = S3.Top();
                node = STACK.Top();//w2,w3 => w2,w1 ::S, nleaf as pback
                pback = S2.Top().LowBack;
                lowback = S2.Top().Low;
                
                S2.Pop();
                STACK.Pop();
                S3.Pop();
                if(pback != null){//ADD NEW SON TO THE NODE
                    if(node.ThirdSon == null){//Two children.
                        if(child == 2){
                            node.ThirdSon = pback;//__CreateNewNode(pback,parent: node);//pback;
                            node.ThirdSon.Parent = node;
                            node.LowOfThird = lowback;
                            pnew = null;
                            return;//INSERTED
                        }
                        else{//child == 1.
                            node.ThirdSon = node.SecondSon;//__CreateNewNode(node.SecondSon,node);
                            node.LowOfThird = node.LowOfSecond;
                            node.SecondSon = pback;//__CreateNewNode(pback,node);
                            node.SecondSon.Parent = node;
                            node.LowOfSecond = lowback;
                            pnew = null;
                            return;//INSERTED.
                        }
                    }//INSERTED:: pnew and low are still null.
                    else{//node has three children. UPDATE pback and lowback!
                        TwoThreeNode<T> _pnew = new TwoThreeNode<T>();//ITERIOR NODE
                        if(child == 3){//pback and thirdchild are sons of new node.
                            _pnew.FirstSon = node.ThirdSon;//__CreateNewNode(node.ThirdSon,_pnew);
                            _pnew.SecondSon = pback;//__CreateNewNode(pback,_pnew);
                            _pnew.ThirdSon = null;
                            _pnew.LowOfSecond = lowback;
                            _pnew.FirstSon.Parent = _pnew;
                            _pnew.SecondSon.Parent = _pnew;
                            low = node.LowOfThird;
                            pnew = _pnew;
                            node.ThirdSon = null;
                        }
                        else{//child <= 2  the third child of node moving to -> pnew
                            _pnew.SecondSon = node.ThirdSon;//__CreateNewNode(node.ThirdSon,_pnew);
                            _pnew.LowOfSecond = node.LowOfThird;
                            //if(_pnew.SecondSon != null)
                            _pnew.SecondSon.Parent = _pnew;
                            _pnew.ThirdSon = null;
                            node.ThirdSon = null;
                        }
                        if(child == 2){//pback is firstSon of pnew
                            _pnew.FirstSon = pback;//__CreateNewNode(pback,_pnew);
                            _pnew.FirstSon.Parent = _pnew;
                            low = lowback;
                            pnew = _pnew;
                        }
                        if(child == 1){//the second child of node move to -> pnew, pback is 2 son of node
                            _pnew.FirstSon = node.SecondSon;//__CreateNewNode(node.SecondSon,_pnew);
                            //if(_pnew.FirstSon != null)
                            _pnew.FirstSon.Parent = _pnew;
                            low = node.LowOfSecond;
                            node.SecondSon = pback;//__CreateNewNode(pback,node);
                            node.SecondSon.Parent = node;
                            node.LowOfSecond = lowback;
                            pnew = _pnew;
                        }
                        S2.Push(new Insert1StackEntry<T>(_pnew,low));
                    }//endElse
                }//endIf
            }//endWhile
           
        }

        private bool __Member(TwoThreeNode<T> node, T item){
            if(item == null)
                return false;
            while(node != null){
                if(node.ThirdSon != null){//iterior with 3 sons.
                    Int32 i = _comp.Compare(item, node.LowOfSecond);
                    if(i == 0)//x == y.
                        return true;
                    else if(i < 0)//x < y
                        node = node.FirstSon;
                    else
                        i = _comp.Compare(item, node.LowOfThird);
                    if(i == 0)//x == z
                        return true;
                    else if(i < 0)//x < z
                        node = node.SecondSon;
                    else//x > z
                        node = node.ThirdSon;
                    //Console.WriteLine("down");
                }
                else if(node.SecondSon != null){//iterior with 2 sons.
                    Int32 i = _comp.Compare(item, node.LowOfSecond);
                    if(i == 0)//x == y.
                        return true;
                    else if(i < 0)//x < y
                        node = node.FirstSon;
                    else
                        node = node.SecondSon;
                    //Console.WriteLine("down");
                }
                else{//leaf
                    if(_comp.Compare(item, node.Value) == 0)
                        return true;
                    return false;
                }
            }
            return false;
        }
        
        
        
        private bool __Insert0(TwoThreeNode<T> r, T item){
            if(item == null)
                return false;
            if(_count == 0){
                _r = new TwoThreeNode<T>();//leaf
                _r.Value = item;
                _count += 1;
                return true;
            }
            else if(_count == 1){
                if(_comp.Compare(_r.Value, item) == 0){//IF EQUALS
                    return false;
                }
                
                TwoThreeNode<T> f1 = _r;
                T v1 = _r.Value;
                _r = new TwoThreeNode<T>();
                
                if(_comp.Compare(v1, item) > 0){//item -> FirstSon.
                    _r.FirstSon = new TwoThreeNode<T>();
                    _r.FirstSon.Value = item;
                    _r.FirstSon.Parent = _r;//PARENT
                    _r.SecondSon = f1;
                    _r.SecondSon.Value = v1;
                    _r.SecondSon.Parent = _r;//PARENT
                    _r.LowOfSecond = v1;
                    _count +=1;
                    _h = 1;
                    return true;
                }
                
                _r.FirstSon = f1;
                _r.SecondSon = new TwoThreeNode<T>();
                _r.FirstSon.Value = v1;
                _r.SecondSon.Value = item;
                _r.FirstSon.Parent = _r;//PARENT
                _r.SecondSon.Parent = _r;//PARENT
                _r.LowOfSecond = item;
                _count += 1;
                _h = 1;
                return true;
            }
            
            else if(_count == 2){//JUST INSERT item as the third child of the ROOT.
                if(_comp.Compare(_r.FirstSon.Value,item) == 0){
                    return false;
                }
                else if(_comp.Compare(_r.SecondSon.Value,item) == 0){
                    return false;
                }
                else if(_comp.Compare(item,_r.FirstSon.Value) < 0){//x < y < z
                    TwoThreeNode<T> leaf = _r.FirstSon;
                    TwoThreeNode<T> temp = _r.SecondSon;
                    T oldmin = _r.LowOfSecond;
                    _r.FirstSon = new TwoThreeNode<T>();
                    _r.FirstSon.Value = item;
                    _r.FirstSon.Parent = _r;//PARENT
                    _r.SecondSon = leaf;
                    _r.ThirdSon = temp;
                    _r.LowOfSecond = _r.SecondSon.Value;
                    _r.LowOfThird = oldmin;
                    _count += 1;
                    return true;
                }
                else if(_comp.Compare(item,_r.SecondSon.Value) < 0){//y < x < z.
                    TwoThreeNode<T> leaf = _r.SecondSon;
                    T oldmin = _r.LowOfSecond;
                    _r.SecondSon = new TwoThreeNode<T>();
                    _r.SecondSon.Value = item;
                    _r.SecondSon.Parent = _r;//PARENT
                    _r.ThirdSon = leaf;
                    _r.LowOfSecond = _r.SecondSon.Value;
                    _r.LowOfThird = oldmin;
                    _count += 1;
                    return true;
                }
                else{//y < z < x
                    _r.ThirdSon = new TwoThreeNode<T>();
                    _r.ThirdSon.Value = item;
                    _r.ThirdSon.Parent = _r;//PARENT
                    _r.LowOfThird = item;
                    _count += 1;
                    return true;
                }
            }
            Boolean added = false;
            TwoThreeNode<T> pback = null;//pback is returned by Insert1 procedure
            T lowback = default(T);//lowvalue in pback.
            TwoThreeNode<T> saveS;
            __Insert1(r,item,ref pback,ref lowback,ref added);
            if(pback != null){
                //Console.WriteLine("new node");
                saveS = r;//__CreateNewNode(r,null);
                r = new TwoThreeNode<T>();//new(Root)
                r.FirstSon = saveS;
                r.SecondSon = pback;
                r.FirstSon.Parent = r;//PARENT
                r.SecondSon.Parent = r;//PARENT
                r.LowOfSecond = lowback;
                r.ThirdSon = null;
                this._r = r;//IMPORTANT
                this._h += 1;
                //__ComputeH();
                return true;
            }
            //__ComputeH();
            return added;
        }
        
        public bool Add(T item){
            if(__Member(_r, item) && _count > 0)
                return false;
            return __Insert0(_r,item);
        }
        
        public void AddRange(IEnumerable<T> items){
            foreach(var item in items){
                Add(item);
            }
        }
        
        //DESCENT TO THE LEAVES.
        private void __Delete2(TwoThreeNode<T> node, T item, ref bool onlyOne, ref LinkedStack<Int32> SC, ref LinkedStack<TwoThreeNode<T>> S, ref bool deleted){
            TwoThreeNode<T> w = node;
            onlyOne = false;
            deleted = false;
            Int32 child = 1;
            while(w.FirstSon != null){
                if(_comp.Compare(item, w.LowOfSecond) < 0){
                    child = 1;
                    SC.Push(child);
                    S.Push(w);
                    w = w.FirstSon;
                    //S.Push(w);
                }
                else if(w.ThirdSon == null || _comp.Compare(item, w.LowOfSecond) < 0){
                    child = 2;
                    SC.Push(child);
                    S.Push(w);
                    w = w.SecondSon;
                    //S.Push(w);
                }
                else{
                    child = 3;
                    SC.Push(child);
                    S.Push(w);
                    w = w.ThirdSon;
                    //S.Push(w);
                }
            }//leaf.
            //wp ->> node.
            if(w.FirstSon == null){
                TwoThreeNode<T> wp = w.Parent;
                if(wp.ThirdSon != null){
                    if(_comp.Compare(item, wp.ThirdSon.Value) == 0){//EQUALS third node.
                        wp.ThirdSon = null;
                        wp.LowOfThird = default(T);
                        deleted = true;
                    }
                    else if(_comp.Compare(item, wp.SecondSon.Value) == 0){//EQAULS second node
                        wp.SecondSon = wp.ThirdSon;//3 moves to the second
                        wp.LowOfSecond = wp.LowOfThird; //update lowOfSecond
                        wp.ThirdSon = null;
                        wp.LowOfThird = default(T);//delete lowOfThird.
                        deleted = true;
                    }
                    else if(_comp.Compare(item, wp.FirstSon.Value) == 0){//EQUALS first node.
                        wp.FirstSon = wp.SecondSon;
                        wp.SecondSon = wp.ThirdSon;
                        wp.LowOfSecond = wp.LowOfThird;
                        wp.ThirdSon = null;
                        wp.LowOfThird = default(T);
                        deleted = true;
                    }
                }
                else{ //2 nodes -> onlyOne 
                    if(_comp.Compare(item, wp.SecondSon.Value) == 0){//EQAULS second node
                        wp.SecondSon = null;
                        wp.LowOfSecond = default(T);
                        onlyOne = true;
                        deleted = true;
                    }
                    else if(_comp.Compare(item, wp.FirstSon.Value) == 0){//EQUALS first node
                        wp.FirstSon = wp.SecondSon;
                        wp.LowOfSecond = default(T);
                        onlyOne = true;
                        deleted = true;
                    }
                }
            }
        }
        
        private void __Delete1(TwoThreeNode<T> node, T item, ref bool one, ref bool del, ref TwoThreeNode<T> G){
            LinkedStack<TwoThreeNode<T>> STACK = new LinkedStack<TwoThreeNode<T>>();
            LinkedStack<Int32> S2 = new LinkedStack<Int32>();
            __Delete2(node, item, ref one, ref S2, ref STACK, ref del);
            if(!one)//NO PROBLEM.
                return;
            
            TwoThreeNode<T> w;//Parent of the deleted leaf.
            S2.Pop();//delete one, to get position of w in GrandPa of the deleted leaf.
            while(!S2.IsEmpty() && !(STACK.Count == 1)){//exlucde root.
                Int32 child = S2.Top();
                S2.Pop();
                w = STACK.Top();
                STACK.Pop();
                if(child == 1){
                    //y has three children.  y - rightsibling of w.
                    if(w.Parent != null && w.Parent.SecondSon != null && w.Parent.SecondSon.ThirdSon != null){
                        TwoThreeNode<T> y = w.Parent.SecondSon;
                        w.SecondSon = y.FirstSon;
                        w.SecondSon.Parent = w;
                        w.LowOfSecond = w.SecondSon.Value;
                        
                        y.FirstSon = y.SecondSon;
                        y.SecondSon = y.ThirdSon;
                        y.LowOfSecond = y.LowOfThird;
                        y.LowOfThird = default(T);
                        y.ThirdSon = null;
                        one = false;
                        return;
                    }
                    //y has two children.
                    else if(w.Parent != null && w.Parent.SecondSon != null){
                        TwoThreeNode<T> y = w.Parent.SecondSon;
                        //Son of the w is the first son of y.
                        TwoThreeNode<T> temp = y.FirstSon;
                        y.FirstSon = w.FirstSon;
                        y.FirstSon.Parent = y;
                        
                        y.ThirdSon = y.SecondSon;
                        y.SecondSon = temp;
                        y.LowOfSecond = temp.Value;
                        y.LowOfThird = y.ThirdSon.Value;
                        TwoThreeNode<T> grandPa = w.Parent;
                        
                        //w.FirstSon = null;
                        grandPa.FirstSon = null;
                        grandPa.FirstSon = grandPa.SecondSon;
                        if(grandPa.ThirdSon == null){//only secondSon
                            one = true;
                            grandPa.LowOfSecond = default(T);
                            G = grandPa;
                            continue;
                        }
                        else{
                            grandPa.SecondSon = grandPa.ThirdSon;
                            grandPa.LowOfSecond = grandPa.LowOfThird;
                            grandPa.LowOfThird = default(T);
                            grandPa.ThirdSon = null;
                            one = false;
                            return;
                        }
                    }
                }
                else if(child == 2){//now y is 1 and w is 2.
                    //y has three children.  y - leftsibling of w.
                    if(w.Parent != null && w.Parent.FirstSon != null && w.Parent.FirstSon.ThirdSon != null){
                        TwoThreeNode<T> y = w.Parent.FirstSon;
                        //the third son of the y is the first son of the w.
                        TwoThreeNode<T> temp = w.FirstSon;
                        
                        w.FirstSon = y.ThirdSon;
                        w.FirstSon.Parent = w;
                        w.SecondSon = temp;
                        w.LowOfSecond = temp.Value;
                        y.LowOfThird = default(T);
                        y.ThirdSon = null;
                        one = false;
                        return;
                    }
                    else if(w.Parent != null && w.Parent.FirstSon != null){//y has two children
                        TwoThreeNode<T> grandPa = w.Parent;
                        
                        //z - rightsibling of w and has three children.
                        if(grandPa.ThirdSon != null && grandPa.ThirdSon.ThirdSon != null){
                            TwoThreeNode<T> z = grandPa.ThirdSon;
                            
                            //the first son of z is the second son of the w.
                            w.SecondSon = z.FirstSon;
                            w.SecondSon.Parent = w;
                            w.LowOfSecond = w.SecondSon.Value;
                            z.FirstSon = z.SecondSon;
                            z.SecondSon = z.ThirdSon;
                            z.LowOfSecond = z.LowOfThird;
                            z.LowOfThird = default(T);
                            z.ThirdSon = null;
                            one = false;
                            return;
                        }
                        else{
                            TwoThreeNode<T> y = w.Parent.FirstSon;
                            y.ThirdSon = w.FirstSon;
                            y.ThirdSon.Parent = y;
                            y.LowOfThird = w.FirstSon.Value;
                            //w.FirstSon = null;
                            
                            if(grandPa.ThirdSon == null){//one son
                                one = true;
                                grandPa.SecondSon = null;
                                grandPa.LowOfSecond = default(T);
                                G = grandPa;
                                continue;
                                //return;//GRANDPA
                            }
                            else{//two sons
                                grandPa.SecondSon = grandPa.ThirdSon;
                                grandPa.LowOfSecond = grandPa.ThirdSon.LowOfThird;
                                grandPa.ThirdSon = null;
                                grandPa.LowOfThird = default(T);
                                one = false;
                                return;
                            }
                        }
                    }
                }
                else if(child == 3){
                    if(w.Parent != null && w.Parent.SecondSon != null && w.Parent.SecondSon.ThirdSon != null){
                        TwoThreeNode<T> y = w.Parent.SecondSon;
                        
                        //the third son of the y is the first son of the w.
                        TwoThreeNode<T> temp = w.FirstSon;
                        
                        w.FirstSon = y.ThirdSon;
                        w.FirstSon.Parent = w;
                        w.SecondSon = temp;
                        w.LowOfSecond = temp.Value;
                        y.LowOfThird = default(T);
                        y.ThirdSon = null;
                        one = false;
                        return;
                    }
                    else if(w.Parent != null && w.Parent.SecondSon != null){
                        TwoThreeNode<T> y = w.Parent.SecondSon;
                        TwoThreeNode<T> grandPa = w.Parent;
                        
                        y.ThirdSon = w.FirstSon;
                        y.ThirdSon.Parent = y;
                        y.LowOfThird = w.FirstSon.Value;
                        grandPa.LowOfSecond = grandPa.ThirdSon.LowOfThird;//remove the third son. (w)
                        grandPa.ThirdSon = null;
                        grandPa.LowOfThird = default(T);
                        one = false;
                        return;//GRANDPA
                    }
                }
            }
        }
        
        private bool __Delete0(TwoThreeNode<T> r, T item){
            if(item == null)
                return false;
            if(_count == 0){
                return false;
            }
            else if(_count == 1){//only root.
                if(_comp.Compare(_r.Value,item) == 0){
                    _r = null;
                    _count -= 1;
                    return true;
                }
                return false;
            }
            else if(_count == 2){//one level tree. (root with two children)
                if(_comp.Compare(_r.FirstSon.Value, item) == 0){
                    this._r = _r.SecondSon;
                    this._r.Parent = null;
                    this._count -= 1;
                    this._h  = 0;
                    return true;
                }
                else if(_comp.Compare(_r.SecondSon.Value, item) == 0){
                    this._r = _r.FirstSon;
                    this._r.Parent = null;
                    this._count -= 1;
                    this._h = 0;
                    return true;
                }
                else
                    return false;
            }
            else if(_count == 3){//root with three children.
                if(_comp.Compare(_r.ThirdSon.Value, item) == 0){
                    this._r.LowOfThird = default(T);
                    this._r.ThirdSon = null;
                    this._count -= 1;
                    return true;
                }
                else if(_comp.Compare(_r.SecondSon.Value, item) == 0){
                    _r.LowOfSecond = _r.LowOfThird;
                    _r.LowOfThird = default(T);
                    _r.SecondSon = _r.ThirdSon;
                    _r.ThirdSon = null;
                    _count -= 1;
                    return true;
                }
                else if(_comp.Compare(_r.FirstSon.Value, item) == 0){
                    _r.LowOfSecond = _r.LowOfThird;
                    _r.LowOfThird = default(T);
                    _r.FirstSon = _r.SecondSon;
                    _r.SecondSon = _r.ThirdSon;
                    _r.ThirdSon = null;
                    _count -= 1;
                    return true;
                }
                else
                    return false;
            }
            
            bool del = false;
            bool one = false;
            TwoThreeNode<T> G = null;
            __Delete1(r, item, ref one, ref del, ref G);
            if(one){
                _count -= 1;
                this._r = G.FirstSon;
                this._r.Parent = null;
                this._h -= 1;
                //__ComputeH();
                return true;
            }
            //__ComputeH();
            return del;
        }
        
        public bool Delete(T item){
            if(!__Member(_r, item) || _count == 0)
                return false;
            return __Delete0(_r,item);
        }
        
        public T DeleteMin(){
            T val = Min();
            if(_count == 0)
                return default(T);
            __Delete0(_r,val);
            return val;
        }
        
        //CHECK STATEMENTS(_h -= 1; h += 1;) IN METHODS __Delete0, __Insert0
        //IF DISTINCT(__ComputeH, THE STATEMENTS) DELETE STATEMENTS AND UNCOMMENT METHOD __ComputeH
        //ELSE DELETE METHOD __ComputeH
        /*
        private void __ComputeH(){
            if(_count <= 1){
                _h = 0;
            }
            else if(_count > 1 && _count <= 3){
                _h = 1;
            }
            else {
                TwoThreeNode<T> node = _r;
                Int32 h = 0;
                while(node.FirstSon != null){
                    node = node.FirstSon;
                    h++;
                }
                _h = h;
            }
        }*/
        #region setOperators
        
        public TwoThreeTree<T> Intersection(TwoThreeTree<T> B){
            if(B == null || B.Count == 0){
                return new TwoThreeTree<T>(_comp);//EMPTY TREE
            }
            ITree<T> T2 = (ITree<T>) B;
            TwoThreeTree<T> C = new TwoThreeTree<T>(_comp);
            
            LinkedStack<T> S1 = new LinkedStack<T>();//CHECK FOR S = Stack<T> 
            LinkedStack<T> S2 = new LinkedStack<T>();
            
            this._visitor.PostOrder(this,(n) => __NodesVals(n,ref S1));//SYMMETRIC ORDER (INORDER)
            this._visitor.PostOrder(T2,(n) => __NodesVals(n,ref S2));
            while( !S1.IsEmpty() && !S2.IsEmpty()){
                if(_comp.Compare(S1.Top(), S2.Top()) == 0){
                    C.Add(S1.Top());
                    S1.Pop();
                    S2.Pop();
                }
                else if(_comp.Compare(S1.Top(), S2.Top()) < 0){
                    S2.Pop();
                }
                else{
                    S1.Pop();
                }
            }
            
            return C;//CHANGE TYPE -> MAKE NEW METHOD.
        }
        
        public TwoThreeTree<T> Union(TwoThreeTree<T> B){
            if(B == null || B.Count == 0){
                TwoThreeTree<T> R = new TwoThreeTree<T>(_comp);
                R._r = this._r;
                R._count = this._count;
                R._h = this._h;
                return R;
            }
            ITree<T> T2 = (ITree<T>) B;
            TwoThreeTree<T> C = new TwoThreeTree<T>(_comp);
            
            LinkedStack<T> S1 = new LinkedStack<T>();//CHECK FOR S = Stack<T> 
            LinkedStack<T> S2 = new LinkedStack<T>();
            
            this._visitor.PostOrder(this,(n) => __NodesVals(n,ref S1));//SYMMETRIC ORDER (INORDER)
            this._visitor.PostOrder(T2,(n) => __NodesVals(n,ref S2));
            while(!S1.IsEmpty()){
                C.Add(S1.Top());
                S1.Pop();
            }
            while(!S2.IsEmpty()){
                C.Add(S2.Top());
                S2.Pop();
            }
            return C;
        }
        
        public TwoThreeTree<T> Difference(TwoThreeTree<T> B){
            if(B == null || B.Count == 0){
                TwoThreeTree<T> R = new TwoThreeTree<T>(_comp);
                R._r = this._r;
                R._count = this._count;
                R._h = this._h;
                return R;
            }
            ITree<T> T2 = (ITree<T>) B;
            TwoThreeTree<T> C = new TwoThreeTree<T>(_comp);
            
            LinkedStack<T> S1 = new LinkedStack<T>();//CHECK FOR S = Stack<T> 
            LinkedStack<T> S2 = new LinkedStack<T>();
            
            this._visitor.PostOrder(this,(n) => __NodesVals(n,ref S1));//SYMMETRIC ORDER (INORDER)
            this._visitor.PostOrder(T2,(n) => __NodesVals(n,ref S2));
            bool flag = true;
            while( !S1.IsEmpty()){
                if(flag && S2.IsEmpty()){
                    flag = false;
                }
                if(flag && _comp.Compare(S1.Top(), S2.Top()) == 0){
                    S1.Pop();
                    S2.Pop();
                }
                else if(flag && _comp.Compare(S1.Top(), S2.Top()) > 0){
                    C.Add(S1.Top());
                    S1.Pop();
                }
                else{
                    if(flag){
                        S2.Pop();
                    }
                    else{
                        C.Add(S1.Top());
                        S1.Pop();
                    }
                }
            }
            
            return C;
        }
        
        public TwoThreeTree<T> SymmetricDifference(TwoThreeTree<T> B){
            TwoThreeTree<T> ANB = Difference(B);//A\B
            TwoThreeTree<T> BNA = B.Difference(this);//B\A
            return ANB.Union(BNA);
        }
        
        public TwoThreeTree<T> Merge(TwoThreeTree<T> B){
            if(Intersection(B)._count != 0){
                return null;//UNDEFINED
            }
            return Union(B);
        }
        
        //RETURNS SortedLinkedList<T> INSTEAD OF 2-3 TREE
        public SortedLinkedList<T> IntersectionList(TwoThreeTree<T> B){
            if(B == null || B.Count == 0){
                return new SortedLinkedList<T>(_comp);
            }
            ITree<T> T2 = (ITree<T>) B;
            
            
            CSharpDataStructures.Structures.Lists.LinkedList<T> C = new CSharpDataStructures.Structures.Lists.LinkedList<T>();
            
            LinkedStack<T> S1 = new LinkedStack<T>();//CHECK FOR S = Stack<T> 
            LinkedStack<T> S2 = new LinkedStack<T>();
            
            this._visitor.PostOrder(this,(n) => __NodesVals(n,ref S1));//POST ORDER ()
            this._visitor.PostOrder(T2,(n) => __NodesVals(n,ref S2));
            while( !S1.IsEmpty() && !S2.IsEmpty()){
                if(_comp.Compare(S1.Top(), S2.Top()) == 0){
                    C.Add(S1.Top());
                    S1.Pop();
                    S2.Pop();
                }
                else if(_comp.Compare(S1.Top(), S2.Top()) < 0){
                    S2.Pop();
                }
                else{
                    S1.Pop();
                }
            }
            
            return new SortedLinkedList<T>((IEnumerable<T>)C, _comp);
        }
        
        public SortedLinkedList<T> UnionList(TwoThreeTree<T> B){
            ITree<T> T2 = (ITree<T>) B;
            CSharpDataStructures.Structures.Lists.LinkedList<T> C = new CSharpDataStructures.Structures.Lists.LinkedList<T>();
            
            LinkedStack<T> S1 = new LinkedStack<T>();//CHECK FOR S = Stack<T> 
            LinkedStack<T> S2 = new LinkedStack<T>();
            
            this._visitor.PostOrder(this,(n) => __NodesVals(n,ref S1));//POST ORDER (INORDER)
            this._visitor.PostOrder(T2,(n) => __NodesVals(n,ref S2));
            while(!S1.IsEmpty()){
                C.Add(S1.Top());
                S1.Pop();
            }
            while(!S2.IsEmpty()){
                C.Add(S2.Top());
                S2.Pop();
            }
            return new SortedLinkedList<T>((IEnumerable<T>)C, _comp);
        }
        
        public SortedLinkedList<T> DifferenceList(TwoThreeTree<T> B){
            ITree<T> T2 = (ITree<T>) B;
            CSharpDataStructures.Structures.Lists.LinkedList<T> C = new CSharpDataStructures.Structures.Lists.LinkedList<T>();
            
            LinkedStack<T> S1 = new LinkedStack<T>();//CHECK FOR S = Stack<T> 
            LinkedStack<T> S2 = new LinkedStack<T>();
            
            this._visitor.PostOrder(this,(n) => __NodesVals(n,ref S1));//POST ORDER (INORDER)
            this._visitor.PostOrder(T2,(n) => __NodesVals(n,ref S2));
            bool flag = true;
            while( !S1.IsEmpty()){
                if(flag && S2.IsEmpty()){
                    flag = false;
                }
                if(flag && _comp.Compare(S1.Top(), S2.Top()) == 0){
                    S1.Pop();
                    S2.Pop();
                }
                else if(flag && _comp.Compare(S1.Top(), S2.Top()) > 0){
                    C.Add(S1.Top());
                    S1.Pop();
                }
                else{
                    if(flag){
                        S2.Pop();
                    }
                    else{
                        C.Add(S1.Top());
                        S1.Pop();
                    }
                }
            }
            
            return new SortedLinkedList<T>((IEnumerable<T>)C, _comp);
        }
        
        public SortedLinkedList<T> SymmetricDifferenceList(TwoThreeTree<T> B){
            SortedLinkedList<T> ANB = UnionList(B);//A OR B
            SortedLinkedList<T> BNA = IntersectionList(B);//A AND B.
            
            return ANB.Difference(BNA);//A OR B - (A AND B)
        }
        
        public SortedLinkedList<T> MergeList(TwoThreeTree<T> B){
            if(IntersectionList(B).Count != 0){
                return null;//UNDEFINED
            }
            return UnionList(B);
        }
        
        
        #endregion
        
        #region ISet
        
        public bool Overlaps(IEnumerable<T> other){
            TwoThreeTree<T> B = new TwoThreeTree<T>(_comp);
            B.AddRange(other);
            B = Intersection(B);//A AND B
            
            return B._count != 0;
        }
        
        public void IntersectWith(IEnumerable<T> other){
            TwoThreeTree<T> B = new TwoThreeTree<T>(_comp);
            B.AddRange(other);
            B = Intersection(B);
            if(B._count == 0){
                Clear();
                return;
            }
            Clear();
            AddRange((IEnumerable<T>) B);
        }
        
        public void UnionWith(IEnumerable<T> other){
            AddRange(other);
        }
        
        public void ExceptWith(IEnumerable<T> other){
            if(other.Count() == 0){
                return;
            }
            TwoThreeTree<T> B = new TwoThreeTree<T>(_comp);
            B.AddRange(other);
            B = Difference(B);
            Clear();
            AddRange((IEnumerable<T>) B);
        }
        
        public void SymmetricExceptWith(IEnumerable<T> other){
            if(other.Count() == 0){
                return;
            }
            TwoThreeTree<T> B = new TwoThreeTree<T>(_comp);
            B.AddRange(other);
            B = SymmetricDifference(B);
            Clear();
            AddRange((IEnumerable<T>) B);
        }
        
        public bool SetEquals(IEnumerable<T> other){
            if(other == null){
                return false;
            }
            TwoThreeTree<T> B = new TwoThreeTree<T>(_comp);
            B.AddRange(other);
            return Equals(B);
        }
        
        public bool IsSubsetOf(IEnumerable<T> other){
            if(other.Count() == 0 && _count == 0){
                return true;
            }
            else if(other.Count() == 0 && _count != 0){
                return false;
            }
            TwoThreeTree<T> B = new TwoThreeTree<T>(_comp);
            B.AddRange(other);
            B = Difference(B);//A\B.
            Int32 c = B._count;
            if(c == 0){
                return true;
            }
            return false;
        }
        
        public bool IsProperSubsetOf(IEnumerable<T> other){
            if(other.Count() == 0){
                return false;
            }
            TwoThreeTree<T> B = new TwoThreeTree<T>(_comp);
            B.AddRange(other);
            TwoThreeTree<T> C = Difference(B);//A\B.
            Int32 c = C._count;
            if(c == 0 && B._count > _count){
                return true;
            }
            return false;
        }
        
        public bool IsSupersetOf(IEnumerable<T> other){
            if(other.Count() == 0){
                return true;
            }
            TwoThreeTree<T> B = new TwoThreeTree<T>(_comp);
            B.AddRange(other);
            TwoThreeTree<T> C = B.Difference(this);//B\A.
            Int32 c = C._count;
            if(c == 0){
                return true;
            }
            return false;
        }
        
        public bool IsProperSupersetOf(IEnumerable<T> other){
            if(other.Count() == 0 && _count == 0){
                return false;
            }
            else if(other.Count() == 0){
                return true;
            }
            TwoThreeTree<T> B = new TwoThreeTree<T>(_comp);
            B.AddRange(other);
            TwoThreeTree<T> C = B.Difference(this);//B\A.
            Int32 c = C._count;
            if(c == 0 && B._count < _count){
                return true;
            }
            return false;
        }
        #endregion
        
        #region ITree
        
        public Node<T> Root(){
            return _r;
        }
        
        public T Value(Node<T> node){
            return node.Value;
        }
        
        public Node<T> Parent(Node<T> node){
            TwoThreeNode<T> np = node as TwoThreeNode<T>;
            if(np == null){//OR np.Parent
                return null;
            }
            return np.Parent;
        }
        
        public Node<T> LeftMostChild(Node<T> node){
            TwoThreeNode<T> np = node as TwoThreeNode<T>;
            if(np == null){
                return null;
            }
            return np.FirstSon;
        }
        
        public Node<T> RightSibling(Node<T> node){
            TwoThreeNode<T> np = node as TwoThreeNode<T>;
            if(np == null || np.Parent == null){
                return null;
            }
            TwoThreeNode<T> pa = np.Parent;
            if(pa.FirstSon.Equals(np)){
                return pa.SecondSon;
            }
            else if(pa.SecondSon.Equals(np)){
                return pa.ThirdSon;
            }
            return null;
        }
        
        public Int32 GetCount(){
            return this._count;
        }
        
        public void SetVisitor(IVisitor<T> visitor){
            this._visitor = visitor;
        }
        
        //MAKENULL
        public void Clear(){
            this._count = 0;
            this._h = 0;
            this._r = null;
        }
        
        #endregion
        
        public Int32 Height{
            get{
                return _h;
            }
        }
        
        #region ICollection
        public Int32 Count{
            get{
                return _count;
            }
        }
        
        public bool IsReadOnly{
            get{
                return false;
            }
        }
        
        void ICollection<T>.Add(T item){
            bool v = Add(item);
        }
        
        public bool Remove(T item){
            return Delete(item);
        }
        
        public bool Contains(T item){
            return __Member(_r, item);
        }
        
        public void CopyTo(T[] array,Int32 arrayIndex){
            CSharpDataStructures.Structures.Lists.LinkedList<T> L = new CSharpDataStructures.Structures.Lists.LinkedList<T>();
            this._visitor.PostOrder(this,(n) => __NodesVals(n,ref L));
            L.CopyTo(array,arrayIndex);
        }
        
        #endregion
        
        public T Min(){
            if(_count == 0)
                return default(T);
            if(_count == 1)
                return _r.Value;
            
            TwoThreeNode<T> node = _r;
            while(node.FirstSon != null){
                node = node.FirstSon;
            }
            return node.Value;
        }
        
        public T Max(){
            if(_count == 0)
                return default(T);
            if(_count == 1)
                return _r.Value;
            
            TwoThreeNode<T> node = _r;
            while(node.FirstSon != null){
                node = node.ThirdSon ?? node.SecondSon;
            }
            return node.Value;
        }
        
        #region 2-3_TREE
        
        public override String ToString(){
            StringBuilder sb = new StringBuilder();
            sb.Append("{ ");
            if(_count == 0){
                sb.Append("}");
                return sb.ToString();
            }
            this._visitor.PostOrder(this,(n) => __PrintVal(n,ref sb));//sorted.
            sb.Append("}");
            return sb.ToString();
        }
        
        private void __PrintVal(Node<T> n,ref StringBuilder sb){
            TwoThreeNode<T> np = n as TwoThreeNode<T>;
            sb.Append(np.FirstSon == null ? n.Value.ToString()+" " : "");//Print values of the leaves of the TREE.
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public IEnumerator<T> GetEnumerator(){
            CSharpDataStructures.Structures.Lists.LinkedList<T> L = new CSharpDataStructures.Structures.Lists.LinkedList<T>();
            this._visitor.PostOrder(this,(n) => __NodesVals(n,ref L));//POST ORDER (LEAVES WILL BE ADDED)
            return L.GetEnumerator();
        }
        
        
        
        public Boolean Equals(TwoThreeTree<T> B){
            if(B == null)
                return false;
            if(IntersectionList(B).Count == _count)
                return true;
            else
                return false;
        }
        
        public override Boolean Equals(Object obj){
            if(obj == null)
                return false;
            TwoThreeTree<T> B = obj as TwoThreeTree<T>;
            if(B == null)
                return false;
            else
                return Equals(B);
        }
        
        //ASC ORDER
        private void __NodesVals(Node<T> n, ref CSharpDataStructures.Structures.Lists.LinkedList<T> L){
            TwoThreeNode<T> np = n as TwoThreeNode<T>;
            if(np.FirstSon == null){
                L.Add(np.Value);
            }
        }
        
        //DESC ORDER.
        private void __NodesVals(Node<T> n, ref CSharpDataStructures.Structures.Lists.LinkedStack<T> S){
            TwoThreeNode<T> np = n as TwoThreeNode<T>;
            if(np.FirstSon == null)
                S.Push(np.Value);
        }
        
        #endregion
        
        #region operators
        public static implicit operator SortedLinkedList<T>(TwoThreeTree<T> A){
            CSharpDataStructures.Structures.Lists.LinkedList<T> L = new CSharpDataStructures.Structures.Lists.LinkedList<T>();
            if(A == null || A.Count == 0)
                return new SortedLinkedList<T>(A._comp);
            A._visitor.PostOrder(A,(n) => A.__NodesVals(n,ref L));
            return new SortedLinkedList<T>((IEnumerable<T>) L,A._comp);
        }
        
        public static implicit operator TwoThreeTree<T>(SortedLinkedList<T> A){
            TwoThreeTree<T> R = new TwoThreeTree<T>();
            if(A == null || A.Count == 0)
                return R;
            R.AddRange((IEnumerable<T>) A);
            return R;
        }
        
        #endregion
    }
}