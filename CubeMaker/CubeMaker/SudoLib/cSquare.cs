using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeMaker.SudoLib
{
    public class cSquare
    {
        int _rowID;
        int _colID;
        int _value;
        bool[] _available;
        List<cSquare> _validationList;
        static int _seed = -1;
        static Random _random;

        public cSquare(int row, int col)
        {
            _rowID = row; // rowID has already been adjusted in the cRow Constructor.
            _colID = col;

            _value = 0;
            _available = new bool[g.PSIZE + 1]; // so values 1 to 9 can be used as indexes.
            _available[0] = false;
            for (int i = 1; i <= g.PSIZE; i++) // 1 - 9
                _available[i] = true;

            _validationList = new List<cSquare>();
        }

        public static int Seed
        {
            set { _seed = value; }
            get { return _seed; }
        }
        public int Value
        {
            set { _value = value; }
            get { return _value; }
        }


        public List<cSquare> ValidationList
        {
            get { return _validationList; }
            set { _validationList = value; }
        }

        public bool[] Available
        {
            set { _available = value; }
            get { return _available; }
        }

        public int AvailableCount()
        {
            int retValue = 0;
            for (int i = 1; i <= g.PSIZE; i++) // 1 - 9
                if (_available[i])
                    retValue++;
            return retValue;
        }

        /// <summary>
        /// Make Values 1 - 9 available (again) for this square.
        /// </summary>
        public void AvailableReset()
        {
            _available[0] = false;
            for (int i = 1; i <= g.PSIZE; i++)
                _available[i] = true;

            return;
        }

        int Next()
        {

            if (_seed < 0)
            {
                _seed = 1;
                g.Banner($"Running PuzzleBuilder with default Seed of {_seed}.");
            }

            if (_random == null) _random = new Random(_seed);

            return _random.Next(1, g.PSIZE + 1);
        }


        /// <summary>
        /// Get a value using g.Next.  If available value is found,
        /// set it and set the avialble member false, so it isn't re-used.
        /// If there are no available values in the available list, then
        /// reset the list, as we will be dropping back to the squares 
        /// preceeding location in the puzzle (square just to the "left" of
        /// this currrent square.) to try a new value there, in hopes that this
        /// change will enable a value to work after moving back to this square.
        /// </summary>
        /// <returns>
        /// True if a value was found, otherwise false.
        /// False can only be returned if their are no more values in the
        /// available list.
        /// </returns>
        public bool TrySetValue()
        {
            /// No need to check if values exist in the available list.  
            /// Whenever it get's emptied, it is refilled because we then
            /// backtrace to the preceeding square.
            bool success = false;
            int tryValue = -1;
            bool doneTrying = false;

            while (!doneTrying)
            {
                int availableCount = AvailableCount();

                if (availableCount >= 2)   // 2 or more.
                {
                    bool foundAvailableValue = false;
                    while (!foundAvailableValue)
                    {
                        tryValue = Next();
                        foundAvailableValue = _available[tryValue];
                    }
                }
                else // if 1 more value available
                {
                    if (availableCount > 0)
                    {
                        for (int i = 1; i < g.PSIZE; i++)
                            if (_available[i])
                            {
                                tryValue = i;
                                break;
                            }
                    }
                    else
                    {
                        tryValue = -1;
                        doneTrying = true;
                        success = false;
                    }

                }
                if (tryValue > 0) // value was found, and now needs to be validated using the rules of Sudoku.
                { 

                    //////////////////////////////////////////////////////////////////////////////////////
                    /// Fine, the value is available, but now we need to check it's region, up/down and //
                    /// across neighbors to ensure that it doesn't violate the rules of sudoku.         //
                    //////////////////////////////////////////////////////////////////////////////////////
                    bool inviolates = false;
                    foreach (cSquare tstSqr in _validationList)
                    {
                        if (tstSqr.Value == tryValue)
                        {
                            /// it's tempting to invalidate the value here, because it can't be used due to 
                            /// conflicting values in the region, row or column, but that can't be done here
                            /// because the values preceeding this value may be the conflicting value, and may
                            /// be subject to change.  (if the current position moves back to a previous square,
                            /// then the current value of the previous square becomes not available for that
                            /// square, which could subsequently make this same value valid for this current
                            /// square.
                             inviolates = true;
                            break;
                        }
                    }
                    if (inviolates)
                    {
                        // this value failed, we are not done trying to find a value and we did not succeeed.
                        _available[tryValue] = success = false;
                        doneTrying = (AvailableCount() < 1);
                    }
                    else
                    {
                        /// Actaully, since we are no longer looking for a value for this square, it seems as
                        /// though it might not be important to set the _available[usedValue] = false, but it
                        /// feels somehow "safer".
                        _available[tryValue] = false;


                        /////////////////////////////////////////////////////////////
                        /// DON'T FORGET TO SET THE VALUE--THAT'S WHY WE'RE HERE!!///
                        /////////////////////////////////////////////////////////////
                        _value = tryValue;
                        doneTrying = success = true;
                    }


                } // tryValue > 0, tryValue was set, and needed to be validated.

            } // while (!doneTrying)
            return success;

        } // public bool TrySetValue()

        public int RowID
        {
            get { return _rowID; }
        }

        public int ColID
        {
            get { return _colID; }
        }

        /// <summary>
        /// SquareID: row and colID combined into a single ID.
        /// ex: 230 = Row 2 (rowID/100), Col 3 (colID/10)
        /// </summary>
        public int SquareID
        {
            get { return _rowID + _colID; }
        }

        public override string ToString()
        {
            // return $"({_rowID/100}, {_colID/10}) {_value}"; // rowID + colID + value as string
            return $"{_value}"; // value as a string.
        }

    } // public class cSquare
}
