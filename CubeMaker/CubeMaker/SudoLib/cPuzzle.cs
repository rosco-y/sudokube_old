using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeMaker.SudoLib
{

    public class cPuzzle
    {
        #region MEMBERS
        cLayer _layer;
        cValidatation _validatation;
        cCurPosition _cur;
        #endregion // MEMBERS

        #region CONSTRUCTION
        public cPuzzle()
        {
            _layer = new cLayer();
            _validatation = new cValidatation(_layer);
        }

        ~cPuzzle()
        {
            _layer = null;
        }

        /// <summary>
        /// Considiering the need for a Clear method so it can be used to
        /// iterate the same cPuzzle obect and make a new puzzle with a new
        /// seed, as when running unattended.
        /// </summary>
        public void Clear()
        {
            _layer = null;
        }

        #endregion // CONSTRUCTION

        #region METHODS
        public bool BuildPuzzle(int seed)
        {
            cSquare.Seed = seed; // cSquare Seed for Random() is static.
            bool success = false;
            _cur = new cCurPosition();
            cCurPosition lastPosition = new cCurPosition();
            int loop = 0;
            bool done = false;
            while (!done)
            {
                _cur.SetEqualToMe(lastPosition);
                if (_layer[_cur.Row][_cur.Col].TrySetValue())
                {
                    /// be careful not to increment before the EndPostion check as the
                    /// last cSquare would be missed.
                    success = done = _cur.EndPosition;
                    _cur++;
                }
                else
                {
                    if (_cur.StartPosition)
                    {
                        if (_layer[_cur.Row][_cur.Col].AvailableCount() < 1)
                        {
                            /// ERROR CONDITION--This would mean that there are no possible
                            /// sudoku solutions, since we are on the first square of the puzzle,
                            /// and no more values available to try??
                            
                            done = true; // what other option is there?

                            string msg = "cPuzzle.BuildPuzzle()" + Environment.NewLine + "ILLEGAL RESULT: No Solution Found.";
                            msg += Environment.NewLine + $"1-9 has been exausted at _layer[{_cur.Row}][{_cur.Col}]???";
                            throw new Exception(msg);
                        }
                    }  // if (_cur.StartPosition)
                    else // not _cur.StartPosition...
                    {

                        /// if the new (previous) cell has available numbers to try (numbers that have not
                        /// yet been tried by the new cell), then we can continue with the newly visited
                        /// cell, else we should keep backspacing until we arrive at a cell that we has values
                        /// we can experiment with.
                        // backSpace -- _layer[_cur.Row][_cur.Col].AvailableReset();
                        backSpace();
                        while ((!_cur.StartPosition && (_layer[_cur.Row][_cur.Col].AvailableCount() < 1)))
                        {
                            cSquare curSquare = _layer[_cur.Row][_cur.Col];
                            int failedValue = curSquare.Value;
                            curSquare.Value = 0;

                            // backspace - curSquare.AvailableReset();
                            backSpace();
                            /// ensure that the square we have just backspaced to has values available to 
                            /// try.  If it doesn't, back up one more space.
                            if (!_cur.StartPosition && (_layer[_cur.Row][_cur.Col].AvailableCount() < 1))
                            {
                                // backspace - _layer[_cur.Row][_cur.Col].AvailableReset();
                                backSpace();
                            }
                            else
                            {
                                cCurPosition t = new cCurPosition(_cur);
                                t++;
                                _layer[t.Row][t.Col].AvailableReset();
                            }
                        }
                    }


                } // else (!_cur.StartPosition)

                if (_cur < lastPosition)
                    g.Banner($"Loop: {++loop}: ({_cur.Row}, {_cur.Col}). <<<-- BACKSPACE.");
                else
                    g.Banner($"Loop: {++loop}: ({_cur.Row}, {_cur.Col}).");

                Console.WriteLine(_layer);

                g.Pause();

            } // while (!done)

            return success;
        } // BuildPuzzle

        void backSpace()
        {
            cSquare sqr = _layer[_cur.Row][_cur.Col];
            sqr.Available[sqr.Value] = false;
            _cur--;
        }

        public void PrintPuzzle()
        {
            Console.WriteLine(_layer.LayerString());
        }
        #endregion // METHODS

    }


}
