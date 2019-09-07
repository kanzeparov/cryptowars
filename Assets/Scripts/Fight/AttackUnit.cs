using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUnit : MonoBehaviour
{
    public float speed = 1f;
    public SpriteRenderer sr;
    private FightNode aimNode;
    private NodeOwner nodeOwner;

    public void SetAim(FightNode aim)
    {
        aimNode = aim;
        SetOwner();
    }

    private void Update()
    {
        if (aimNode == null)
            return;
        transform.position += (aimNode.transform.position - transform.position).normalized * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var fightNode = other.gameObject.GetComponent<FightNode>();
        if (fightNode == null)
            return;
        if (fightNode != aimNode)
            return;
        fightNode.MergeAttackUnits(1, nodeOwner);
        Destroy(gameObject);
    }
    
    private void SetOwner()
    {
        switch (nodeOwner)
        {
            case NodeOwner.Player:
                sr.color = new Color(159f/256f, 230f/256f, 62f/256f);
                break;
            case NodeOwner.Enemy:
                sr.color = new Color(248f/256f, 134f/256f, 152f/256f);
                break;
            case NodeOwner.Nobody:
                sr.color = new Color(219f/256f, 219f/256f, 219f/256f);
                break;
        }
    }
}
