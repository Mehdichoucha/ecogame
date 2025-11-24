using UnityEngine;

/// <summary>
/// Système audio simple pour Just Fire - Sons procéduraux sans fichiers externes
/// </summary>
public class JustFireAudio : MonoBehaviour
{
    [Header("Audio Settings")]
    public bool enableAudio = true;
    public float masterVolume = 0.3f;
    
    private AudioSource audioSource;
    private JustFireGame gameReference;
    
    void Start()
    {
        // Créer AudioSource si pas présent
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Configuration de base
        audioSource.playOnAwake = false;
        audioSource.volume = masterVolume;
        
        // Trouver référence au jeu
        gameReference = GetComponent<JustFireGame>();
        if (gameReference == null)
        {
            gameReference = FindObjectOfType<JustFireGame>();
        }
        
        Debug.Log("🔊 Système audio Just Fire initialisé");
    }
    
    void Update()
    {
        if (!enableAudio) return;
        
        // Sons basés sur les actions du jeu
        CheckAndPlaySounds();
    }
    
    void CheckAndPlaySounds()
    {
        if (gameReference == null) return;
        
        // Son d'urgence quand le feu est critique
        if (gameReference.fireLevel > 85f)
        {
            PlayAlarmSound();
        }
        
        // Sons de réussite/échec (via les timers d'effets visuels)
        // Note: Cette méthode nécessiterait l'accès aux variables privées du jeu
        // Pour la présentation, nous utilisons une approche simplifiée
    }
    
    // Méthodes publiques pour être appelées par le jeu
    public void PlaySuccessSound()
    {
        if (!enableAudio) return;
        
        // Son de succès - tonalité montante
        StartCoroutine(PlayToneSequence(new float[] { 440f, 554f, 659f }, 0.1f));
        Debug.Log("🔊 Son de succès");
    }
    
    public void PlayErrorSound()
    {
        if (!enableAudio) return;
        
        // Son d'erreur - tonalité descendante
        StartCoroutine(PlayToneSequence(new float[] { 330f, 261f }, 0.15f));
        Debug.Log("🔊 Son d'erreur");
    }
    
    public void PlayAlarmSound()
    {
        if (!enableAudio || audioSource.isPlaying) return;
        
        // Son d'alarme - bip rapide
        StartCoroutine(PlayToneSequence(new float[] { 800f, 600f }, 0.05f));
    }
    
    public void PlayGameOverSound(bool victory)
    {
        if (!enableAudio) return;
        
        if (victory)
        {
            // Mélodie de victoire
            StartCoroutine(PlayToneSequence(new float[] { 523f, 659f, 784f, 1047f }, 0.3f));
        }
        else
        {
            // Son de défaite
            StartCoroutine(PlayToneSequence(new float[] { 220f, 196f, 174f, 147f }, 0.2f));
        }
        
        Debug.Log($"🔊 Son de fin de partie - {(victory ? "Victoire" : "Défaite")}");
    }
    
    public void PlayStartSound()
    {
        if (!enableAudio) return;
        
        // Mélodie d'introduction
        StartCoroutine(PlayToneSequence(new float[] { 262f, 330f, 392f, 523f }, 0.2f));
        Debug.Log("🔊 Son de démarrage");
    }
    
    // Méthodes alias pour compatibilité
    public void PlaySequenceCompleteSound()
    {
        // Son spécial pour séquence complète (plus élaboré que le simple succès)
        if (!enableAudio) return;
        StartCoroutine(PlayToneSequence(new float[] { 392f, 523f, 659f }, 0.15f));
        Debug.Log("🔊 Son de séquence complète");
    }
    
    public void PlayGameEndSound(bool victory)
    {
        // Alias pour PlayGameOverSound
        PlayGameOverSound(victory);
    }
    
    // Génération de sons procéduraux simples
    System.Collections.IEnumerator PlayToneSequence(float[] frequencies, float duration)
    {
        foreach (float frequency in frequencies)
        {
            PlayTone(frequency, duration);
            yield return new WaitForSeconds(duration);
        }
    }
    
    void PlayTone(float frequency, float duration)
    {
        if (!enableAudio) return;
        
        // Générer un son simple (onde sinusoïdale)
        int sampleRate = AudioSettings.outputSampleRate;
        int samples = (int)(sampleRate * duration);
        float[] audioData = new float[samples];
        
        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / sampleRate;
            audioData[i] = Mathf.Sin(2 * Mathf.PI * frequency * t) * masterVolume;
            
            // Envelope pour éviter les clics
            if (i < samples * 0.1f)
                audioData[i] *= i / (samples * 0.1f);
            else if (i > samples * 0.9f)
                audioData[i] *= (samples - i) / (samples * 0.1f);
        }
        
        // Créer et jouer l'AudioClip
        AudioClip clip = AudioClip.Create("ProceduralTone", samples, 1, sampleRate, false);
        clip.SetData(audioData, 0);
        
        audioSource.clip = clip;
        audioSource.Play();
    }
    
    // Contrôles publics
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        if (audioSource != null)
            audioSource.volume = masterVolume;
    }
    
    public void ToggleAudio()
    {
        enableAudio = !enableAudio;
        Debug.Log($"🔊 Audio {(enableAudio ? "activé" : "désactivé")}");
    }
}