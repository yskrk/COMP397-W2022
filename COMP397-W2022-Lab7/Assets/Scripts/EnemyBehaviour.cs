using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerBehabiour>().gameObject.transform;
        agent = GetComponent<NavMeshAgent>();
        InvokeRepeating(nameof(FindPlayer), 0.2f, 0.1f);
    }

    // // Update is called once per frame
    // void Update()
    // {

    // }

    public void FindPlayer()
    {
        agent.destination = player.position;
    }
}
