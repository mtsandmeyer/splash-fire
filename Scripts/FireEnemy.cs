using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemy : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public GameObject explosionParticles;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Invoke("Die", 10);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            player.GetComponent<PlayerHealth>().TickUpScore();
            Die();
        }
    }

    private void Die()
    {
        Instantiate(explosionParticles, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
