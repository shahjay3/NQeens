using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Console.WriteLine("Press 1 if you want regular hill climbing, Press 2 if you want min-conflict");
            int choice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Please enter the number of queens you would like to solve for and press enter");
            int numOfQueens= Convert.ToInt32(Console.ReadLine());
            int[] nQueensBoard = new int[numOfQueens];

            //Generate the raondom board and find the heuristic
            nQueensBoard = generateRandomBoard(numOfQueens);
            int h=Heuristic(nQueensBoard, numOfQueens);
            // start the stop watch to see how long the algorithm runs
            Stopwatch watch = new Stopwatch();
            if(choice == 2)
            {    // this starts the min conflict algorithm for the user if the user wants
                MinConflictsSolution(nQueensBoard, numOfQueens);
            }
            else
            {
                watch.Start();
                do
                {  // check to see if the board's hueristic is 0, if it is, print the board 
                    if (h == 0)
                    {
                        watch.Stop();
                        Console.WriteLine(watch.Elapsed);
                        printBoard(nQueensBoard, numOfQueens,numStates,numRandRestarts);
                        break;
                    }
                    // if heuristic is not 0, then find the boards lowest heuristic valued successor
                    nQueensNode bestSuccessor = SuccessorFunction(nQueensBoard, numOfQueens);
                    // see if the best successors value is less than the current states heurisitc
                    if (bestSuccessor.Hproperty < h)
                    {
                        // if it is, then replace the current board and hueristic value with that of the successors
                        nQueensBoard = bestSuccessor.Nqueens;
                        h = bestSuccessor.Hproperty;
                        numStates++;
                    }
                    else
                    {
                        // if the best successors heuristic is greater than that of the current state, generate a new board
                        nQueensBoard = generateRandomBoard(numOfQueens);
                        h = Heuristic(nQueensBoard, numOfQueens);
                        numRandRestarts++;
                    }

                } while (true);
            }
        }

        //generates a random state
        public static int[] generateRandomBoard(int numOfQueens)
        {   // this method generates a random board, we intialize the board to all 0's first
            int[] nQueensBoard = new int[numOfQueens];
            Random rand = new Random();
           
            // we then put a queen randomly in a row in each column, this loop ensures that there is only 1 queen per column
            for (int i = 0; i < numOfQueens; i++)
            {
                nQueensBoard[i] = Convert.ToInt32(rand.Next()) % numOfQueens;
            }
            return nQueensBoard;
        }

        // just prints  board
        public static void printBoard(int[] nQueensBoard,int numOfQueens,int numStates, int numRestarts)
        {
            int[,] finalBoard = new int[numOfQueens, numOfQueens];

            for(int i = 0; i < numOfQueens; i++)
            {
                finalBoard[nQueensBoard[i], i ] = 1;
            }
            //print the board
            for(int i =0; i < numOfQueens; i++)
            {
                for(int j = 0; j < numOfQueens; j++)
                {
                    Console.Write(finalBoard[i, j] + " ");

                }
                Console.Write(Environment.NewLine);
            }
               // prints the number of random restarts and the number of states
            Console.WriteLine("The Number of Random Restarts is: " + numRestarts);
            Console.WriteLine("The Number of States traversed is : " + numStates);


        }

        public static int Heuristic(int[] Board, int numQueens)
        {
            int heuristic = 0;
            for(int i = 0; i < numQueens; i++)
            {
                for(int j = i + 1; j < numQueens; j++)
                { // loop through the array, if any of the columns are equal, then increment the heuristic
                    // this is beacuse there are 2 qeeuns on the same row
                    if (Board[i] == Board[j])
                    {
                        heuristic++;
                    }
                    // loop through the 1d arrayand ehck the forward and backward diagonal of every queen, if 
                    // there is a queen, then increment the heuristic value
                    if(Board[i]== Board[j]+ (j-i) || Board[i] == Board[j] - (j - i))
                    {
                        heuristic++;
                    }
                }
            }
            // return the heuristic
            return heuristic;
        }

        public static nQueensNode SuccessorFunction(int[] Board,int numQueens)
        {  // initialize a list of nodes
            List<nQueensNode> nodeList = new List<nQueensNode>();

            for(int i =0; i < numQueens; i++)
            {    
                int originalValue = Board[i];

                while (Board[i] != 0)
                {   // go through each column, decrement the row value, find the heuristic and create the object
                    // store the object in a list for further processing
                    Board[i]--;
                    int h = Heuristic(Board, numQueens);
                    nodeList.Add(new nQueensNode(Board, h, numQueens));
                }

                Board[i] = originalValue;

                while (Board[i]+1 < numQueens)
                {  // go through each column, increment the row value, findthe heuristic, and create the object
                    // store object in nodeList for further processing
                    Board[i]++;
                    int h = Heuristic(Board, numQueens);
                    nodeList.Add(new nQueensNode(Board, h, numQueens));
                }

                Board[i] = originalValue;
            }
            // find the successor in the nodeList that has the least heuristic value
            nQueensNode Successor = nodeList.Aggregate((n1, n2) => n1.Hproperty < n2.Hproperty ? n1 : n2);
            // return the successor
            return Successor;
        }

        public static void MinConflictsSolution(int[] board, int numQueens)
        {
            Stopwatch watch = new Stopwatch();

            // initialize new board
            int[] newBoard = new int[numQueens];
            // set the max number of steps that min conflicts is allowed to take before random restart
            int maxSteps = 10000;
            // set numStates that we traversed and the number of random restarts to 0
            int numStates = 0;
            int numRestarts = 0;
            int[] boardWithLeastH = new int[numQueens];
            // copy the board to a new board
            for (int i = 0; i < numQueens; i++)
            {
                newBoard[i] = board[i];
            }

           watch.Start();
             while (true) { 
                // find a column where there is a conflicting queen
             int columnConflict = TheColumnWithConflicts(newBoard, numQueens);
            
             int leastHeuristic = 10000;
                // iterate the board at that confliciting column
              for (int i = 0; i < numQueens; i++)
               {
                newBoard[columnConflict] = i;
                int h = Heuristic(newBoard, numQueens);
                if (h < leastHeuristic)
                {   // this loop finds the board with the least hueristic and stores the board and the 
                        // hueristic
                    leastHeuristic = h;
                    for (int j = 0; j < numQueens; j++)
                    {
                        boardWithLeastH[j] = newBoard[j];
                    }
                }
            } //end of for loop
                // check to see if the leastHeuristic found is 0, if so we have our goal
            if (leastHeuristic == 0)
            {
                watch.Stop();
                Console.WriteLine(watch.Elapsed);
                printBoard(boardWithLeastH, numQueens, numStates, numRestarts);
                break;
            }
            else
            {
                    numStates++;
                    for (int i = 0; i < numQueens; i++)
                { // if the boardWith least huristic is not 0, then we reset newBoard with this board with least heuristic
                        
                        newBoard[i] = boardWithLeastH[i];
                }
            }
            // if we have reached the max number of steps allowed, we generate a new board
                if (numStates == maxSteps) {
                    numStates = 0;
                    numRestarts++;
                    newBoard = generateRandomBoard(numQueens);
                }

                //repeat in a while loop until goal is found
            }// end of while
        }// end of min Conflicts

        public static int TheColumnWithConflicts(int[] Board,int numQueens)
        {
            // find two rows with the same conflict and jsut pick one randomly: 
            Random rand = new Random();
            int num;
            for (int i = 0; i < numQueens; i++)
            {
                for (int j = i + 1; j < numQueens; j++)
                {    // if two rows are queal that means that 2 queens are conflicting
                    if (Board[i] == Board[j])
                    {   // rnadomly select either queen and return it
                        num = Convert.ToInt32(rand.Next()) % 2;
                        if (num == 1)
                        {
                            return i;
                        }
                        else
                        {
                            return j;
                        }

                    }
                    else if (Board[i] == Board[j] + (j - i) || Board[i] == Board[j] - (j - i))
                    {  // search the diagonla to see if any queens are conflicting, randomnly choose a 
                        //queen and return the column
                        num = Convert.ToInt32(rand.Next()) % 2;
                        if (num == 1)
                        {
                            return i;
                        }
                        else
                        {
                            return j;
                        }
                    }
                }
            }
            // if no conflicts, return -1
            return -1;

        }



    } //end of class
} //end of namespace
