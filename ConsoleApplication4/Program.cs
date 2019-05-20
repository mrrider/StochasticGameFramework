using ConsoleApplication4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleApplication4.Game;

namespace ConsoleApplication4
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            game.logsEnabled = true;
            game.numberOfAgents = 2;
            game.numberOfTurns = 5;
            game.numberOfStatesPerAgent = 2;
            game.numberOfActionsPerAgent = 2;
            game.generateDataForAgents();
            //game.doGame();

            Console.ReadLine();
        }
    }
}
