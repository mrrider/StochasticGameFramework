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
            game.generateDataForAgents();

            //agent1._probability = agent1.generateRandomProbability(agent2States);
            //agent2._probability = agent2.generateRandomProbability(agent1States);

            //agentsList.Add(agent1);
            //agentsList.Add(agent2);



            //var agent1Reward = agent1.doAction(agent2._currentState);
            //var agent2Reward = agent2.doAction(agent1._currentState);

            //Console.WriteLine("Agent 1 was in state " + agent1Reward.agentState + " choosed action " + agent1Reward.agentAction + " and got " + agent1Reward.reward);
            //Console.WriteLine("Agent 2 was in state " + agent2Reward.agentState + " choosed action " + agent2Reward.agentAction + " and got " + agent2Reward.reward);

            Console.ReadLine();
        }
    }
}
