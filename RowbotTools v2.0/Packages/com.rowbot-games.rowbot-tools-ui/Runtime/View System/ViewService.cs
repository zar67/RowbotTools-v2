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
        private ViewStatesManager m_viewsManager = null;
        private Dictionary<string, View> m_allViews = new Dictionary<string, View>();

        /// <summary>
        /// Instantiation of the all View addressables.
        /// </summary>
        public override void LateInit()
        {
            base.LateInit();

            m_viewsManager = Object.FindObjectOfType<ViewStatesManager>();
            if (m_viewsManager == null)
            {
                Debug.LogError("Could not find ViewsManager in ViewService init, make sure there is a ViewsManager in the boot scene.");
                return;
            }

            // Instantiate Views
            ServicesManager.GetService<AddressablesService>().LoadAssets<GameObject>(m_viewsManager.ViewAssetsLabel, (views) => 
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
                    newObject.SetActive(false);
                }
            });
        }

        /// <summary>
        /// Destruction of all the View objects.
        /// </summary>
        public override void Cleanup()
        {
            base.Cleanup();

            // Destroy Views
        }
    }
}