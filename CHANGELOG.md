# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2024-01-08

### Added
- Initial release of Quantum Leap Plugin
- Core API functionality with `QuantumLeapManager`
- Configurable logging system with `QuantumLeapLogger`
- MonoBehaviour component `QuantumLeapComponent` for easy scene integration
- Support for GET and POST HTTP requests
- Custom headers support
- Configurable request timeouts
- Event-driven architecture with callbacks
- Comprehensive error handling
- Git-based Unity Package Manager support
- Editor integration with menu items
- Basic usage sample with documentation
- Assembly definitions for Runtime and Editor
- MIT License

### Features
- **HTTP API Support**: GET/POST requests with custom headers
- **Configurable Logging**: Multiple log levels (Debug, Info, Warning, Error)
- **Easy Integration**: MonoBehaviour component for scene integration
- **Async/Await Support**: Modern C# async patterns
- **Error Handling**: Comprehensive error handling and logging
- **Event-Driven**: Subscribe to API responses and errors
- **Thread-Safe**: Proper async/await support
- **Unity Package Manager**: Git-based installation

### Technical Details
- Minimum Unity version: 2021.3 LTS
- .NET compatibility: 4.x and .NET Standard 2.1
- Cross-platform support
- Proper assembly separation (Runtime/Editor)

## [Unreleased]

### Planned
- Unit tests implementation
- Additional samples (POST requests, authentication)
- Performance optimizations
- Enhanced error reporting
- Retry mechanisms for failed requests
- Request caching system
- WebSocket support

---

## Version History

- **1.0.0**: Initial release with core functionality 