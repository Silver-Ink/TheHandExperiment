using UnityEngine;

public class MiniGame : MonoBehaviour
{
    

    [SerializeField] private GameObject cube; // Référence au cube à changer

    public void StartingMiniGame(GameObject callingObject)
    {
        var script = cube.GetComponent<ChangeColor>();
        var otherScript = cube.GetComponent<TutorialScript>();
        if (!callingObject.TryGetComponent(out PhysicsButton button)) return;
        if (button.color != ChangeColor.ColorType.Start) return;
        if (script == null)
        {
            if (otherScript == null) return;
            otherScript.StartMiniGame();
        }
        else script.StartMiniGame();
        callingObject.SetActive(false);
    }

    public void ChangeCubeColoring(ChangeColor.ColorType color)
    {
        var script = cube.GetComponent<ChangeColor>();
        var otherScript = cube.GetComponent<TutorialScript>();
        if (script != null)
        {
            if (!script.IsMiniGamePlaying) //Permet de ne pas changer la couleur du cube lorsque les réponses sont affichées
            {
                script.ChangeMaterial(color);  // Change la couleur selon l'enum ColorType
            }
        }
        else
        {
            if (otherScript == null) return;
            if (!otherScript.IsMiniGamePlaying) //Permet de ne pas changer la couleur du cube lorsque les réponses sont affichées
            {
                otherScript.ChangeMaterial(color);  // Change la couleur selon l'enum ColorType
            }
        }
    }
}
