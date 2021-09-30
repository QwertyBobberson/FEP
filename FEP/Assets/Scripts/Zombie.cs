using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField] private NavMeshAgent pathFinding;
    [SerializeField] GameObject player;

    [SerializeField] float health;
    [SerializeField] int maxHealth;
    [SerializeField] float startingSpeed;
    public int Health
    {
        get
        {
            return (int)health;
		}
        set
        {
            float change = health - value;
            float percentHealth = (float)health / maxHealth;

            pathFinding.speed = startingSpeed * percentHealth;
            health -= change * percentHealth;

            if(health < .5f)
            {
                Destroy(pathFinding);
                Destroy(this);
			}
		}
	}

	private void Start()
	{
        System.Random rand = new System.Random();
        health = maxHealth;
        pathFinding.speed = startingSpeed;

        transform.position = new Vector3(rand.Next(-50, 50), 0, rand.Next(-50, 50));
	}

	void Update()
    {
        pathFinding.SetDestination(player.transform.position);
	}
}
