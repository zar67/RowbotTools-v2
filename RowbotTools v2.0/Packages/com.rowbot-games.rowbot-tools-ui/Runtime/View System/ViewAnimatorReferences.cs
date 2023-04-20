namespace RowbotTools.UI.ViewSystem
{
    using UnityEngine;

    /// <summary>
    /// A class containing the animator information for a view.
    /// </summary>
    [System.Serializable]
    public class ViewAnimatorReferences
    {
        [SerializeField] private Animator m_animator;
        [SerializeField] private string m_openIdleStateName;
        [SerializeField] private string m_closedIdleStateName;
        [SerializeField] private string m_transitionInTrigger;
        [SerializeField] private string m_transitionOutTrigger;

        public ViewAnimatorReferences()
        {
            m_animator = null;
            m_openIdleStateName = "Open_Idle";
            m_closedIdleStateName = "Closed_Idle";
            m_transitionInTrigger = "transition_in";
            m_transitionOutTrigger = "transition_out";
        }

        /// <summary>
        /// Sets the transiiton in trigger on the animator.
        /// </summary>
        public void TransitionIn()
        {
            m_animator.SetTrigger(m_transitionInTrigger);
        }

        /// <summary>
        /// Sets the transiiton out trigger on the animator.
        /// </summary>
        public void TransitionOut()
        {
            m_animator.SetTrigger(m_transitionOutTrigger);
        }

        /// <summary>
        /// Checks if the animator is in the open idle state.
        /// </summary>
        public bool IsInOpenIdleState()
        {
            return m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_openIdleStateName);
        }

        /// <summary>
        /// Checks if the animator is in the closed idle state.
        /// </summary>
        public bool IsInClosedIdleState()
        {
            return m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_closedIdleStateName);
        }
    }
}