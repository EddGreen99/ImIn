using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject devicePanel;
    [SerializeField] private AI ai, activeAI;
    [SerializeField] private GridManager gridManager;
    [SerializeField] static private int totalSuspicion, suspicionThreshold = 10, suspicionIncrement = 1, downloadWait;
    [SerializeField] public float downloadTime, currentDownloadTime;
    public static bool isCounting = false;
    private static bool aiActive = false;
    private static int score;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Score: " + score);
        currentDownloadTime = downloadTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Suspicion at " + totalSuspicion + " / " + suspicionThreshold);

        if (totalSuspicion > suspicionThreshold && aiActive == false)
        {
            ReleaseAI();        
        }
        if (isCounting)
        {
            if(currentDownloadTime > 0)
            {
                float x = currentDownloadTime -= Time.deltaTime;
                if(x < 0) { x = 0; }
                currentDownloadTime = x;
            }
            else
            {
                isCounting = false;
                playerWin();
            }
            
        }
        
    }

    public float getCurrentDownloadTime()
    {
        return currentDownloadTime;
    }
    void ReleaseAI()
    {
        Debug.Log("Releasing the AI");
        aiActive = true;
        Node targetNode = gridManager.getTargetNode();
        activeAI = Instantiate(ai, targetNode.transform.position, Quaternion.identity) as AI;
    }

    public int GetTotalSuspicion()
    {
        return totalSuspicion;
    }

    static public void AddToTotalSuspicion(int num)
    {
        totalSuspicion += num;
    }

    static public void IncrementTotalSuspicion()
    {
        AddToTotalSuspicion(suspicionIncrement);
    }

    static private void resetSuspicion()
    {
        totalSuspicion = 0;
    }

    static public void playerWin()
    {
        Debug.Log("Player win :)");
        deleteAI();
        resetSuspicion();
        Application.LoadLevel(Application.loadedLevel);
        score++;
    }

    static public void playerLose()
    {
        deleteAI();
        resetSuspicion();
        Debug.Log("Player loss :(");
        SceneManager.LoadScene("Menu");
    }

    static private void deleteAI()
    {
        AI ai = (AI)FindObjectOfType<AI>();
        Destroy(ai);
        aiActive = false;
        
    }

    static public void WinCountDown()
    {
        isCounting = true;
    }

    static public int GetScore()
    {
        return score;
    }
}
