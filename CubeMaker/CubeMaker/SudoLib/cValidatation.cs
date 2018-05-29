using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeMaker.SudoLib
{
    public class cValidatation
    {
        cLayer _layer;
        List<cSquare> _squareList;
        #region CONSTRUCTION / DESTRUCTION
        public cValidatation(cLayer layer)
        {
            _layer = layer;
            buildValidationLinkedLists();
        }
        #endregion CONSTRUCTION / DESTRUCTION

        #region CONSTRUCT LINKS

        /// <summary>
        /// Each cSquare object contains a member:
        ///  List<cSquare> _validationList;
        ///  
        /// buildValidationLinkedLists iterates over every square in the puzzle and
        /// populates their linked lists with all of there relative neighbors.  That is,
        /// all of the puzzle elements that the cSquare's value cannnot duplicate, as it
        /// would then violate the rules of sudoku.
        /// 
        /// NOTE:
        /// These lists are built before an attempt to make a sudoku puzzle, as they are
        /// used during the puzzle development process:  When a value is considered for
        /// a square, it then iterates through it's _validationList, to verify that the
        /// value in concideration is not already in use, and therefore cannot be used 
        /// again.
        /// </summary>
        void buildValidationLinkedLists()
        {
            for (int row = 0; row < g.PSIZE; row++)
            {
                for (int col = 0; col < g.PSIZE; col++)
                {
                    _squareList = _layer[row][col].ValidationList;
                    addRegionLinks(row, col);
                    addRowLinks(row);
                    addColumnLinks(col);
                }
            }
        }

        /// <summary>
        /// AddLink(row, col) verifies that a cSquare doesn't already exist in the
        /// _validationList before adding the cSquare at location _layer[row][col].
        /// If the same cSquare was added dupicately, the list would still function,
        /// but uncessary duplicate checks would be made:
        /// (i.e., is this value = value at _layer[0][1]?  OK then, is this value = 
        /// value at _layer[0][1]?  Obviously, The check at [0][1] only needs to be 
        /// done one time.)
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        void AddLink(int row, int col)
        {
            cSquare sqr = _layer[row][col];
            if (!_squareList.Contains(sqr))
            {
                _squareList.Add(sqr);
            }
        }

        #region REGION, ROW, COLUMN

        void BuildRegionList(int row, int col)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {

                    AddLink(row + i, col + j);
                }
            }
        }

        void addRegionLinks(int row, int col)
        {

            eRegion thisRegion = GetRegion(row, col);

            int iRow = -1;
            int iCol = -1;
            switch (thisRegion)
            {
                case eRegion.TopLeft:
                    iRow = iCol = 0;
                    break;
                case eRegion.TopCenter:
                    iRow = 0; iCol = 3;
                    break;
                case eRegion.TopRight:
                    iRow = 0; iCol = 6;
                    break;
                case eRegion.CenterLeft:
                    iRow = 3; iCol = 0;
                    break;
                case eRegion.Center:
                    iRow = 3; iCol = 3;
                    break;
                case eRegion.CenterRight:
                    iRow = 3; iCol = 6;
                    break;
                case eRegion.LowerLeft:
                    iRow = 6; iCol = 0;
                    break;
                case eRegion.LowerCenter:
                    iRow = 6; iCol = 3;
                    break;
                case eRegion.LowerRight:
                    iRow = 6; iCol = 6;
                    break;
                default:
                    throw new Exception("Invalid eRegion in cValidation.addRegionLinks()");
            }
            BuildRegionList(iRow, iCol);

        }

        /// <summary>
        /// Add each cSquare in this row to the ValidationLinks[eLinks.Row] collection
        /// </summary>
        /// <param name="row">
        /// Add cSquares from this (int)row
        /// </param>
        /// <returns>
        /// First cSqare added to this collection, so that it can be added to 
        /// another link using a List<cSquare>.Add(addRowLinks(row) type of syntax,
        /// thereby building one continuing list that can be traversed simply by 
        /// traversing each element of the ValidationList of a cSquare.
        /// </returns>
        void addRowLinks(int row)
        {
            for (int col = 0; col < g.PSIZE; col++)
                AddLink(row, col);
        }

        void addColumnLinks(int col)
        {
            List<cSquare> retList = new List<cSquare>();

            for (int row = 0; row < g.PSIZE; row++)
                AddLink(row, col);

        }

        #endregion REGION, ROW, COLUMN

        #endregion CONSTRUCT LINKS


        #region TEST VALIDITY OF LINKS
        public void CheckLinks()
        {
            for (int row = 0; row < g.PSIZE; row++)
            {
                for (int col = 0; col < g.PSIZE; col++)
                {
                    ClearValues();
                    traverseLinks(row, col);
                    Console.WriteLine(_layer.LayerString());
                    Console.ReadKey();
                }
            }
        }

        void ClearValues()
        {
            for (int i = 0; i < g.PSIZE; i++)
                for (int j = 0; j < g.PSIZE; j++)
                    _layer[i][j].Value = 0;
        }

        void traverseLinks(int row, int col)
        {
            List<cSquare> list = _layer[row][col].ValidationList;
            int sqrNum = 0;
            int total = 0;
            foreach (cSquare sqr in list)
            {
                /// setting the sqr value with not testing to see if it's
                /// already been set ensures that each member of the list
                /// is only visited once. (just check to see that all the
                /// values are sequential, and that the maximum value is
                /// the length of the list.
                sqr.Value = ++sqrNum;
                total += sqr.Value;
            }

            g.Banner($"Sum of Values is {total}.");
        }

        #endregion VALIDITY OF LINKS

        #region REGIONS

        eTopBottom _topBottom;
        eLeftRight _leftRight;

        enum eLeftRight
        {
            NotSet,
            Left,
            Center,
            Right
        }

        enum eTopBottom
        {
            NotSet,
            Top,
            Center,
            Bottom
        }
        public enum eRegion
        {
            TopLeft,
            TopCenter,
            TopRight,
            CenterLeft,
            Center,
            CenterRight,
            LowerLeft,
            LowerCenter,
            LowerRight,
            NumRegions,
            NotSet
        }


        void SetLeftRight(int col)
        {
            if (col < 3)
            {
                _leftRight = eLeftRight.Left;
            }
            else
            {
                if (col < 6)
                {
                    _leftRight = eLeftRight.Center;
                }
                else
                    _leftRight = eLeftRight.Right;
            }

        }

        void setTopBottom(int row)
        {
            if (row < 3)
            {
                _topBottom = eTopBottom.Top;
            }
            else
            {
                if (row < 6)
                {
                    _topBottom = eTopBottom.Center;
                }
                else
                    _topBottom = eTopBottom.Bottom;
            }
        }


        eRegion GetRegion(int row, int col)
        {
            eRegion eReturn = eRegion.NotSet;

            _leftRight = eLeftRight.NotSet;
            _topBottom = eTopBottom.NotSet;
            SetLeftRight(col);
            setTopBottom(row);
            switch (_leftRight)
            {


                case eLeftRight.Left:
                    ///////////////////////
                    // LEFT-RIGHT = LEFT //
                    ///////////////////////
                    switch (_topBottom)
                    {
                        case eTopBottom.Top:
                            eReturn = eRegion.TopLeft;
                            break;
                        case eTopBottom.Center:
                            eReturn = eRegion.CenterLeft;
                            break;
                        case eTopBottom.Bottom:
                            eReturn = eRegion.LowerLeft;
                            break;
                        default:
                            throw new Exception("Invalid eTopBottom value in cValidationLists.GetRegion()");
                    }
                    break;




                case eLeftRight.Center:
                    /////////////////////////
                    // LEFT-RIGHT = CENTER //
                    /////////////////////////
                    switch (_topBottom)
                    {
                        case eTopBottom.Top:
                            eReturn = eRegion.TopCenter;
                            break;
                        case eTopBottom.Center:
                            eReturn = eRegion.Center;
                            break;
                        case eTopBottom.Bottom:
                            eReturn = eRegion.LowerCenter;
                            break;
                        default:
                            throw new Exception("Invalid eTopBottom value in cValidationLists.GetRegion()");
                    }

                    break;
                case eLeftRight.Right:
                    switch (_topBottom)
                    {
                        ////////////////////////
                        // LEFT-RIGHT = RIGHT //
                        ////////////////////////
                        case eTopBottom.Top:
                            eReturn = eRegion.TopRight;
                            break;
                        case eTopBottom.Center:
                            eReturn = eRegion.CenterRight;
                            break;
                        case eTopBottom.Bottom:
                            eReturn = eRegion.LowerRight;
                            break;
                        default:
                            throw new Exception("Invalid eTopBottom value in cValidationLists.GetRegion()");
                    }
                    break;
                default:
                    throw new Exception("Invalid LeftRight enum value in cValiationLists.GetRegion()");
            } // END SWITCH
            return eReturn;

        }
        // END GetRegion
        #endregion REGIONS
    }
}
