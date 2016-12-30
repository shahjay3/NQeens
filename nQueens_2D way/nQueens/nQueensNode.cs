using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nQueens
{
    class nQueensNode
    { 
        // private variables corresponding to the nQueens board and the hueristic value
        private int[,] nQueensBoard;
        private int heuristic;

      // Basic constructor for the Queen Node object, only stores the nQueen Board and the corresponding heuristic
        public nQueensNode(int[,] nQueens, int heuristic,int numQueens)
        {
            nQueensBoard = new int[numQueens, numQueens];
            nQueensBoard = nQueens.Clone() as int[,];
            this.heuristic = heuristic;
            
        }
        //Public Properties (get and set methods) corresponding to the nQueens board and the heuristic value
        public int[,] Nqueens { get { return nQueensBoard; } set { nQueensBoard=value; } }
        public int Hproperty { get { return heuristic; } set { heuristic=value; } }


    }
}
