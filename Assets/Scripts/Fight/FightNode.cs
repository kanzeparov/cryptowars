using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FightNode : MonoBehaviour
{
	public static FightNodeEvent onMouseOverEvent = new FightNodeEvent();
	public static FightNodeEvent onMouseExitEvent = new FightNodeEvent();
	public static List<FightNode> fightNodes = new List<FightNode>();

	[Header("Params")]
	public NodeOwner startNodeOwner;
	public int totalHealth;
	public float healthPerSecond;

	[Header("Refs")]
	public SpriteRenderer sr;
	public SpriteRenderer outerSR;
	public Text healthText;
	
	[HideInInspector] public int currentHealth;
	[HideInInspector] public NodeOwner currentNodeOwner;

	private void Awake()
	{
		onMouseOverEvent = new FightNodeEvent();
		currentHealth = totalHealth;
		currentNodeOwner = startNodeOwner;
		StartCoroutine(RegenerationCoroutine());
		Select(false);
		SetHealthUI();
		SetOwner();
		
		fightNodes.Add(this);
	}

	public void Select(bool active)
	{
		outerSR.gameObject.SetActive(active);
	}

	public int SendAttackGroup()
	{
		int attackUnitsCount = currentHealth - (currentHealth/2);
		currentHealth /= 2;
		return attackUnitsCount;
	}

	public void MergeAttackUnits(int damage, NodeOwner damageOwner)
	{
		if (damageOwner == currentNodeOwner)
		{
			currentHealth += damage;
			currentHealth = Mathf.Clamp(currentHealth, 0, totalHealth);
		}
		else
		{
			currentHealth -= damage;
			if (currentHealth == 0)
				currentNodeOwner = NodeOwner.Nobody;
			else if (currentHealth < 0)
			{
				currentNodeOwner = damageOwner;
				currentHealth = Mathf.Clamp(currentHealth, 0, totalHealth);
			}
		}
		SetOwner();
		SetHealthUI();
	} 

	private void SetOwner()
	{
		switch (currentNodeOwner)
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

	private void SetHealthUI()
	{
		healthText.text = currentHealth + "/" + totalHealth;
	}

	private IEnumerator RegenerationCoroutine()
	{
		float tickTime = 1 / healthPerSecond;
		while (true)
		{
			yield return new WaitForSeconds(tickTime);
			if (currentNodeOwner != NodeOwner.Nobody)
			{
				currentHealth++;
				currentHealth = Mathf.Clamp(currentHealth, 0, totalHealth);
				SetHealthUI();
			}
		}
	}

	private void OnMouseOver()
	{
		onMouseOverEvent.Invoke(this);
	}

	private void OnMouseExit()
	{
		onMouseExitEvent.Invoke(this);
	}
}

public class FightNodeEvent : UnityEvent<FightNode> {}
