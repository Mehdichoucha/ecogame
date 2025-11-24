using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// JEU JUST FIRE - VERSION SIMPLE ET STABLE
/// Utilisez les flèches pour suivre les séquences et éteindre le feu !
/// </summary>
public class JustFireGame : MonoBehaviour
{
    [Header("Gameplay")]
    public float fireLevel = 50f;
    public float fireSpreadSpeed = 8f;
    public float successReward = 20f;
    public int sequenceLength = 4;
    
    [Header("Score")]
    public int sequencesCompleted = 0;
    public int mistakes = 0;
    
    // Variables internes
    private float gameTimer = 60f;
    private bool gameActive = true;
    private string currentSequence = "";
    private int currentPosition = 0;
    private readonly string[] arrows = { "↑", "↓", "←", "→" };
    private string currentMessage = "";
    private float messageTimer = 0f;
    
    // Effets visuels
    private float successEffectTimer = 0f;
    private float errorEffectTimer = 0f;
    private float gameStartTime = 0f;
    private bool showStartScreen = true;
    
    // Input System
    private Keyboard keyboard;
    
    // Audio System
    private JustFireAudio audioSystem;
    
    [Header("Audio Files")]
    public AudioClip backgroundMusic;      // Musique d'ambiance
    public AudioClip urgentMusic;          // Musique urgente à 12 secondes
    
    // Audio Sources
    private AudioSource musicSource;      // Pour la musique de fond
    private AudioSource urgentSource;     // Pour la musique urgente
    private bool urgentMusicPlayed = false;
    
    void Start()
    {
        keyboard = Keyboard.current;
        audioSystem = GetComponent<JustFireAudio>();
        
        // Configuration des sources audio
        SetupAudioSources();
        
        // Désactiver JustFireAudio si des fichiers audio sont assignés
        ConfigureAudioPriority();
        
        gameStartTime = Time.time;
        GenerateNewSequence();
        Debug.Log("🔥 JUST FIRE - Jeu démarré !");
        Debug.Log("🎮 Utilisez les flèches du clavier pour suivre les séquences !");
        ShowMessage("🔥 SAUVEZ LA FORÊT ! Suivez les séquences de flèches !", 3f);
    }
    
    void Update()
    {
        // Toujours gérer les inputs (même quand le jeu est arrêté pour permettre le redémarrage)
        HandleInput();
        
        if (!gameActive) return;
        
        // Décompter le timer
        gameTimer -= Time.deltaTime;
        
        // Vérifier si on doit jouer la musique urgente (12 secondes avant la fin)
        if (gameTimer <= 12f && !urgentMusicPlayed && urgentMusic != null)
        {
            PlayUrgentMusic();
        }
        
        if (gameTimer <= 0)
        {
            EndGame(fireLevel < 100f, fireLevel < 100f ? "🎉 BRAVO ! Vous avez sauvé la forêt !" : "⏰ TEMPS ÉCOULÉ !");
            return;
        }
        
        // Le feu se propage
        fireLevel += fireSpreadSpeed * Time.deltaTime;
        fireLevel = Mathf.Clamp(fireLevel, 0f, 100f);
        
        // Alarme si feu critique
        if (fireLevel > 80f && audioSystem != null && audioSystem.enableAudio)
        {
            audioSystem.PlayAlarmSound();
        }
        
        // Vérifier game over
        if (fireLevel >= 100f)
        {
            EndGame(false, "💥 LA FORÊT A BRÛLÉ ! Game Over...");
            return;
        }
        
        // Décompter le timer des messages et effets
        if (messageTimer > 0)
        {
            messageTimer -= Time.deltaTime;
        }
        
        if (successEffectTimer > 0)
        {
            successEffectTimer -= Time.deltaTime;
        }
        
        if (errorEffectTimer > 0)
        {
            errorEffectTimer -= Time.deltaTime;
        }
    }
    
