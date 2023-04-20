namespace RowbotTools.UI.ViewSystem
{
    using RowbotTools.Core.ServiceSystem;
    using RowbotTools.Core.Utilities;
    using System;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    /// <summary>
    /// An abstract class for intializing and managing all states and views in the project.
    /// </summary>
    [RequireComponent(typeof(DontDestroyOnLoad))]
    public class ViewSystemManager : MonoBehaviour
    {
        [SerializeField] private string m_startingStateName;
        [SerializeField] private Canvas m_mainCanvas;
        [SerializeField] private AssetLabelReference m_viewAssetsLabel;

        private StateService m_stateService = null;

        /// <summary>
        /// The name of the starting state type.
        /// </summary>
        public string StartingStateName => m_startingStateName;

        /// <summary>
        /// The main canvas for all the Views to be instantiated under.
        /// </summary>
        public Canvas MainCanvas => m_mainCanvas;

        /// <summary>
        /// The label all of the View addressable assets have been assigned.
        /// </summary>
        public AssetLabelReference ViewAssetsLabel => m_viewAssetsLabel;

        private void Start()
        {
            m_stateService = ServiceSystemManager.Get<StateService>();
        }

        private void Update()
        {
            m_stateService.UpdateCurrentState();
        }
    }
}