# Just Fire - Mini Jeu Unity

## Description
Un mini-jeu où le joueur doit éteindre un incendie en tapant rapidement des séquences de touches fléchées.

## Installation et Configuration

### Étape 1: Ouvrir la scène
1. Ouvrez Unity
2. Ouvrez la scène `Assets/Scenes/JustFireGame.unity`

### Étape 2: Configuration automatique de la scène
1. Dans la hiérarchie, créez un GameObject vide
2. Ajoutez le script `SceneSetup` à ce GameObject
3. Dans l'inspecteur, clic droit sur le composant `SceneSetup`
4. Sélectionnez "Setup Complete Scene"
5. Cela va créer automatiquement tous les éléments UI nécessaires

### Étape 3: Configurer le GameManager
1. Sélectionnez l'objet "GameManager" dans la hiérarchie
2. Dans l'inspecteur, assignez les références suivantes:
   - Fire Slider: Glissez l'objet "FireSlider" de la hiérarchie
   - Sequence Display: Glissez l'objet "SequenceDisplay"
   - Game State Text: Glissez l'objet "GameStateText"

### Étape 4: Tester le jeu
1. Appuyez sur Play
2. Utilisez les touches fléchées (↑ ↓ ← →) pour reproduire la séquence affichée
3. Regardez le feu diminuer quand vous réussissez une séquence!

## Comment Jouer

1. **Objectif**: Éteindre le feu avant qu'il n'atteigne la gauche de l'écran
2. **Contrôles**: Utilisez les touches fléchées du clavier
3. **Gameplay**: 
   - Une séquence aléatoire de 5 à 10 flèches s'affiche en bas
   - Tapez la séquence exacte le plus rapidement possible
   - Chaque séquence réussie fait apparaître de l'eau qui éteint le feu
   - Si vous êtes trop lent, le feu gagne!

## Structure du Code

- **GameManager.cs**: Logique principale du jeu
- **FireEffect.cs**: Effets visuels du feu (couleur, scintillement)
- **PierController.cs**: Contrôle l'embarcadère qui lance l'eau
- **SceneSetup.cs**: Script utilitaire pour configurer la scène rapidement

## Personnalisation

Vous pouvez ajuster les paramètres dans le GameManager:
- `minSequenceLength` / `maxSequenceLength`: Longueur des séquences
- `fireSpeed`: Vitesse de progression du feu
- `waterEffectiveness`: Efficacité de l'eau pour éteindre le feu

## Prochaines Améliorations Possibles

1. Ajouter des sprites et animations plus détaillées
2. Ajouter des effets sonores
3. Implémenter différents niveaux de difficulté
4. Ajouter un système de score
5. Créer des effets de particules pour l'eau et le feu