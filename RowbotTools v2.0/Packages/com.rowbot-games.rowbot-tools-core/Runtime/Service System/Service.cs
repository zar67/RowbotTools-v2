namespace RowbotTools.Core.ServiceSystem
{
    /// <summary>
    /// A base Service class to inherit from.
    /// </summary>
    public abstract class Service
    {
        /// <summary>
        /// The base initialization of the service, ran in the Start of the ServicesManager.
        /// </summary>
        public virtual void Init()
        {
        
        }

        /// <summary>
        /// The base late initialization, ran in Start of the ServicesManager after all Services have been initialized.
        /// </summary>
        public virtual void LateInit()
        {
        
        }

        /// <summary>
        /// The base Update, called every frame by the ServicesManager.
        /// </summary>
        public virtual void Update()
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