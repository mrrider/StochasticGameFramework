using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    public class Probs
    {
        public string prob;
        public double probTotal;
        public List<KeyValuePair<Agent, TransferPair>> agentsVector = new List<KeyValuePair<Agent, TransferPair>>();

        public Probs(string _prob, double _probTotal, List<KeyValuePair<Agent, TransferPair>> _agentsVector)
        {
            prob = _prob;
            probTotal = _probTotal;
            agentsVector = _agentsVector;
        }
    }
}
