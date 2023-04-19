namespace RowbotTools.Core.ServiceSystem
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// An abstract class for intializing ad managing all the services in the project.
    /// </summary>
    public abstract class ServicesManager : MonoBehaviour
    {
        protected static Dictionary<string, Service> m_services = new Dictionary<string, Service>();

        /// <summary>
        /// Converts the generic service type into a string identifier.
        /// </summary>
        protected static string GetServiceID<T>() where T : Service
        {
            return typeof(T).ToString();
        }

        /// <summary>
        /// Gets the Service of the given type.
        /// </summary>
        public static T GetService<T>() where T : Service
        {
            string serviceID = GetServiceID<T>();
            if (m_services.ContainsKey(serviceID))
            {
                return (T)m_services[serviceID];
            }

            Debug.LogError($"Could not find {serviceID} in the services list, are you sure you created the service?");
            return null;
        }

        /// <summary>
        /// Creates a new instance of the service and adds it to the list of services, if it doesn't already exist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool CreateService<T>(out T newService) where T : Service, new()
        {
            string serviceID = GetServiceID<T>();
            if (m_services.ContainsKey(serviceID))
            {
                Debug.LogWarning($"{serviceID} already exists in the services list, you cannot add multiple services of the same type.");
                newService = null;
                return false;
            }

            newService = new T();
            m_services.Add(serviceID, newService);

            return true;
        }

        /// <summary>
        /// The base Awake calls CreateService for the essential services of the Robot Tools Core packcage.
        /// Ovveride this function to add more services, but still call the base!
        /// </summary>
        protected virtual void Awake()
        {
        
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