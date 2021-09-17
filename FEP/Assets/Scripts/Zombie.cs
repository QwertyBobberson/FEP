using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField] private NavMeshAgent pathFinding;
    [SerializeField] GameObject player;

    [SerializeField] int health;
    [SerializeField] int maxHealth;
    [SerializeField] float startingSpeed;
    public int Health
    {
        get
        {
            return health;
		}
        set
        {
            int change = health - value;
            float percentBone = (float)health / maxHealth;



            pathFinding.speed = startingSpeed * percentBone;
		}
	}

	private void Start()
	{
        health = maxHealth;
        pathFinding.speed = startingSpeed;
	}

	void Update()
    {
        pathFinding.SetDestination(player.transform.position);
	}
}
