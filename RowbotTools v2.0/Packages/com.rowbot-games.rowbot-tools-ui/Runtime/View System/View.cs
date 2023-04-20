namespace RowbotTools.UI.ViewSystem
{
    using System.Collections;
    using UnityEngine;

    public abstract class View : MonoBehaviour
    {
        [SerializeField] private ViewAnimatorReferences m_animatorReferences = new ViewAnimatorReferences();

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
        }

        /// <summary>
        /// Clean up of the View.
        /// </summary>
        public virtual void CleanUp()
        {
        
        }

        /// <summary>
        /// Function called at the beginning of the View's opening sequence, before the transition animation has played.
        /// </summary>
        protected virtual void OpenStarted()
        {
            transform.SetAsLastSibling();
            gameObject.SetActive(true);

            m_animatorReferences.TransitionIn();

            StartCoroutine(WaitUntilTransitionInComplete());
        }

        /// <summary>
        /// Function called at the end of the View's opening sequence, after the transition animation has completed.
        /// </summary>
        protected virtual void OpenComplete()
        {

        }

        /// <summary>
        /// Function called at the beginning of the View's closing sequence, before the transition animation has played.
        /// </summary>
        protected virtual void CloseStarted()
        {
            m_animatorReferences.TransitionOut();

            StartCoroutine(WaitUntilTransitionOutComplete());
        }

        /// <summary>
        /// Function called at the end of the View's closing sequence, after the transition animation has completed.
        /// </summary>
        protected virtual void CloseComplete()
        {
            gameObject.SetActive(false);
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