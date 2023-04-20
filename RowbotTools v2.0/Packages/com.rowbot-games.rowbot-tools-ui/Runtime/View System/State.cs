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
            m_viewService = ServiceSystemManager.Get<ViewService>();
            m_stateService = ServiceSystemManager.Get<StateService>();
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
            OpenViews();
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
            CloseViews();
        }

        /// <summary>
        /// Opens all the views that should open when this state does.
        /// </summary>
        protected virtual void OpenViews()
        {
        
        }

        /// <summary>
        /// Closes all the views that should close when this state does.
        /// </summary>
        protected virtual void CloseViews()
        {
        
        }
    }
}