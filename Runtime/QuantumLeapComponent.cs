using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuantumLeap
{
    /// <summary>
    /// MonoBehaviour component for easy integration of Quantum Leap plugin
    /// Can be attached to GameObjects in Unity scenes
    /// </summary>
    public class QuantumLeapComponent : MonoBehaviour
    {
        [Header("Quantum Leap Settings")]
        [SerializeField] private bool _autoInitialize = true;
        [SerializeField] private bool _logToConsole = true;
        [SerializeField] private QuantumLeapLogger.LogLevel _logLevel = QuantumLeapLogger.LogLevel.Info;

        [Header("API Configuration")]
        [SerializeField] private string _defaultApiUrl = "https://jsonplaceholder.typicode.com/posts/1";
        [SerializeField] private float _requestTimeout = 30f;

        [Header("Events")]
        [SerializeField] private bool _enableEvents = true;

        /// <summary>
        /// Event fired when the component is initialized
        /// </summary>
        public event Action OnComponentInitialized;

        /// <summary>
        /// Event fired when API data is received
        /// </summary>
        public event Action<string> OnDataReceived;

        /// <summary>
        /// Event fired when an error occurs
        /// </summary>
        public event Action<string> OnErrorOccurred;

        private bool _isInitialized = false;

        /// <summary>
        /// Gets whether the component is initialized
        /// </summary>
        public bool IsInitialized => _isInitialized;

        private void Awake()
        {
            if (_autoInitialize)
            {
                Initialize();
            }
        }

        private void Start()
        {
            if (!_autoInitialize)
            {
                Initialize();
            }
        }

        private void OnDestroy()
        {
            Cleanup();
        }

        /// <summary>
        /// Initializes the Quantum Leap component
        /// </summary>
        public void Initialize()
        {
            if (_isInitialized) return;

            try
            {
                // Configure logger
                QuantumLeapLogger.CurrentLogLevel = _logLevel;
                QuantumLeapLogger.IncludeTimestamps = true;
                QuantumLeapLogger.IncludeLogLevel = true;

                // Subscribe to events if enabled
                if (_enableEvents)
                {
                    QuantumLeapManager.OnInitialized += OnManagerInitialized;
                    QuantumLeapManager.OnApiResponseReceived += OnApiResponseReceived;
                    QuantumLeapManager.OnError += OnManagerError;
                }

                // Initialize the manager with custom timeout
                QuantumLeapManager.Initialize(_requestTimeout);

                _isInitialized = true;
                OnComponentInitialized?.Invoke();

                if (_logToConsole)
                {
                    QuantumLeapLogger.Log($"QuantumLeapComponent initialized on {gameObject.name} with timeout: {_requestTimeout}s");
                }
            }
            catch (Exception ex)
            {
                QuantumLeapLogger.LogError($"Failed to initialize QuantumLeapComponent: {ex.Message}");
                OnErrorOccurred?.Invoke($"Initialization failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Fetches data from the default API URL
        /// </summary>
        /// <returns>Coroutine for the fetch operation</returns>
        public Coroutine FetchDefaultData()
        {
            return StartCoroutine(FetchDataCoroutine(_defaultApiUrl));
        }

        /// <summary>
        /// Fetches data from a specified URL
        /// </summary>
        /// <param name="url">The URL to fetch data from</param>
        /// <returns>Coroutine for the fetch operation</returns>
        public Coroutine FetchData(string url)
        {
            return StartCoroutine(FetchDataCoroutine(url));
        }

        /// <summary>
        /// Posts data to a specified URL
        /// </summary>
        /// <param name="url">The URL to post data to</param>
        /// <param name="data">The data to post (JSON string)</param>
        /// <returns>Coroutine for the post operation</returns>
        public Coroutine PostData(string url, string data)
        {
            return StartCoroutine(PostDataCoroutine(url, data));
        }

        private IEnumerator FetchDataCoroutine(string url)
        {
            if (!_isInitialized)
            {
                QuantumLeapLogger.LogError("Component not initialized. Call Initialize() first.");
                yield break;
            }

            var task = QuantumLeapManager.FetchDataAsync(url);
            
            while (!task.IsCompleted)
            {
                yield return null;
            }

            if (task.Exception != null)
            {
                var errorMessage = $"Failed to fetch data from {url}: {task.Exception.Message}";
                QuantumLeapLogger.LogError(errorMessage);
                OnErrorOccurred?.Invoke(errorMessage);
                yield break;
            }

            try
            {
                var result = task.Result;
                OnDataReceived?.Invoke(result);

                if (_logToConsole)
                {
                    QuantumLeapLogger.Log($"Data fetched successfully from {url}", result);
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to fetch data from {url}: {ex.Message}";
                QuantumLeapLogger.LogError(errorMessage);
                OnErrorOccurred?.Invoke(errorMessage);
            }
        }

        private IEnumerator PostDataCoroutine(string url, string data)
        {
            if (!_isInitialized)
            {
                QuantumLeapLogger.LogError("Component not initialized. Call Initialize() first.");
                yield break;
            }

            var task = QuantumLeapManager.PostDataAsync(url, data);
            
            while (!task.IsCompleted)
            {
                yield return null;
            }

            if (task.Exception != null)
            {
                var errorMessage = $"Failed to post data to {url}: {task.Exception.Message}";
                QuantumLeapLogger.LogError(errorMessage);
                OnErrorOccurred?.Invoke(errorMessage);
                yield break;
            }

            try
            {
                var result = task.Result;
                OnDataReceived?.Invoke(result);

                if (_logToConsole)
                {
                    QuantumLeapLogger.Log($"Data posted successfully to {url}", result);
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to post data to {url}: {ex.Message}";
                QuantumLeapLogger.LogError(errorMessage);
                OnErrorOccurred?.Invoke(errorMessage);
            }
        }

        private void OnManagerInitialized()
        {
            if (_logToConsole)
            {
                QuantumLeapLogger.Log("QuantumLeapManager initialized successfully");
            }
        }

        private void OnApiResponseReceived(string url, object data)
        {
            if (_logToConsole)
            {
                QuantumLeapLogger.Log($"API response received from {url}", data);
            }
        }

        private void OnManagerError(string error)
        {
            if (_logToConsole)
            {
                QuantumLeapLogger.LogError($"Manager error: {error}");
            }
            OnErrorOccurred?.Invoke(error);
        }

        private void Cleanup()
        {
            if (_enableEvents)
            {
                QuantumLeapManager.OnInitialized -= OnManagerInitialized;
                QuantumLeapManager.OnApiResponseReceived -= OnApiResponseReceived;
                QuantumLeapManager.OnError -= OnManagerError;
            }

            _isInitialized = false;
        }

        /// <summary>
        /// Gets the default API URL
        /// </summary>
        public string DefaultApiUrl => _defaultApiUrl;

        /// <summary>
        /// Gets the current request timeout in seconds
        /// </summary>
        public float RequestTimeout => _requestTimeout;

        /// <summary>
        /// Sets the default API URL
        /// </summary>
        /// <param name="url">The new default URL</param>
        public void SetDefaultApiUrl(string url)
        {
            _defaultApiUrl = url;
        }

        /// <summary>
        /// Sets the request timeout (only affects new requests, not already initialized manager)
        /// </summary>
        /// <param name="timeoutSeconds">Timeout in seconds</param>
        public void SetRequestTimeout(float timeoutSeconds)
        {
            if (timeoutSeconds <= 0)
            {
                QuantumLeapLogger.LogWarning("Timeout must be greater than 0 seconds");
                return;
            }

            _requestTimeout = timeoutSeconds;
            QuantumLeapLogger.LogDebug($"Request timeout updated to: {timeoutSeconds} seconds");
        }
    }
} 