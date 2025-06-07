using UnityEngine;

namespace ProjectChimera.Core
{
    /// <summary>
    /// Base class for all Project Chimera manager components.
    /// Managers coordinate major game systems and maintain singleton-like behavior.
    /// </summary>
    public abstract class ChimeraManager : ChimeraMonoBehaviour
    {
        [Header("Manager Properties")]
        [SerializeField] private bool _initializeOnAwake = true;
        [SerializeField] private bool _persistAcrossScenes = false;

        /// <summary>
        /// Whether this manager is currently initialized and running.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Whether this manager should persist across scene loads.
        /// </summary>
        public bool PersistAcrossScenes => _persistAcrossScenes;

        protected override void Awake()
        {
            base.Awake();

            // Handle persistence across scenes
            if (_persistAcrossScenes)
            {
                DontDestroyOnLoad(gameObject);
            }

            // Initialize immediately if configured to do so
            if (_initializeOnAwake)
            {
                InitializeManager();
            }
        }

        /// <summary>
        /// Initializes the manager. Can be called manually or automatically on Awake.
        /// </summary>
        public virtual void InitializeManager()
        {
            if (IsInitialized)
            {
                LogWarning("Manager is already initialized");
                return;
            }

            LogDebug("Initializing manager");
            
            OnManagerInitialize();
            IsInitialized = true;
            
            LogDebug("Manager initialization complete");
        }

        /// <summary>
        /// Shuts down the manager and cleans up resources.
        /// </summary>
        public virtual void ShutdownManager()
        {
            if (!IsInitialized)
            {
                LogWarning("Manager is not initialized");
                return;
            }

            LogDebug("Shutting down manager");
            
            OnManagerShutdown();
            IsInitialized = false;
            
            LogDebug("Manager shutdown complete");
        }

        /// <summary>
        /// Override this method to implement manager-specific initialization logic.
        /// </summary>
        protected abstract void OnManagerInitialize();

        /// <summary>
        /// Override this method to implement manager-specific shutdown logic.
        /// </summary>
        protected abstract void OnManagerShutdown();

        protected override void OnDestroy()
        {
            if (IsInitialized)
            {
                ShutdownManager();
            }
            base.OnDestroy();
        }

        /// <summary>
        /// Called every frame if the manager needs to perform updates.
        /// Override in derived classes that need frame-by-frame updates.
        /// </summary>
        protected virtual void Update()
        {
            if (!IsInitialized) return;

            OnManagerUpdate();
        }

        /// <summary>
        /// Override this method to implement manager-specific update logic.
        /// Only called when the manager is initialized.
        /// </summary>
        protected virtual void OnManagerUpdate()
        {
            // Base implementation - override in derived classes
        }

        /// <summary>
        /// Called at fixed intervals for physics-related updates.
        /// Override in derived classes that need fixed timestep updates.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            if (!IsInitialized) return;

            OnManagerFixedUpdate();
        }

        /// <summary>
        /// Override this method to implement manager-specific fixed update logic.
        /// Only called when the manager is initialized.
        /// </summary>
        protected virtual void OnManagerFixedUpdate()
        {
            // Base implementation - override in derived classes
        }
    }
}