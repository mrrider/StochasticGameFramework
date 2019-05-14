using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    public class Game
    {
        private List<Agent> _agents;
        public List<AgentAction> allActions;
        private bool _logsEnabled;
        private int _numberOfAgents;
        private int _numberOfTurns;
        private int _numberOfStatesPerAgent;
        private GameState _currentGameState;
        private List<GameState> _gameStates = new List<GameState>();
        private Random r = new Random();

        public Game()
        {
            _agents = new List<Agent>();
        }

        #region DataGeneration
        private void generateAgentsAndStates()
        {
            int index = 0;
            for (int i = 0; i < _numberOfAgents; i++)
            {
                Agent agent = new Agent(i);
                List<AgentState> agentStates = new List<AgentState>();
                for (int j = 0; j < _numberOfStatesPerAgent; j++)
                {
                    AgentState state = new AgentState(index, "AGENT" + i + "-STATE" + index++);
                    agentStates.Add(state);
                }
                agent.statesList = agentStates;
                _agents.Add(agent);
            }
            
        }

        private void logAgentsAndStates()
        {
            if (_logsEnabled)
            {
                foreach (Agent agent in _agents)
                {
                    Console.WriteLine(agent.ToString() + " has states: ");
                    foreach (AgentState state in agent.statesList)
                        Console.WriteLine("\t" + agent.ToString() +  state.ToString());
                }
            }
        }

        private void generateGameStates()
        {
            var keyValue = new List<KeyValuePair<Agent, AgentState>>();

            var firstAgent = _agents.FirstOrDefault();


            foreach (AgentState currentState in firstAgent.statesList)
            {
                var otherAgents = new List<Agent>();
                foreach(var a in _agents)
                {
                    otherAgents.Add(a);
                }
                otherAgents.RemoveAt(0);
                var currentKeyValuePair = new KeyValuePair<Agent, AgentState>(firstAgent, currentState);
                var agentsStatesList = new List<KeyValuePair<Agent, AgentState>>();
                agentsStatesList.Add(currentKeyValuePair);
                recourse(otherAgents, agentsStatesList);
            }
        }

        private void recourse(List<Agent> agentsList, List<KeyValuePair<Agent, AgentState>> agentsStatesList)
        {
            if (agentsList.Count == 0)
                return;
            if(agentsList.Count == 1) //Last agent in list
            {
                foreach(AgentState lastAgentState in agentsList.FirstOrDefault().statesList)
                {
                    List<KeyValuePair<Agent, AgentState>>  statesBeforeAgents = new List<KeyValuePair<Agent, AgentState>>();
                    foreach (KeyValuePair<Agent, AgentState> kp in agentsStatesList)
                    {
                        statesBeforeAgents.Add(kp);
                    }
                    statesBeforeAgents.Add(new KeyValuePair<Agent, AgentState>(agentsList.FirstOrDefault(), lastAgentState));
                    int nextState = _gameStates.Count + 1;
                    GameState gameState = new GameState(statesBeforeAgents, "GST" + nextState);
                    _gameStates.Add(gameState);
                }
            }else
            {
                var currentAgent = agentsList.FirstOrDefault();
                bool secondIteration = false;
                foreach (AgentState currentState in currentAgent.statesList)
                {
                    if (secondIteration)
                    {
                        var last = agentsStatesList.LastOrDefault() ;
                        agentsStatesList.Remove(last);
                    }
                    var currentKeyValuePair = new KeyValuePair<Agent, AgentState>(currentAgent, currentState);
                    agentsStatesList.Add(currentKeyValuePair);
                    agentsList.Remove(currentAgent);
                    recourse(agentsList, agentsStatesList);
                    secondIteration = true;
                }
            }
        }

        private void logGameStates()
        {
            if (_logsEnabled)
            {

                foreach (GameState gameState in _gameStates)
                {
                    Console.WriteLine(gameState.ToString());
                }
            }
        }

        public GameState getRandomStartGameState()
        {
            Random random = new Random();
            var state = _gameStates[random.Next(_gameStates.Count)];
            Console.WriteLine("Game Start State is " + state.gameState);
            return state;
        }

        private void generateActions()
        {
            allActions = new List<AgentAction>();
            int index = 0;

            //foreach (AgentState currentstate in allStates)
            //{
            //    foreach (AgentState nextstate in allStates)
            //    {
            //        allActions.Add(new AgentAction(index, currentstate, nextstate, "ACTION" + index));
            //        index++;
            //    }
            //}
                
        }

        private void logActions()
        {
            if (_logsEnabled)
            {
                foreach (AgentAction action in allActions)
                    Console.WriteLine(action.ToString());
            }
        }

        private void generateStartStateForAgents()
        {
            foreach(Agent a in _agents)
            {
                a._currentState = _currentGameState.agentsStates.Find(x => x.Key.Id == a.Id).Value;
            }
        }

        private void logAgentsStates()
        {
            foreach (Agent a in _agents)
            {
                Console.WriteLine("Start state for Agent " + a.Id + " is " + a._currentState);
            }
        }

        private void generateActionsForAgent()
        {
            int index = 0;
            foreach(var a in _agents)
            {
                var actionList = new List<AgentAction>();
                foreach (GameState gameState in _gameStates) { 
                    foreach(var currState in a.statesList)
                    {
                        foreach(var otherState in a.statesList)
                        {
                            actionList.Add(new AgentAction(index, gameState, currState, otherState, "A" + a.Id + "ST" + index++));
                        }
                    }
                }
                a.actionsList = actionList;
            }
        }

        private void logAgentsActions()
        {
            if (_logsEnabled)
            {
                foreach (Agent agent in _agents)
                {
                    Console.WriteLine(agent.ToString() + " has actions: ");
                    foreach (AgentAction action in agent.actionsList)
                        Console.WriteLine("\t" + action.ToString());
                }
            }
        }

        private void generateRandomRewardsForAgents()
        {
            foreach(var a in _agents)
            {
                var rewardList = new List<AgentReward>();
                double probLeft = 1;
                var lastAction = a.actionsList.LastOrDefault();
                foreach (var action in a.actionsList)
                {
                    double prob = 0;
                    if (action.Equals(lastAction))
                    {
                        prob = probLeft;
                    }else
                    {
                        prob = getRandomNumber(0, probLeft);
                    }
                    int reward = r.Next(-10, 10);
                    AgentReward rw = new AgentReward(action, reward, prob);
                    rewardList.Add(rw);
                    probLeft -= prob;
                }
                a.rewardsList = rewardList;
            }
        }

        private void logAgentsRewards()
        {
            if (_logsEnabled)
            {
                foreach (Agent agent in _agents)
                {
                    Console.WriteLine(agent.ToString() + " has rewards: ");
                    foreach (AgentReward reward in agent.rewardsList)
                        Console.WriteLine("\t" + reward.ToString());
                }
            }
        }

        public void generateDataForAgents()
        {
            generateAgentsAndStates();
            logAgentsAndStates();
            generateGameStates();
            logGameStates();
            _currentGameState = getRandomStartGameState();
            generateStartStateForAgents();
            logAgentsStates();
            generateActionsForAgent();
            logAgentsActions();
            generateRandomRewardsForAgents();
            logAgentsRewards();
        }
        
        #endregion

        public void doGame()
        {

        }

        public List<Agent> agents
        {
            get
            {
                return _agents;
            }
            set
            {
                _agents = value;
            }
        }

        public bool logsEnabled
        {
            get
            {
                return _logsEnabled;
            }
            set
            {
                _logsEnabled = value;
            }
        }

        public int numberOfAgents
        {
            get
            {
                return _numberOfAgents;
            }
            set
            {
                _numberOfAgents = value;
            }
        }

        public int numberOfTurns
        {
            get
            {
                return _numberOfTurns;
            }
            set
            {
                _numberOfTurns = value;
            }
        }

        public int numberOfStatesPerAgent
        {
            get
            {
                return _numberOfStatesPerAgent;
            }
            set
            {
                _numberOfStatesPerAgent = value;
            }
        }

        public List<GameState> gameStates
        {
            get
            {
                return _gameStates;
            }
            set
            {
                _gameStates = value;
            }
        }

        #region Utils
        public double getRandomNumber(double min, double max)
        {
            return r.NextDouble() * max;
        }

        #endregion
    }
}
