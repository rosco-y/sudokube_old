using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CubeMaker.SudoLib;

namespace CubeMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            int seed = 1;
            if (args.Count() > 0)
            {
                seed = int.Parse(args[0]);
                g.Banner($"Using seed {seed}");
            }
            else
                g.Banner($"Using default seed: {seed}.");

            cPuzzle puzzle = new cPuzzle();
            if (puzzle.BuildPuzzle(seed))
            {
                puzzle.PrintPuzzle();
            }
        }
    }
}
