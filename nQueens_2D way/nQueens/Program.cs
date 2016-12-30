using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nQueens
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define variables for number of random restarts and the number of states traversed:
            int numRandRestarts = 0;
            int numStates = 0;
            
            //Ask User to enter the number of queens to solve for
            Console.WriteLine("Please enter the number of queens you would like to solve for and press enter");
            int numOfQueens= Convert.ToInt32(Console.ReadLine());

            //Generate the board for N x N queens
            int[,] nQueensBoard = new int[numOfQueens,numOfQueens];
            List<nQueensNode> successorList = new List<nQueensNode>();
            // generate the initial state
            nQueensBoard = generateRandomBoard(numOfQueens);
            // get the initial state's heuristic
            int h =AttackingPairsHeuristicFunction(nQueensBoard, numOfQueens);

            Console.WriteLine("Final State");
            Console.WriteLine(" ");

            do
            {
                //if the states heuristic is 0, then we are done
                if (h == 0)
                {
                    printBoard(nQueensBoard, numOfQueens);
                    break;
                }
                // otherwise generate the successors of the state
                successorList = SuccessorList(nQueensBoard, numOfQueens);
                // gets the state with the lowest heuristic value
                nQueensNode Successor = successorList.Aggregate((n1, n2) => n1.Hproperty < n2.Hproperty ? n1 : n2);
                //clear the successorsList
                successorList.Clear();
                
                // if the first in the list with minimum heuristic is 0, we found our goal
                if (Successor.Hproperty == 0)
                {
                    printBoard(Successor.Nqueens, numOfQueens);
                    break;
                }
                else if (h > Successor.Hproperty)
                {   // if the first state int he list has a smaller heuristic than the current state, make that our current state
                    nQueensBoard = Successor.Nqueens;
                    h = Successor.Hproperty;
                    numStates++; // incrmeent the number of states traversed
                }
                else
                {   //if the lowest heuristic in the list is still higher than the initial boards,then generate new board. 
                    nQueensBoard = generateRandomBoard(numOfQueens);
                    h = AttackingPairsHeuristicFunction(nQueensBoard, numOfQueens);
                    numRandRestarts++; // increment the number of random restarts 
                }
            } while (true);
            Console.WriteLine("The Number of Random Restarts is: " + numRandRestarts);
            Console.WriteLine("The Number of States traversed is : " + numStates);
        }

        //generates a random state
        public static int[,] generateRandomBoard(int numOfQueens)
        {   // this method generates a random board, we intialize the board to all 0's first
            int[,] nQueensBoard = new int[numOfQueens, numOfQueens];
            Random rand = new Random();
           
            // we then put a queen randomly in a row in each column, this loop ensures that there is only 1 queen per column
            for (int i = 0; i < numOfQueens; i++)
            {
                nQueensBoard[Convert.ToInt32(rand.Next())%numOfQueens, i] = 1;
            }
            return nQueensBoard;
        }

        // just prints random board
        public static void printBoard(int[,] nQueensBoard,int numOfQueens)
        {
            for(int i = 0; i < numOfQueens;i++)
            {
                for(int j = 0; j < numOfQueens; j++)
                {
                    Console.Write(nQueensBoard[i, j]+" ");
                }
                Console.Write(Environment.NewLine);
            }
        }

        /// <summary>
        /// This function finds the queen in each column, once found, it is fed into the findForward Attacking pairs function to find
        /// the number of queens that can attack on both forward diagonals and row.
        /// </summary>
        
        public static int AttackingPairsHeuristicFunction(int[,] nQueensBoard, int numOfQueens)
        {  
            // define the heuristic value:
            int heuristic = 0; 
            // for each column, find the queen, once we find the queen, we will feed it into the find forward attack pairs function
            for(int col = 0; col < numOfQueens; col++)
            {
               for(int row =0;row< numOfQueens; row++)
                {
                    if (nQueensBoard[row, col] == 1)
                    {
                      heuristic += findForwardAttackPairs(nQueensBoard, numOfQueens, row, col);
                        break;
                        
                    }
                }
            }
            return heuristic;
        }

        /// <summary>
        /// This function finds the direct and indirect attacking pairs for each queen. it takes input from the Attacking pairs hueristic 
        /// function. 
        /// This function searches all forward diagonals and forward row to count the number of queens in its path. this is done so that 
        /// we count both direct and indirect queens. I did this in this fashion so that we do not double count. 
        /// </summary>
        
        public static int findForwardAttackPairs(int[,] nQueensBoard, int numOfQueens,int row, int col)
        {
            int incrementRow = row;
            int decrementRow = row;
        
            int numAttackPairs = 0;
            // if we are at the last column in the board, then go ahead and return numAttack Pairs, which is 0
            if (col == numOfQueens - 1)
            {
                return numAttackPairs;
            }

            for (int i = col + 1; i < numOfQueens; i++)
            {
                incrementRow++;
                // check downward diagonal for attacking queens
                if (incrementRow < numOfQueens)
                {
                    numAttackPairs += nQueensBoard[incrementRow, i]; 
                }
                //check upward diagonal for attacking queens
                decrementRow--;
                if (decrementRow >= 0)
                {
                    numAttackPairs += nQueensBoard[decrementRow, i];
                }
                //check the forward row for attacking queens
                numAttackPairs += nQueensBoard[row, i];
            }

            return numAttackPairs;
          
            
        } // end of findForwardAttackPairs

        /// <summary>
        /// Returns the successor of the current board as a list of successors
        /// </summary>
        public static List<nQueensNode> SuccessorList(int[,] QueensBoard, int numQueens)
        {
            List<nQueensNode> QueenList = new List<nQueensNode>();

            // move the queen to each of the 7 positions in the column and create the object
            for(int col = 0; col < numQueens; col++)
            {
                for (int row = 0; row < numQueens; row++)
                {
                    if (QueensBoard[row, col] == 1)
                    {
                        // gets the list of successors   
                        QueenList.AddRange(MoveBoard(QueensBoard,numQueens, row, col));
                        QueensBoard[row, col] = 1;
                        break;
                    }
                }
            }
            return QueenList;
        }

        /// <summary>
        /// This function moves the queen up and down the column for each column, creates the board object with its
        /// hueristic value and returnst he list of successors for the initial state
        /// </summary>
        public static List<nQueensNode> MoveBoard(int[,] QueensBoard,int numQueens, int row, int col)
        {
            List<nQueensNode> nodeList = new List<nQueensNode>();
            int heuristic=0;

            QueensBoard[row, col] = 0;
            int[,] Board = new int[numQueens,numQueens];
            Board = QueensBoard;
            // move the queen from row =0 to row = row-1, each move generates a new object and is put into a list
            for (int i = 0; i < row ; i++)
            {
                //swtich 0 --> 1
                Board[i, col] = 1;
                //get heurisitc:
                heuristic= AttackingPairsHeuristicFunction(Board, numQueens);
                nQueensNode node = new nQueensNode(Board, heuristic,numQueens);
                nodeList.Add(node);
                Board[i,col]=0;
             
            }
            // move the queen from row+1 to end of the board, each move generates a new object and is put into a list
            for (int i = row + 1; i < numQueens; i++)
            {
                //swtich 0 --> 1
                Board[i, col] = 1;
                //get heurisitc:
                heuristic = AttackingPairsHeuristicFunction(Board, numQueens);
                nQueensNode node = new nQueensNode(Board, heuristic, numQueens);
                nodeList.Add(node);
                Board[i, col] = 0;
            }

            return nodeList;
        }


        public static void MinConflictsFunction(int[,] Board, int numQueens)
        {
            int[] minConflictsBoard = new int[numQueens];
            //switch to 1D board
            for (int col = 0; col < numQueens; col++)
            {
                for (int row = 0; row < numQueens; row++)
                {
                    if (Board[row, col] == 1)
                    {
                        minConflictsBoard[col] = row;
                    }
                }
            }

            
            int heuristic = MinConflictsHeuristic(minConflictsBoard, numQueens);
            if (heuristic == 0)
            {
                //you are done
            }
            


        } // end of min conflicts function

        public static int MinConflictsHeuristic(int[] minConflictsBoard, int numQueens)
        {
            int heuristic = 0;
            for(int i = 0; i < numQueens; i++)
            {
                for(int j = i + 1; j < numQueens; j++)
                {
                    if (minConflictsBoard[i] == minConflictsBoard[j])
                    {
                        heuristic++;
                    }
                    if(minConflictsBoard[i]==minConflictsBoard[j]+ (j-i) || minConflictsBoard[i] == minConflictsBoard[j] - (j - i))
                    {
                        heuristic++;
                    }
                }
            }

            return heuristic;
        }

       


    } //end of class
} //end of namespace
