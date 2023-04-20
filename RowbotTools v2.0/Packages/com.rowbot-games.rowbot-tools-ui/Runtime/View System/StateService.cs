namespace RowbotTools.UI.ViewSystem
{
    using RowbotTools.Core.ServiceSystem;
    using RowbotTools.Core.Utilities;
    using System;
    using System.Collections.Generic;

    public class StateService : Service
    {
        private Dictionary<string, State> m_allStates = new Dictionary<string, State>();
        private Stack<State> m_stateStack = new Stack<State>();

        private string m_currentState;

        /// <summary>
        /// Creates all states and initializes them.
        /// </summary>
        public override void Init()
        {
            base.Init();

            // Create All State Instances
            foreach (Type state in AssemblyUtilities.GetAllOfType<State>())
            {
                State newState = Activator.CreateInstance(state) as State;

                newState.Init();
                m_allStates.Add(state.Name, newState);
            }
        }

        /// <summary>
        /// Cleans up all the states.
        /// </summary>
        public override void Cleanup()
        {
            base.Cleanup();

            foreach (KeyValuePair<string, State> stateMap in m_allStates)
            {
                stateMap.Value.CleanUp();
            }
        }

        /// <summary>
        /// Updates the currently active state.
        /// </summary>
        public void UpdateCurrentState()
        {
            m_allStates[m_currentState].Update();
        }
    }
}