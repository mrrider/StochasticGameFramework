
namespace ConsoleApplication4
{
    public class AgentState
    {
        private int _id;
        public int id
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
        public string state
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

        public AgentState(int id, string state)
        {
            _id = id;
            _state = state;
        }

        public override string ToString()
        {
            return "S" + _id;
        }
    }
}
