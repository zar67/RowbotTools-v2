namespace RowbotTools.UI.ViewSystem
{
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    /// <summary>
    /// An abstract class for intializing and managing all the states and views in the project.
    /// </summary>
    public class ViewStatesManager : MonoBehaviour
    {
        [SerializeField] private Canvas m_mainCanvas;
        [SerializeField] private AssetLabelReference m_viewAssetsLabel;

        /// <summary>
        /// The main canvas for all the Views to be instantiated under.
        /// </summary>
        public Canvas MainCanvas => m_mainCanvas;

        /// <summary>
        /// The label all of the View assets have been assigned.
        /// </summary>
        public AssetLabelReference ViewAssetsLabel => m_viewAssetsLabel;
    }
}