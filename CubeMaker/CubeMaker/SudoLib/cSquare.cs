using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeMaker.SudoLib
{
    public class cSquare
    {
        int _row;
        int _col;

        int _value;
        bool[] _available;
        bool[] _isInViolationList;
        List<cSquare> _validationList;
        static int _seed = -1;
        static Random _random;

        public cSquare(int row, int col)
        {
            _row = row; // rowID has already been adjusted in the cRow Constructor.
            _col = col;

            _value = 0;
            _available = new bool[g.PSIZE + 1]; // so values 1 to 9 can be used as indexes.
            _isInViolationList = new bool[g.PSIZE + 1];
            _available[0] = false;
            for (int i = 1; i <= g.PSIZE; i++) // 1 - 9
                _available[i] = true;

            _validationList = new List<cSquare>();
        }

        public static int Seed
        {
            set {
                /// I was concidering a check to see if _random was null, and if not,
                /// reinstantiate it with a (potentially) new seed value, but it occurred to 
                /// me that a new puzzle would start with all new cSquares, which would mean
                /// they have a new, shared Random (_random) variable., so I don't believe that
                /// will be necessary.
                _seed = value;
            }
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

        bool legalMovesExist()
        {
            bool legalMovesExist = false;

            for (int i = 1; i <= g.PSIZE; i++)
                /// legal move exist OR EQUALS "i" is NOT in the violation list.
                legalMovesExist |= (!_isInViolationList[i]);

            return legalMovesExist;
        }

        bool checkAvailable(int i)
        {
            return _available[i - 1]; // translate 1-9 to 0-8.
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


        /// <summary>
        /// isInViolationList[n] is set true for all of this cell's sudoCousins.  If a 
        /// sudoCousin's value is 9, then no other related cells can contain a 9 without
        /// violating the sudoRules.
        /// </summary>
        void InitializeViolationArray()
        {
            _isInViolationList[0] = true; // no zero's in sudoku...?

            for (int i = 1; i <= g.PSIZE; i++)
                _isInViolationList[i] = false;

            foreach (cSquare sqr in _validationList)
            {
                // it would not be legal to use the values found in the validationList twice,
                // so we add them to the violiationList.
                _isInViolationList[sqr.Value] = true;
            }
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

            InitializeViolationArray();

            while (!doneTrying)
            {
                int availableCount = AvailableCount();
                if (legalMovesExist()) // at least one value that doesn't violate the rules of sudoku.
                {
                    if (availableCount >= 2)   // 2 or more.
                    {
                        bool foundAvailableValue = false;
                        while (!foundAvailableValue)
                        {
                            tryValue = Next();
                            foundAvailableValue = _available[tryValue];
                        }
                        // found a value, now see if it is in violation
                        // if it's not in the list, it's sudo-legal.
                        success = doneTrying = !_isInViolationList[tryValue];
                    }
                    else // if 1 more value available
                    {
                        if (availableCount > 0)
                        {
                            /// only one value available, find value directly.
                            for (int i = 1; i <= g.PSIZE; i++)
                                if (_available[i])
                                {

                                    doneTrying = success = !_isInViolationList[i]; ;
                                    if (success)
                                        tryValue = i;
                                    break;
                                }
                        }
                        else
                        {
                            /// all out of values, must backspace and try new values.
                            tryValue = -1;
                            doneTrying = true;
                            success = false;
                        }

                    }
                }
                else
                {
                    doneTrying = true;
                    success = false;
                    tryValue = -1;
                }

            } // while (!doneTrying)
            if (success)
                _value = tryValue;
            return success;

        } // public bool TrySetValue()

        public int Row
        {
            get { return _row; }
        }

        public int Col
        {
            get { return _col; }
        }


        public override string ToString()
        {
            // return $"({_rowID/100}, {_colID/10}) {_value}"; // rowID + colID + value as string
            return $"{_value}"; // value as a string.
        }

    } // public class cSquare
}
