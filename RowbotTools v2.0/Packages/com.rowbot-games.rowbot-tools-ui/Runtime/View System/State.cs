namespace RowbotTools.UI.ViewSystem
{
    using RowbotTools.Core.ServiceSystem;

    /// <summary>
    /// An abstract class for handling a UI state.
    /// </summary>
    public abstract class State
    {
        protected ViewService m_viewService = null;
        protected StateService m_stateService = null;

        /// <summary>
        /// Initialization of the state.
        /// </summary>
        public virtual void Init()
        {
            m_viewService = ServicesManager.Get<ViewService>();
            m_stateService = ServicesManager.Get<StateService>();
        }

        /// <summary>
        /// Clean up of the state.
        /// </summary>
        public virtual void CleanUp()
        {
        
        }

        /// <summary>
        /// The entrance point of the state.
        /// </summary>
        public virtual void Enter()
        {

        }

        /// <summary>
        /// Update function of the state.
        /// </summary>
        public virtual void Update()
        {
        
        }

        /// <summary>
        /// The exit point of the state.
        /// </summary>
        public virtual void Exit()
        {

        }
    }
}