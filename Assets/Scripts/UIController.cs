using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [SerializeField] private Image shadow;

    [SerializeField] private TextMeshProUGUI victoryText;
    [SerializeField] private TextMeshProUGUI defeatText;

    [SerializeField] private Button quitBtn;

    [SerializeField] private InputScript inputScript;
    [SerializeField] private CameraScript cameraScript;

    private void InitializeGUI()
    {
        inputScript.enabled = false;
        cameraScript.enabled = false;

        shadow.gameObject.SetActive(true);
        quitBtn.gameObject.SetActive(true);
    }

    public void ShowVictory()
    {
        InitializeGUI();
        victoryText.gameObject.SetActive(true);
    }

    public void ShowDefeat()
    {
        InitializeGUI();
        defeatText.gameObject.SetActive(true);
    }
}
