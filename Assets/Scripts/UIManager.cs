using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static Node node;
    [SerializeField] public GameObject deviceMenu;
    [SerializeField] private string cameraTitle, computerTitle, serverTitle;
    [SerializeField] private string cameraDescription, computerDescription, serverDescription; 
    [SerializeField] private TextMeshProUGUI deviceTitleText, deviceDescriptionText, interactButtonText, connectButtonText;
    [SerializeField] private string connect, malfunction, beginDownload;
    [SerializeField] private Button connectButton, interactButton;
    public void SetupMenu(Node n)
    {
        node = n;
        Debug.Log("Setting up device menu to: " + node);
        Node.Devices device = node.GetDeviceType();
        Node.Teams team = node.GetTeam();
        bool isAjacent = node.IsAjacentToPlayer();
        if (device == Node.Devices.Camera)
        {
            SetupCamera();
        }
        else if (device == Node.Devices.Computer)
        {
            SetupComputer();
        }
        else if (device == Node.Devices.Server)
        {
            SetupServer(isAjacent);
        }

        if (team == Node.Teams.Player || !isAjacent)
        {
            Debug.Log("Node is not a valid target for connection (" + team + ", " + node.IsAjacentToPlayer() + ")");
            connectButton.interactable = false;
        }
        else
        {
            Debug.Log("Node is a valid target for connection");
            connectButton.interactable = true;
        }

        deviceMenu.gameObject.SetActive(true);
    }

    public void SetupCamera()
    {
        deviceTitleText.text = cameraTitle;
        deviceDescriptionText.text = cameraDescription;
        interactButtonText.text = malfunction;

    }

    public void SetupComputer()
    {
        deviceTitleText.text = computerTitle;
        deviceDescriptionText.text = computerDescription;
        interactButtonText.text = malfunction;
    }

    public void SetupServer(bool isAjacent)
    {
        deviceTitleText.text = serverTitle;
        deviceDescriptionText.text = serverDescription;
        interactButtonText.text = beginDownload;
        if (!isAjacent)
        {
            interactButton.interactable = false;
        }
        else
        {
            interactButton.interactable = true;
        }
    }

    public void Connect()
    {
        node.RevealNetwork();
        node.SetTeam(Node.Teams.Player);
        GameManager.IncrementTotalSuspicion();
    }

    public void Interact()
    {
        node.interact();
    }



}
