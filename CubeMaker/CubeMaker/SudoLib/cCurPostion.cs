using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeMaker.SudoLib
{
    public class cCurPosition
    {
        int _curRow;
        int _curCol;
        bool _success = false;
        public cCurPosition()
        {
            _curRow = _curCol = 0;
        }

        public cCurPosition(int row, int col)
        {
            _curRow = row;
            _curCol = col;
        }

        #region OVERRIDE OPERATORS

        public static cCurPosition operator ++(cCurPosition a)
        {
            if (a._curCol >= g.PSIZE - 1)
            {
                if (a._curRow < (g.PSIZE - 1))
                {
                    a._curCol = 0;
                    a._curRow++;
                }
            }
            else
            {
                a._curCol++;
                if ((a._curRow == (g.PSIZE - 1)) && (a._curCol == (g.PSIZE - 1)))
                {
                    a._success = true;
                }
            }
            return a;
        }

        public static cCurPosition operator --(cCurPosition a)
        {
            // somewhere within a row, move one to the left.
            if (a._curCol > 0)
            {
                a._curCol--;
            }
            else
            {
                /// at beginning of the row, test to see if there are preceeding rows
                if (a._curRow > 0)
                {
                    /// move to end of previous row.
                    a._curRow--;
                    a._curCol = g.PSIZE - 1;
                }

            }

            return a;
        }

        #region OVERRIDE EQUALITY TEST OPERATORS

        public override int GetHashCode()
        {
            //return base.GetHashCode();
            int hash = 13;
            hash = (hash * 7) + _curRow.GetHashCode();
            hash = (hash * 7) + _curCol.GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            cCurPosition cObj = (cCurPosition)obj;
            return (
                (cObj.Row == this.Row) &&                    
                (cObj.Col == this.Col));
        }

        public static bool operator ==(cCurPosition a, cCurPosition b)
        {
            return (
                (a.Row == b.Row) &&
                (a.Col == b.Col));
        }

        public static bool operator != (cCurPosition a, cCurPosition b)
        {
            return ((a.Row != b.Row) ||
                    (a.Col != b.Col));
        }


        public static bool operator < (cCurPosition a, cCurPosition b)
        {
            bool lessThan = false;
            if (a.Row < b.Row)
            {
                lessThan = true;
            }
            else
            {
                if (a.Row == b.Row)
                {
                    if (a.Col < b.Col)
                        lessThan = true;
                }
            }
            return lessThan;
        }

        public static bool operator > (cCurPosition a, cCurPosition b)
        {
            bool greaterThan = false;
            if (a.Row > b.Row)
            {
                greaterThan = true;
            }
            else
            {
                if (a.Row == b.Row)
                {
                    if (a.Col > b.Col)
                        greaterThan = true;
                }
            }

            return greaterThan;
        }

        #endregion OVERRIDE EQUALITY TEST OPERATORS

        #endregion OVERRIDE OPERATORS

        #region PUBLIC PROPERTIES

        public int Row
        {
            get { return _curRow; }            
        }

        public int Col
        {
            get { return _curCol; }
        }

        public bool Success
        {
            get { return _success; }
        }

        public bool StartPosition
        {
            get { return ((this._curCol == 0) && (this._curRow == 0)); }
        }

        public bool EndPosition
        {
            get { return ((this._curRow == g.PSIZE - 1) && (this._curCol == g.PSIZE - 1)); }
        }

        #endregion PUBLIC PROPERTIES

        #region PUBLIC METHODS
        public void SetEqual(cCurPosition aCur)
        {
            aCur._curRow = this.Row;
            aCur._curCol = this.Col;
        }
        #endregion PUBLIC METHODS


    }
}
