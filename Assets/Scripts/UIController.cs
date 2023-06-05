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

    [SerializeField] private Button continueBtn;
    [SerializeField] private Button menuBtn;
    private void ActivateMenuUI()
    {
        inputScript.enabled = false;
        cameraScript.enabled = false;

        shadow.gameObject.SetActive(true);    
    }

    private void DeactivateMenuUI()
    {
        shadow.gameObject.SetActive(false);

        inputScript.enabled = true;
        cameraScript.enabled = true;
    }

    public void ShowVictory()
    {
        ActivateMenuUI();
        quitBtn.gameObject.SetActive(true);
        victoryText.gameObject.SetActive(true);
    }

    public void ShowDefeat()
    {
        ActivateMenuUI();
        quitBtn.gameObject.SetActive(true);
        defeatText.gameObject.SetActive(true);
    }

    public void ShowPause()
    {
        ActivateMenuUI();
        continueBtn.gameObject.SetActive(true);
        menuBtn.gameObject.SetActive(true);
    }

    public void HidePause()
    {
        continueBtn.gameObject.SetActive(false);
        menuBtn.gameObject.SetActive(false);
        DeactivateMenuUI();
    }
}
