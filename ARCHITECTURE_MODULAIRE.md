# 🏗️ Architecture Modulaire - Just Fire

## 🎯 Vue d'ensemble de l'architecture

Votre jeu "Just Fire" a été refactorisé en **7 modules indépendants** qui communiquent via des **événements** et des **interfaces claires**.

```
GameManager (Orchestrateur)
    ├── SequenceManager    (Gestion des séquences)
    ├── TimerManager       (Chronométrage)
    ├── FireManager        (Niveau de feu)
    ├── UIManager          (Interface utilisateur)
    ├── InputManager       (Entrées clavier)
    └── AudioManager       (Audio complet)
```

---

## 📋 Modules créés

### 🎮 **GameManager.cs** - Orchestrateur principal
**Responsabilités :**
- Coordination de tous les modules
- Gestion des états du jeu (StartScreen, Playing, GameOver)
- Communication inter-modules via événements
- Logique métier principale

**API principale :**
```csharp
void StartGame()
void RestartGame()  
void EndGame(bool victory)
float GetFireLevel()
bool IsGameActive()
```

### 🎯 **SequenceManager.cs** - Gestion des séquences
**Responsabilités :**
- Génération des séquences de flèches
- Validation des inputs
- Progression dans les séquences
- Historique et statistiques

**API principale :**
```csharp
void GenerateNewSequence()
bool ProcessInput(string input)
string GetExpectedInput()
float GetProgressPercentage()
```

### ⏱️ **TimerManager.cs** - Chronométrage
**Responsabilités :**
- Gestion du timer de jeu
- Événements temporels (urgent, fin)
- Formatage du temps
- Contrôles avancés (pause, ajout de temps)

**API principale :**
```csharp
void StartTimer()
void StopTimer()
void AddTime(float time)
string GetFormattedTimeRemaining()
```

### 🔥 **FireManager.cs** - Niveau de feu
**Responsabilités :**
- Progression du feu
- Récompenses et pénalités
- Seuils critiques
- États de feu (Safe, Dangerous, Critical)

**API principale :**
```csharp
void IncreaseFire(float amount)
void ReduceFire(float amount)
FireState GetFireState()
Color GetFireColor()
```

### 🎨 **UIManager.cs** - Interface utilisateur
**Responsabilités :**
- Rendu OnGUI complet
- Écrans (Start, Game, End)
- Messages et effets visuels
- Styles visuels cohérents

**API principale :**
```csharp
void ShowStartScreen()
void ShowGameUI()
void ShowEndScreen(bool victory, int sequences, int mistakes)
void ShowMessage(string message)
```

### ⌨️ **InputManager.cs** - Entrées
**Responsabilités :**
- Détection des touches
- Validation des inputs
- Historique des entrées
- Support WASD et flèches

**API principale :**
```csharp
void ProcessInput()
bool AnyKeyPressed()
bool RestartKeyPressed()
void SimulateInput(string input)
```

### 🔊 **AudioManager.cs** - Audio complet
**Responsabilités :**
- Musique de fond et urgente
- Sons procéduraux
- Contrôles de volume
- Transitions audio

**API principale :**
```csharp
void StartBackgroundMusic()
void PlayUrgentMusic()
void PlaySuccessSound()
void SetMasterVolume(float volume)
```

---

## 🔗 Communication inter-modules

### Architecture basée sur les événements :

```csharp
// Timer → GameManager
timerManager.OnTimeUp += () => EndGame(false);
timerManager.OnUrgentTime += () => audioManager.PlayUrgentMusic();

// Fire → Audio
fireManager.OnFireCritical += () => audioManager.PlayAlarmSound();

// Input → GameManager
inputManager.OnValidInput += ProcessValidInput;
inputManager.OnRestartRequested += RestartGame;

// Sequence → UI
sequenceManager.OnSequenceCompleted += HandleSequenceCompleted;
```

---

## ⚙️ Configuration Unity

### Étapes d'installation :

1. **Créer un GameObject principal** nommé "GameManager"

2. **Attacher SEULEMENT le script GameManager.cs**
   - Les autres modules sont ajoutés automatiquement

