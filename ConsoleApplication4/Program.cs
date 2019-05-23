using StochasticGameFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StochasticGameFramework.Game;

namespace StochasticGameFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            game.logsEnabled = true;
            game.numberOfAgentsPerCoalition = 2;
            game.numberOfCoalitions = 2;
            game.numberOfTurns = 5;
            game.numberOfStatesPerAgent = 2;
            game.numberOfActionsPerAgent = 2;
            game.generateData();
            game.playGame();


            Console.ReadLine();
        }
    }
}
