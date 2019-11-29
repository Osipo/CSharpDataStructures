using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using STACK = CSharpDataStructures.Structures.Lists.LinkedStack<CSharpDataStructures.Vectors.Matrix>;
namespace CSharpDataStructures.Vectors {
    enum MatrixReduce { ByRow = 1, ByColumn = 2}
    class Matrix : IDisposable, IEnumerable<Double> {
        private Double[][] _base;
        private Int32 _rowsc;
        private Int32 _columnsc;
        private Double _det;
        private Int32 _rank;
        private Boolean _changedM; //after changing elem of M -> compute Determinant again.
        
        //Get max index of subArray. (A[][]) REPLACE 
        private Int32 __returnMaxIndex2D(Double[][] a){
            Int32 t = 0;
            Int32 t2 = -1;
            foreach(Double[] ar in a){
                t = ar.Length;
                if(t > t2){
                    t2 = t;
                }
            }
            return t2;
        }
        
        #region .ctors
        //isInternal private parameter for one public .ctor Matrix(Double[][] A)
        private Matrix(Double[][] array, Int32 rows, Int32 columns, Boolean isInternal){
            //If rows || columns > array.UpperBounds
            if(array.Length <= rows)
                rows = array.Length;
            Int32 idx = __returnMaxIndex2D(array);//MOVE IN IF
            if(idx <= columns || isInternal) //For public .ctor Matrix(Double[][] array) columns = 0, so set it = idx.
                columns = idx;
            if(rows <= 0)
                throw new ArgumentOutOfRangeException("rows");
            if(columns <= 0)
                throw new ArgumentOutOfRangeException("columns");
            this._base = new Double[rows][];
            for(int i = 0; i < _base.Length; i++){
                _base[i] = new Double[columns];
                for(int j = 0; j < columns;j++){//GetUpperBound need for jagged arrays
                    _base[i][j] = j <= array[i].GetUpperBound(0) ? array[i][j] : 0; //if not presented -> 0.
                }
            }
            
            this._rowsc = rows;
            this._columnsc = columns;
            this._det = Double.PositiveInfinity; //+inf
            this._changedM = false;
            this._rank = -1;
        }
        
        
        
        public Matrix(Double[][] array,Int32 rows, Int32 columns) : this(array,rows,columns,false) {}
        
        public Matrix(Double[][] array, Int32 columns) : this(array,array.Length,columns,false) {}
        
        public Matrix(Double[][] array) : this(array,array.Length,0,true) {}//MaxIndexOf subArray == columns
        
        //Create Zero Matrix rows*columns
        public Matrix(Int32 rows, Int32 columns){
            if(rows <= 0)
                throw new ArgumentOutOfRangeException("rows");
            if(columns <= 0)
                throw new ArgumentOutOfRangeException("columns");
            
            this._base = new Double[rows][];
            for(int i = 0; i < rows; i++){
                this._base[i] = new Double[columns];
            }
            this._rowsc = rows;
            this._columnsc = columns;
            this._det = Double.PositiveInfinity;
            this._changedM = false;
        }
        
        
        public Matrix(Matrix a){
            this._base = a.ToArray();
            this._rowsc = a.ToArray().Length;
            this._columnsc = a.ToArray()[0].Length;
            this._det = a.Determinant;
            this._changedM = false;
            this._rank = a.Rank;
        }
        
        //Get Matrix from Matrix a without row_i and column_j.  HANDLE PARAMS
        public Matrix(Matrix a, Int32 i, Int32 j){
            Int32 t1 = a.GetRowsCount() - 1;
            Int32 t2 = a.GetColumnsCount() - 1;
            Int32 t3 = t1 + 1;
            Int32 t4 = t2 + 1; 
            Double[][] nb = new Double[t1][];
            Int32 k = 0; Int32 kk = 0;//k is the offset on row, kk is the offset on column.
            for(Int32 ii = 0; ii < t3; ii++){
                if(ii == i){
                    k = 1;
                    continue;
                }
                nb[ii - k] = new Double[t2];
                //Console.WriteLine(ii - k);
                for(Int32 jj = 0; jj < t4; jj++){
                    if(jj == j){
                        kk = 1;
                        continue;
                    }
                    nb[ii - k][jj - kk] = a[ii,jj];
                }
                kk = 0;
            }
            this._base = nb;
            this._rowsc = t1;
            this._columnsc = t2;
            this._det = Double.PositiveInfinity;
            this._rank = -1;
            this._changedM = false;
        }
        
