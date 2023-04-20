namespace RowbotTools.UI.ViewSystem
{
    using RowbotTools.Core.ServiceSystem;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// A Service for initializing and managing Views.
    /// </summary>
    public class ViewService : Service
    {
        private AddressablesService m_addressablesService = null;
        private ViewSystemManager m_viewsManager = null;
        private Dictionary<string, View> m_allViews = new Dictionary<string, View>();

        /// <summary>
        /// Instantiation of the all View addressables.
        /// </summary>
        public override void LateInit()
        {
            base.LateInit();

            m_viewsManager = Object.FindObjectOfType<ViewSystemManager>();
            if (m_viewsManager == null)
            {
                Debug.LogError("Could not find ViewsManager in ViewService init, make sure there is a ViewsManager in the boot scene.");
                return;
            }

            m_addressablesService = ServiceSystemManager.Get<AddressablesService>();

            // Instantiate Views
            m_addressablesService.LoadAssets<GameObject>(m_viewsManager.ViewAssetsLabel, (views) => 
            {
                foreach (GameObject view in views)
                {
                    GameObject newObject = Object.Instantiate(view, m_viewsManager.MainCanvas.transform);
                    newObject.name = newObject.name.Replace("(Clone)", "");

                    View newView = newObject.GetComponent<View>();
                    if (newView == null)
                    {
                        Debug.LogError($"{newObject.name} does not have a View component, cannot be a View addressable.");
                        Object.Destroy(newObject);
                        continue;
                    }

                    m_allViews.Add(newView.GetType().Name, newView);

                    newView.Init();
                }
            });
        }

        public override void Cleanup()
        {
            base.Cleanup();

            foreach (KeyValuePair<string, View> viewMap in m_allViews)
            {
                viewMap.Value.CleanUp();
            }
        }

        /// <summary>
        /// Gest the instantiated View of the given type.
        /// </summary>
        public View Get<T>() where T : View
        {
            string viewID = typeof(T).Name;
            if (m_allViews.ContainsKey(viewID))
            {
                return (T)m_allViews[viewID];
            }

            Debug.LogError($"Could not find {viewID} in the views list, are you sure you created the view?");
            return null;
        }

        /// <summary>
        /// Opens a view.
        /// </summary>
        public void Open<T>() where T : View
        {
            View view = Get<T>();
            if (view == null)
            {
                Debug.LogError($"Failed to open {typeof(T).Name}");
                return;
            }

            if (!view.IsOpenOrOpening)
            {
                view.Open();
            }
        }

        /// <summary>
        /// Closes a view.
        /// </summary>
        public void Close<T>() where T : View
        {
            View view = Get<T>();
            if (view == null)
            {
                Debug.LogError($"Failed to close {typeof(T).Name}");
                return;
            }

            if (!view.IsClosedOrClosing)
            {
                view.Close();
            }
        }
    }
}