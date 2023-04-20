namespace RowbotTools.Core.ServiceSystem
{
    using RowbotTools.Core.Utilities;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// An abstract class for intializing and managing all the services in the project.
    /// </summary>
    public class ServicesManager : MonoBehaviour
    {
        private static List<Type> m_enabledServices = new List<Type>();

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

#if UNITY_EDITOR
        /// <summary>
        /// Editor only function for initializing the enabled services list.
        /// </summary>
        public static void InitializeEnabledServices(IEnumerable<Type> allServices)
        {
            m_enabledServices = new List<Type>();
            foreach (Type service in allServices)
            {
                m_enabledServices.Add(service);
            }
        }

        /// <summary>
        /// Editor only function for checking whether a service is enabled.
        /// </summary>
        public static bool IsServiceEnabled(Type service)
        {
            return m_enabledServices.Contains(service);
        }

        /// <summary>
        /// Editor only function for adding a service to the enabled services list.
        /// </summary>
        public static void EnableService(Type service)
        {
            if (!m_enabledServices.Contains(service))
            {
                m_enabledServices.Add(service);
            }
        }

        /// <summary>
        /// Editor only function for removing a service from the enabled services list.
        /// </summary>
        public static void DisableService(Type service)
        {
            if (m_enabledServices.Contains(service))
            {
                m_enabledServices.Remove(service);
            }
        }
#endif

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
        protected virtual void OnDestroy()
        {
            foreach (KeyValuePair<string, Service> serviceMap in m_services)
            {
                serviceMap.Value.Cleanup();
            }
        }
    }
}