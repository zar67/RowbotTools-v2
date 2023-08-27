namespace RowbotTools.UI.ViewSystem
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// An abstract class for handling a single view in the UI.
    /// </summary>
    public abstract class View : MonoBehaviour
    {
        /// <summary>
        /// Enum to determine the current state of a view.
        /// </summary>
        public enum ViewState
        {
            Uninitialized,
            Closed,
            Opening,
            Open,
            Closing
        }

        [SerializeField] private ViewAnimatorReferences m_animatorReferences = new ViewAnimatorReferences();

        private ViewState m_viewState = ViewState.Uninitialized;

        /// <summary>
        /// Gets whether the view is currently open or is opening.
        /// </summary>
        public bool IsOpenOrOpening => m_viewState == ViewState.Opening || m_viewState == ViewState.Open;

        /// <summary>
        /// Gets whether the view is currently closed or is closing.
        /// </summary>
        public bool IsClosedOrClosing => m_viewState == ViewState.Closing || m_viewState == ViewState.Closed;

        /// <summary>
        /// Set the View object active and handle the opening of the View.
        /// </summary>
        public void Open()
        {
            OpenStarted();
        }

        /// <summary>
        /// Set the View object inactive and handle the closing of the View.
        /// </summary>
        public void Close()
        {
            CloseStarted();
        }

        /// <summary>
        /// Initiialization of the View.
        /// </summary>
        public virtual void Init()
        {
            gameObject.SetActive(false);
            m_viewState = ViewState.Closed;
        }

        /// <summary>
        /// Clean up of the View.
        /// </summary>
        public virtual void CleanUp()
        {
            m_viewState = ViewState.Uninitialized;
        }

        /// <summary>
        /// The beginning of the View's opening sequence, before the transition animation has played.
        /// </summary>
        protected virtual void OpenStarted()
        {
            transform.SetAsLastSibling();
            gameObject.SetActive(true);

            m_animatorReferences.TransitionIn();
            m_viewState = ViewState.Opening;

            StartCoroutine(WaitUntilTransitionInComplete());
        }

        /// <summary>
        /// The end of the View's opening sequence, after the transition animation has completed.
        /// </summary>
        protected virtual void OpenComplete()
        {
            m_viewState = ViewState.Open;
        }

        /// <summary>
        /// The beginning of the View's closing sequence, before the transition animation has played.
        /// </summary>
        protected virtual void CloseStarted()
        {
            m_animatorReferences.TransitionOut();
            m_viewState = ViewState.Closing;

            StartCoroutine(WaitUntilTransitionOutComplete());
        }

        /// <summary>
        /// The end of the View's closing sequence, after the transition animation has completed.
        /// </summary>
        protected virtual void CloseComplete()
        {
            gameObject.SetActive(false);
            m_viewState = ViewState.Closed;
        }

        private IEnumerator WaitUntilTransitionInComplete()
        {
            yield return new WaitUntil(() => m_animatorReferences.IsInOpenIdleState());

            OpenComplete();
        }

        private IEnumerator WaitUntilTransitionOutComplete()
        {
            yield return new WaitUntil(() => m_animatorReferences.IsInClosedIdleState());

            CloseComplete();
        }
    }
}