﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CyberSoldier : Enemy
{


    public float stopDistance, attackRange, attackRate, nextAttack;
    private Vector3 startPosition;
    public int damage;
    public Transform attackPoint;
    public LayerMask playerLayer;


    private bool isMoving;
    private Animator animator;
    private NavMeshAgent agent;




    private void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;
        health = 300 * levelingSystem.enemiesCurrentLevel / 2;
        damage = 20 * levelingSystem.enemiesCurrentLevel / 2;

    }

    private void FixedUpdate()
    {
        ChasePlayer(player);
        AnimateCyberSoldier();
    }

    private void ChasePlayer(PlayerController target)
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        float startPointDistance = Vector3.Distance(transform.position, startPosition);

        if (distance < lookRadius)
        {
            FaceTarget(player);
            isMoving = true;
            agent.SetDestination(target.transform.position);
        }
        if (distance <= stopDistance)
        {
            agent.SetDestination(transform.position);
            isMoving = false;
            Attack();
        }
        if (distance > abandonDistance)
        {
            agent.SetDestination(startPosition);
            transform.LookAt(startPosition);
            if (startPointDistance < 50)
            {
                isMoving = false;
            }
        }
    }


    void AnimateCyberSoldier()
    {
        animator.SetBool("isRunning", isMoving);
    }

    void Attack()
    {
        if (Time.time > nextAttack)
        {
            animator.Play("Punch");
            nextAttack = Time.time + attackRate;
            // Detect player in range of attack
            Collider[] hitSphere = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);
            //Damage Player
            foreach (Collider playerBody in hitSphere)
            {
                player.TakeDamage(damage);            
            }
        }
    }

}
