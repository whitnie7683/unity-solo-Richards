using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyController : MonoBehaviour
{
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = GameObject.Find("Player").transform.position;
    }
}
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public enum State { Patrol, Chase, Attack, Search }

    [Header("Refrences")]
    public Transform[] patrolPoints;
    public Transform player;
    NavMeshAgent agent;

    [Header("Perception")]
    public float veiwRadius = 10f;
    [Range(0, 390)] public float veiwAngle = 110f;
    public LayerMask obstacleMask;
    public LayerMask playerMask;
    public float hearingRadius = 6f;

    [Header("Combat")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public int damage = 10;

    [Header("search")]
    public float searchDuration = 5f;
    public float memoryTime = 5f; // how Long to remember Last seen pos

    [Header("Tuning")]
    public float patrolSpeed = 3.0f;
    public float chaseSpeed = 2.5f;
    public float stoppingDistance = 3.0f;

    public float aggroRange = 10f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if the player is within "aggro" range
        if (distanceToPlayer <= aggroRange)
        {
            // Search behavior: Set the player as the destination
            object p = agent.SetDestination(player.position);

            // Check if the player is within "attack" range
            if (distanceToPlayer <= attackRange)
            {
                // Combat behavior: Attack the player
                Attack();
            }
        }
    }

    void Attack()
    {
        // Implement the enemy's attack logic here
        Debug.Log("Enemy is attacking!");
        // Example: Trigger an attack animation, play a sound, deal damage to the player
    }
}
public class Actor : MonoBehaviour
{
    public float maxHealth = 10f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        throw new NotImplementedException();
    }
}
public class PlayerCombat : MonoBehaviour
{
    public float attackRange = 2f;
    public float damage = 10f;
    public LayerMask enemyLayer; // Assign the "Enemy" layer in the Inspector
    public Animator animator; // Reference the Animator component

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Detect left mouse click
        {
            Attack();
        }
    }

    void Attack()
    {
        // Play attack animation
        animator.SetTrigger("Attack");

        // Raycast from the camera or weapon point
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, attackRange, enemyLayer))
        {
            // If the raycast hits an object on the enemy layer
            Actor enemy = hit.transform.GetComponent<Actor>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}

