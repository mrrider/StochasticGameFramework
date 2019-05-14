
namespace ConsoleApplication4
{
    public class AgentAction
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

        private AgentState _currentState;
        public AgentState currentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                _currentState = value;
            }
        }

        private AgentState _nextState;
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

        private GameState _gameState;
        public GameState gameState
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

        private string _action;
        public string action
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

        public AgentAction(int id, GameState gameState, AgentState currentState, AgentState nextState, string action)
        {
            _id = id;
            _currentState = currentState;
            _nextState = nextState;
            _action = action;
            _gameState = gameState;
        }

        public override string ToString()
        {
            return "ACT" + _id + " " + _action + ";" + currentState.state + ">" + nextState.state + " - " + _gameState.ToString();
        }
    }
}
