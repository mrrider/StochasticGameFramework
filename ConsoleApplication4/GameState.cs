using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    public class GameState
    {
        private List<KeyValuePair<Agent, AgentState>> _agentsStates;
        private string _gameState;

        public GameState(List<KeyValuePair<Agent, AgentState>> agentStates, string gameState)
        {
            _agentsStates = agentStates;
            _gameState = gameState;
        }

        public List<KeyValuePair<Agent, AgentState>> agentsStates
        {
            get
            {
                return _agentsStates;
            }
            set
            {
                _agentsStates = value;
            }
        }

        public string gameState
        {
            get
            {
                return _gameState;
            }
            set
            {
                _gameState = value;
            }
        }

        public override string ToString()
        {
            string res = "";
            foreach(KeyValuePair<Agent, AgentState> curr in _agentsStates)
            {
                res += curr.Key.ToString() + curr.Value.ToString() + ";";
            }

            return _gameState + " - " + res;
        }
    }
}
