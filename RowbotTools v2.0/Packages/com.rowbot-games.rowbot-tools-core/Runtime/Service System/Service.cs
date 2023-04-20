namespace RowbotTools.Core.ServiceSystem
{
    /// <summary>
    /// A base Service class to inherit from.
    /// </summary>
    public abstract class Service
    {
#if UNITY_EDITOR
        public string Name;
#endif
        /// <summary>
        /// The base initialization of the service, ran in the Start of the ServicesManager.
        /// </summary>
        public virtual void Init()
        {
        
        }

        /// <summary>
        /// The base clean up of the service, ran in the OnDestroy of the ServicesManager.
        /// </summary>
        public virtual void Cleanup()
        {
        
        }
    }
}