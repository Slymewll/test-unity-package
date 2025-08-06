using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuantumLeap
{
    /// <summary>
    /// Logging system for the Quantum Leap plugin
    /// Provides different log levels and formatting options
    /// </summary>
    public static class QuantumLeapLogger
    {
        /// <summary>
        /// Log levels for the Quantum Leap plugin
        /// </summary>
        public enum LogLevel
        {
            Debug = 0,
            Info = 1,
            Warning = 2,
            Error = 3
        }

        /// <summary>
        /// Current log level - only messages at or above this level will be logged
        /// </summary>
        public static LogLevel CurrentLogLevel = LogLevel.Info;

        /// <summary>
        /// Whether to include timestamps in log messages
        /// </summary>
        public static bool IncludeTimestamps = true;

        /// <summary>
        /// Whether to include the log level in log messages
        /// </summary>
        public static bool IncludeLogLevel = true;

        /// <summary>
        /// Event fired when a log message is created
        /// </summary>
        public static event Action<LogLevel, string> OnLogMessage;

        /// <summary>
        /// Logs a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="context">Optional context object</param>
        public static void LogDebug(string message, object context = null)
        {
            Log(LogLevel.Debug, message, context);
        }

        /// <summary>
        /// Logs an info message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="context">Optional context object</param>
        public static void Log(string message, object context = null)
        {
            Log(LogLevel.Info, message, context);
        }

        /// <summary>
        /// Logs a warning message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="context">Optional context object</param>
        public static void LogWarning(string message, object context = null)
        {
            Log(LogLevel.Warning, message, context);
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="context">Optional context object</param>
        public static void LogError(string message, object context = null)
        {
            Log(LogLevel.Error, message, context);
        }

        /// <summary>
        /// Logs a message with the specified log level
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="message">The message to log</param>
        /// <param name="context">Optional context object</param>
        public static void Log(LogLevel level, string message, object context = null)
        {
            if (level < CurrentLogLevel) return;

            var formattedMessage = FormatMessage(level, message, context);
            
            // Log to Unity console
            switch (level)
            {
                case LogLevel.Debug:
                    Debug.Log($"[QuantumLeap Debug] {formattedMessage}");
                    break;
                case LogLevel.Info:
                    Debug.Log($"[QuantumLeap] {formattedMessage}");
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning($"[QuantumLeap] {formattedMessage}");
                    break;
                case LogLevel.Error:
                    Debug.LogError($"[QuantumLeap] {formattedMessage}");
                    break;
            }

            // Fire the event
            OnLogMessage?.Invoke(level, formattedMessage);
        }

        /// <summary>
        /// Formats a log message with timestamp and context
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="message">The message to format</param>
        /// <param name="context">Optional context object</param>
        /// <returns>The formatted message</returns>
        private static string FormatMessage(LogLevel level, string message, object context)
        {
            var parts = new List<string>();

            // Add timestamp if enabled
            if (IncludeTimestamps)
            {
                parts.Add($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]");
            }

            // Add log level if enabled
            if (IncludeLogLevel)
            {
                parts.Add($"[{level}]");
            }

            // Add the main message
            parts.Add(message);

            // Add context if provided
            if (context != null)
            {
                parts.Add($"Context: {context}");
            }

            return string.Join(" ", parts);
        }

        /// <summary>
        /// Clears all log messages (useful for testing)
        /// </summary>
        public static void ClearLogs()
        {
            // This is a placeholder - in a real implementation, you might want to clear a log file
            // or clear stored log messages in memory
            Debug.Log("[QuantumLeap] Logs cleared");
        }
    }
} 