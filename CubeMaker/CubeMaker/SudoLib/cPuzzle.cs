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
        cCurPosition _pos;
        

        #region CONSTRUCTION
        public cPuzzle()
        {
            _layer = new cLayer();
        }

        ~cPuzzle()
        {
            _layer = null;
        }



        #endregion // CONSTRUCTION

      
        #endregion // OVERRIDES
    }


}
