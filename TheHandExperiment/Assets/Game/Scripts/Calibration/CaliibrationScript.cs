using UnityEngine;

public class CaliibrationScript : MonoBehaviour
{
    // Nom du LayerMask à utiliser pour le raycast
    [SerializeField] private string LayerMaskStringName;

    // Références aux objets pour définir l'origine et la direction du raycast
    [SerializeField] private GameObject LeftRaycastOrigin;
    [SerializeField] private GameObject LeftRaycastDirection;
    [SerializeField] private GameObject LeftRaycastDirectionUp;

    // Références aux personnages à ajuster
    [SerializeField] private GameObject Character1;
    [SerializeField] private GameObject Character2;

    // Booléen pour savoir si le raycast doit être effectué
    private bool performRaycast = false;

    // Stockage de la hauteur initiale du personnage pour l'ajustement
    private float InitalCharacterHeight;

    // Start est appelé une seule fois avant la première exécution de Update
    // Initialisation de la hauteur initiale du personnage
    void Start()
    {
        InitalCharacterHeight = Character1.transform.position.y; // Sauvegarder la hauteur initiale du personnage
    }

    // Update est appelé à chaque frame
    // Effectue le raycast si `performRaycast` est vrai et ajuste la hauteur des personnages
    void Update()
    {
        // Si le raycast doit être effectué
        if (performRaycast)
        {
            // Effectuer le raycast et obtenir la distance
            float dist = Raycast();
            if (dist != 0f)
            {
                // Afficher la distance du raycast
                Debug.Log("Distance " + dist.ToString());

                // Ajuster les positions des personnages en fonction de la distance du raycast
                Character1.transform.position += new Vector3(0, -dist, 0); // Déplacer Character1 selon la distance du raycast
                Character2.transform.position += new Vector3(0, -dist, 0); // Déplacer Character2 de la même manière

                // Mettre à jour l'ajustement de la hauteur dans le Singleton
                CalibrationSingleton.Instance.HeightAjustment = Character1.transform.position.y - InitalCharacterHeight;
            }
        }
    }

    // Fonction qui effectue le raycast pour déterminer la distance à l'objet ciblé
    public float Raycast()
    {
        // Définir les positions des points d'origine et de direction du raycast
        Vector3 leftO = LeftRaycastOrigin.transform.position;  // Position de départ du raycast
        Vector3 leftD = LeftRaycastDirection.transform.position; // Direction du raycast
        Vector3 leftDup = LeftRaycastDirectionUp.transform.position; // Direction vers le haut du raycast

        // Variable pour stocker le résultat du raycast
        RaycastHit LeftRC;

        // Masque de couche à utiliser pour le raycast (ici le bit 6)
        int mask = 1 << 6;

        // Si la direction du raycast est vers le bas (c'est-à-dire que la main est orientée vers le bas)
        if ((leftD - leftO).y >= 0)
            return 0f; // Si la direction n'est pas vers le bas, on ne lance pas le raycast

        // Effectuer le raycast vers le bas pour détecter l'objet "BottomPlane"
        bool rc1 = Physics.Raycast(leftO, leftD - leftO, out LeftRC, 1000, mask);

        // Si un objet est détecté
        if (rc1)
        {
            // Vérifier si l'objet détecté est le "BottomPlane"
            if (LeftRC.collider.gameObject.tag == "BottomPlane")
            {
                // Effectuer un raycast supplémentaire pour obtenir la distance exacte
                rc1 = Physics.Raycast(leftO, leftDup - leftO, out LeftRC, 1000, mask);

                if (rc1)
                    return -LeftRC.distance;  // Retourner la distance négative
                else
                    Debug.Log("No hit "); // Afficher un message si aucun objet n'est détecté
            }
        }
        else
        {
            // Si aucun objet n'est détecté, retourner 0
            return 0f;
        }

        // Retourner la distance du raycast
        return LeftRC.distance;
    }

    // Fonction appelée pour démarrer le processus de calibration
    public void StartCallibration()
    {
        performRaycast = true;  // Indiquer que le raycast peut être effectué
    }

    // Fonction appelée pour arrêter le processus de calibration
    public void EndCallibration()
    {
        performRaycast = false;  // Empêcher le raycast d'être effectué
    }
}
