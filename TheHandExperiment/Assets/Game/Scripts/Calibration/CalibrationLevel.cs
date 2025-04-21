using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CalibrationLevel : MonoBehaviour
{
    [SerializeField] private GameObject StartButton;
    [SerializeField] private GameObject ExitButton;
    [SerializeField] private GameObject ConfirmButton;

    [SerializeField] private TextMeshProUGUI InstructionText;
    [SerializeField] private TextMeshProUGUI StartText;

    [SerializeField] private string IntroText;
    [SerializeField] private string CalibrationText;
    [SerializeField] private string SuccessText;

    [SerializeField] private CaliibrationScript CaliibrationScript;

    private bool secondAjustement = false;

    // Initialise les boutons et le texte d'instruction au lancement de la scène
    void Start()
    {
        StartButton.SetActive(true);
        ExitButton.SetActive(true);
        ConfirmButton.SetActive(false);

        OnPressStart();

        InstructionText.text = IntroText + CalibrationText;
    }

    // Lance la calibration et met à jour l'interface utilisateur
    public void OnPressStart()
    {
        StartButton.SetActive(false);
        ConfirmButton.SetActive(true);

        if (secondAjustement)
            InstructionText.text = CalibrationText;

        CaliibrationScript.StartCallibration();
    }

    // Retourne au menu principal
    public void OnPressExit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Termine la calibration, met à jour l'interface et prépare un éventuel recalibrage
    public void OnPressConfirm()
    {
        InstructionText.text = SuccessText;
        ConfirmButton.SetActive(false);
        StartButton.SetActive(true);
        StartText.text = "Recalibrate";

        CaliibrationScript.EndCallibration();
        secondAjustement = true;
    }
}