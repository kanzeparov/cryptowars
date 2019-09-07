using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoSingleton<FightManager>
{   
    public ConnectionLine connectionLine;
    public AttackUnit attackUnitPrefab;

    public GameObject winMenu;
    
    private FightNode attackingNode;
    private FightNode attackedNode;
    
    private bool isClicking = false;
    private Vector2 mousePosition;

    private Camera mainCamera;
    private List<int> actionsSequence = new List<int>();
    
    private void Start()
    {
        FightNode.onMouseOverEvent.AddListener(SelectNode);
        FightNode.onMouseExitEvent.AddListener(ExitNode);
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!isClicking && Input.GetMouseButton(0))
        {
            isClicking = true;
            MouseButtonDown();
        } 
        else if (isClicking && !Input.GetMouseButton(0))
        {
            isClicking = false;
            MouseButtonUp();
        }

        FindMousePosition();
        SetConnection();  
        
        foreach (var fightNode in FightNode.fightNodes)
        {
            if (fightNode.currentNodeOwner != NodeOwner.Player)
                return;
        }
        Win();
    }

    private void SelectNode(FightNode fightNode)
    {   
        if (fightNode.currentNodeOwner == NodeOwner.Player && isClicking)
        {
            if (attackingNode == null)
            {
                fightNode.Select(true);
                attackingNode = fightNode;
            }
        }
        
        if (attackingNode != null && fightNode != attackingNode && isClicking)
        {
            if (attackedNode != null)
                attackedNode.Select(false);
            
            fightNode.Select(true);
            attackedNode = fightNode;
        }
    }

    private void ExitNode(FightNode fightNode)
    {
        if (attackedNode != null && attackedNode == fightNode)
        {
            attackedNode.Select(false);
            attackedNode = null;
        }
    }

    private void FindMousePosition()
    {
        Ray camRay = mainCamera.ScreenPointToRay(InputPosition());
        RaycastHit hit;
			
        if (Physics.Raycast(camRay, out hit, 10f))
        {
            mousePosition = hit.point;
        }			
    }

    private void SetConnection()
    {
        if (attackingNode == null)
        {
            connectionLine.gameObject.SetActive(false);
            return;
        }
        
        connectionLine.gameObject.SetActive(true);

        if (attackedNode == null)
        {
            connectionLine.SetPosition(attackingNode.transform.position, mousePosition,
                attackingNode.currentHealth * 0.001f);
            return;
        }
        
        connectionLine.SetPosition(attackingNode.transform.position, attackedNode.transform.position,
            attackingNode.currentHealth * 0.001f);
    }

    private Vector2 InputPosition()
    {
        return Input.mousePosition;
    }

    private void MouseButtonDown()
    {
        
    }
    
    private void MouseButtonUp()
    {
        if (attackingNode != null && attackedNode != null)
        {
            var attackUnitsCount = attackingNode.SendAttackGroup();

            for (int i = 0; i < attackUnitsCount; i++)
            {
                var spawnPos = attackingNode.transform.position + (Vector3) Random.insideUnitCircle;
                spawnPos.z = 0f;
                AttackUnit attackUnit = Instantiate(attackUnitPrefab, spawnPos, Quaternion.identity);
                attackUnit.SetAim(attackedNode);
            }

            int attackingIndex = 0;
            int attackedIndex = 0;

            for (int i = 0; i < FightNode.fightNodes.Count; i++)
            {
                if (attackingNode == FightNode.fightNodes[i])
                    attackingIndex = i;
                if (attackedNode == FightNode.fightNodes[i])
                    attackedIndex = i;
            }
            
            actionsSequence.Add(attackingIndex);
            actionsSequence.Add(attackedIndex);
        }
        
        if (attackingNode != null)
        {
            attackingNode.Select(false);
            attackingNode = null;
        }
        
        if (attackedNode != null)
        {
            attackedNode.Select(false);
            attackedNode = null;
        }
        
        connectionLine.gameObject.SetActive(false);
    }

    private void Win()
    {
        winMenu.SetActive(true);
    }

}
