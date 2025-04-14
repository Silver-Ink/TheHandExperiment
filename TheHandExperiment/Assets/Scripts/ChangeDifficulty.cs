using TMPro;
using UnityEngine;

public class ChangeDifficulty : MonoBehaviour
{

    [SerializeField]
    private GameObject textDifficulty;

    private TMP_Text text;

    private void Start()
    {
        text = textDifficulty.GetComponent<TMP_Text>();
    }

    public void IncreaseDifficulty()
    {
        PlayerScoreManager.Instance.AddDifficulty();
        text.text = PlayerScoreManager.Instance.GetStringDifficulty();
    }

    public void DecreaseDifficulty()
    {
        PlayerScoreManager.Instance.SubstractDifficulty();
        text.text = PlayerScoreManager.Instance.GetStringDifficulty();
    }
}