        //Get Matrix from Matrix a without row_i.  HANDLE PARAMS
        public Matrix(Matrix a, Int32 row_i){
            Int32 t1 = a.GetRowsCount() - 1;
            Int32 t2 = a.GetColumnsCount();
            if(t1 == 0)
                return;
            Int32 t3 = t1 + 1;
            Int32 k = 0;
            Double[][] nb = new Double[t1][];
            for(Int32 i = 0; i < t3; i++){
                if(i == row_i){
                    k = 1;
                    continue;
                }
                nb[i - k] = new Double[t2];
                for(Int32 j = 0; j < t2; j++){
                    nb[i - k][j] = a[i,j];
                }
                
            }
            
            this._base = nb;
            this._rowsc = t1;
            this._columnsc = t2;
            this._det = Double.PositiveInfinity;
            this._changedM = false;
            this._rank = -1;
        }
        
        #endregion
        
        public static Matrix M(Matrix a, Int32 c_i,MatrixReduce mode){
            if(mode == MatrixReduce.ByRow)
                return new Matrix(a,c_i);
            Int32 t1 = a.GetRowsCount();
            Int32 t2 = a.GetColumnsCount() - 1;
            if(t2 == 0)
                return null;
            Int32 t3 = t2 + 1;
            Int32 k = 0;
            Double[][] nb = new Double[t1][];
            for(Int32 i = 0; i < t1; i++){
                nb[i] = new Double[t2];
                for(Int32 j = 0; j < t3; j++){
                    if(j == c_i){
                        k = 1;
                        continue;
                    }
                    nb[i][j - k] = a[i,j];
                }
            }
            
            return new Matrix(nb);
        }
        
        ~Matrix(){
            Dispose(false);
        }
        
        //Return copy of _base Array. It is NOT reference.
        public Double[][] ToArray(){
            Double[][] ar = new Double[_base.Length][];
            for(int i = 0; i < _base.Length; i++){
                ar[i] = new Double[_base[i].Length];
                for(int j = 0; j < _base[i].Length; j++){
                    ar[i][j] = _base[i][j];
                }
            }
            return ar;
        }
        
        
        public void ScalarMul(Double c){
            for(int i = 0; i < _base.Length; i++){
                Double[] a = _base[i];
                for(int j = 0; j < a.Length; j++){
                    a[j] *= c;
                }
            }    
            if(_rowsc == _columnsc && !Double.IsPositiveInfinity(_det)){
                this._det *= Math.Pow(c,_rowsc);
            }
            _rank = -1;
        }
        
        //Return changed Matrix. M^t
        public Matrix Transpose(){
            Double[][] nbase = new Double[_columnsc][];
            for(Int32 i = 0; i < _columnsc; i++){
                nbase[i] = GetColumn(i);
            }
            
            Int32 t = _columnsc;
            this._columnsc = this._rowsc;
            this._rowsc = t;
            this._base = nbase;
            return this;
        }
        
        //Get Matrix from the current Matrix with rows from r1 to r2 [r1,r2] and columns from c1 to c2 [c1,c2]  HANDLE PARAMS
        public Matrix GetSubMatrix(Int32 r1, Int32 c1, Int32 r2, Int32 c2){
            Int32 t1 = r2 - r1 + 1;
            Int32 t2 = c2 - c1 + 1;
            Double[][] nb = new Double[t1][];
            for(Int32 i = r1, k = 0; i <= r2; i++, k++){
                nb[k] = new Double[t2];
                for(Int32 j = c1, kk = 0; j <= c2; j++,kk++){
                    nb[k][kk] = this[i,j];
                }
            }
            return new Matrix(nb);
        }
        
        public Matrix Add(Matrix B){
            if(B.GetColumnsCount() != this.GetColumnsCount() || B.GetRowsCount() != this.GetRowsCount())
                throw new ArgumentException("Addition is allowed for only matrixes of the identical size.","B");
            
            Double[][] t = this.ToArray();
            for(Int32 i = 0; i < _rowsc; i++){
                Double[] a = B.GetRow(i);
                for(Int32 j = 0; j < _columnsc; j++){
                    //this[i,j] += a[j];
                    t[i][j] += a[j];
                    
                }
            }
            //_changedM = true;
            //_rank = -1;
            //return this;
            return new Matrix(t);
        }
        
