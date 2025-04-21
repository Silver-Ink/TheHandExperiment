# TheHandExperiment 🖐️

Une expérience en réalité virtuelle qui nous fait redécouvrir notre perception des mains.

---

## 👥 Auteurs

- Audrey SANCHEZ  
- Martin JOUEN  
- Léo PELAZZA

---

## 🧠 Concept

Le joueur participe à un mini-jeu où la perception de ses mains change à chaque niveau grâce à des points de vue différents.

### 🎮 Fonctionnalités

- Calibration du joueur
- Tutoriel interactif
- 8 niveaux fonctionnels avec des perspectives variées
- Difficulté du mini-jeu modulable
- Sauvegarde des scores dans un fichier `.csv`

---

## 📁 Structure du projet

Assets/
└── Game/
    ├── Camera/        → Caméras utilisées dans les niveaux
    ├── Fonts/         → Polices du jeu
    ├── Prefabs/       → Objets réutilisables
    ├── Results/       → Fichiers de score (.csv)
    ├── Scenes/
    │   └── Levels/    → Scènes des différents niveaux
    ├── Scripts/
    │   ├── Buttons/       → Scripts pour les boutons
    │   ├── Calibration/   → Calibration du joueur
    │   ├── Camera/        → Gestion des caméras & orientation du texte
    │   ├── MiniGame/      → Logique du mini-jeu
    │   ├── Scores/        → Gestion des scores
    │   └── UI/            → Scripts de l'interface
    └── Sound/         → Sons utilisés dans le jeu

(Racine du projet : packages et dépendances Unity)

---

## 🖥️ Prérequis

- Casque VR avec **tracking des mains activé** (Meta Quest 2, Quest Pro, etc.)  
- **Aucune manette requise**  
- **Pas de déplacements nécessaires** → expérience recommandée en position assise  
- Unity **version 6** avec Android SDK installé  
- Une table devant le joueur (optionnelle mais recommandée)

---

## ▶️ Utilisation dans l'éditeur

1. Ouvrir la scène `MainMenu`  
2. Entrer un nom dans le champ `PlayerScoreManager`  
3. Lancer la scène de calibration (`Calibration`)  
4. Le jeu peut être quitté à tout moment  
5. Les résultats seront sauvegardés dans `Assets/Game/Results`

> 🔄 Rafraîchir le dossier dans l’explorateur pour voir apparaître le fichier `.csv`.

---

## ⚠️ À savoir

- Si le jeu est lancé directement depuis un niveau (et non via le menu principal), **le score ne sera pas enregistré**.  
- La sauvegarde d’un niveau se fait **uniquement** lorsqu’il est terminé.  
- Si le fichier `.csv` est **ouvert pendant le jeu**, la sauvegarde échouera.

---

## 🛠️ Astuce calibration (table)

Si le joueur observe un décalage entre la table physique et la table virtuelle :

1. Pincer **l’index et le pouce de la main droite**, paume tournée vers le ciel.  
2. Maintenir la position jusqu’à ce que le **logo Meta** apparaisse.  
3. Le recalibrage devrait s’effectuer automatiquement.
