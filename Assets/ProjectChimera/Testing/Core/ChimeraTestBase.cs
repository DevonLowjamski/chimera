using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using ProjectChimera.Core;

namespace ProjectChimera.Testing.Core
{
    /// <summary>
    /// Base class for all Project Chimera test classes
    /// Provides common testing utilities and setup/teardown functionality
    /// </summary>
    public abstract class ChimeraTestBase
    {
        protected GameObject _testGameObject;
        protected List<ChimeraManager> _testManagers = new List<ChimeraManager>();
        
        /// <summary>
        /// Sets up the basic test environment
        /// </summary>
        protected virtual void SetupTestEnvironment()
        {
            // Create a test GameObject to hold managers
            _testGameObject = new GameObject("TestEnvironment");
            _testManagers.Clear();
        }
        
        /// <summary>
        /// Creates and initializes a test manager of the specified type
        /// </summary>
        /// <typeparam name="T">Type of manager to create</typeparam>
        /// <returns>The created manager instance</returns>
        protected virtual T CreateTestManager<T>() where T : ChimeraManager
        {
            if (_testGameObject == null)
            {
                SetupTestEnvironment();
            }
            
            T manager = _testGameObject.AddComponent<T>();
            _testManagers.Add(manager);
            
            // Initialize the manager if it has an initialization method
            if (manager != null)
            {
                // Simulate manager initialization
                manager.enabled = true;
            }
            
            return manager;
        }
        
        /// <summary>
        /// Cleans up the test environment
        /// </summary>
        protected virtual void CleanupTestEnvironment()
        {
            // Clean up managers
            foreach (var manager in _testManagers)
            {
                if (manager != null)
                {
                    manager.enabled = false;
                }
            }
            _testManagers.Clear();
            
            // Destroy test GameObject
            if (_testGameObject != null)
            {
                Object.DestroyImmediate(_testGameObject);
                _testGameObject = null;
            }
        }
        
        /// <summary>
        /// Waits for a specified number of frames
        /// </summary>
        /// <param name="frameCount">Number of frames to wait</param>
        /// <returns>Coroutine for waiting</returns>
        protected IEnumerator WaitForFrames(int frameCount)
        {
            for (int i = 0; i < frameCount; i++)
            {
                yield return null;
            }
        }
        
        /// <summary>
        /// Asserts that a manager is properly initialized
        /// </summary>
        /// <param name="manager">Manager to check</param>
        protected void AssertManagerInitialized(ChimeraManager manager)
        {
            Assert.IsNotNull(manager, "Manager should not be null");
            Assert.IsTrue(manager.enabled, "Manager should be enabled");
        }
    }
}
