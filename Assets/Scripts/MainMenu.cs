using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string recentBest;
    [SerializeField] private TextMeshProUGUI recentBestText;
    private int score;
    void Start()
    {
        score = GameManager.GetScore();
        if(score > 0)
        {
            recentBestText.text = recentBest + score;
            recentBestText.gameObject.SetActive(true);
        }
        else
        {
            recentBestText.gameObject.SetActive(false);
        }
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Im In");
        Debug.Log("Switching to main scene");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
    


}
