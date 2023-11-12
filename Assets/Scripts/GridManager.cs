//original simple grid adapted from https://pastebin.com/cip74C4E

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height, minGridComplexity = 0, maxGridComplexity = 1, minRectSize = 0, nodeDensity, targets;

    private int connectorCount, tileCount, nodeCount;

    [SerializeField] private Tile tile;

    [SerializeField] private Connector connector;

    [SerializeField] private Node node, testNode1, testNode2, spawnedNode, previousNode, targetNode;

    [SerializeField] private Transform camera, spawnedNodeTransform, previosNodeTransform;

    [SerializeField] private GameObject loadingPanel;

    [SerializeField] public static bool isFirstNode = true;

    [SerializeField] private Dictionary<Vector2, Tile> tilesDict = new Dictionary<Vector2, Tile>();
    [SerializeField] private List<Tile> tilesList;

    [SerializeField] private Dictionary<Vector2, Node> nodesDict = new Dictionary<Vector2, Node>();
    [SerializeField] private List<Node> nodesList;

    [SerializeField] private List<Connector> connectors;

    void Start()
    {
        GenerateGrid();
        GenerateNodes();
        loadingPanel.SetActive(false);
    }

    void GenerateGrid()
    {
        tilesDict = new Dictionary<Vector2, Tile>();
        connectorCount = 0;
        previousNode = null;
        for (int i = 0; i < maxGridComplexity; i++)
        {
            //Creates a rectangle to create nodes in on the grid. Multiple rectangles make more complex game levels.

            Vector2 rectPoint = new Vector2(UnityEngine.Random.Range(0, width), UnityEngine.Random.Range(0, height));
            float rectWidth = UnityEngine.Random.Range(rectPoint.x + minRectSize, width);
            float rectHeight = UnityEngine.Random.Range(rectPoint.y + minRectSize, height);
            Rect rect = new Rect(rectPoint.x, rectPoint.y, rectWidth, rectHeight);


            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector2 xy = new Vector2(x, y);

                    //Creates a new tile if the iterated potential tile is within the rectancle and no tile exists from previous rectangles.
                    if (rect.Contains(xy) && !tilesDict.ContainsKey(xy)){
                        Tile spawnedTile = CreateGridSquare(x, y);
                        tilesDict[new Vector2(x, y)] = spawnedTile;
                        tilesList.Add(spawnedTile);
                        Debug.Log("Tile is: " + tilesDict[new Vector2(x, y)]);
                        tileCount++;

                    }
                    else
                    {
                        //skip creating this tile.
                        //It is either outside the rectangle or a tile already exists in this location.
                    }

                }
            }

        }


        camera.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
    }

    Node CreateNode(int x, int y, Node.Devices device)
    {

        previousNode = spawnedNode;
        spawnedNode = Instantiate(node, new Vector3(x, y), Quaternion.identity) as Node;
        spawnedNode.name = $"Node {nodeCount}";
        spawnedNode.SetDeviceType(device);

        Debug.Log("Created node " + spawnedNode);

        nodesDict[new Vector2(x, y)] = spawnedNode;
        nodesList.Add(spawnedNode);
        nodeCount++;
        return spawnedNode;
    }

    void GenerateNodes()
    {
        int nodesToCreate = Mathf.RoundToInt(tileCount / nodeDensity);
        List<Node> cameras = GenerateCameras(nodesToCreate / 2);
        List<Node> computers = GenerateComputers(nodesToCreate / 2);
        ConnectNodeGroups(cameras, computers);
        
        //Sets the latest node as the starting point.
        Node lastNode = nodesList[nodesList.Count - 1];
        lastNode.SetIsExit(true);
    }

    void GenerateTargets(int num)
    {

    }


    void ConnectNodeGroups(List<Node> nodeList1, List<Node> nodeList2)
        //Takes two lists of nodes and creates a random connector between two nodes, one from each list.
    {
        Node node1 = nodeList1[UnityEngine.Random.Range(0, nodeList1.Count)];
        Node node2 = nodeList2[UnityEngine.Random.Range(0, nodeList2.Count)];
        Debug.Log("Linking node groups between " + node1 + " and " + node2);
        CreateConnector(node1, node2);
    }
    List<Node> GenerateCameras(int num)
    {
        //Vector2 xy = new Vector2(UnityEngine.Random.Range(0, width), (UnityEngine.Random.Range(0, height)));
        var cameraNodes = new List<Node>();
        for (int i = 0; i < num; i++)
        {
            cameraNodes.Add(CreateNode(UnityEngine.Random.Range(0, width), UnityEngine.Random.Range(0, height), Node.Devices.Camera));
            if(i > 0)
            {
                CreateConnector(previousNode, spawnedNode);
            }

        }


        return cameraNodes;
    }

    List<Node> GenerateComputers(int num)
    {
        //Vector2 xy = new Vector2(UnityEngine.Random.Range(0, width), UnityEngine.Random.Range(0, height));

        //Creates num computers
        int interval = Mathf.RoundToInt(tileCount / num);
        var computerNodes = new List<Node>();
        for (int i = 0; i < tilesList.Count; i++)
        {
            if (i % interval == 0)
            {
                Tile tile = tilesList[i];
                Vector2 tilePos = tile.getPosition();
                if (i == 0)
                {
                    //Temporary way of hardcoding a single target
                    targetNode = CreateNode((int)tilePos.x, (int)tilePos.y, Node.Devices.Server);
                    computerNodes.Add(targetNode);
                }
                else
                {
                    computerNodes.Add(CreateNode((int)tilePos.x, (int)tilePos.y, Node.Devices.Computer));
                }
                
            }
        
        }

        //Creates network links
        int computerNodesCount = computerNodes.Count;
        for (int i = 1; i < computerNodesCount; i++)
        {
            //Creates a chain of links between every computer node
            CreateConnector(computerNodes[i - 1], computerNodes[i]);

            //Creates An extra random link for each computer
            int randomNodeIndex = UnityEngine.Random.Range(0, computerNodesCount);
            if(randomNodeIndex != i)
            {
                CreateConnector(computerNodes[i], computerNodes[randomNodeIndex]);
            }


        }

        return computerNodes;
    }

    Connector CreateConnector(Node node1, Node node2)
    {
        Connector spawnedConnector = Instantiate(connector);
        spawnedConnector.name = $"Connector {connectorCount}";
        Debug.Log("Connecter " + connectorCount + " instanteated");
        connectorCount++;

        //Debug.Log("Attempting to setup " + spawnedConnector.name + " with " + previousNode.name + " and " + spawnedNode.name);
        spawnedConnector.setup(node1, node2);
        node1.addConnectedNode(node2);
        node1.addConnectedConnector(spawnedConnector); 

        node2.addConnectedNode(node1);
        node2.addConnectedConnector(spawnedConnector);

        List<Node> tempNodes = new List<Node>();
        tempNodes.Add(node1);
        tempNodes.Add(node2);
        spawnedConnector.AddNodes(tempNodes);
        return spawnedConnector;
    }

    Tile CreateGridSquare(int x, int y)
    {
        var spawnedTile = Instantiate(tile, new Vector3(x, y), Quaternion.identity);
        spawnedTile.name = $"Tile {x} {y}";
        spawnedTile.setPosition(new Vector2(x, y));

        var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
        spawnedTile.Init(isOffset);

        return spawnedTile;
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (tilesDict.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    
    public List<Node> getNodes()
    {
        return nodesList;
    }
    
    public Node getTargetNode()
    {
        return targetNode;
    }
}