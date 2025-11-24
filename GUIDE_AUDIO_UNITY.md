# 🔊 Guide Audio Unity - Just Fire

## 🎮 Configuration audio dans Unity

### Option 1 : Utiliser le système audio intégré (Recommandé)

Votre jeu a déjà un système audio procédural qui génère des sons sans fichiers !

#### Étapes dans Unity :

1. **Sélectionner votre GameObject principal**
   - Cliquez sur l'objet qui contient le script `JustFireGame`

2. **Ajouter le script audio**
   - Dans l'Inspector, cliquez sur "Add Component"
   - Tapez "JustFireAudio" et sélectionnez le script

3. **Le système audio se configure automatiquement !**
   - Un composant `AudioSource` sera créé automatiquement
   - Les sons se joueront aux moments appropriés

#### Sons inclus :
- 🔊 **Son de succès** : Quand vous appuyez sur la bonne flèche
- 🎵 **Séquence complète** : Quand vous terminez une séquence
- ❌ **Son d'erreur** : Quand vous vous trompez
- 🚨 **Alarme** : Quand le feu devient critique (>80%)
- 🏆 **Victoire/Défaite** : À la fin de partie

---

## 🎵 Option 2 : Ajouter des fichiers audio externes

### Étapes pour importer des sons :

1. **Créer un dossier Audio**
   ```
   Assets/Audio/
   ```

2. **Importer vos fichiers audio**
   - Glissez vos fichiers .mp3, .wav, .ogg dans le dossier Audio
   - Unity les convertira automatiquement

3. **Modifier le script pour utiliser des AudioClips**

#### Code pour AudioClips :

```csharp
[Header("Audio Clips")]
public AudioClip successSound;
public AudioClip errorSound;
public AudioClip alarmSound;
public AudioClip victorySound;

void PlaySuccessSound()
{
    if (successSound != null)
        audioSource.PlayOneShot(successSound);
}
```

4. **Assigner les clips dans l'Inspector**
   - Glissez vos fichiers audio vers les champs correspondants

---

## ⚙️ Paramètres Audio

### Dans le script JustFireAudio :

```csharp
[Header("Audio Settings")]
public bool enableAudio = true;      // Activer/désactiver l'audio
public float masterVolume = 0.3f;    // Volume général (0.0 - 1.0)
```

### Contrôles avancés :

```csharp
// Pour ajuster le volume de chaque son
public float successVolume = 1.0f;
public float errorVolume = 0.8f;
public float alarmVolume = 0.6f;

// Dans PlaySuccessSound() :
audioSource.PlayOneShot(successSound, successVolume);
```

---

## 🎚️ Mixer Audio (Avancé)

### Créer un Audio Mixer :

1. **Assets** → **Create** → **Audio Mixer**
2. **Créer des groupes** : Music, SFX, Master
3. **Assigner l'AudioSource** au bon groupe
4. **Contrôler le volume** via script :

```csharp
public AudioMixer audioMixer;

public void SetMasterVolume(float volume)
{
    audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
}
```

---

## 🎵 Sons de musique de fond

### Ajouter une musique d'ambiance :

```csharp
[Header("Background Music")]
public AudioClip backgroundMusic;

void Start()
{
    if (backgroundMusic != null)
    {
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.volume = 0.2f;
        audioSource.Play();
    }
}
```

---

## 🔧 Debugging Audio

### Vérifications dans Unity :

1. **AudioListener** : Doit être sur la Main Camera
2. **AudioSource** : Volume > 0, pas en mute
3. **Console** : Vérifiez les messages de debug
4. **Audio Settings** : Window → Audio

### Messages de debug dans votre jeu :
```
🔊 Son de succès joué !
🔊 Son d'erreur joué !
🔊 Son de fin de partie - Victoire/Défaite
```

---

## 🎯 Configuration actuelle de votre jeu

Votre jeu est maintenant configuré avec :
- ✅ Système audio procédural intégré
- ✅ Sons automatiques aux bons moments
- ✅ Volume ajustable
- ✅ Pas besoin de fichiers externes

**Pour activer :** Ajoutez simplement le script `JustFireAudio` à votre GameObject principal !

---

## 💡 Tips supplémentaires

- **Performance** : Les sons procéduraux sont plus légers que les fichiers audio
- **Simplicité** : Pas de gestion de fichiers externes
- **Customisation** : Modifiez les fréquences dans `JustFireAudio.cs`
- **Volume** : Ajustez `masterVolume` entre 0.0 et 1.0