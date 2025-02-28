using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject onMatchUIs;
    public Text playerHealthText;
    public Text opponentHealthText;

    public Text resultText;
    private CanvasGroup resultCanvasGroup;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        resultCanvasGroup = resultText.GetComponent<CanvasGroup>();
        resultCanvasGroup.alpha = 0;
    }
    private void Start()
    {
        onMatchUIs.SetActive(false);
    }

    public void TurnOnMatchUIs()
    {
        onMatchUIs.gameObject.SetActive(true);
    }
    public void TurnOffMatchUIs()
    {
        onMatchUIs.gameObject.SetActive(false);
    }

    public void UpdateMatchUI()
    {
        playerHealthText.text = $"Health : {GameManager.instance.matchManager.playersHealth.ToString()}";
        opponentHealthText.text = $"Opponent's Health{GameManager.instance.matchManager.opponentsHealth.ToString()}";
    }

    public IEnumerator PostResultUI(bool didWin)
    {
        resultText.text = didWin ? "Win" : "Lose";

        resultCanvasGroup.alpha = 0;

        while(resultCanvasGroup.alpha < 1)
        {
            resultCanvasGroup.alpha += (1f / 3f) * Time.deltaTime;

            yield return null;
        }

        resultCanvasGroup.alpha = 0;
    }

}
