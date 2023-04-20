namespace RowbotTools.Core.ServiceSystem
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.AddressableAssets.ResourceLocators;
    using UnityEngine.ResourceManagement.AsyncOperations;

    /// <summary>
    /// A service for managing the loading and unloading of addressables.
    /// </summary>
    public class AddressablesService : Service
    {
        private bool m_isInitialized = false;

        /// <summary>
        /// Initialization of the addressables.
        /// </summary>
        public override void Init()
        {
            base.Init();

            m_isInitialized = false;

            Addressables.InitializeAsync().Completed += OnAddressablesInitialized;
        }

        /// <summary>
        /// Cleanup of the addressables.
        /// </summary>
        public override void Cleanup()
        {
            base.Cleanup();
        }

        /// <summary>
        /// Loads an asset of a given reference. Calls the loadedCallback if asset is successfully loaded.
        /// </summary>
        public void LoadAsset<T>(AssetReference assetReference, Action<T> loadedCallback)
        {
            if (!m_isInitialized)
            {
                Debug.LogWarning("Addressables aren't properly initialized, please make sure addressables are initialized before loading assets.");
                return;
            }

            assetReference.LoadAssetAsync<T>().Completed += (asyncOperationHandle) =>
            {
                if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    loadedCallback?.Invoke(asyncOperationHandle.Result);
                }
                else
                {
                    Debug.LogError($"Failed to load adressable {assetReference.Asset.name}");
                }
            };
        }

        /// <summary>
        /// Loads all assets with a given label. Calls the loadedCallback if assets are successfully loaded.
        /// </summary>
        public void LoadAssets<T>(AssetLabelReference assetsLabel, Action<IEnumerable<T>> loadedCallback)
        {
            Addressables.LoadAssetsAsync<T>(assetsLabel, null).Completed += (asyncOperationHandle) =>
            {
                if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    loadedCallback?.Invoke(asyncOperationHandle.Result);
                }
                else
                {
                    Debug.LogError($"Failed to load adressables with label {assetsLabel}");
                }
            };
        }

        private void OnAddressablesInitialized(AsyncOperationHandle<IResourceLocator> asyncOperationHandle)
        {
            if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                m_isInitialized = true;
            }
            else
            {
                Debug.LogError("Failed to initialize addressables");
            }
        }
    }
}