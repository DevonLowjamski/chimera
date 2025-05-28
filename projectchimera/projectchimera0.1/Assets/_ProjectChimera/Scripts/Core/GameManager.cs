using UnityEngine;
using ProjectChimera.Data;

namespace ProjectChimera.Core
{
    /// <summary>
    /// Central game manager that coordinates all game systems
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("Game State")]
        [SerializeField] private GameState currentGameState = GameState.MainMenu;
        
        [Header("Data Management")]
        [SerializeField] private DataManager dataManager;
        
        // Singleton instance
        public static GameManager Instance { get; private set; }
        
        // Events
        public System.Action<GameState> OnGameStateChanged;
        
        // Properties
        public GameState CurrentGameState => currentGameState;
        public DataManager DataManager => dataManager;
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeGame();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void InitializeGame()
        {
            // Initialize data manager
            if (dataManager == null)
                dataManager = FindFirstObjectByType<DataManager>();
            
            if (dataManager == null)
            {
                GameObject dataManagerGO = new GameObject("DataManager");
                dataManager = dataManagerGO.AddComponent<DataManager>();
                dataManagerGO.transform.SetParent(transform);
            }
            
            dataManager.Initialize();
        }
        
        public void ChangeGameState(GameState newState)
        {
            if (currentGameState == newState) return;
            
            GameState previousState = currentGameState;
            currentGameState = newState;            
            Debug.Log($"Game state changed from {previousState} to {newState}");
            OnGameStateChanged?.Invoke(newState);
        }
    }
    
    [System.Serializable]
    public enum GameState
    {
        MainMenu,
        Loading,
        Gameplay,
        Paused,
        Settings,
        Genetics,
        Market
    }
}