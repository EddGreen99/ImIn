using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    

    public enum Teams { Neutral, Player, Enemy };
    public enum Devices { Server, Camera, Computer, SecurityComputer, Printer };
    private Teams currentTeam = Teams.Neutral;
    private Devices device;
    [SerializeField] private int suspicion, suspicionIncrement, serverSuspicionIncrement;
    private Vector2 position;
    private bool isExit = false;
    private List<Node> connectedNodes = new List<Node>();
    private List<Connector> connectedConnectors = new List<Connector>();
    private UIManager uiManager;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private Connector connector;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Color NeutralColor, PlayerColor, EnemyColor, TargetColor, ExitColor;
    // Start is called before the first frame update
    void Start()
    {
        //this.gameObject.SetActive(false);
        Debug.Log("Node Generated");
        Physics.queriesHitTriggers = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = GameObject.Find("GameManager");
        uiManager = FindObjectOfType<UIManager>();
        /*
         if (GridManager.isFirstNode)
         {
             SetColor(TargetColor);
             GridManager.isFirstNode = false;
         }
         else
         {
             SetColor(NeutralColor);
         }
        */
        if (device == Devices.Server)
        {
            SetColor(TargetColor);
            
        }
        else if(isExit)
        {
            SetColor(ExitColor);
            this.gameObject.SetActive(true);
        }
        else
        {
            SetColor(NeutralColor);
            
        }


        List<Node> nodes = gridManager.getNodes();
        //Instantiate(connector, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
        //Transform[] points = new Transform[2];
       // points[0] = this.transform;
        //points[1] = nodes[Random.Range(0, nodes.Count)].transform;
        //connector.setup(points);

    }


    // This actually works on touch screens for whatever reason. I'm not complaining, saves me some raycasting.
    private void OnMouseDown()
    {
        Debug.Log("Node Touched");
        SelectNode();
    }

    public void addConnectedConnector(Connector connector)
    {
        connectedConnectors.Add(connector);
    }
    public void addConnectedNode(Node node)
    {
        Debug.Log("Adding node " + node + " to node list " + connectedNodes.Count);
        connectedNodes.Add(node);
    }
    private void SelectNode()
    {
        uiManager.SetupMenu(this);
    }

    public void SetTeam(Teams team)
    {
        currentTeam = team;
        if (team == Teams.Player)
        { 
            SetColor(PlayerColor);
        }
        else if(team == Teams.Enemy)
        {
            SetColor(EnemyColor);
        }
        else
        {   
            SetColor(NeutralColor);     
        }
    }

    public Teams GetTeam()
    {
        return currentTeam;
    }

    public void SetIsExit(bool b)
    {
        isExit = b;
        if(b == true)
        {
            SetTeam(Node.Teams.Player);
            this.gameObject.SetActive(true);
            SetColor(ExitColor);
            RevealNetwork();
        }
    }

    public void SetDeviceType(Devices d)
    {
        device = d;
    }

    public Devices GetDeviceType()
    {
        return device;
    }

    private void SetColor(Color color)
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); //Silly repeated line but it stops an edge case Nullpointerexception for now.
        Debug.Log("Setting color to " + color + " with " + spriteRenderer);
        spriteRenderer.color = color;
    }

    public void setPosition(Vector2 vector2)
    {
        position = vector2;
    }

    public void RevealNetwork()
    {
        
        foreach(Connector connector in connectedConnectors)
        {
            Debug.Log("Activating connector: " + connector);
            connector.gameObject.SetActive(true);
        }
        
        foreach(Node node in connectedNodes)
        {
            Debug.Log("Activating node: " + node);
            node.gameObject.SetActive(true);
            Debug.Log(node.gameObject.activeSelf);
        }
        IncreaseSuspicion();
    }

    public bool IsAjacentToPlayer()
    {
        foreach(Node node in connectedNodes)
        {
            Node.Teams connectedTeam = node.GetTeam();
            if(connectedTeam == Node.Teams.Player) { return true; }
        }
        return false;
    }

    public List<Node> getConnectedNodes()
    {
        return connectedNodes;
    }

    public void IncreaseSuspicion()
    {
        suspicion += suspicionIncrement;
        GameManager.AddToTotalSuspicion(suspicion);
    }

    public int getSuspicion()
    {
        return suspicion;
    }

    public void interact()
    {
        if(device == Devices.Server)
        {
            Debug.Log("Downloading from server: " + this.name);
            GameManager.AddToTotalSuspicion(serverSuspicionIncrement);
            GameManager.WinCountDown();
        }
        else
        {
            GameManager.AddToTotalSuspicion(suspicionIncrement);
        }
    }

    public bool getIsExit()
    {
        return isExit;
    }
}

