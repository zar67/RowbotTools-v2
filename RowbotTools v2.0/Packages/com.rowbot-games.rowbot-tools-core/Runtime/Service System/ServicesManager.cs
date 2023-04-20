namespace RowbotTools.Core.ServiceSystem
{
    using RowbotTools.Core.Utilities;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// An abstract class for intializing and managing all the services in the project.
    /// </summary>
    
    [RequireComponent(typeof(DontDestroyOnLoad))]
    public class ServicesManager : MonoBehaviour
    {
        protected static Dictionary<string, Service> m_services = new Dictionary<string, Service>();

        /// <summary>
        /// Gets the Service of the given type.
        /// </summary>
        public static T GetService<T>() where T : Service
        {
            string serviceID = typeof(T).Name;
            if (m_services.ContainsKey(serviceID))
            {
                return (T)m_services[serviceID];
            }

            Debug.LogError($"Could not find {serviceID} in the services list, are you sure you created the service?");
            return null;
        }

        private void Awake()
        {
            foreach (Type service in AssemblyUtilities.GetAllOfType<Service>())
            {
                m_services.Add(service.Name, Activator.CreateInstance(service) as Service);
            }
        }

        /// <summary>
        /// The base Start initializes all the services.
        /// </summary>
        protected virtual void Start()
        {
            foreach (KeyValuePair<string, Service> serviceMap in m_services)
            {
                serviceMap.Value.Init();
            }

            foreach (KeyValuePair<string, Service> serviceMap in m_services)
            {
                serviceMap.Value.LateInit();
            }
        }

        /// <summary>
        /// The base OnDestroy cleans up all the services.
        /// </summary>
        protected virtual void OnDisable()
        {
            foreach (KeyValuePair<string, Service> serviceMap in m_services)
            {
                serviceMap.Value.Cleanup();
            }
        }
    }
}