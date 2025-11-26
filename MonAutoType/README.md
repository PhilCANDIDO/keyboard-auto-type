# MonAutoType - Auto-Type Tool for Windows

## Installation

1. **Install .NET 8 SDK**: https://dotnet.microsoft.com/download/dotnet/8.0
2. **Open the MonAutoType folder in Visual Studio Code**
3. **Install the C# Dev Kit extension** in VSC if not already done

## Build

In the VSC terminal, from the `MonAutoType` folder:
```bash
dotnet build
```

## Run

```bash
dotnet run
```

Or directly:
```bash
.\bin\Debug\net8.0-windows\MonAutoType.exe
```

## Usage

1. **Enter the text** in the text area
2. **Click "Auto-Type"**
3. **Quickly switch focus** to the target window (you have a configurable delay)
4. The text will be automatically typed

## Tested Characters

The application correctly handles all Unicode characters including:
- Accented letters: é, è, à, ù, ç, ô, ï
- Symbols: €, @, #, ^, °
- Special characters: œ, æ, ñ
- Emojis and non-Latin characters

## Recommended Settings

- **Delay before typing**: 5 seconds
- **Delay between characters**: 15 ms
- **Delay between lines**: 100 ms

## Technical Notes

- Uses the Windows `SendInput` API with Unicode
- Works independently of keyboard layout (AZERTY, QWERTY, etc.)
- Uses Virtual Key Codes for Enter key (RDP/HTML5 compatible)
- Compatible with Windows 10/11

## Troubleshooting

If the application doesn't work:
1. Check that .NET 8 is installed: `dotnet --version`
2. Run as administrator if necessary
3. Some antivirus software may block keyboard simulation
