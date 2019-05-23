using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StochasticGameFramework
{
    public class Game
    {
        #region PrivateVariables

        public List<Coalition> _coalitions;
        
        private bool _logsEnabled;
        private int _numberOfAgentsPerCoalition;
        private int _numberOfTurns;
        private int _numberOfStatesPerAgent;
        private int _numberOfActionsPerAgent;
        private int _numberOfCoalitions;
        private Random r = new Random();
        public List<WSum> _sums;
        public List<Probs> _probs;

        #endregion

        #region DataGeneration

        private void generateCoalitions()
        {
            _coalitions = new List<Coalition>();
            for (int i = 1; i <= numberOfCoalitions; i++)
            {
                Coalition c = new Coalition(i, "C"+i);
                _coalitions.Add(c);
            }
        }

        private void generateDataForCoalitions()
        {
            foreach(var coalition in _coalitions)
            {
                coalition.coalitionMembers = generateAgentsAndStates(coalition.id);
                foreach(var agent in coalition.coalitionMembers)
                {
                    agent._currentState = generateStartStateForAgent(agent);
                    agent.actionsList = generateActionsForAgent(agent);
                }
            }
        }

        private List<Agent> generateAgentsAndStates(int coalitionId)
        {
            List<Agent> result = new List<Agent>();
            for (int i = 1; i <= _numberOfAgentsPerCoalition; i++)
            {
                Agent agent = new Agent(int.Parse(coalitionId.ToString() +  i.ToString()));
                List<AgentState> agentStates = new List<AgentState>();
                for (int j = 0; j < _numberOfStatesPerAgent; j++)
                {
                    var stateId = int.Parse(agent.Id.ToString() + j.ToString());
                    AgentState state = new AgentState(stateId, "A" + agent.Id + "-S" + stateId);
                    agentStates.Add(state);
                }
                agent.statesList = agentStates;
                result.Add(agent);
            }
            return result;
        }

        private AgentState generateStartStateForAgent(Agent agent)
        {
            Random rnd = new Random();
            int r = rnd.Next(agent.statesList.Count);
            return agent.statesList[r];
        }
       
        private List<AgentAction> generateActionsForAgent(Agent agent)
        {
            var actionList = new List<AgentAction>();
            for(int i = 0; i < _numberOfActionsPerAgent; i++)
            {
                int reward = r.Next(-10, 10);
                int actionId = int.Parse(agent.Id.ToString() + 00 + i.ToString());
                actionList.Add(new AgentAction(actionId,  "A" + agent.Id + "-ACT" + actionId, reward));
            }    
            return actionList;
        }
        
        public void generateData()
        {
            generateCoalitions();
            generateDataForCoalitions();
            logInputData();
        }
        #endregion

        #region DataPerStep
        private List<AgentProb> generateRandomProbsForAgent(Agent agent)
        {
            var probList = new List<AgentProb>();
            double probLeft = 1;

            var actionCount = agent.actionsList.Count * agent.statesList.Count;
            var lastAction = agent.actionsList.LastOrDefault();
            var lastState = agent.statesList.LastOrDefault();

            foreach (var action in agent.actionsList)
            {
                foreach (var state in agent.statesList)
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
            return probList;
        }
        
        private void generateDataForStep()
        {
            foreach (var coalition in _coalitions)
            {
                foreach (var agent in coalition.coalitionMembers)
                {
                    agent.probList = generateRandomProbsForAgent(agent);
                }
            }
        }

        private void countDataForSimplex()
        {
            _sums = null;
            _probs = null;
            var agents = generateWSums();
            findProbs(agents);
        }

        private List<Agent> generateWSums()
        {
            _sums = new List<WSum>();
            List<Agent> agents = new List<Agent>();
            foreach (var c in _coalitions)
                foreach (var a in c.coalitionMembers)
                    agents.Add(a);

            var firstAgent = agents.FirstOrDefault();

            foreach (AgentState currentState in firstAgent.statesList)
                foreach(AgentAction currentAction in firstAgent.actionsList)
                {
                    var otherAgents = new List<Agent>();
                    foreach (var a in agents)
                        otherAgents.Add(a);

                    otherAgents.RemoveAt(0);
                    var currentKeyValuePair = new KeyValuePair<Agent, KeyValuePair<AgentAction, AgentState>>
                                                    (firstAgent, new KeyValuePair<AgentAction, AgentState>(currentAction, currentState));
                    var agentsWSList = new List<KeyValuePair<Agent, KeyValuePair<AgentAction, AgentState>>>();
                    agentsWSList.Add(currentKeyValuePair);
                    wSumsRecursion(otherAgents, agentsWSList);
                }
            return agents;
        }

        private void wSumsRecursion(List<Agent> agentsList, List<KeyValuePair<Agent, KeyValuePair<AgentAction, AgentState>>> agentsWSList)
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

                bool secondStateIter = false;
                foreach (var currentState in currentAgent.statesList)
                {
                    bool secondIteration = false;
                    foreach (var currentAction in currentAgent.actionsList)
                    {
                        if (secondIteration || secondStateIter)
                        {
                            var last = agentsWSList.LastOrDefault();
                            agentsWSList.Remove(last);
                        }

                        var currentKeyValuePair = new KeyValuePair<Agent, KeyValuePair<AgentAction, AgentState>>
                                                        (currentAgent, new KeyValuePair<AgentAction, AgentState>(currentAction, currentState));

                        agentsWSList.Add(currentKeyValuePair);
                        agentsList.Remove(currentAgent);
                        wSumsRecursion(agentsList, agentsWSList);
                        secondIteration = true;
                    }
                    secondStateIter = true;
                }
            }
        }

        private void findProbs(List<Agent> agents)
        {
            _probs = new List<Probs>();

            var firstAgent = agents.FirstOrDefault();

            foreach (AgentState currentState in firstAgent.statesList)
            {
                foreach (AgentAction currentAction in firstAgent.actionsList)
                {
                    foreach (AgentState nextState in firstAgent.statesList)
                    {
                        var otherAgents = new List<Agent>();
                        foreach (var a in agents)
                        {
                            otherAgents.Add(a);
                        }
                        otherAgents.RemoveAt(0);
                        var currentRew = firstAgent.probList.FindAll(x => x.action.action.Equals(currentAction.action))
                                                                .Find(y => y.nextState.state.Equals(nextState.state));
                        var currentKeyValuePair = new KeyValuePair<Agent, TransferPair>(firstAgent, new TransferPair(currentState, currentAction, nextState, currentRew.probability));

                        var agentsPRList = new List<KeyValuePair<Agent, TransferPair>>();

                        agentsPRList.Add(currentKeyValuePair);
                        probsRecursion(otherAgents, agentsPRList);
                    }
                }
            }
        }

        private void probsRecursion(List<Agent> agentsList, List<KeyValuePair<Agent, TransferPair>> agentsPRList)
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
                            probsRecursion(agentsList, agentsPRList);
                            secondIteration = true;
                        }
                    }
                }
            }
        }

        #endregion

        private WSum doSimplex()
        {
            var vector = new double[_sums.Count];
            for (int i = 0; i < _sums.Count; i++)
            {
                vector[i] = _sums[i].totalSum;
            }

            int matrixHeight = (_probs.Count / _sums.Count) + 1;
            var matrix = new double[matrixHeight, _sums.Count];

            int currentRow = 0;
            int currentCol = 0;
            for (int i = 0; i < _probs.Count; i++)
            {
                matrix[currentRow, currentCol] =  _probs[i].probTotal;
                if(currentCol == _sums.Count-1)
                {
                    currentRow++;
                    currentCol = 0;
                }else
                    currentCol++;
            }

            for (int i = 0; i < _sums.Count; i++)
            {
                matrix[matrixHeight-1, i] = 1;
            }

            var resVector = new double[matrixHeight];
            for(int i = 0; i < matrixHeight; i++)
            {
                if (i == matrixHeight - 1)
                    resVector[i] = 1;
                else
                    resVector[i] = 0;
            }

            var s = new Simplex(vector, matrix, resVector);

            var answer = s.maximize();
            //temp
            //if (answer.Item1 == 0)
            //    return null;
            //temp
            var wsum = analyzeRes(answer);

            return wsum;
        }

        private WSum analyzeRes(Tuple<double, double[]> answer)
        {
            var resSums = new double[_sums.Count];
            for(int i = 0; i < _sums.Count; i++)
                resSums[i] = answer.Item2[i];

            var index = Array.FindIndex(resSums, x => x == 1);
            if (index == -1)
                index = 3;
            var result = _sums[index];

            return result;
        }

        public void playGame()
        {
            for (int iteration = 1; iteration <= _numberOfTurns; iteration++)
            {
                Console.WriteLine("Step number " + iteration);
                int index = 0;
                WSum optimal = null;
                bool error = false;
                bool loop = true;
                do
                {
                    Console.WriteLine("\t Generation " + ++index);
                    generateDataForStep();
                    countDataForSimplex();
                    logWSums();
                    optimal = doSimplex();
                    if (optimal != null)
                        error = moveGameState(optimal);
                    loop = optimal == null || !error;
                } while (loop);
            }
        }

        public bool moveGameState(WSum optimal)
        {
            var agentsListPair = optimal.sumList;
            var prob = _probs;

            for (int i = 0; i < agentsListPair.Count; i++)
            {
                var element = (from sublist in prob
                                       from item in sublist.agentsVector
                                       where item.Key.Id == agentsListPair[i].Key.Id
                                            && item.Value.action.id == agentsListPair[i].Value.Key.id
                                            && item.Value.stateBefore.id == agentsListPair[i].Value.Value.id
                                       select sublist).ToList();
                prob = null;
                prob = element;
            }

            double sum = prob.Sum(x => x.probTotal);
            double probability = getRandomNumber(0, sum);
            double cumulative = 0.0;
            Probs result = null;
            foreach (var strat in prob)
            {
                cumulative += strat.probTotal;
                if (probability < cumulative)
                {
                    result = strat;
                    break;
                }
            }
            if (result == null)
                return false;
            foreach (var coalition in _coalitions)
            {
                foreach (var agent in coalition.coalitionMembers)
                {
                    var agentActionStatePair = agentsListPair.Find(x => x.Key.Id == agent.Id).Value;
                    agent.totalReward += agentActionStatePair.Key.reward;
                    agent._currentState = result.agentsVector.Find(x => x.Key.Id == agent.Id).Value.stateAfter;
                }
            }

            return true;
        }

        #region Properties

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

        public int numberOfCoalitions
        {
            get
            {
                return _numberOfCoalitions;
            }
            set
            {
                _numberOfCoalitions = value;
            }
        }

        public int numberOfAgentsPerCoalition
        {
            get
            {
                return _numberOfAgentsPerCoalition;
            }
            set
            {
                _numberOfAgentsPerCoalition = value;
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

        #endregion

        #region Utils
        public double getRandomNumber(double min, double max)
        {
            //Random random = new Random();
            return r.NextDouble() * max;
        }

        #endregion

        #region Logs

        private void logInputData()
        {
            if (_logsEnabled)
            {
                Console.WriteLine("Input Data:");
                foreach (var c in _coalitions)
                {
                    Console.WriteLine("\t Coalition " + c.id + "; " + c.CoalitionName);
                    foreach (var a in c.coalitionMembers)
                    {
                        Console.WriteLine("\t \t Agent " + a.Id + "; Start state " + a._currentState);
                        Console.WriteLine("\t \t \t States:");
                        foreach (var st in a.statesList)
                        {
                            Console.WriteLine("\t \t \t \t " + st.id + "; " + st.state);
                        }

                        Console.WriteLine("\t \t \t Actions:");
                        foreach (var st in a.actionsList)
                        {
                            Console.WriteLine("\t \t \t \t " + st.id + "; " + st.action + "; reward = " + st.reward);
                        }

                        //Console.WriteLine("\t \t \t Probs:");
                        //foreach (var st in a.probList)
                        //{
                        //    Console.WriteLine("\t \t \t \t " + st.action + " -> " + st.nextState + "; prob = " + st.probability);
                        //}
                    }
                }
            }
        }

        //private void logAgentsActions()
        //{
        //    if (_logsEnabled)
        //    {
        //        foreach (Agent agent in _agents)
        //        {
        //            Console.WriteLine(agent.ToString() + " has actions: ");
        //            foreach (AgentAction action in agent.actionsList)
        //                Console.WriteLine("\t" + action.ToString());
        //        }
        //    }
        //}

        //private void logAgentsProbs()
        //{
        //    if (_logsEnabled)
        //    {
        //        foreach (Agent agent in _agents)
        //        {
        //            Console.WriteLine(agent.ToString() + " has probs: ");
        //            foreach (AgentProb prob in agent.probList)
        //                Console.WriteLine("\t" + prob.ToString());
        //        }
        //    }
        //}

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

        #endregion
    }
}


 //for (int i = 0; i<matrix.GetLength(0); i++)
 //           {
 //               string res = "";
 //               for (int j = 0; j<matrix.GetLength(1); j++)
 //               {
 //                   res += matrix[i, j] + ";";
 //               }
 //               Console.WriteLine(res);
 //           }