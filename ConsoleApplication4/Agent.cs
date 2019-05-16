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
        public int totalReward = 0;

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
        }

        public void doRandomAction(GameState gameState, bool logsEnabled)
        {
            //var avaliableActions = _actionsList.FindAll(x => x.currentState == _currentState).FindAll(y => y.gameState == gameState);
            var avaliableActionsRewards = new List<AgentReward>();
            //foreach(AgentAction act in avaliableActions)
            //{
            //    avaliableActionsRewards.Add(_rewardsList.Find(x => x.action == act));
            //}

            avaliableActionsRewards.Sort((x, y) => x.probability.CompareTo(y.probability));

            var totalProbForAvail = avaliableActionsRewards.Sum(x => x.probability);


            double random = Game.getRandomNumber(0, totalProbForAvail);
            double cumulative = 0.0;

            AgentReward reward = null;
            foreach (AgentReward prob in avaliableActionsRewards)
            {
                cumulative += prob.probability;
                if (random < cumulative)
                {
                    reward = prob;
                    break;
                }
            }

            _currentState = reward.action.nextState;
            totalReward += reward.reward;

            //Console.WriteLine(this.ToString() + "-" + reward.action + ";total" + totalReward );
        }

        public override string ToString()
        {
            return "A" + _id;
        }
    }
}
