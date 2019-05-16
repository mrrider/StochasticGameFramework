using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    public class WSum
    {
        public WSum(string wsum, int total, List<KeyValuePair<Agent, KeyValuePair<AgentAction, AgentState>>> list)
        {
            wSum = wsum;
            totalSum = total;
            sumList = list;
        }

        public string wSum;

        public int totalSum;

        public List<KeyValuePair<Agent, KeyValuePair<AgentAction, AgentState>>> sumList = new List<KeyValuePair<Agent, KeyValuePair<AgentAction, AgentState>>>();

        public override string ToString()
        {
            return "W=" + totalSum;
        }
    }
}
