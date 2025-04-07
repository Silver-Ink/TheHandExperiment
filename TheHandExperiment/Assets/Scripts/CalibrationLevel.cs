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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartButton.SetActive(true);
        ExitButton.SetActive(true);
        ConfirmButton.SetActive(false);
        
        OnPressStart();

        InstructionText.text = IntroText + CalibrationText;
    }

    public void OnPressStart()
    {
        StartButton.SetActive(false);
        ConfirmButton.SetActive(true);
        
        if (secondAjustement)
            InstructionText.text = CalibrationText;

        CaliibrationScript.StartCallibration();

    }
    public void OnPressExit()
    {
        SceneManager.LoadScene("MainMenu");
    }
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
