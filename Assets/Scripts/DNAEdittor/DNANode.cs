using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DNANode : MonoBehaviour
{
    public static DNANodeEvent onMouseOver;
    public static DNANodeEvent onMouseExit;

    private void Awake()
    {
        if (onMouseOver == null)
            onMouseOver = new DNANodeEvent();
        if (onMouseExit == null)
            onMouseExit = new DNANodeEvent();
    }

    private void OnMouseOver()
    {
        onMouseOver.Invoke(this);
    }

    private void OnMouseExit()
    {
        onMouseExit.Invoke(this);
    }
}

public class DNANodeEvent : UnityEvent<DNANode> { }