        public Matrix Mull(Matrix B){
            if(this.GetColumnsCount() != B.GetRowsCount())
                throw new ArgumentException("Count of Columns in Matrix A must be equal to count of rows in Matrix B.","B");
            
            Double[][] nb = new Double[_rowsc][];
            Int32 t = B.GetColumnsCount();
            for(Int32 i = 0; i < _rowsc; i++){
                nb[i] = new Double[t];
                for(Int32 j = 0; j < t; j++){
                    for(Int32 k = 0; k < _columnsc; k++){
                        nb[i][j] += this[i,k]*B[k,j];
                    }
                }
            }
            /*
            this._columnsc = B.GetColumnsCount();
            this._base = nb;
            this._rank = -1;
            if(!(Double.IsPositiveInfinity(_det) || Double.IsPositiveInfinity(B.Determinant)))
                this._det *= B.Determinant;
            */
            //return this;
            Matrix AB = new Matrix(nb);
            if(!(Double.IsPositiveInfinity(this._det) || Double.IsPositiveInfinity(B.Determinant)))
                AB._det = this._det * B.Determinant;
            return AB;
        }
        
        private Boolean __HasZeroRow(Double[] row){
            foreach(Double item in row)
                if(item != 0)
                    return false;
            return true;
        }
        
        //Recursive Descend by Row.  
        //MAKE NON-RECURSIVE
        private Double __GetDeterminant(){
            if(!this.Squared)
                return Double.PositiveInfinity;
            
            Double r = 0d;
            
            if(_rowsc == 3){
                return (this[0,0]*this[1,1]*this[2,2])+(this[0,1]*this[1,2]*this[2,0])+(this[1,0]*this[2,1]*this[0,2])
                    - (this[0,2]*this[1,1]*this[2,0]) - (this[0,0]*this[1,2]*this[2,1]) - (this[0,1]*this[1,0]*this[2,2]);
            }
            else if(_rowsc == 2){
                return (this[0,0]*this[1,1]) - (this[1,0]*this[0,1]);
            }
            
            for(Int32 i = 0; i < _rowsc; i++){
                if(__HasZeroRow(_base[i]))
                    return 0d;
            }
            
            for(Int32 i = 0; i < _rowsc; i++){
                if(this[i,0] == 0)
                    continue;//skip zeros.
                Double d = Math.Pow(-1,(i+1)+1);//(-1)^i+j. where j == 1 and i+1 == i from N.
                Matrix RJ = new Matrix(this,i,0);
                r+= d*this[i,0]*RJ.__GetDeterminant();
            }
            return r;
        }
        
        private Double __GetNRDeterminant(){
            if(!this.Squared)
                return Double.PositiveInfinity;
            
            Double r = 0d;
            
            if(_rowsc == 3){
                return (this[0,0]*this[1,1]*this[2,2])+(this[0,1]*this[1,2]*this[2,0])+(this[1,0]*this[2,1]*this[0,2])
                    - (this[0,2]*this[1,1]*this[2,0]) - (this[0,0]*this[1,2]*this[2,1]) - (this[0,1]*this[1,0]*this[2,2]);
            }
            else if(_rowsc == 2){
                return (this[0,0]*this[1,1]) - (this[1,0]*this[0,1]);
            }
            
            for(Int32 i = 0; i < _rowsc; i++){
                if(__HasZeroRow(_base[i]))
                    return 0d;
            }
            STACK S = new STACK();
            return -1d;
        }
        
        //clear matrix from 0..0 rows.
        private Matrix __ClearZeros(){
            Matrix M = new Matrix(this.ToArray());
            for(Int32 i = 0; i < _rowsc; i++){
                if(__HasZeroRow(_base[i]))
                    M = new Matrix(M,i);
                if(__HasZeroRow(M.GetColumn(i)))
                    M = Matrix.M(M,i,MatrixReduce.ByColumn);
            }
            return M;
        }
        
        private Int32 __CountNonZeroRows(){
            Int32 r = 0;
            for(Int32 i = 0; i < _rowsc; i++){
                if(__HasZeroRow(_base[i]))
                    continue;
                r+=1;
            }
            return r;
        }
        
