namespace RowbotTools.Core.ServiceSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(ServicesManager))]
    public class ServicesManagerEditor : Editor
    {
        private List<Type> m_allServices = new List<Type>();
        private List<Type> m_allRowbotToolsServices = new List<Type>();
        private List<Type> m_allCustomServices = new List<Type>();

        public ServicesManagerEditor()
        {
            m_allServices = GetAllServices();
            m_allRowbotToolsServices = GetAllRowbotToolsServices();
            m_allCustomServices = GetAllCustomServices();

            ServicesManager.InitializeEnabledServices(m_allServices);
        }

        private void OnEnable()
        {
            List<Type> refreshedServices = GetAllServices();

            foreach (Type service in m_allServices)
            {
                if (!refreshedServices.Contains(service))
                {
                    ServicesManager.DisableService(service);
                }
            }

            m_allRowbotToolsServices = GetAllRowbotToolsServices();
            m_allCustomServices = GetAllCustomServices();
            m_allServices = refreshedServices;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
            EditorGUILayout.LabelField("Default Services", EditorStyles.boldLabel);

            bool guiPreviouslyEnabled = GUI.enabled;
            GUI.enabled = false;

            foreach (var service in m_allRowbotToolsServices)
            {
                EditorGUILayout.Toggle(service.Name, true);
            }

            GUI.enabled = guiPreviouslyEnabled;

            EditorGUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
            EditorGUILayout.LabelField("Custom Services", EditorStyles.boldLabel);

            foreach (Type service in m_allCustomServices)
            {
                EditorGUI.BeginChangeCheck();
                bool enabled = EditorGUILayout.Toggle(service.Name, ServicesManager.IsServiceEnabled(service));
                if (EditorGUI.EndChangeCheck())
                {
                    if (enabled)
                    {
                        ServicesManager.EnableService(service);
                    }
                    else
                    {
                        ServicesManager.DisableService(service);
                    }
                }
            }
        }

        private List<Type> GetAllServices()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(Service)))
                .ToList();
        }

        private List<Type> GetAllRowbotToolsServices()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(Service)))
                .Where(type => type.Namespace != null && type.Namespace.Contains(nameof(RowbotTools)))
                .ToList();
        }

        private List<Type> GetAllCustomServices()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(Service)))
                .Where(type => type.Namespace == null || !type.Namespace.Contains(nameof(RowbotTools)))
                .ToList();
        }
    }
}