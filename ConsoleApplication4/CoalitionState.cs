
using System.Collections.Generic;

namespace StochasticGameFramework
{
    public class CoalitionState
    {
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

        private string _state;
        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        private List<AgentState> _agentsStates;

        public List<AgentState> AgentsStates
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

        public CoalitionState(int id, string state, List<AgentState> list)
        {
            _id = id;
            _state = state;
            _agentsStates = list;
        }

        public override string ToString()
        {
            return "S" + _id;
        }
    }
}
