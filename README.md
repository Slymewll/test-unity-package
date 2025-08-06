# Quantum Leap Plugin

[![Unity Version](https://img.shields.io/badge/Unity-2021.3%2B-blue.svg)](https://unity3d.com/get-unity/download)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Package Version](https://img.shields.io/badge/Version-1.0.0-orange.svg)](package.json)

A powerful Unity plugin for fetching data from APIs and logging it with a configurable logging system and easy integration.

## 🚀 Features

- ✅ **HTTP API Support**: GET/POST requests with custom headers
- ✅ **Configurable Logging**: Multiple log levels (Debug, Info, Warning, Error)  
- ✅ **Easy Integration**: MonoBehaviour component for scene integration
- ✅ **Async/Await Support**: Modern C# async patterns
- ✅ **Error Handling**: Comprehensive error handling and logging
- ✅ **Event-Driven**: Subscribe to API responses and errors
- ✅ **Thread-Safe**: Proper async/await support
- ✅ **Unity Package Manager**: Git-based installation

## 📦 Installation

### Method 1: Git URL (Recommended)

1. Open Unity and go to `Window > Package Manager`
2. Click the `+` button and select `Add package from git URL...`
3. Enter the following URL:
   ```
   https://github.com/quantumleap/unity-plugin.git
   ```
4. Click `Add`

### Method 2: Git URL with Version

For a specific version, append the version tag:
```
https://github.com/quantumleap/unity-plugin.git#v1.0.0
```

### Method 3: Local Package

1. Clone this repository
2. In Unity Package Manager, click `+` and select `Add package from disk...`
3. Navigate to the cloned folder and select `package.json`

## 🎯 Quick Start

### Basic Usage

```csharp
using QuantumLeap;

// Initialize the plugin
QuantumLeapManager.Initialize();

// Fetch data from an API
var apiData = await QuantumLeapManager.FetchDataAsync("https://api.example.com/data");

// Log the data
QuantumLeapLogger.Log("API Response", apiData);
```

### Using the MonoBehaviour Component

1. **Add Component**: Add `QuantumLeapComponent` to any GameObject
2. **Configure**: Set API URL, timeout, and logging preferences in Inspector
3. **Use**: Call methods like `FetchDefaultData()` or `FetchData(url)`

### Editor Menu Integration

Access plugin features through `Tools > Quantum Leap`:
- **Create Test GameObject**: Quickly set up a test object
- **Test API Connection**: Test connectivity from the editor
- **Plugin Info**: View plugin information

## 📚 API Reference

### QuantumLeapManager

Core static class for API operations:

```csharp
// Initialize with default timeout (30s)
QuantumLeapManager.Initialize();

// Initialize with custom timeout
QuantumLeapManager.Initialize(60f);

// GET request
var response = await QuantumLeapManager.FetchDataAsync(url);

// POST request
var response = await QuantumLeapManager.PostDataAsync(url, jsonData);

// With custom headers
var headers = new Dictionary<string, string> { {"Authorization", "Bearer token"} };
var response = await QuantumLeapManager.FetchDataAsync(url, headers);
```

### QuantumLeapLogger

Configurable logging system:

```csharp
// Set log level
QuantumLeapLogger.CurrentLogLevel = QuantumLeapLogger.LogLevel.Debug;

// Log messages
QuantumLeapLogger.Log("Info message");
QuantumLeapLogger.LogWarning("Warning message");
QuantumLeapLogger.LogError("Error message");
QuantumLeapLogger.LogDebug("Debug message");
```

### QuantumLeapComponent

MonoBehaviour for easy scene integration:

```csharp
// Get component
var component = GetComponent<QuantumLeapComponent>();

// Fetch data
component.FetchData("https://api.example.com/data");

// Subscribe to events
component.OnDataReceived += (data) => Debug.Log($"Received: {data}");
component.OnErrorOccurred += (error) => Debug.LogError($"Error: {error}");
```

## 🧪 Testing

### Runtime Testing

1. Import the "Basic API Usage" sample from Package Manager
2. Follow the sample README instructions
3. Use the provided `BasicApiUsageExample` script

### Editor Testing

Use the editor menu items:
- `Tools > Quantum Leap > Test API Connection`
- `Tools > Quantum Leap > Create Test GameObject`

## 🎨 Samples

The package includes samples accessible through Package Manager:

### Basic API Usage
Demonstrates fundamental plugin usage patterns including:
- Plugin initialization
- Making API calls
- Error handling
- Custom headers

## 🔧 Configuration

### Package Configuration

Edit `package.json` to modify:
- Version number
- Dependencies
- Repository URLs

### Runtime Configuration

```csharp
// Configure logging
QuantumLeapLogger.CurrentLogLevel = QuantumLeapLogger.LogLevel.Info;
QuantumLeapLogger.IncludeTimestamps = true;

// Configure timeout
QuantumLeapManager.Initialize(45f); // 45 seconds timeout
```

## 🏗️ Project Structure

```
├── package.json              # Package manifest
├── README.md                 # This file
├── CHANGELOG.md             # Version history
├── LICENSE                  # MIT License
├── Runtime/                 # Runtime scripts
│   ├── QuantumLeapManager.cs
│   ├── QuantumLeapLogger.cs
│   ├── QuantumLeapComponent.cs
│   └── QuantumLeapPlugin.Runtime.asmdef
├── Editor/                  # Editor scripts
│   ├── QuantumLeapPluginEditor.cs
│   └── QuantumLeapPlugin.Editor.asmdef
├── Tests~/                  # Unit tests
└── Samples~/               # Sample projects
    └── BasicUsage/
        ├── Scripts/
        └── README.md
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📋 Requirements

- **Unity**: 2021.3 LTS or later
- **.NET**: 4.x or .NET Standard 2.1
- **Platform**: All platforms supported

## 🐛 Troubleshooting

### Common Issues

**Plugin not initializing:**
- Ensure you call `QuantumLeapManager.Initialize()` before making API calls
- Check Unity Console for initialization errors

**API calls failing:**
- Verify internet connection
- Check API endpoint accessibility
- Review request headers and authentication

**Assembly reference errors:**
- Ensure proper assembly references in your project
- Check that the package is properly imported

### Debug Mode

Enable detailed logging:
```csharp
QuantumLeapLogger.CurrentLogLevel = QuantumLeapLogger.LogLevel.Debug;
```

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🔗 Links

- [Repository](https://github.com/quantumleap/unity-plugin)
- [Issues](https://github.com/quantumleap/unity-plugin/issues)
- [Unity Package Manager Documentation](https://docs.unity3d.com/Manual/upm-ui.html)

## 📞 Support

For support and questions:
- Open an [issue](https://github.com/quantumleap/unity-plugin/issues) on GitHub
- Email: support@quantumleap.com
- Documentation: Check the README files in samples

---

Made with ❤️ for the Unity community 