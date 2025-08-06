using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace QuantumLeap
{
    /// <summary>
    /// Main manager class for the Quantum Leap plugin
    /// Handles API fetching, initialization, and core functionality
    /// </summary>
    public static class QuantumLeapManager
    {
        private static bool _isInitialized = false;
        private static HttpClient _httpClient;
        private static readonly object _lockObject = new object();

        /// <summary>
        /// Event fired when the plugin is initialized
        /// </summary>
        public static event Action OnInitialized;

        /// <summary>
        /// Event fired when an API request is completed
        /// </summary>
        public static event Action<string, object> OnApiResponseReceived;

        /// <summary>
        /// Event fired when an error occurs
        /// </summary>
        public static event Action<string> OnError;

        /// <summary>
        /// Gets whether the plugin is initialized
        /// </summary>
        public static bool IsInitialized => _isInitialized;

        /// <summary>
        /// Initializes the Quantum Leap plugin with default timeout (30 seconds)
        /// </summary>
        public static void Initialize()
        {
            Initialize(30f);
        }

        /// <summary>
        /// Initializes the Quantum Leap plugin with custom timeout
        /// </summary>
        /// <param name="timeoutSeconds">Timeout in seconds</param>
        public static void Initialize(float timeoutSeconds)
        {
            if (_isInitialized)
            {
                QuantumLeapLogger.LogWarning("QuantumLeapManager is already initialized");
                return;
            }

            lock (_lockObject)
            {
                if (_isInitialized) return;

                try
                {
                    _httpClient = new HttpClient();
                    _httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
                    
                    _isInitialized = true;
                    
                    QuantumLeapLogger.Log($"QuantumLeapManager initialized successfully with timeout: {timeoutSeconds}s");
                    OnInitialized?.Invoke();
                }
                catch (Exception ex)
                {
                    QuantumLeapLogger.LogError($"Failed to initialize QuantumLeapManager: {ex.Message}");
                    OnError?.Invoke($"Initialization failed: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Fetches data from a REST API endpoint
        /// </summary>
        /// <param name="url">The API endpoint URL</param>
        /// <param name="headers">Optional headers to include in the request</param>
        /// <returns>The API response as a string</returns>
        public static async Task<string> FetchDataAsync(string url, Dictionary<string, string> headers = null)
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("QuantumLeapManager must be initialized before fetching data");
            }

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            request.Headers.Add(header.Key, header.Value);
                        }
                    }

                    var response = await _httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    
                    var content = await response.Content.ReadAsStringAsync();
                    
                    QuantumLeapLogger.Log($"API Response received from {url}: {content}");
                    OnApiResponseReceived?.Invoke(url, content);
                    
                    return content;
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to fetch data from {url}: {ex.Message}";
                QuantumLeapLogger.LogError(errorMessage);
                OnError?.Invoke(errorMessage);
                throw;
            }
        }

        /// <summary>
        /// Posts data to a REST API endpoint
        /// </summary>
        /// <param name="url">The API endpoint URL</param>
        /// <param name="data">The data to post (JSON string)</param>
        /// <param name="headers">Optional headers to include in the request</param>
        /// <returns>The API response as a string</returns>
        public static async Task<string> PostDataAsync(string url, string data, Dictionary<string, string> headers = null)
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("QuantumLeapManager must be initialized before posting data");
            }

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    request.Content = new StringContent(data, Encoding.UTF8, "application/json");
                    
                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            request.Headers.Add(header.Key, header.Value);
                        }
                    }

                    var response = await _httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    
                    var content = await response.Content.ReadAsStringAsync();
                    
                    QuantumLeapLogger.Log($"POST Response received from {url}: {content}");
                    OnApiResponseReceived?.Invoke(url, content);
                    
                    return content;
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to post data to {url}: {ex.Message}";
                QuantumLeapLogger.LogError(errorMessage);
                OnError?.Invoke(errorMessage);
                throw;
            }
        }

        /// <summary>
        /// Shuts down the plugin and cleans up resources
        /// </summary>
        public static void Shutdown()
        {
            if (!_isInitialized) return;

            lock (_lockObject)
            {
                if (!_isInitialized) return;

                try
                {
                    _httpClient?.Dispose();
                    _httpClient = null;
                    _isInitialized = false;
                    
                    QuantumLeapLogger.Log("QuantumLeapManager shut down successfully");
                }
                catch (Exception ex)
                {
                    QuantumLeapLogger.LogError($"Error during shutdown: {ex.Message}");
                }
            }
        }
    }
} 