using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StochasticGameFramework
{
    public class Coalition
    {
        public int id;
        public string CoalitionName;
        public List<Agent> coalitionMembers;

        public Coalition(int _id, string name)
        {
            id = _id;
            CoalitionName = name;
        }
    }
}
