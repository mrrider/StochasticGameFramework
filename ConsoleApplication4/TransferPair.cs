using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StochasticGameFramework
{
    public class TransferPair
    {
        public AgentState stateBefore;
        public AgentAction action;
        public AgentState stateAfter;
        public double prob;

        public TransferPair(AgentState _stateBefore, AgentAction _action, AgentState _stateAfter, double _prob)
        {
            stateBefore = _stateBefore;
            action = _action;
            stateAfter = _stateAfter;
            prob = _prob;
        }
    }
}
