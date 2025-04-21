using UnityEngine;

public class MiniGame : MonoBehaviour
{
    [SerializeField] private GameObject cube; // Référence au cube que l'on va manipuler

    // Méthode pour démarrer le mini-jeu lorsqu'un objet l'appelle
    public void StartingMiniGame(GameObject callingObject)
    {
        var script = cube.GetComponent<ChangeColor>(); // Récupère le script ChangeColor du cube
        var otherScript = cube.GetComponent<TutorialScript>(); // Récupère le script TutorialScript du cube

        // Si l'objet appelant est un bouton avec la couleur de départ, on lance le mini-jeu
        if (!callingObject.TryGetComponent(out PhysicsButton button)) return;
        if (button.color != ChangeColor.ColorType.Start) return;

        // Si le script ChangeColor existe, on lance le mini-jeu avec lui, sinon avec TutorialScript
        if (script == null)
        {
            if (otherScript == null) return;
            otherScript.StartMiniGame();
        }
        else script.StartMiniGame();

        // Désactive l'objet appelant une fois que le mini-jeu a commencé
        callingObject.SetActive(false);
    }

    // Méthode pour changer la couleur du cube pendant le mini-jeu
    public void ChangeCubeColoring(ChangeColor.ColorType color)
    {
        var script = cube.GetComponent<ChangeColor>(); // Récupère le script ChangeColor du cube
        var otherScript = cube.GetComponent<TutorialScript>(); // Récupère le script TutorialScript du cube

        // Si le script ChangeColor existe et que le mini-jeu n'est pas en cours, on change la couleur
        if (script != null)
        {
            if (!script.IsMiniGamePlaying)
            {
                script.ChangeMaterial(color); // Change la couleur selon l'énumération
            }
        }
        else
        {
            if (otherScript == null) return;
            if (!otherScript.IsMiniGamePlaying) // Si le mini-jeu n'est pas en cours, change la couleur
            {
                otherScript.ChangeMaterial(color); // Change la couleur selon l'énumération
            }
        }
    }
}