3. **Configurer dans l'Inspector :**
   ```
   Game Settings:
   - Game Time Limit: 60
   - Sequence Length: 4
   - Fire Spread Speed: 8
   - Success Reward: 20

   Audio Settings:
   - Background Music: [Votre fichier]
   - Urgent Music: [Votre fichier]
   ```

4. **C'est tout !** L'architecture se configure automatiquement

---

## ✅ Avantages de cette architecture

### 🔧 **Maintenabilité**
- Chaque module a une responsabilité claire
- Code organisé et facile à modifier
- Debugging simplifié

### 📈 **Extensibilité**
- Ajout facile de nouvelles fonctionnalités
- Réutilisation des modules dans d'autres projets
- Configuration flexible

### 🚀 **Performance**
- Exécution optimisée
- Pas de duplication de code
- Gestion mémoire améliorée

### 🧪 **Testabilité**
- Modules testables indépendamment
- Simulation d'événements possible
- Debugging par module

---

## 🔄 Migration depuis l'ancien code

### Ancien système (Monolithique) :
```
JustFireGame.cs (669 lignes)
└── Tout dans un seul fichier
```

### Nouveau système (Modulaire) :
```
GameManager.cs     (200 lignes) - Orchestration
SequenceManager.cs (150 lignes) - Séquences
TimerManager.cs    (180 lignes) - Timer
FireManager.cs     (200 lignes) - Feu
UIManager.cs       (300 lignes) - Interface
InputManager.cs    (200 lignes) - Entrées
AudioManager.cs    (250 lignes) - Audio
```

**Total :** 7 modules spécialisés vs 1 fichier monolithique

---

## 📊 API complète pour chaque module

### GameManager Events :
```csharp
OnGameStart
OnGameEnd
OnGameRestart
OnCorrectInput
OnIncorrectInput
OnSequenceCompleted
```

### SequenceManager Events :
```csharp
OnSequenceCompleted
OnNewSequenceGenerated
OnSequenceReset
```

### TimerManager Events :
```csharp
OnTimeUp
OnUrgentTime
OnTimeUpdated
OnTimerStarted
OnTimerStopped
```

### FireManager Events :
```csharp
OnFireLevelChanged
OnFireCritical
OnFireDanger
OnFireOut
OnFireMax
```

### InputManager Events :
```csharp
OnValidInput
OnInvalidInput
OnRestartRequested
OnQuitRequested
OnAnyKeyPressed
```

### AudioManager Events :
```csharp
OnSoundPlayed
OnBackgroundMusicStarted
OnUrgentMusicStarted
```

---

## 🎯 Utilisation avancée

### Exemple d'extension - Nouveau power-up :
```csharp
// Dans FireManager, ajoutez :
public void ActivateFireBoost(float duration)
{
    StartCoroutine(TemporaryFireBoost(2f, duration));
}

// Dans GameManager, connectez :
powerUpManager.OnFireBoostCollected += fireManager.ActivateFireBoost;
```

### Exemple de customisation - Nouvelle difficulté :
```csharp
// Configuration facile via GameManager
public void SetDifficulty(DifficultyLevel level)
{
    switch(level)
    {
        case DifficultyLevel.Easy:
            timerManager.SetTotalTime(90f);
            fireManager.SetSpreadSpeed(5f);
            break;
        case DifficultyLevel.Hard:
            timerManager.SetTotalTime(45f);
            fireManager.SetSpreadSpeed(12f);
            break;
    }
}
```

---

## 🔧 Debugging et maintenance

### Logs structurés par module :
```
🎮 GameManager - Jeu initialisé
🎯 SequenceManager initialisé - Longueur: 4
⏱️ TimerManager initialisé - Durée: 60s
🔥 FireManager initialisé - Niveau initial: 50
🎨 UIManager initialisé
⌨️ InputManager initialisé
🔊 AudioManager initialisé
```

### Statistiques par module disponibles :
- SequenceManager : Précision, séquences générées
- TimerManager : Temps écoulé, état
- FireManager : Efficacité, déclenchements critiques
- InputManager : Précision des touches, historique
- AudioManager : Sons joués, temps audio

---

**🎯 "Just Fire" dispose maintenant d'une architecture professionnelle, maintenable et extensible !**