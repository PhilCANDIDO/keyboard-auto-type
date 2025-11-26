# MonAutoType - Outil de saisie automatique pour Windows

## Installation

1. **Installer .NET 8 SDK** : https://dotnet.microsoft.com/download/dotnet/8.0
2. **Ouvrir le dossier MonAutoType dans Visual Studio Code**
3. **Installer l'extension C# Dev Kit** dans VSC si pas déjà fait

## Compilation

Dans le terminal VSC, depuis le dossier `MonAutoType` :
```bash
dotnet build
```

## Exécution

```bash
dotnet run
```

Ou directement :
```bash
.\bin\Debug\net8.0-windows\MonAutoType.exe
```

## Utilisation

1. **Saisir le texte** dans la zone de texte
2. **Cliquer sur "Auto-Type"**
3. **Placer rapidement le focus** sur la fenêtre cible (vous avez 3 secondes)
4. Le texte sera automatiquement tapé

## Caractères testés

L'application gère correctement tous les caractères Unicode incluant :
- Lettres accentuées : é, è, à, ù, ç, ô, ï
- Symboles : €, @, #, ^, °
- Caractères spéciaux : œ, æ, ñ
- Emojis et caractères non-latins

## Configuration recommandée

- **Délai avant saisie** : 3 secondes
- **Délai entre caractères** : 15 ms
- **Délai entre lignes** : 100 ms

## Notes techniques

- Utilise l'API Windows `SendInput` avec Unicode
- Fonctionne indépendamment du layout clavier (AZERTY, QWERTY, etc.)
- Compatible Windows 11

## Dépannage

Si l'application ne fonctionne pas :
1. Vérifier que .NET 8 est installé : `dotnet --version`
2. Exécuter en tant qu'administrateur si nécessaire
3. Certains antivirus peuvent bloquer la simulation de saisie