namespace RowbotTools.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A Utilities class for Assemblies.
    /// </summary>
    public static class AssemblyUtilities
    {
        /// <summary>
        /// Gets all of the different derived types of type T.
        /// </summary>
        public static List<Type> GetAllOfType<T>()
        {
               return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(T)))
                .ToList(); 
        }

        /// <summary>
        /// Gets all of the different derived types of type T.
        /// </summary>
        public static List<string> GetAllNamesOfType<T>()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
             .SelectMany(assembly => assembly.GetTypes())
             .Where(type => type.IsSubclassOf(typeof(T)))
             .Select(type => type.Name)
             .ToList();
        }

        /// <summary>
        /// Gets all of the different derived types of type T in the given namespace.
        /// </summary>
        public static List<Type> GetAllOfTypeInNamespace<T>(string namespaceToCheck)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(T)))
                .Where(type => type.Namespace != null && type.Namespace.Contains(namespaceToCheck))
                .ToList();
        }

        /// <summary>
        /// Gets all of the different derived types of type T that aren't in the given namespace.
        /// </summary>
        public static List<Type> GetAllOfTypeExcludingNamespace<T>(string namespaceToCheck)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(T)))
                .Where(type => type.Namespace == null || !type.Namespace.Contains(namespaceToCheck))
                .ToList();
        }
    }
}