    void HandleInput()
    {
        string inputKey = "";
        
        // Vérifier que le clavier est disponible
        if (keyboard == null) 
        {
            keyboard = Keyboard.current;
            if (keyboard == null) return;
        }
        
        // La touche R fonctionne toujours (même en fin de partie)
        if (keyboard.rKey.wasPressedThisFrame) 
        {
            Debug.Log("⌨️  Touche R détectée !");
            RestartGame();
            return;
        }
        
        // Écran d'accueil - toute touche pour commencer
        if (showStartScreen)
        {
            if (keyboard.anyKey.wasPressedThisFrame)
            {
                showStartScreen = false;
                gameStartTime = Time.time;
                
                // Démarrer la musique de fond quand le jeu commence
                StartBackgroundMusic();
                
                return;
            }
            else return;
        }
        
        // Les autres touches ne fonctionnent que pendant le jeu
        if (!gameActive) return;
        
        if (keyboard.upArrowKey.wasPressedThisFrame) inputKey = "↑";
        else if (keyboard.downArrowKey.wasPressedThisFrame) inputKey = "↓";
        else if (keyboard.leftArrowKey.wasPressedThisFrame) inputKey = "←";
        else if (keyboard.rightArrowKey.wasPressedThisFrame) inputKey = "→";
        
        if (!string.IsNullOrEmpty(inputKey))
        {
            ProcessInput(inputKey);
        }
    }
    
    void ProcessInput(string inputKey)
    {
        string expected = GetExpectedKey();
        
        if (inputKey == expected)
        {
            // Bonne touche
            currentPosition++;
            
            // Son de succès (seulement si sons procéduraux activés)
            if (audioSystem != null && audioSystem.enableAudio) audioSystem.PlaySuccessSound();
            
            if (currentPosition >= currentSequence.Length)
            {
                // Séquence terminée
                sequencesCompleted++;
                fireLevel = Mathf.Max(0f, fireLevel - successReward);
                ShowMessage($"✅ Séquence #{sequencesCompleted} réussie ! Feu réduit de {successReward}%", 2f);
                successEffectTimer = 1f; // Effet visuel de succès
                
                // Son de séquence terminée (seulement si sons procéduraux activés)
                if (audioSystem != null && audioSystem.enableAudio) audioSystem.PlaySequenceCompleteSound();
                
                GenerateNewSequence();
            }
        }
        else
        {
            // Mauvaise touche
            mistakes++;
            ShowMessage("❌ Mauvaise touche ! Recommencez la séquence", 2f);
            errorEffectTimer = 1f; // Effet visuel d'erreur
            currentPosition = 0; // Reset de la séquence
            
            // Son d'erreur (seulement si sons procéduraux activés)
            if (audioSystem != null && audioSystem.enableAudio) audioSystem.PlayErrorSound();
        }
    }
    
    string GetExpectedKey()
    {
        if (currentPosition >= currentSequence.Length) return "";
        return currentSequence[currentPosition].ToString();
    }
    
    void GenerateNewSequence()
    {
        currentSequence = "";
        
        for (int i = 0; i < sequenceLength; i++)
        {
            currentSequence += arrows[Random.Range(0, arrows.Length)];
        }
        
        currentPosition = 0;
        ShowMessage($"Nouvelle séquence: {currentSequence}", 2f);
    }
    
    void ShowMessage(string message, float duration = 2f)
    {
        currentMessage = message;
        messageTimer = duration;
    }
    
    void EndGame(bool victory, string message)
    {
        gameActive = false;
        ShowMessage(message, 10f);
        
        // Arrêter toute la musique à la fin du jeu
        StopAllMusic();
        
        // Son de fin de partie
        
        // Son de fin de partie (seulement si sons procéduraux activés)
        if (audioSystem != null && audioSystem.enableAudio)
        {
            audioSystem.PlayGameOverSound(victory);
        }        if (victory)
        {
            Debug.Log($"🏆 VICTOIRE ! Séquences: {sequencesCompleted}, Erreurs: {mistakes}");
        }
        else
        {
            Debug.Log($"💥 DÉFAITE ! Séquences: {sequencesCompleted}, Erreurs: {mistakes}");
        }
        
        Debug.Log("Appuyez sur R pour recommencer");
    }
    
