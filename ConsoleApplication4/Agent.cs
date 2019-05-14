using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleApplication4.Game;

namespace ConsoleApplication4
{
    public class Agent
    {
        private List<AgentState> _statesList;
        private List<AgentAction> _actionsList;
        private List<AgentReward> _rewardsList; 
        public AgentState _currentState;

        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public List<AgentState> statesList
        {
            get
            {
                return _statesList;
            }
            set
            {
                _statesList = value;
            }
        }

        public List<AgentAction> actionsList
        {
            get
            {
                return _actionsList;
            }
            set
            {
                _actionsList = value;
            }
        }

        public List<AgentReward> rewardsList
        {
            get
            {
                return _rewardsList;
            }
            set
            {
                _rewardsList = value;
            }
        }

        public Agent(int id)
        {
            _id = id;
            //_rewards = generateRandomRewards();
        }

        //public AgentReward doAction(string opponentState)
        //{
        //    Random r = new Random();
        //    double random = r.NextDouble();
        //    double cumulative = 0.0;

        //    var avaliableActions = _probability.FindAll(x => x.agentState == _currentState).FindAll(y => y.opponentState == opponentState);
        //    avaliableActions.Sort((x, y) => x.probability.CompareTo(y.probability));
        //    if (Game.logsEnabled)
        //    {
        //        string json = JsonConvert.SerializeObject(avaliableActions, Formatting.Indented);
        //        Console.WriteLine("Avaliable actions for Agent " + Id);
        //        Console.WriteLine(json);
        //    }
        //    AgentReward reward = null;
        //    foreach(AgentProbability prob in avaliableActions)
        //    {
        //        cumulative += prob.probability;
        //        if(random < cumulative)
        //        {
        //            reward = _rewards.FindAll(x => x.agentState == prob.agentState).FindLast(y => y.agentAction == prob.agentAction);
        //            break;
        //        }
        //    }

        //    return reward;
        //}

        //public List<AgentProbability> generateRandomProbability(List<agentStates> opponentStates)
        //{
        //    var result = new List<AgentProbability>();
        //    Random r = new Random();
        //    foreach (agentStates agentState in _statesList)
        //    {
        //        foreach (agentStates oppState in opponentStates)
        //        {
        //            foreach (agentActions agentAcction in _actionsList)
        //            {
        //                double randomProbability = r.NextDouble();
        //                AgentProbability prob = new AgentProbability(agentState, oppState, agentAcction, randomProbability);
        //                result.Add(prob);
        //            }
        //        }
        //    }
        //    if (Game.logsEnabled)
        //    {
        //        string json = JsonConvert.SerializeObject(result, Formatting.Indented);
        //        Console.WriteLine("Probability matrix for Agent " + Id);
        //        Console.WriteLine(json);
        //    }

        //    return result;
        //}

        //private List<AgentReward> generateRandomRewards()
        //{
        //    var result = new List<AgentReward>();
        //    Random r = new Random();
        //    foreach (agentStates state in _statesList)
        //    {
        //        foreach (agentActions action in _actionsList)
        //        {
        //            int rInt = r.Next(-10, 10);
        //            AgentReward reward = new AgentReward(state, action, rInt);
        //            result.Add(reward);
        //        }

        //    }
        //    if (Game.logsEnabled)
        //    {
        //        string json = JsonConvert.SerializeObject(result, Formatting.Indented);
        //        Console.WriteLine("Rewards matrix for Agent " + Id);
        //        Console.WriteLine(json);
        //    }
        //    return result;
        //}

        public override string ToString()
        {
            return "A" + _id;
        }
    }
}
