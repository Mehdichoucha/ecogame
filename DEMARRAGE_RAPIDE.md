# Guide de Démarrage Rapide - Just Fire

## 🚀 Mise en Route Immédiate (5 minutes)

### Étape 1: Ouvrir Unity
1. Ouvrez votre projet Unity dans le dossier `ecogame`
2. Allez dans `Assets > Scenes > JustFireGame.unity` (ou utilisez SampleScene.unity)

### Étape 2: Configuration Automatique
1. **Créer un GameObject vide** dans la scène (clic droit dans Hierarchy > Create Empty)
2. **Renommez-le "SceneSetup"**
3. **Glissez le script `SceneSetup.cs`** sur ce GameObject (depuis Assets/Scripts/)
4. **Dans l'inspecteur**, clic droit sur le composant SceneSetup
5. **Cliquez sur "Setup Complete Scene"**

✨ **MAGIE !** Votre interface utilisateur est maintenant créée automatiquement !

### Étape 3: Connecter le GameManager
1. **Sélectionnez l'objet "GameManager"** dans la Hierarchy
2. **Dans l'inspecteur**, remplissez les champs:
   - **Fire Slider**: Glissez "FireSlider" depuis la Hierarchy
   - **Sequence Display**: Glissez "SequenceDisplay"  
   - **Game State Text**: Glissez "GameStateText"

### Étape 4: Test Immédiat
1. **Appuyez sur PLAY** ▶️
2. **Regardez la séquence** affichée en bas (ex: ↑ → ↓ ←)
3. **Tapez les flèches** correspondantes sur votre clavier
4. **Regardez le feu diminuer** quand vous réussissez ! 🔥➡️💧

---

## 🎮 Contrôles de Jeu

| Touche | Action |
|--------|--------|
| ↑ ↓ ← → | Reproduire la séquence |
| R | Redémarrer (en mode debug) |
| F | Passer la séquence (debug) |
| W | Victoire instantanée (debug) |
| L | Défaite instantanée (debug) |

---

## 🛠️ Améliorations Rapides

### Ajouter des Sprites Simples
1. **Créez un GameObject vide** et ajoutez le script `SimpleSpriteMaker`
2. **Dans l'inspecteur**, clic droit et utilisez:
   - "Create Simple Pier Sprite" 
   - "Create Water Effect"
   - "Create Simple Fire Background"

### Ajouter le Debug
1. **Ajoutez le script `GameDebugger`** à votre GameManager
2. Cochez "Enable Debug Keys" et "Show Debug Info"
3. **Vous verrez maintenant** un panel debug en jeu !

---

## ⚡ Débogage Rapide

### Le jeu ne démarre pas ?
- ✅ Vérifiez que les références UI sont bien assignées dans GameManager
- ✅ Assurez-vous d'être en mode 2D (Project Settings > Editor > Default Behavior Mode)

### Les touches ne marchent pas ?
- ✅ Vérifiez que le focus est sur la fenêtre de jeu (cliquez dessus)
- ✅ Appuyez sur R pour redémarrer

### Pas de texte visible ?
- ✅ Importez TextMeshPro quand Unity le propose
- ✅ Vérifiez que vos UI sont dans le Canvas

---

## 🎯 Test de Fonctionnement

**Checklist rapide:**
- [ ] Le feu diminue de droite à gauche ✅
- [ ] Les séquences s'affichent en bas ✅  
- [ ] Les bonnes touches font progresser ✅
- [ ] Les mauvaises touches reset la séquence ✅
- [ ] Une séquence complète fait reculer le feu ✅
- [ ] On peut gagner (feu éteint) ✅
- [ ] On peut perdre (feu à gauche) ✅

**Votre prototype est prêt ! 🎉**

---

## 🔧 Personnalisation Facile

Dans le **GameManager**, ajustez:
- `Min/Max Sequence Length`: Longueur des séquences (5-10 par défaut)
- `Fire Speed`: Vitesse du feu (plus élevé = plus difficile)
- `Water Effectiveness`: Puissance de l'eau (plus élevé = plus facile)

---

**Temps total de setup: ~5 minutes**
**Temps de développement: ~2 heures**  
**Résultat: Prototype de jeu fonctionnel ! 🚀**