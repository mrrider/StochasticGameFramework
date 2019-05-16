
namespace ConsoleApplication4
{
    public class AgentProb
    {

        private AgentAction _action;
        private AgentState _nextState;
        private double _probability;

        public AgentAction action
        {
            get
            {
                return _action;
            }
            set
            {
                _action = value;
            }
        }

        public AgentState nextState
        {
            get
            {
                return _nextState;
            }
            set
            {
                _nextState = value;
            }
        }

        public double probability
        {
            get
            {
                return _probability;
            }
            set
            {
                _probability = value;
            }
        }

        public AgentProb(AgentAction agentAction, AgentState nextState, double prob)
        {
            action = agentAction;
            _nextState = nextState;
            _probability = prob;
        }

        public override string ToString()
        {
            return _action.ToString() + "->" + _nextState.ToString() + ":" + _probability;
        }
    }
}
