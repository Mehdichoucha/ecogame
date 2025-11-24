# 🎵 Guide d'Intégration Audio - Just Fire

## 🎯 Vos Fichiers Audio Spécifiques

Votre jeu va utiliser :
1. **Musique d'ambiance** - Joue tout au long du jeu
2. **Musique urgente** - Se déclenche à 12 secondes de la fin

## 📁 Étape 1 : Importer vos fichiers dans Unity

### 1. Placez vos fichiers audio :
```
Assets/Audio/
├── musique_ambiance.mp3 (ou .wav/.ogg)
└── musique_urgente.mp3 (ou .wav/.ogg)
```

### 2. Dans Unity :
- Glissez vos fichiers audio dans le dossier `Assets/Audio/`
- Unity les importera automatiquement
- Vous les verrez dans la fenêtre Project

## 🎮 Étape 2 : Configuration dans l'Inspector

### 1. Sélectionnez votre GameObject principal
- Celui qui contient le script `JustFireGame`

### 2. Dans l'Inspector, section "Audio Files" :
- **Background Music** : Glissez votre musique d'ambiance
- **Urgent Music** : Glissez votre musique urgente

![Configuration Audio](https://via.placeholder.com/400x200/2E8B57/FFFFFF?text=Audio+Files+Configuration)

## ⚙️ Paramètres Audio Configurés

### Musique d'Ambiance :
```csharp
Volume: 0.4f (40%)     // Modéré pour ne pas couvrir les effets
Loop: true             // Joue en boucle
Démarre: Au lancement du jeu
```

### Musique Urgente :
```csharp
Volume: 0.7f (70%)     // Plus fort pour créer la pression
Loop: false            // Joue une seule fois
Démarre: À 12 secondes de la fin
Effet: Réduit la musique d'ambiance à 15%
```

## 🎯 Comportement Audio Programmé

### Timeline du jeu :
```
[0s ────────────────────── 48s ─── 60s]
 │                          │      │
 Musique d'ambiance        Musique │
 (Volume normal)           urgente │
                           +       │
                           Ambiance│
                           réduite │
                                   Fin
```

### Événements automatiques :
- ✅ **Démarrage** : Musique d'ambiance quand vous appuyez sur une touche
- ✅ **12 secondes restantes** : Musique urgente + message d'alerte
- ✅ **Redémarrage (R)** : Reset complet de l'audio
- ✅ **Fin de partie** : Arrêt de toute musique + son de victoire/défaite

## 🔧 Paramètres Ajustables

### Dans le script, vous pouvez modifier :

```csharp
// Volumes
musicSource.volume = 0.4f;    // Musique d'ambiance (0.0 - 1.0)
urgentSource.volume = 0.7f;   // Musique urgente (0.0 - 1.0)

// Timing de la musique urgente
if (gameTimer <= 12f)         // Changez 12f pour autre timing
```

## 🎵 Formats Audio Supportés

Unity supporte :
- **.mp3** (Recommandé pour la musique)
- **.wav** (Haute qualité, plus lourd)
- **.ogg** (Bon compromis)
- **.aif**

### Recommandations :
- **Musique d'ambiance** : .mp3, 44.1kHz, stéréo
- **Musique urgente** : .mp3, 44.1kHz, stéréo
- **Taille** : Idéalement < 5MB par fichier

## 🔊 Test et Debug

### Messages dans la Console Unity :
```
🎵 Musique d'ambiance démarrée
⚠️ Musique urgente démarrée ! Plus que 12 secondes !
🔇 Toute la musique arrêtée
🎵 Musique d'ambiance redémarrée
```

### Vérifications :
1. **Audio Listener** présent sur Main Camera
2. **Fichiers assignés** dans l'Inspector
3. **Volume Master** de Unity > 0
4. **Pas de mute** sur les AudioSource

## 🚀 Instructions Complètes

1. **Importez vos fichiers** dans `Assets/Audio/`
2. **Sélectionnez votre GameObject** principal
3. **Assignez les clips** dans l'Inspector
4. **Testez** - la musique démarre automatiquement !

## 💡 Fonctionnalités Bonus Incluses

- **Transition douce** : La musique d'ambiance s'atténue pour la musique urgente
- **Reset propre** : Redémarrage avec R remet tout à zéro
- **Gestion d'erreurs** : Le jeu fonctionne même sans fichiers audio
- **Performance** : Optimisé pour ne pas affecter le gameplay

---

**Votre système audio est maintenant prêt ! 🎯**

Importez vos fichiers et testez - l'immersion sera au rendez-vous !