        //Descending Mean Algorithm for Squared Matrix.
        //heuristic !!!
        private Int32 __GetSRank(){
            Int32 k = _rowsc;
            Matrix T = this;
            if(T.Determinant != 0)//M_k is not zero.
                return k;
            while(k > 1){
                for(Int32 t1 = 0; t1 < _rowsc; t1++){
                    for(Int32 t2 = 0; t2 < _columnsc; t2++){
                        Matrix TT = new Matrix(T,t1,t2);
                        if(TT.Determinant != 0)
                            return k - 1;//Exists M_k minor where any Minor_k+1 == 0
                    }
                }
                T = new Matrix(T,0,0);//REPLACE 
                k -= 1;
            }
            return k;
        }
        
        private void __MoveRowTo_i(Int32 pos){
            Int32 k = pos;
            while(k < _rowsc && _base[k][pos] == 0)
                k++;
            if(k == _rowsc)
                return;
            this.SwapRows(pos,k);
        }
        
        //Gauss method. for squared-matrix. HANDLE PRECISION
        private Matrix __GetDMatrix(){
            Matrix T = new Matrix(this.ToArray());
            for(Int32 j = 0; j < T._columnsc - 1; j++){
                
                T.__MoveRowTo_i(j);//make first column 1,0,0..0 :[0..n]
                Double c = T[0,j] != 0 ? T[0,j] : 1d;
                T.MullRow(j,1d/c);//multiply first row on 1/column_first_element. -> PRECISION
                for(Int32 i = j; i < T._rowsc - 1; i++){//first column.
                    if(T[i+1,j] == 0){
                        continue;
                    }
                    
                    T.AdditionRows(j,i+1,-T[i+1,j]);//make first elem of row_i == 0.
                }
                
            }
            T.MullRow(T._rowsc - 1,T[T._rowsc - 1,T._columnsc - 1]);
            return T;
        }
        
        
        private Int32 __GetRank(){
            Matrix T = __ClearZeros();//Remove 0..0 rows.
            if(T == null || T.GetRowsCount() == 0 || T.GetColumnsCount() == 0) //Zero-Matrix.
                return 0;
                
            if(T.GetRowsCount() == 1 || T.GetColumnsCount() == 1){//One-row. (vector-row/column)
                return 1;
            }
            if(T.Squared){
                //return T.__GetDMatrix().__CountNonZeroRows(); BY PRECISION
                return T.__GetSRank();
            }
            Int32 t1 = T.GetRowsCount();
            Int32 t2 = T.GetColumnsCount();
            
            //Make square Matrix with additional zero rows/columns.
            while(t1 < t2){
                T = T.AddRow(new Double[t2]);
                t1+=1;
            }
            while(t1 > t2){
                T = T.AddColumn(new Double[t1]);
                t2+=1;
            }
            
            //return T.__GetDMatrix().__CountNonZeroRows(); BY PRECISION
            return T.__GetSRank();
        }
        
        
        
        public Matrix GetInverse(){
            if(IsSpecial){
                return null;
            }
            
            Double[][] nb = new Double[_rowsc][];
            for(Int32 i = 0; i < _rowsc; i++){
                nb[i] = new Double[_columnsc];
                for(Int32 j = 0; j < _columnsc; j++){
                    Double d = Math.Pow(-1,(i+1)+(j+1));//(-1)^i+j
                    Double Mij = (new Matrix(this,i,j)).Determinant;
                    nb[i][j] = d*Mij;
                }
            }
            Matrix Ally = new Matrix(nb);
            Ally.Transpose();
            Ally.ScalarMul(1d/this.Determinant);//round
            return Ally;
        }
        
        #region Det_by_LUP_Decomposition
        
