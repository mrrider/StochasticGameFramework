using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    public class Game
    {
        #region PrivateVariables

        private List<Agent> _agents;
        private bool _logsEnabled;
        private int _numberOfAgents;
        private int _numberOfTurns;
        private int _numberOfStatesPerAgent;
        private int _numberOfActionsPerAgent;
        //private GameState _currentGameState;
        //private List<GameState> _gameStates = new List<GameState>();
        private Random r = new Random();
        public List<WSum> _sums;
        public List<Probs> _probs;
        #endregion

        #region Constructors
        public Game()
        {
            _agents = new List<Agent>();
        }

        #endregion

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
                    AgentState state = new AgentState(index, "A" + i + "-S" + index++);
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
                        Console.WriteLine("\t" + agent.ToString() + state.ToString());
                }
            }
        }

        //private void generateGameStates()
        //{
        //    var firstAgent = _agents.FirstOrDefault();

        //    foreach (AgentState currentState in firstAgent.statesList)
        //    {
        //        var otherAgents = new List<Agent>();
        //        foreach (var a in _agents)
        //        {
        //            otherAgents.Add(a);
        //        }
        //        otherAgents.RemoveAt(0);
        //        var currentKeyValuePair = new KeyValuePair<Agent, AgentState>(firstAgent, currentState);
        //        var agentsStatesList = new List<KeyValuePair<Agent, AgentState>>();
        //        agentsStatesList.Add(currentKeyValuePair);
        //        recourse(otherAgents, agentsStatesList);
        //    }
        //}

        //private void recourse(List<Agent> agentsList, List<KeyValuePair<Agent, AgentState>> agentsStatesList)
        //{
        //    if (agentsList.Count == 0)
        //        return;
        //    if (agentsList.Count == 1) //Last agent in list
        //    {
        //        foreach (AgentState lastAgentState in agentsList.FirstOrDefault().statesList)
        //        {
        //            List<KeyValuePair<Agent, AgentState>> statesBeforeAgents = new List<KeyValuePair<Agent, AgentState>>();
        //            foreach (KeyValuePair<Agent, AgentState> kp in agentsStatesList)
        //            {
        //                statesBeforeAgents.Add(kp);
        //            }
        //            statesBeforeAgents.Add(new KeyValuePair<Agent, AgentState>(agentsList.FirstOrDefault(), lastAgentState));
        //            int nextState = _gameStates.Count + 1;
        //            GameState gameState = new GameState(statesBeforeAgents, "GST" + nextState);
        //            _gameStates.Add(gameState);
        //        }
        //    } else
        //    {
        //        var currentAgent = agentsList.FirstOrDefault();
        //        bool secondIteration = false;
        //        foreach (AgentState currentState in currentAgent.statesList)
        //        {
        //            if (secondIteration)
        //            {
        //                var last = agentsStatesList.LastOrDefault();
        //                agentsStatesList.Remove(last);
        //            }
        //            var currentKeyValuePair = new KeyValuePair<Agent, AgentState>(currentAgent, currentState);
        //            agentsStatesList.Add(currentKeyValuePair);
        //            agentsList.Remove(currentAgent);
        //            recourse(agentsList, agentsStatesList);
        //            secondIteration = true;
        //        }
        //    }
        //}

        //private void logGameStates()
        //{
        //    if (_logsEnabled)
        //    {

        //        foreach (GameState gameState in _gameStates)
        //        {
        //            Console.WriteLine(gameState.ToString());
        //        }
        //    }
        //}

        //public GameState getRandomStartGameState()
        //{
        //    Random random = new Random();
        //    var state = _gameStates[random.Next(_gameStates.Count)];
        //    Console.WriteLine("Game Start State is " + state.gameState);
        //    return state;
        //}

        private void generateStartStateForAgents()
        {
            foreach (Agent a in _agents)
            {
                a._currentState = a.statesList[0];
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
            foreach (var a in _agents)
            {
                var actionList = new List<AgentAction>();
                for(int i = 0; i < _numberOfActionsPerAgent; i++)
                {
                    int reward = r.Next(0, 10);
                    actionList.Add(new AgentAction(index,  "A" + a.Id + "-ACT" + index++, reward));
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

        private void generateRandomProbsForAgents()
        {
            foreach (var a in _agents)
            {
                var probList = new List<AgentProb>();

                double probLeft = 1;

                var actionCount = a.actionsList.Count * a.statesList.Count;
                var lastAction = a.actionsList.LastOrDefault();
                var lastState = a.statesList.LastOrDefault();

                foreach (var action in a.actionsList)
                {
                    foreach(var state in a.statesList)
                    {
                        double prob = 0;
                        if (action.Equals(lastAction) && state.Equals(lastState))
                        {
                            prob = probLeft;
                        }
                        else
                        {
                            prob = getRandomNumber(0, probLeft / actionCount);
                        }
                        actionCount--;
                        prob = Math.Round(prob, 2);
                        AgentProb pr = new AgentProb(action, state, prob);
                        probList.Add(pr);
                        probLeft -= prob;
                    }
                }
                a.probList = probList;
            }
        }

        private void logAgentsProbs()
        {
            if (_logsEnabled)
            {
                foreach (Agent agent in _agents)
                {
                    Console.WriteLine(agent.ToString() + " has probs: ");
                    foreach (AgentProb prob in agent.probList)
                        Console.WriteLine("\t" + prob.ToString());
                }
            }
        }

        private void generateWSums()
        {
            _sums = new List<WSum>();
            var firstAgent = _agents.FirstOrDefault();

            foreach (AgentState currentState in firstAgent.statesList)
            {

                foreach(AgentAction currentAction in firstAgent.actionsList)
                {
                    var otherAgents = new List<Agent>();
                    foreach (var a in _agents)
                    {
                        otherAgents.Add(a);
                    }
                    otherAgents.RemoveAt(0);
                    var currentKeyValuePair = new KeyValuePair<Agent, KeyValuePair<AgentAction, AgentState>>
                                                    (firstAgent, new KeyValuePair<AgentAction, AgentState>(currentAction, currentState));
                    var agentsWSList = new List<KeyValuePair<Agent, KeyValuePair<AgentAction, AgentState>>>();
                    agentsWSList.Add(currentKeyValuePair);
                    recourseForWS(otherAgents, agentsWSList);
                }
            }
        }

        private void recourseForWS(List<Agent> agentsList, List<KeyValuePair<Agent, KeyValuePair<AgentAction, AgentState>>> agentsWSList)
        {
            if (agentsList.Count == 0)
                return;
            if (agentsList.Count == 1) //Last agent in list
            {
                foreach (AgentState lastAgentState in agentsList.FirstOrDefault().statesList)
                {
                    foreach(AgentAction lastAgnetAction in agentsList.FirstOrDefault().actionsList)
                    {
                        var rewardsBeforeAgents = new List<KeyValuePair<Agent, KeyValuePair<AgentAction, AgentState>>>();
                        foreach (var kp in agentsWSList)
                        {
                            rewardsBeforeAgents.Add(kp);
                        }

                        var lstKeyValuePair = new KeyValuePair<Agent, KeyValuePair<AgentAction, AgentState>>
                                                        (agentsList.FirstOrDefault(), new KeyValuePair<AgentAction, AgentState>(lastAgnetAction, lastAgentState));

                        rewardsBeforeAgents.Add(lstKeyValuePair);
                        int nextindex = _sums.Count + 1;

                        int total = 0;
                        foreach (var ws in rewardsBeforeAgents)
                        {
                            total += ws.Value.Key.reward;
                        }

                        WSum wsum = new WSum("WSUM" + nextindex, total, rewardsBeforeAgents);
                        _sums.Add(wsum);
                    }
                }
            }
            else
            {
                var currentAgent = agentsList.FirstOrDefault();
                bool secondIteration = false;
                foreach (var currentState in currentAgent.statesList)
                {
                    foreach(var currentAction in currentAgent.actionsList)
                    {
                        if (secondIteration)
                        {
                            var last = agentsWSList.LastOrDefault();
                            agentsWSList.Remove(last);
                        }

                        var currentKeyValuePair = new KeyValuePair<Agent, KeyValuePair<AgentAction, AgentState>>
                                                        (currentAgent, new KeyValuePair<AgentAction, AgentState>(currentAction, currentState));

                        agentsWSList.Add(currentKeyValuePair);
                        agentsList.Remove(currentAgent);
                        recourseForWS(agentsList, agentsWSList);
                        secondIteration = true;
                    }
                }
            }
        }

        private void logWSums()
        {
            Console.WriteLine("Wsums:");
            foreach (var sum in _sums)
            {
                string text = "\t" + sum.wSum + ": (";
                foreach (var pair in sum.sumList)
                {
                    text += "A" + pair.Key.Id + "-" + pair.Value.Key.action + ";";
                }
                text += ") = " + sum.totalSum;
                Console.WriteLine(text);
            }
        }

        private void countProbs()
        {
            _probs = new List<Probs>();

            var firstAgent = _agents.FirstOrDefault();

            foreach (AgentState currentState in firstAgent.statesList)
            {
                foreach (AgentAction currentAction in firstAgent.actionsList)
                {
                    foreach (AgentState nextState in firstAgent.statesList)
                    {
                        var otherAgents = new List<Agent>();
                        foreach (var a in _agents)
                        {
                            otherAgents.Add(a);
                        }
                        otherAgents.RemoveAt(0);
                        var currentRew = firstAgent.probList.FindAll(x => x.action.action.Equals(currentAction.action))
                                                                .Find(y => y.nextState.state.Equals(nextState.state));
                        var currentKeyValuePair = new KeyValuePair<Agent, TransferPair>(firstAgent, new TransferPair(currentState, currentAction, nextState, currentRew.probability));

                        var agentsPRList = new List<KeyValuePair<Agent, TransferPair>>();

                        agentsPRList.Add(currentKeyValuePair);
                        recourseForWS(otherAgents, agentsPRList);
                    }
                }
            }
        }

        private void recourseForWS(List<Agent> agentsList, List<KeyValuePair<Agent, TransferPair>> agentsPRList)
        {
            if (agentsList.Count == 0)
                return;
            if (agentsList.Count == 1) //Last agent in list
            {
                foreach (AgentState lastAgentState in agentsList.FirstOrDefault().statesList)
                {
                    foreach (AgentAction lastAgnetAction in agentsList.FirstOrDefault().actionsList)
                    {
                        foreach (AgentState lastAgentNextState in agentsList.FirstOrDefault().statesList)
                        {
                            var rewardsBeforeAgents = new List<KeyValuePair<Agent, TransferPair>>();
                            foreach (var kp in agentsPRList)
                            {
                                rewardsBeforeAgents.Add(kp);
                            }
                            var currentRew = agentsList.FirstOrDefault().probList.FindAll(x => x.action.action.Equals(lastAgnetAction.action))
                                                                .Find(y => y.nextState.state.Equals(lastAgentNextState.state));

                            var lstKeyValuePair = new KeyValuePair<Agent, TransferPair>(agentsList.FirstOrDefault(), 
                                                            new TransferPair(lastAgentState, lastAgnetAction, lastAgentNextState, currentRew.probability));

                            rewardsBeforeAgents.Add(lstKeyValuePair);
                            int nextindex = _probs.Count + 1;

                            double total = 0;
                            foreach (var ws in rewardsBeforeAgents)
                            {
                                if (ws.Equals(rewardsBeforeAgents.FirstOrDefault()))
                                {
                                    total = ws.Value.prob;
                                }else
                                {
                                    total *= ws.Value.prob;
                                }

                            }

                            Probs probs = new Probs("ProBS" + nextindex, total, rewardsBeforeAgents);
                            _probs.Add(probs);
                        }
                    }
                }
            }
            else
            {
                var currentAgent = agentsList.FirstOrDefault();
                bool secondIteration = false;
                foreach (var currentState in currentAgent.statesList)
                {
                    foreach (var currentAction in currentAgent.actionsList)
                    {
                        foreach (var nextState in currentAgent.statesList)
                        {
                            if (secondIteration)
                            {
                                var last = agentsPRList.LastOrDefault();
                                agentsPRList.Remove(last);
                            }

                            var currentRew = currentAgent.probList.FindAll(x => x.action.action.Equals(currentAction.action))
                                                                .Find(y => y.nextState.state.Equals(nextState.state));

                            var lstKeyValuePair = new KeyValuePair<Agent, TransferPair>(agentsList.FirstOrDefault(),
                                                            new TransferPair(currentState, currentAction, nextState, currentRew.probability));


                            agentsPRList.Add(lstKeyValuePair);
                            agentsList.Remove(currentAgent);
                            recourseForWS(agentsList, agentsPRList);
                            secondIteration = true;
                        }
                    }
                }
            }
        }

        private void logProbs()
        {
            double counter = 0;
            Console.WriteLine("Probs:");
            foreach (var prob in _probs)
            {
                string text = "\t" + prob.prob + ": (";
                foreach (var pair in prob.agentsVector)
                {
                    text += "A" + pair.Key.Id + "-" + pair.Value.stateBefore + "->" + pair.Value.action + "->" + pair.Value.stateAfter + ";";
                }
                text += ") = " + prob.probTotal;
                counter += prob.probTotal;
                Console.WriteLine(text);
            }
            Console.WriteLine("TotalProbs = " + counter);
        }

        private void countPerLines()
        {
            var result = new List<List<Probs>>();
            int x = 0;
            int index = 0;
            List<Probs> resLoop = new List<Probs>();
            foreach (var prob in _probs)
            {
                if (index == x)
                {
                    if(x!=0)
                        result.Add(resLoop);
                    resLoop = null;
                    resLoop = new List<Probs>();
                    x += 16;
                }

                resLoop.Add(prob);
                index++;
            }

            foreach(var i in result)
            {
                
                double total = 0;
                foreach (var j in i)
                {
                    total += j.probTotal;
                }
                Console.WriteLine("Sum q = " + total);
            }

        }

        private int doSimplex()
        {
            var vector = new double[_sums.Count];
            for (int i = 0; i < _sums.Count; i++)
            {
                vector[i] = _sums[i].totalSum;
            }
            var matrix = new double[5,_sums.Count];

            for(int i = 0; i < _probs.Count; i++)
            {
                if(i >= 0 && i <= 15)
                {
                    matrix[0,i] = _probs[i].probTotal;
                }
                if (i >= 16 && i <= 31)
                {
                    matrix[1,i -16] = _probs[i].probTotal;
                }
                if (i >= 32 && i <= 47)
                {
                    matrix[2,i-32] =  _probs[i].probTotal;
                }
                if (i >= 48)
                {
                    matrix[3, i-48] =  _probs[i].probTotal;
                }
            }

            for (int i = 0; i < _sums.Count; i++)
            {
                matrix[4,i] = 1;
            }
            for(int i = 0; i < matrix.GetLength(0); i++)
            {
                string res = "";
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    res += matrix[i, j] + ";";
                }
                Console.WriteLine(res);
            }
            var s = new Simplex(
              vector, matrix,
              //new[,] {
              //        {0.1, 0.5, 0.333333, 1},
              //        {30, 15, 19, 12},
              //        {1000, 6000, 4100, 9100},
              //        {50, 20, 21, 10},
              //        {4, 6, 19, 30}
              //            }, 
              //vector2
            new double[] { 0,0,0,0,1 }
            );

            var answer = s.maximize();
            //var res = _sums[int.Parse(answer.Item1.ToString())];
            Console.WriteLine(answer.Item1);
            Console.WriteLine(string.Join(", ", answer.Item2));

            return int.Parse(answer.Item1.ToString());
        }

        public void generateDataForAgents()
        {
            
            int i = 0;
            do
            {
                _agents = null;
                _agents = new List<Agent>();
                _sums = null;
                _probs = null;
                generateAgentsAndStates();
                logAgentsAndStates();
                //generateGameStates();
                //logGameStates();
                //_currentGameState = getRandomStartGameState();
                generateStartStateForAgents();
                logAgentsStates();
                generateActionsForAgent();
                logAgentsActions();
                generateRandomProbsForAgents();
                logAgentsProbs();
                //GenerateAgentsRewards();
                generateWSums();
                logWSums();
                countProbs();
                logProbs();
                //countPerLines();
                i = doSimplex();
            } while (i == 0);
        }

        #endregion

        //public void doGame()
        //{
        //    for (int iteration = 1; iteration <= _numberOfTurns; iteration++)
        //    {
        //        Console.WriteLine("Turn " + iteration);
        //        foreach (Agent agent in _agents)
        //        {
        //            string res = "\t Agent A" + agent.Id + "is in state " + agent._currentState.ToString();
        //            agent.doRandomAction(_currentGameState, _logsEnabled);
        //            res += "; new state " + agent._currentState.ToString() + " total reward is " + agent.totalReward;
        //            Console.WriteLine(res);
        //        }
        //        if (!moveGameState())
        //            break;
        //    }
        //}

        //public bool moveGameState()
        //{
        //    var agentStates = new List<KeyValuePair<Agent, AgentState>>();
        //    foreach(Agent agent in _agents)
        //    {
        //        agentStates.Add(new KeyValuePair<Agent, AgentState>(agent, agent._currentState));
        //    }
        //    var found = false;
        //    foreach (GameState gs in _gameStates)
        //    {
        //        found = gs.agentsStates.All(agentStates.Contains) && gs.agentsStates.Count == agentStates.Count;
        //        if (found)
        //        {
        //            _currentGameState = gs;
        //            Console.WriteLine("New Game State " + _currentGameState);
        //            break;
        //        }
        //    }
        //    if (!found)
        //    {
        //        Console.WriteLine("ERROR");
        //    }
        //    return found;
        //}

        #region Properties
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

        public int numberOfActionsPerAgent
        {
            get
            {
                return _numberOfActionsPerAgent;
            }
            set
            {
                _numberOfActionsPerAgent = value;
            }
        }

        //public List<GameState> gameStates
        //{
        //    get
        //    {
        //        return _gameStates;
        //    }
        //    set
        //    {
        //        _gameStates = value;
        //    }
        //}

        #endregion

        #region Utils
        public double getRandomNumber(double min, double max)
        {
            //Random random = new Random();
            return r.NextDouble() * max;
        }

        #endregion
    }
}
