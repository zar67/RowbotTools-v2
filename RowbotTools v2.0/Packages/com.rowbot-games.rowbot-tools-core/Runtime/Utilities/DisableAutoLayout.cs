namespace RowbotTools.Core.Utilities
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Component to auto disable any auto layout components after a given number of frames.
    /// </summary>
    public class DisableAutoLayout : MonoBehaviour
    {
        [SerializeField] private bool m_findInChildren = true;
        [SerializeField] private int m_refreshFrameCount = 5;

        private LayoutGroup[] m_layoutGroups;
        private ContentSizeFitter[] m_contentSizeFitters;
        private LayoutElement[] m_layoutElements;

        /// <summary>
        /// Forces a layout refresh of the cached layout components.
        /// If the layout components have changed since the last refresh any new components won't be refreshed, call FindAutoLayoutComponents instead 
        /// to re-find all the components and refresh them.
        /// </summary>
        public void ForceLayoutRefresh()
        {
            if (m_layoutElements == null|| m_contentSizeFitters == null || m_layoutGroups == null )
            {
                return;
            }

            foreach (var layoutElement in m_layoutElements)
            {
                if (layoutElement != null)
                {
                    layoutElement.enabled = true;
                }
            }

            foreach (var sizeFitter in m_contentSizeFitters)
            {
                if (sizeFitter != null)
                {
                    sizeFitter.enabled = true;
                }
            }

            foreach (var layoutGroup in m_layoutGroups)
            {
                if (layoutGroup != null)
                {
                    layoutGroup.enabled = true;
                }
            }

            StartCoroutine(DisableAfterFrameCount());
        }

        /// <summary>
        /// Fetches all the LayoutGroup components in the objects, and any children if selected, and refreshes them.
        /// Only call this function if the layout components have changed, if they are the same just call ForceLayoutRefresh instead.
        /// </summary>
        public void FindAutoLayoutComponents()
        {
            if (m_findInChildren)
            {
                m_layoutGroups = GetComponentsInChildren<LayoutGroup>();
                m_contentSizeFitters = GetComponentsInChildren<ContentSizeFitter>();
                m_layoutElements = GetComponentsInChildren<LayoutElement>();
            }
            else
            {
                m_layoutGroups = GetComponents<LayoutGroup>();
                m_contentSizeFitters = GetComponents<ContentSizeFitter>();
                m_layoutElements = GetComponents<LayoutElement>();
            }

            ForceLayoutRefresh();
        }

        private void Awake()
        {
            FindAutoLayoutComponents();
        }

        private IEnumerator DisableAfterFrameCount()
        {
            for (int i = 0; i < m_refreshFrameCount; i++)
            {
                yield return new WaitForEndOfFrame();
            }

            foreach (var layoutGroup in m_layoutGroups)
            {
                if (layoutGroup != null)
                {
                    layoutGroup.enabled = false;
                }
            }

            foreach (var sizeFitter in m_contentSizeFitters)
            {
                if (sizeFitter != null)
                {
                    sizeFitter.enabled = false;
                }
            }

            foreach (var layoutElement in m_layoutElements)
            {
                if (layoutElement != null)
                {
                    layoutElement.enabled = false;
                }
            }
        }
    }
}