        public Double[][] __MatrixDecompose(out int[] perm, out int toggle)
        {
          // Doolittle LUP decomposition with partial pivoting.
          // returns: result is L (with 1s on diagonal) and U;
          // perm holds row permutations; toggle is +1 or -1 (even or odd)
          int rows = _base.Length;
          int cols = _base[0].Length; 
          if (rows != cols)
            throw new Exception("Non-square mattrix");

          int n = rows; // convenience

          double[][] result = this.ToArray(); // 

          perm = new int[n]; // set up row permutation result
          for (int i = 0; i < n; ++i) { perm[i] = i; }

          toggle = 1; // toggle tracks row swaps

          for (int j = 0; j < n - 1; ++j) // each column
          {
            double colMax = Math.Abs(result[j][j]); 
            int pRow = j;
            //for (int i = j + 1; i < n; ++i) // deprecated
            //{
            //  if (result[i][j] > colMax)
            //  {
            //    colMax = result[i][j];
            //    pRow = i;
            //  }
            //}

            for (int i = j + 1; i < n; ++i) // reader Matt V needed this:
            {
              if (Math.Abs(result[i][j]) > colMax)
              {
                colMax = Math.Abs(result[i][j]);
                pRow = i;
              }
            }
            // Not sure if this approach is needed always, or not.

            if (pRow != j) // if largest value not on pivot, swap rows
            {
              double[] rowPtr = result[pRow];
              result[pRow] = result[j];
              result[j] = rowPtr;

              int tmp = perm[pRow]; // and swap perm info
              perm[pRow] = perm[j];
              perm[j] = tmp;

              toggle = -toggle; // adjust the row-swap toggle
            }

            // -------------------------------------------------------------
            // This part added later (not in original code) 
            // and replaces the 'return null' below.
            // if there is a 0 on the diagonal, find a good row 
            // from i = j+1 down that doesn't have
            // a 0 in column j, and swap that good row with row j

            if (result[j][j] == 0.0)
            {
              // find a good row to swap
              int goodRow = -1;
              for (int row = j + 1; row < n; ++row)
              {
                if (result[row][j] != 0.0)
                  goodRow = row;
              }

              if (goodRow == -1)
                throw new Exception("Cannot use Doolittle's method");

              // swap rows so 0.0 no longer on diagonal
              double[] rowPtr = result[goodRow];
              result[goodRow] = result[j];
              result[j] = rowPtr;

              int tmp = perm[goodRow]; // and swap perm info
              perm[goodRow] = perm[j];
              perm[j] = tmp;

              toggle = -toggle; // adjust the row-swap toggle
            }
            // -------------------------------------------------------------

            //if (Math.Abs(result[j][j]) < 1.0E-20) // deprecated
            //  return null; // consider a throw

            for (int i = j + 1; i < n; ++i)
            {
              result[i][j] /= result[j][j];
              for (int k = j + 1; k < n; ++k)
              {
                result[i][k] -= result[i][j] * result[j][k];
              }
            }

          } // main j column loop

          return result;
        } // MatrixDecompose
        
        public Double MatrixDeterminant(){
            int[] perm;
            int toggle;
            double[][] lum = __MatrixDecompose(out perm, out toggle);
            if (lum == null)
                throw new Exception("Unable to compute MatrixDeterminant");
            double result = toggle;
            for (int i = 0; i < lum.Length; ++i)
                result *= lum[i][i];
            return result;
        }
        
        #endregion
        #region Base Transformations
        
        public void SwapRows(Int32 r1, Int32 r2){
            if(r1 < 0 || r1 >= _rowsc)
                throw new  ArgumentOutOfRangeException("r1");
            if(r2 < 0 || r2 >= _rowsc)
                throw new  ArgumentOutOfRangeException("r2");
            Double[] t = this.GetRow(r1);//new array.
            this._base[r1] = this._base[r2];
            this._base[r2] = t;
            if(!Double.IsPositiveInfinity(_det))
                this._det = -this._det;
        }
        
        public void MullRow(Int32 row, Double c){
            Double[] t = this.GetRow(row);
            for(Int32 i = 0; i < _columnsc; i++){
                t[i] *= c;
            }
            this._base[row] = t;
            if(!Double.IsPositiveInfinity(_det)){
                this._det *= c;
            }
        }
        public void AdditionRows(Int32 r1, Int32 r2, Double c){
            if(r1 < 0 || r1 >= _rowsc)
                throw new  ArgumentOutOfRangeException("r1");
            if(r2 < 0 || r2 >= _rowsc)
                throw new  ArgumentOutOfRangeException("r2");
                
            Double[] t = this.GetRow(r1);
            for(Int32 i = 0; i < _columnsc; i++){
                t[i] *= c;
            }
            Double[] t2 = this.GetRow(r2);
            for(Int32 i = 0; i < _columnsc; i++){
                t2[i] += t[i];
            }
            this._base[r2] = t2;
            this._changedM = true;
        }
        
        public void SwapColumns(Int32 c1, Int32 c2){
            if(c1 < 0 || c1 >= _columnsc)
                throw new  ArgumentOutOfRangeException("c1");
            if(c2 < 0 || c2 >= _columnsc)
                throw new  ArgumentOutOfRangeException("c2");
            Double[] t = this.GetColumn(c1);
            this.SetColumn(c1,this.GetColumn(c2));
            this.SetColumn(c2,t);
            
            if(!Double.IsPositiveInfinity(_det))
                this._det = -this._det;
        }
        
