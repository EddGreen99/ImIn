using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Connector : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Transform[] points;
    private List<Node> nodes = new List<Node>();
    [SerializeField] private int pointCount;
    [SerializeField] public static Node node1test, node2test;
    [SerializeField] Vector3 x, y;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.positionCount = pointCount;
        Debug.Log(this.name + " started");
        //this.setup(node1test, node2test);
        
    }

    public void setup(Node node1, Node node2)
    {
        this.gameObject.SetActive(false);
        Debug.Log("Starting setup");
        lineRenderer = this.GetComponent<LineRenderer>();
        Debug.Log("Linerenderer" + lineRenderer + " length of " + lineRenderer.positionCount);
        x = node1.GetComponent<Transform>().position;
        y = node2.GetComponent<Transform>().position;

        List<Vector3> positions = new List<Vector3>();
        positions.Add(node1.GetComponent<Transform>().position);
        positions.Add(node2.GetComponent<Transform>().position);
        //lineRenderer.SetPositions(positions.ToArray());

        //Debug.Log("Node 1 pos: " + x);
        //Debug.Log("Node 2 pos: " + y);

        //lineRenderer.SetPosition(0, new Vector3(6, 9));
        //lineRenderer.SetPosition(1, new Vector3(9, 6));

        try
        { 
            lineRenderer.SetPosition(0, node1.GetComponent<Transform>().position);
            lineRenderer.SetPosition(1, node2.GetComponent<Transform>().position);
            Debug.Log("Setup " + this.name + " with " + node1.name + " and " + node2.name);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void Reveal()
    {
        this.gameObject.SetActive(true);
        foreach(Node node in nodes)
        {
            node.gameObject.SetActive(true);
        }

        
    }

    public void AddNodes(List<Node> n)
    {
        foreach(Node node in n)
        {
            nodes.Add(node);
        }
        
    }
}
