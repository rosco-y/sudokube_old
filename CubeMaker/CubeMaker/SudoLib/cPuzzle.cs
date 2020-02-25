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
                        /// no available values for this cell, but we are not at start position, so
                        /// restore the avialable values for this cell and backspace to try a new value
                        /// in the previous cell and try again.
                        bool doneBackspacing = (_layer[_cur.Row][_cur.Col].AvailableCount() > 0);
                        while (!doneBackspacing)
                        {
                            cSquare curSquare = _layer[_cur.Row][_cur.Col];
                            curSquare.AvailableReset();
                            int failedValue = curSquare.Value;
                            curSquare.Value = 0;

                            /// might be redundant if AvailableCount was 0, but OK....
                            curSquare.Available[failedValue] = false;
                            _cur--;
                            /// ensure that the square we have just backspaced to has values available to 
                            /// try.  If it doesn't, back up one more space.
                            doneBackspacing = _layer[_cur.Row][_cur.Col].AvailableCount() > 0;
                        }
                    }


                } // else (!_cur.StartPosition)

                //if (_cur < lastPosition)
                //    g.Banner($"Loop: {++loop}: ({_cur.Row}, {_cur.Col}). <<<-- BACKSPACE.");
                //else
                //    g.Banner($"Loop: {++loop}: ({_cur.Row}, {_cur.Col}).");

                //Console.WriteLine(_layer);
                //g.Pause();

            } // while (!done)

            return success;
        } // BuildPuzzle

        public void PrintPuzzle()
        {
            Console.WriteLine(_layer.LayerString());
            g.Pause();                           
        }
        #endregion // METHODS

    }


}