        public void MullColumn(Int32 column, Double c){
            Double[] t = this.GetColumn(column);
            for(Int32 i = 0; i < _rowsc; i++){
                t[i] *= c;
                this._base[i][column] = t[i];
            }
            if(!Double.IsPositiveInfinity(_det)){
                this._det *= c;
            }
        }
        
        public void AdditionColumns(Int32 c1, Int32 c2, Double c){
            if(c1 < 0 || c1 >= _columnsc)
                throw new  ArgumentOutOfRangeException("c1");
            if(c2 < 0 || c2 >= _columnsc)
                throw new  ArgumentOutOfRangeException("c2");
                
            Double[] t = this.GetColumn(c1);
            for(Int32 i = 0; i < _rowsc; i++){
                t[i] *= c;
            }
            Double[] t2 = this.GetColumn(c2);
            for(Int32 i = 0; i < _rowsc; i++){
                t2[i] += t[i];
            }
            this.SetColumn(c2,t2);
            this._changedM = true;
        }
        
        #endregion
        public Double this[Int32 i, Int32 j]{
            get {
                if(i < 0 || i > _base.Length)
                    throw new ArgumentOutOfRangeException("i");
                Double[] ar = _base[i];
                if(j < 0 || j > ar.Length)
                    throw new ArgumentOutOfRangeException("j");
                return ar[j];
            }
            set {
                if(i < 0 || i > _base.Length)
                    throw new ArgumentOutOfRangeException("i");
                Double[] ar = _base[i];
                if(j < 0 || j > ar.Length)
                    throw new ArgumentOutOfRangeException("j");
                ar[j] = value;
                _changedM = true;
            }
        }
        
        //Reference BE CAREFULL!
        public Double[] this[Int32 row]{
            get{
                if(row < 0 || row > _base.Length)
                    throw new ArgumentOutOfRangeException("row");
                return _base[row];
            }
        }
        
        
        public Double[] GetRow(Int32 row){
            if(row < 0 || row >= _rowsc)
                throw new  ArgumentOutOfRangeException("row");
            
            Double[] r = new Double[_columnsc];
            for(Int32 i = 0; i < _columnsc; i++){
                r[i] = _base[row][i];
            }
            return r;
        }
        
        //Get row with n elements
        public Double[] GetRow(Int32 row, Int32 n){
            if(row < 0 || row >= _rowsc)
                throw new  ArgumentOutOfRangeException("row");
            if(n <= 0 || n > _columnsc)
                throw new  ArgumentOutOfRangeException("n");
            Double[] r = new Double[n];
            for(Int32 i = 0; i < n; i++){
                r[i] = _base[row][i];
            }
            return r;
        }
        
        //Get row with n elements from the offset position.
        public Double[] GetRow(Int32 row, Int32 offset, Int32 n){
            if(row < 0 || row >= _rowsc)
                throw new  ArgumentOutOfRangeException("row");
            if(n <= 0 || n > _columnsc)
                throw new  ArgumentOutOfRangeException("n");
            if(offset < 0 || offset > _columnsc)
                throw new  ArgumentOutOfRangeException("offset");
            if(offset + n > _columnsc)
                throw new  ArgumentOutOfRangeException("offset, n");
            Double[] r = new Double[n];
            for(Int32 i = offset; i < n; i++){
                r[i] = _base[row][i];
            }
            return r;
        }
        
        public Double[] GetColumn(Int32 column){
            if(column < 0 || column >= _columnsc)
                throw new  ArgumentOutOfRangeException("column");
            
            Double[] c = new Double[_rowsc];
            for(int i = 0; i < _rowsc; i++){
                c[i] = _base[i][column];
            }
            
            return c;
        }
        
        //Get column with n elements
        public Double[] GetColumn(Int32 column, Int32 n){
            if(column < 0 || column >= _columnsc)
                throw new  ArgumentOutOfRangeException("column");
            if(n <= 0 || n > _rowsc)
                throw new  ArgumentOutOfRangeException("n");
            Double[] c = new Double[n];
            for(Int32 i = 0; i < n; i++){
                c[i] = _base[i][column];
            }
            return c;
        }
        
