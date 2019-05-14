
namespace ConsoleApplication4
{
    public class AgentReward
    {
        private AgentAction _action;
        private int _reward;
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

        public int reward
        {
            get
            {
                return _reward;
            }
            set
            {
                _reward = value;
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

        public AgentReward(AgentAction agentAction, int reward, double prob)
        {
            action = agentAction;
            _reward = reward;
            _probability = prob;
        }

        public override string ToString()
        {
            return "REWARD " + _action.action + " - " + _reward + " PROB " + _probability;
        }
    }
}