    void RestartGame()
    {
        Debug.Log("🔄 Redémarrage du jeu demandé !");
        fireLevel = 50f;
        gameTimer = 60f;
        sequencesCompleted = 0;
        mistakes = 0;
        gameActive = true;
        showStartScreen = false;
        successEffectTimer = 0f;
        errorEffectTimer = 0f;
        currentPosition = 0;
        urgentMusicPlayed = false; // Reset du flag de musique urgente
        
        // Arrêter toute musique et redémarrer l'ambiance
        StopAllMusic();
        StartBackgroundMusic();
        
        GenerateNewSequence();
        ShowMessage("🔄 Jeu redémarré !", 2f);
        Debug.Log("🔄 Jeu redémarré !");
    }
    
    string GetColoredSequence()
    {
        string result = "";
        
        for (int i = 0; i < currentSequence.Length; i++)
        {
            if (i < currentPosition)
            {
                result += $"<color=green>{currentSequence[i]}</color> ";
            }
            else if (i == currentPosition)
            {
                result += $"<color=yellow>{currentSequence[i]}</color> ";
            }
            else
            {
                result += $"<color=white>{currentSequence[i]}</color> ";
            }
        }
        
        return result.Trim();
    }
    
    void OnGUI()
    {
        if (showStartScreen)
        {
            DrawStartScreen();
            return;
        }
        
        // Style de base modernisé
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 24;
        style.normal.textColor = Color.white;
        
        // Fond dégradé plus moderne
        DrawModernBackground();
        
        // En-tête avec titre du jeu
        DrawGameTitle();
        
        // Timer avec style amélioré
        DrawModernTimer();
        
        // Stats avec icônes et couleurs
        DrawModernStats();
        
        // Séquence avec mise en valeur
        DrawModernSequence();
        
        // Instructions stylisées
        DrawModernInstructions();
        
        // Messages avec effets
        DrawModernMessages();
        
        // Barre de feu améliorée
        DrawModernFireBar();
    }
    