        //Get column with n elements from the offset position.
        public Double[] GetColumn(Int32 column, Int32 offset, Int32 n){
            if(column < 0 || column >= _columnsc)
                throw new  ArgumentOutOfRangeException("column");
            if(n <= 0 || n > _rowsc)
                throw new  ArgumentOutOfRangeException("n");
            if(offset < 0 || offset > _rowsc)
                throw new  ArgumentOutOfRangeException("offset");
            if(offset + n > _rowsc)
                throw new  ArgumentOutOfRangeException("offset, n");
            Double[] c = new Double[n];
            for(Int32 i = offset; i < n; i++){
                c[i] = _base[i][column];
            }
            return c;
        }
        
        //Replace row_i with newrow
        public void SetRow(Int32 row, Double[] newrow){
            if(row < 0 || row >= _rowsc)
                throw new ArgumentOutOfRangeException("row");
            if(newrow.Length < _columnsc)
                throw new ArgumentOutOfRangeException("newrow Length");
            for(Int32 i = 0; i < _columnsc; i++){
                this[row,i] = newrow[i];
            }
            this._changedM = true;
        }
        
        //Replace row_i with newrow from 0 to n elements. 
        public void SetRow(Int32 row, Double[] newrow, Int32 n){
            if(row < 0 || row >= _rowsc)
                throw new ArgumentOutOfRangeException("row");
            if(n <= 0 || n > _columnsc || n > newrow.Length)
                throw new ArgumentOutOfRangeException("n");
            for(Int32 i = 0; i < n; i++){
                this[row,i] = newrow[i];
            }
            this._changedM = true;
        }
        
        //Set row with n elements from offset in newrow 
        public void SetRow(Int32 row, Double[] newrow, Int32 offset, Int32 n){
            if(row < 0 || row >= _rowsc)
                throw new ArgumentOutOfRangeException("row");
            if(n <= 0 || n > _columnsc || n > newrow.Length)
                throw new ArgumentOutOfRangeException("n");
            if(offset < 0 || offset > _columnsc || offset >= newrow.Length)
                throw new ArgumentOutOfRangeException("offset");
            if(offset + n > _columnsc || offset + n > newrow.Length)
                throw new ArgumentOutOfRangeException("offset, n");
            for(Int32 i = offset; i < n; i++){
                this[row,i] = newrow[i];
            }
            this._changedM = true;
        }
        
        //Replace column_i with newcolumn.
        public void SetColumn(Int32 column,Double[] newcolumn){
            if(column < 0 || column >= _columnsc)
                throw new  ArgumentOutOfRangeException("column");
            if(newcolumn.Length < _rowsc)
                throw new ArgumentOutOfRangeException("newcolumn Length");
            for(Int32 i = 0;i < _rowsc;i++){
                this[i,column] = newcolumn[i];
            }
            this._changedM = true;
        }
        
        //Replace column_i with newcolumn from 0 to n elements.
        public void SetColumn(Int32 column,Double[] newcolumn, Int32 n){
            if(column < 0 || column >= _columnsc)
                throw new  ArgumentOutOfRangeException("column");
            if(n < 0 || n > _rowsc || n > newcolumn.Length)
                throw new  ArgumentOutOfRangeException("n");
            for(Int32 i = 0; i < n; i++){
                this[i,column] = newcolumn[i];
            }
            this._changedM = true;
        }
        
        public void SetColumn(Int32 column,Double[] newcolumn, Int32 offset, Int32 n){
            if(column < 0 || column >= _columnsc)
                throw new  ArgumentOutOfRangeException("column");
            if(n < 0 || n > _rowsc || n > newcolumn.Length)
                throw new  ArgumentOutOfRangeException("n");
            if(offset < 0 || offset > _rowsc || offset >= newcolumn.Length)
                throw new  ArgumentOutOfRangeException("offset");
            if(offset + n > _rowsc || offset + n > newcolumn.Length)
                throw new ArgumentOutOfRangeException("offset, n");
            for(Int32 i = offset; i < n; i++){
                this[i,column] = newcolumn[i];
            }
            this._changedM = true;
        }
        
        //Create new Matrix from the current with new column. Add this column to the end. HANDLE PARAMS
        public Matrix AddColumn(Double[] column){
            Double[][] nb = new Double[_rowsc][];
            for(Int32 i = 0, k = 0; i < _rowsc; i++, k++){
                nb[i] = new Double[_columnsc + 1];
                for(Int32 j = 0; j < _columnsc; j++){
                    nb[i][j] = this[i,j];
                }
                nb[i][_columnsc] = column[i];
            }
            return new Matrix(nb);
        }
        
