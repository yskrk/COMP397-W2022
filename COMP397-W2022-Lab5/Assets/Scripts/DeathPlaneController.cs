using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlaneController : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Debug.Log("player collided with deathPlane");
            Respawn(other.gameObject);
        }
    }

    private void Respawn(GameObject player)
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = respawnPoint.position;
        player.GetComponent<CharacterController>().enabled = true;
    }
}
