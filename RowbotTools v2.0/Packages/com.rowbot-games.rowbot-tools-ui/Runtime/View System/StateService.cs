namespace RowbotTools.UI.ViewSystem
{
    using RowbotTools.Core.ServiceSystem;
    using RowbotTools.Core.Utilities;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class StateService : Service
    {
        private Dictionary<string, State> m_allStates = new Dictionary<string, State>();
        private Stack<State> m_stateStack = new Stack<State>();

        private string m_currentStateString = null;

        private State m_currentState => m_allStates[m_currentStateString];

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
        /// Initial transition of state from State type string.
        /// </summary>
        public void SetStartingState(string state)
        {
            if (!m_allStates.ContainsKey(state))
            {
                Debug.LogError($"Starting state {state} is not a valid state type.");
                return;
            }
            
            m_allStates[state].Enter();
            m_currentStateString = state;
        }

        /// <summary>
        /// Exit the current state and Enter the next state.
        /// </summary>
        public void ChangeState<T>() where T : State
        {
            if (!m_allStates.ContainsKey(typeof(T).Name))
            {
                Debug.LogError($"Cannot change state to {typeof(T).Name} as it does not exist in the States list.");
                return;
            }

            if (m_currentStateString != null)
            {
                m_currentState.Exit();
            }

            m_currentStateString = typeof(T).Name;
            m_currentState.Enter();
        }

        /// <summary>
        /// Updates the currently active state.
        /// </summary>
        public void UpdateCurrentState()
        {
            if (m_currentStateString == null)
            {
                return;
            }

            m_allStates[m_currentStateString].Update();
        }
    }
}