    void DrawStartScreen()
    {
        // Fond animé plus doux
        float time = Time.time;
        GUI.color = new Color(0.2f + 0.03f * Mathf.Sin(time), 0.1f, 0.3f + 0.05f * Mathf.Sin(time * 0.7f), 1f); // Couleurs plus douces
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        GUI.color = Color.white;
        
        // Titre principal plus doux
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 50; // Réduit de 60 à 50
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        
        // Effet de pulsation très subtil
        float pulse = 1f + 0.1f * Mathf.Sin(time * 2f); // Pulsation plus douce
        titleStyle.normal.textColor = new Color(0.9f, 0.6f + 0.2f * pulse, 0.4f, 1f); // Orange plus doux
        
        GUI.Label(new Rect(0, Screen.height * 0.2f, Screen.width, 80), "🔥 JUST FIRE 🔥", titleStyle);
        
        // Sous-titre plus doux
        GUIStyle subtitleStyle = new GUIStyle(GUI.skin.label);
        subtitleStyle.fontSize = 28; // Réduit de 32 à 28
        subtitleStyle.fontStyle = FontStyle.Bold;
        subtitleStyle.alignment = TextAnchor.MiddleCenter;
        subtitleStyle.normal.textColor = new Color(0.8f, 0.8f, 0.5f, 1f); // Jaune plus doux
        
        GUI.Label(new Rect(0, Screen.height * 0.35f, Screen.width, 40), "Sauvez la Forêt !", subtitleStyle);
        
        // Description du jeu inchangée mais couleur plus douce
        GUIStyle descStyle = new GUIStyle(GUI.skin.label);
        descStyle.fontSize = 22; // Réduit de 24 à 22
        descStyle.alignment = TextAnchor.MiddleCenter;
        descStyle.normal.textColor = new Color(0.7f, 0.8f, 0.7f, 1f); // Plus doux
        descStyle.wordWrap = true;
        
        string description = "Suivez les séquences de flèches pour éteindre le feu\navant qu'il ne consume toute la forêt !\n\n" +
                           "⬆️ ⬇️ ⬅️ ➡️ Flèches directionnelles\n🔄 R pour redémarrer\n⏰ 60 secondes pour réussir";
        
        GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.5f, Screen.width * 0.8f, 200), description, descStyle);
        
        // Instruction pour commencer (clignotante mais plus douce)
        GUIStyle startStyle = new GUIStyle(GUI.skin.label);
        startStyle.fontSize = 24; // Réduit de 28 à 24
        startStyle.fontStyle = FontStyle.Bold;
        startStyle.alignment = TextAnchor.MiddleCenter;
        
        float blink = 0.6f + 0.4f * Mathf.Sin(time * 3f); // Clignotement plus doux
        startStyle.normal.textColor = new Color(0.5f, 0.8f, 0.9f, blink); // Cyan plus doux
        
        GUI.Label(new Rect(0, Screen.height * 0.8f, Screen.width, 40), "Appuyez sur n'importe quelle touche pour commencer", startStyle);
        
        // Créateurs inchangé
        GUIStyle creditStyle = new GUIStyle(GUI.skin.label);
        creditStyle.fontSize = 16; // Réduit de 18 à 16
        creditStyle.alignment = TextAnchor.MiddleCenter;
        creditStyle.normal.textColor = new Color(0.6f, 0.6f, 0.6f, 1f);
        
        GUI.Label(new Rect(0, Screen.height * 0.9f, Screen.width, 30), "Créé pour sauver notre planète 🌍", creditStyle);
    }
    
    void DrawModernBackground()
    {
        // Effet de succès - flash vert très doux
        if (successEffectTimer > 0)
        {
            float alpha = successEffectTimer * 0.1f; // Réduit de 0.3f à 0.1f
            GUI.color = new Color(0.6f, 1f, 0.6f, alpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        }
        
        // Effet d'erreur - flash rouge très doux
        if (errorEffectTimer > 0)
        {
            float alpha = errorEffectTimer * 0.1f; // Réduit de 0.3f à 0.1f
            GUI.color = new Color(1f, 0.7f, 0.7f, alpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        }
        
        // Fond dégradé plus doux - gris foncé
        GUI.color = new Color(0.1f, 0.1f, 0.1f, 0.8f); // Plus transparent
        GUI.DrawTexture(new Rect(0, 0, Screen.width, 300), Texture2D.whiteTexture);
        
        // Bordure décorative plus subtile
        GUI.color = new Color(0.4f, 0.4f, 0.6f, 0.2f); // Moins intense
        GUI.DrawTexture(new Rect(0, 298, Screen.width, 2), Texture2D.whiteTexture);
        
        GUI.color = Color.white;
    }
    
    void DrawGameTitle()
    {
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 32; // Réduit de 36 à 32
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.normal.textColor = new Color(0.9f, 0.7f, 0.5f, 1f); // Orange plus doux
        titleStyle.alignment = TextAnchor.MiddleCenter;
        
        // Effet d'ombre plus subtil
        GUIStyle shadowStyle = new GUIStyle(titleStyle);
        shadowStyle.normal.textColor = new Color(0, 0, 0, 0.3f); // Ombre plus douce
        
        GUI.Label(new Rect(1, 11, Screen.width, 50), "🔥 JUST FIRE 🔥", shadowStyle);
        GUI.Label(new Rect(0, 10, Screen.width, 50), "🔥 JUST FIRE 🔥", titleStyle);
    }
    
    void DrawModernTimer()
    {
        GUIStyle timerStyle = new GUIStyle(GUI.skin.label);
        timerStyle.fontSize = 28; // Réduit de 32 à 28
        timerStyle.fontStyle = FontStyle.Bold;
        timerStyle.alignment = TextAnchor.MiddleCenter;
        
        // Couleur selon l'urgence - plus douce
        Color timerColor;
        if (gameTimer > 40) timerColor = new Color(0.5f, 0.9f, 0.5f, 1f); // Vert plus doux
        else if (gameTimer > 20) timerColor = new Color(0.9f, 0.8f, 0.4f, 1f); // Jaune plus doux
        else timerColor = new Color(0.9f, 0.5f, 0.5f, 1f); // Rouge plus doux
        
        timerStyle.normal.textColor = timerColor;
        
        // Effet d'ombre plus subtil
        GUIStyle shadowStyle = new GUIStyle(timerStyle);
        shadowStyle.normal.textColor = new Color(0, 0, 0, 0.3f);
        
        int minutes = (int)(gameTimer / 60);
        int seconds = (int)(gameTimer % 60);
        string timeText = $"⏰ {minutes:00}:{seconds:00}";
        
        GUI.Label(new Rect(Screen.width/2 - 99, 61, 200, 40), timeText, shadowStyle);
        GUI.Label(new Rect(Screen.width/2 - 100, 60, 200, 40), timeText, timerStyle);
    }
    
    void DrawModernStats()
    {
        GUIStyle statsStyle = new GUIStyle(GUI.skin.label);
        statsStyle.fontSize = 24; // Réduit de 26 à 24
        statsStyle.fontStyle = FontStyle.Bold;
        
        // Niveau de feu avec couleur plus douce
        Color fireColor = Color.Lerp(new Color(0.8f, 0.7f, 0.4f, 1f), new Color(0.9f, 0.6f, 0.4f, 1f), fireLevel / 100f);
        statsStyle.normal.textColor = fireColor;
        
        GUIStyle shadowStyle = new GUIStyle(statsStyle);
        shadowStyle.normal.textColor = new Color(0, 0, 0, 0.3f);
        
        string fireText = $"🔥 Danger: {fireLevel:F0}%";
        GUI.Label(new Rect(21, 111, 400, 30), fireText, shadowStyle);
        GUI.Label(new Rect(20, 110, 400, 30), fireText, statsStyle);
        
        // Score avec couleurs plus douces
        statsStyle.normal.textColor = new Color(0.6f, 0.9f, 0.8f, 1f); // Cyan plus doux
        string scoreText = $"📊 Réussies: {sequencesCompleted} | Erreurs: {mistakes}";
        GUI.Label(new Rect(21, 141, 400, 30), scoreText, shadowStyle);
        GUI.Label(new Rect(20, 140, 400, 30), scoreText, statsStyle);
    }
    
    void DrawModernSequence()
    {
        GUIStyle sequenceStyle = new GUIStyle(GUI.skin.label);
        sequenceStyle.fontSize = 36; // Réduit de 40 à 36
        sequenceStyle.fontStyle = FontStyle.Bold;
        sequenceStyle.richText = true;
        sequenceStyle.alignment = TextAnchor.MiddleCenter;
        
        // Fond plus subtil pour la séquence
        GUI.color = new Color(0, 0, 0, 0.2f); // Plus transparent
        GUI.DrawTexture(new Rect(20, 170, Screen.width - 40, 60), Texture2D.whiteTexture);
        GUI.color = Color.white;
        
        // Bordure plus douce
        GUI.color = new Color(0.7f, 0.7f, 0.5f, 0.3f); // Moins intense
        for (int i = 0; i < 2; i++) // Bordure plus fine
        {
            GUI.DrawTexture(new Rect(20 + i, 170 + i, Screen.width - 40 - 2*i, 1), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(20 + i, 229 - i, Screen.width - 40 - 2*i, 1), Texture2D.whiteTexture);
        }
        GUI.color = Color.white;
        
        GUI.Label(new Rect(0, 180, Screen.width, 50), GetColoredSequence(), sequenceStyle);
    }
    
    void DrawModernInstructions()
    {
        GUIStyle instructStyle = new GUIStyle(GUI.skin.label);
        instructStyle.fontSize = 20; // Réduit de 22 à 20
        instructStyle.fontStyle = FontStyle.Bold;
        instructStyle.normal.textColor = new Color(0.7f, 0.8f, 0.7f, 1f); // Vert plus doux
        instructStyle.alignment = TextAnchor.MiddleCenter;
        
        GUI.Label(new Rect(0, 240, Screen.width, 30), "🎮 Flèches du clavier: ⬆️ ⬇️ ⬅️ ➡️ | R = Redémarrer", instructStyle);
    }
    
    void DrawModernMessages()
    {
        if (messageTimer > 0 && !string.IsNullOrEmpty(currentMessage))
        {
            GUIStyle messageStyle = new GUIStyle(GUI.skin.label);
            messageStyle.fontSize = 24; // Réduit de 28 à 24
            messageStyle.fontStyle = FontStyle.Bold;
            messageStyle.alignment = TextAnchor.MiddleCenter;
            
            // Couleur selon le type de message - plus douce
            if (currentMessage.Contains("✅"))
                messageStyle.normal.textColor = new Color(0.5f, 0.9f, 0.5f, 1f); // Vert plus doux
            else if (currentMessage.Contains("❌"))
                messageStyle.normal.textColor = new Color(0.9f, 0.6f, 0.5f, 1f); // Rouge plus doux
            else
                messageStyle.normal.textColor = new Color(0.6f, 0.8f, 0.9f, 1f); // Cyan plus doux
            
            // Fond semi-transparent plus subtil
            GUI.color = new Color(0, 0, 0, 0.5f); // Moins opaque
            GUI.DrawTexture(new Rect(0, 270, Screen.width, 40), Texture2D.whiteTexture);
            GUI.color = Color.white;
            
            GUI.Label(new Rect(0, 275, Screen.width, 30), currentMessage, messageStyle);
        }
    }
    
    void DrawModernFireBar()
    {
        float barWidth = (Screen.width - 40) * (fireLevel / 100f);
        
        // Fond de la barre plus doux
        GUI.color = new Color(0.3f, 0.3f, 0.3f, 0.6f); // Plus transparent
        GUI.DrawTexture(new Rect(20, Screen.height - 50, Screen.width - 40, 30), Texture2D.whiteTexture);
        
        // Barre de feu avec couleurs plus douces
        Color fireColor1, fireColor2;
        if (fireLevel < 30)
        {
            fireColor1 = new Color(0.8f, 0.8f, 0.5f, 1f); // Jaune plus doux
            fireColor2 = new Color(0.9f, 0.7f, 0.4f, 1f); // Orange plus doux
        }
        else if (fireLevel < 70)
        {
            fireColor1 = new Color(0.9f, 0.7f, 0.4f, 1f); // Orange doux
            fireColor2 = new Color(0.9f, 0.6f, 0.4f, 1f); // Orange foncé doux
        }
        else
        {
            fireColor1 = new Color(0.9f, 0.6f, 0.5f, 1f); // Rouge doux
            fireColor2 = new Color(0.8f, 0.5f, 0.4f, 1f); // Rouge intense doux
        }
        
        // Effet de pulsation très subtil pour les niveaux critiques
        float pulse = fireLevel > 80 ? 0.9f + 0.1f * Mathf.Sin(Time.time * 5f) : 1f; // Pulsation plus lente et douce
        
        GUI.color = Color.Lerp(fireColor1, fireColor2, 0.5f) * pulse;
        GUI.DrawTexture(new Rect(20, Screen.height - 50, barWidth, 30), Texture2D.whiteTexture);
        
        // Bordure plus subtile
        GUI.color = new Color(0.8f, 0.8f, 0.8f, 1f); // Gris clair au lieu de blanc
        GUI.Box(new Rect(20, Screen.height - 50, Screen.width - 40, 30), "");
        
        // Texte sur la barre plus doux
        GUIStyle barTextStyle = new GUIStyle(GUI.skin.label);
        barTextStyle.fontSize = 18; // Réduit de 20 à 18
        barTextStyle.fontStyle = FontStyle.Bold;
        barTextStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f, 1f); // Blanc cassé
        barTextStyle.alignment = TextAnchor.MiddleCenter;
        
        GUI.Label(new Rect(20, Screen.height - 50, Screen.width - 40, 30), $"🔥 {fireLevel:F0}%", barTextStyle);
        
        // Reset de la couleur
        GUI.color = Color.white;
    }
    
    // === MÉTHODES AUDIO ===
    
    void SetupAudioSources()
    {
        // AudioSource pour la musique de fond (seulement si clip assigné)
        if (backgroundMusic != null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.volume = 0.4f;
            musicSource.playOnAwake = false;
            Debug.Log("🎵 Musique d'ambiance configurée : " + backgroundMusic.name);
        }
        else
        {
            Debug.Log("💡 Aucune musique d'ambiance assignée");
        }
        
        // AudioSource pour la musique urgente (seulement si clip assigné)
        if (urgentMusic != null)
        {
            urgentSource = gameObject.AddComponent<AudioSource>();
            urgentSource.clip = urgentMusic;
            urgentSource.loop = false;
            urgentSource.volume = 0.7f;
            urgentSource.playOnAwake = false;
            Debug.Log("⚠️ Musique d'urgence configurée : " + urgentMusic.name);
        }
        else
        {
            Debug.Log("💡 Aucune musique d'urgence assignée");
        }
    }
    
    void ConfigureAudioPriority()
    {
        bool hasCustomAudio = (backgroundMusic != null || urgentMusic != null);
        
        if (hasCustomAudio && audioSystem != null)
        {
            // Désactiver les sons procéduraux de JustFireAudio
            audioSystem.enableAudio = false;
            Debug.Log("🔇 Sons procéduraux désactivés - Utilisation des fichiers audio personnalisés");
        }
        else if (audioSystem != null)
        {
            // Garder les sons procéduraux
            audioSystem.enableAudio = true;
            Debug.Log("🔊 Sons procéduraux activés - Aucun fichier audio personnalisé détecté");
        }
    }
    
    void PlayUrgentMusic()
    {
        urgentMusicPlayed = true;
        
        // Réduire le volume de la musique de fond
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.volume = 0.15f;
            Debug.Log("🔉 Volume musique d'ambiance réduit");
        }
        
        // Jouer la musique urgente
        if (urgentSource != null && urgentMusic != null)
        {
            urgentSource.Play();
            Debug.Log("⚠️ Musique urgente démarrée ! Plus que 12 secondes !");
            ShowMessage("⚠️ URGENCE ! Plus que 12 secondes !", 3f);
        }
        else
        {
            // Si pas de musique d'urgence, juste afficher le message
            ShowMessage("⚠️ URGENCE ! Plus que 12 secondes !", 3f);
            Debug.Log("⚠️ Alerte 12 secondes (pas de musique d'urgence)");
        }
    }
    
    void StopAllMusic()
    {
        if (musicSource != null) musicSource.Stop();
        if (urgentSource != null) urgentSource.Stop();
        Debug.Log("🔇 Toute la musique arrêtée");
    }
    
    void StartBackgroundMusic()
    {
        if (musicSource != null && backgroundMusic != null && !musicSource.isPlaying)
        {
            musicSource.volume = 0.4f;
            musicSource.Play();
            Debug.Log("🎵 Musique d'ambiance redémarrée");
        }
        else if (backgroundMusic == null)
        {
            Debug.Log("💡 Pas de musique d'ambiance à démarrer");
        }
    }
}