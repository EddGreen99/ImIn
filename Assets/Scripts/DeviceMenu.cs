using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeviceMenu : MonoBehaviour
{
    static bool menuOpen = false;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI interactButtonText;
    [SerializeField] private string connect, disconnect;
    private static string countdownNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //closeButton.onClick.AddListener(CloseMenu);
        if (GameManager.isCounting)
        {
            float f = gameManager.getCurrentDownloadTime();
            interactButtonText.text = f.ToString();
        }
        
    }

     public void OpenMenu()
    {
        menuUI.SetActive(true);
    }

    public void CloseMenu()
    {
        Debug.Log("Closing window" + this.name);
        menuUI.SetActive(false);
    }

    public void SetupMenu(Node node)
    {

    }

    public static void updateCountdown()
    {

    }

}
