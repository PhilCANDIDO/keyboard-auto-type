# MonAutoType - Keyboard Auto-Type Utility for Windows

[![GitHub Release](https://img.shields.io/github/v/release/PhilCANDIDO/keyboard-auto-type)](https://github.com/PhilCANDIDO/keyboard-auto-type/releases)
[![Platform](https://img.shields.io/badge/platform-Windows-blue)](https://github.com/PhilCANDIDO/keyboard-auto-type)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/download/dotnet/8.0)

## Overview

**MonAutoType** is a lightweight Windows utility designed for IT professionals working in production environments. It automates keyboard input by simulating keystrokes, making it invaluable for scenarios where clipboard paste functionality is restricted or unavailable.

This tool was developed by **Emerging-IT** to address real-world challenges faced by IT operations teams, system administrators, and DevOps engineers who regularly work with secured environments.

## The Problem It Solves

In modern IT production environments, security policies often restrict clipboard access across:

- **Remote Desktop (RDP) sessions** with clipboard redirection disabled
- **Privileged Access Management (PAM) solutions** like Wallix Bastion, CyberArk, BeyondTrust
- **Virtual Desktop Infrastructure (VDI)** environments with strict security policies
- **Jump servers and bastion hosts** in DMZ architectures
- **Air-gapped systems** requiring manual data entry
- **HTML5 web-based remote access clients** with limited clipboard support

When you need to enter complex passwords, configuration scripts, JSON payloads, or multi-line commands in these environments, manual typing becomes tedious and error-prone. **MonAutoType** bridges this gap by simulating genuine keyboard input that bypasses clipboard restrictions.

## Key Features

- **Unicode Support**: Handles all characters including accented letters (é, è, à, ç), symbols (€, @, #, °), and special characters
- **Multi-line Text**: Processes text with line breaks correctly using virtual key codes
- **Configurable Delay**: Adjustable countdown timer (1-30 seconds) to switch to target window
- **RDP/HTML5 Compatible**: Uses Windows Virtual Key Codes for Enter key, ensuring compatibility with Wallix Bastion HTML5 RDP sessions and similar PAM solutions
- **Keyboard Layout Independent**: Works regardless of the system keyboard layout (AZERTY, QWERTY, etc.)
- **Self-Contained Executable**: Single portable file with no external dependencies
- **Modern UI**: Clean Windows Forms interface with rounded buttons and dark-themed countdown window

## Technical Implementation

MonAutoType uses the Windows `SendInput` API with `KEYEVENTF_UNICODE` flag for character input, allowing it to:

1. Send characters directly by their Unicode code point
2. Bypass keyboard layout mapping issues
3. Support the full Unicode character set
4. Simulate authentic keyboard events recognized by all applications

For special keys like Enter, the application uses Virtual Key Codes (`VK_RETURN = 0x0D`) instead of Unicode characters, ensuring maximum compatibility with remote desktop protocols and web-based clients.

## Use Cases in IT Production

### Privileged Access Management (PAM)
When connecting through PAM solutions like Wallix Bastion, CyberArk, or similar products, clipboard functionality may be intentionally disabled for security audit compliance. MonAutoType allows you to efficiently enter credentials or commands without compromising the security model.

### DevOps & Configuration Management
Entering complex YAML configurations, JSON payloads, or multi-line shell scripts into servers accessed through restricted jump hosts.

### Database Administration
Executing SQL queries or scripts on database servers accessed through secured channels where copy-paste is unavailable.

### Compliance & Security Audits
Working in environments where clipboard logging is mandatory and you need an alternative input method that doesn't trigger clipboard events.

## Installation

### Option 1: Download Pre-built Executable (Recommended)

1. Go to the [Releases page](https://github.com/PhilCANDIDO/keyboard-auto-type/releases)
2. Download `MonAutoType.exe` from the latest release
3. Run the application - no installation required

### Option 2: Build from Source

**Prerequisites:**
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

**Build Command:**
```bash
dotnet publish MonAutoType/MonAutoType.csproj -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -o ./publish
```

The executable will be created at `./publish/MonAutoType.exe`

## Usage

1. **Launch** `MonAutoType.exe`
2. **Paste or type** the text you want to auto-type in the text area
3. **Set the delay** (1-30 seconds) - this gives you time to switch to the target window
4. **Click "Auto-Type"** button
5. **Quickly switch** to your target application/window during the countdown
6. The text will be **automatically typed** character by character

### Tips for Best Results

- Use a delay of 5+ seconds for remote sessions to account for network latency
- For very long texts, consider breaking them into smaller chunks
- Test with a simple text editor first before using with production systems
- Some applications may have input rate limiting - adjust if characters are skipped

## System Requirements

- **Operating System**: Windows 10/11 (x64)
- **Tested on**: Windows 11 Pro Version 24H2
- **Runtime**: Self-contained (no .NET installation required for pre-built executable)
- **Permissions**: Standard user (Administrator rights may be required for some protected windows)

## Limitations

- Windows only (uses Win32 API)
- Cannot type into windows running with higher privileges unless MonAutoType is also elevated
- Some security software may flag keyboard simulation tools
- Very fast typing may overwhelm some remote desktop sessions

## Disclaimer and Warranty

> **IMPORTANT: PLEASE READ CAREFULLY**
>
> This software is provided **"AS IS"**, without warranty of any kind, express or implied, including but not limited to the warranties of merchantability, fitness for a particular purpose, and non-infringement.
>
> **Emerging-IT and the author(s) of this software:**
> - Make no guarantees about the reliability, suitability, or accuracy of this software
> - Accept no responsibility for any damage, data loss, security incidents, or other issues that may arise from the use of this software
> - Are not liable for any direct, indirect, incidental, special, exemplary, or consequential damages
>
> **By using this software, you acknowledge and agree that:**
> - You use this software entirely at your own risk
> - You are solely responsible for ensuring compliance with your organization's security policies
> - You will not use this software for any malicious, unauthorized, or illegal purposes
> - Any damages or issues resulting from the use of this software are your sole responsibility
>
> **This tool is intended for legitimate IT administration purposes only.** Misuse of keyboard simulation software may violate computer security laws and organizational policies. Always obtain proper authorization before using this tool in any environment.

## Security Considerations

- This tool simulates keyboard input - use responsibly and only on systems you are authorized to access
- Be aware that the text you enter is visible in the application window
- Consider your organization's security policies regarding automation tools
- The application does not store, transmit, or log any entered text

## Contributing

Contributions are welcome! Please feel free to submit issues or pull requests on GitHub.

## License

This project is open source. See the repository for license details.

## Author

**Emerging-IT** - IT Production Services & Solutions

---

*Built with .NET 8.0 and Windows Forms*
