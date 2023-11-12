using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
    
{
    [SerializeField] private float transformSmoothness, transformSpeed, elapsedTime, moveDuration;
    [SerializeField] private List<Node> connectedNodes;
    [SerializeField] private Node node, previousNode;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Vector2 targetPos, currentPos;
    private bool hunting = false;

    // Start is called before the first frame update
    void Start()
    {
        
        gridManager = FindObjectOfType<GridManager>();
        node = gridManager.getTargetNode();
        targetPos = currentPos = transform.position;
        Debug.Log("AI started at node " + node);
    }

    void transformToPosition(Vector2 v2)
    {
        targetPos = v2;
        Debug.Log("Transforming to " + targetPos);
        currentPos = transform.position;
    }

    void Update()
    {
        if (hunting)
        {
            Debug.Log("AI moving towards " + targetPos + " from " + currentPos);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, transformSpeed * Time.deltaTime);
            if ((Vector2)transform.position == targetPos)
            {
                hunting = false;
                if(node.getIsExit())
                {
                    GameManager.playerLose();
                }
            }
        }
        else { huntExit(); };
        
    }

    private void huntExit()
    {
        Debug.Log("AI finding ajacent nodes to " + node);
        connectedNodes = node.getConnectedNodes();
        
        //node = null;
        int s = 0;
        foreach(Node n in connectedNodes)
        {
            if (n == previousNode)
            {
                //skip this node, the AI was here last time.
            }
            else if(n.getSuspicion() > s)
            {
                s = n.getSuspicion();
                node = n;
                hunting = true;
            }
            previousNode = node;
            Debug.Log("Hunting target: " + node);
            //transformToPosition(node.transform.position);
        }
        targetPos = node.transform.position;

    }
}