        //Create new Matrix from the current with new row. Add this row to the end. HANDLE PARAMS
        public Matrix AddRow(Double[] row){
            Double[][] nb = new Double[_rowsc + 1][];
            //nb[0] = row;
            for(Int32 i = 0; i < _rowsc; i++){
                nb[i] = this.GetRow(i);
            }
            nb[_rowsc] = row;
            return new Matrix(nb);
        }
        
        
        public Int32 GetRowsCount(){
            return _rowsc;
        }
        
        public Int32 GetColumnsCount(){
            return _columnsc;
        }
        
        
        public Boolean Squared {
            get{
                return _rowsc == _columnsc;
            }
        }
        
        public Double Determinant {
            get {
                if(Double.IsPositiveInfinity(_det) || _changedM){
                    _changedM = false;
                    _det = __GetDeterminant();
                }
                return _det;
            }
        }
        
        public Boolean IsSpecial {
            get {return this.Determinant == 0;}
        }
        public Boolean IsFullRank {
            get{ return this.Rank == Math.Min(_rowsc,_columnsc);}
        }
        public Int32 Rank {
            get {
                if(_rank == -1){
                    _rank = __GetRank();
                }
                return _rank;
            }
        }
        
        public void Dispose(){
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool isDisposing){
            if(isDisposing){
                for(Int32 i = 0; i < _rowsc; i++){
                    _base[i] = null;
                }
                this._base = null;
                _rowsc = 0;
                _columnsc = 0;
                _det = 0;
                _rank = 0;
                _changedM = false;
            }
        }
        
        public override String ToString(){
            StringBuilder sb = new StringBuilder();
            foreach(Double[] a in _base){
                sb.Append("|");
                foreach(Double elem in a){
                    sb.Append($"{(elem < 0 ? elem+" " :" "+elem+" " )}");
                }
                sb.Append("|\n");
            }
            return sb.ToString();
        }
        
        #region IEnumerable<Double> implementation 
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public IEnumerator<Double> GetEnumerator(){
            for(Int32 i = 0; i < _rowsc; i++){
                for(Int32 j = 0; j < _columnsc; j++){
                    yield return this[i,j];
                }
            }
        }
        #endregion
        //This method returns xml representation of the object.
        public String ToXml(){
            TextWriter sw = new StringWriter();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.Encoding = Encoding.UTF8;
            using(XmlWriter writer = XmlWriter.Create(sw,settings)){
                writer.WriteStartElement("mtable");
                foreach(Double[] row in _base){
                    writer.WriteStartElement("mtr");
                    foreach(Double elem in row){
                        writer.WriteStartElement("mtd");
                        writer.WriteElementString("mtn",elem.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Flush();
            }
            return sw.ToString();
        }
        
        public static Matrix GetEMatrix(Int32 rows, Int32 columns){
            if(rows <= 0)
                throw new ArgumentOutOfRangeException("rows");
            if(columns <= 0)
                throw new ArgumentOutOfRangeException("columns");
            
            if(rows != columns)
                throw new ArgumentException(String.Format("Rows and columns must be equal. Actual: {0} and {1}",rows,columns),"rows columns");
            
            Matrix E = new Matrix(rows,columns); 
            
            for(Int32 i = 0; i < rows; i++){
                for(Int32 j = 0; j < columns;j++){
                    E[i,j] = (i == j) ? 1 : 0;
                }
            }
            E._rank = rows;
            return E;
        }
        
        public static Matrix operator *(Matrix A, Matrix B){
            return A.Mull(B);
        }
        
        public static Matrix operator +(Matrix A, Matrix B){
            return A.Add(B);
        }
        
        //GET NEW MATRIX
        public static Matrix operator *(Double d, Matrix B){
            Matrix C = new Matrix(B._base);//REPLACE
            for(int i = 0; i < C._base.Length; i++){
                Double[] a = C._base[i];
                for(int j = 0; j < a.Length; j++){
                    a[j] *= d;
                }
            }    
            if(B._rowsc == B._columnsc && !Double.IsPositiveInfinity(B._det)){
                C._det = B._det * Math.Pow(d,B._rowsc);
            }
            C._rank = -1;
            
            //B.ScalarMul(d);
            return B;
        }
    }
}