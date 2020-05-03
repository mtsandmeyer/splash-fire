using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth = 100f;
    public AudioClip deadSFX;
    public Slider healthSlider;
    // Tells if this player is currently hurting, and cannot be hurt immediately again.
    private bool hurting = false;
    private bool isDead = false;
    private int score = 0;

    public Text screenText;
    public Text scoreText;
    public string thisScene;

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmount;
        }

        if (currentHealth <= 0)
        {
            PlayerDies();
        }

        healthSlider.value = currentHealth / 100f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hurting)
        {
            return;
        }

        if (other.CompareTag("Enemy"))
        {
            TakeDamage(10);
            StartCoroutine(PlayerHurting());
        }
        if (other.CompareTag("HardEnemy"))
        {
            TakeDamage(20);
            StartCoroutine(PlayerHurting());
        }
    }

    private void Update()
    {
        if (transform.position.y < -15)
        {
            PlayerDies();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (hurting)
        {
            return;
        }

        if (other.CompareTag("Enemy"))
        {
            TakeDamage(10);
            StartCoroutine(PlayerHurting());
        }
        if (other.CompareTag("HardEnemy"))
        {
            TakeDamage(20);
            StartCoroutine(PlayerHurting());
        }
    }

    // Player can only be damaged every 0.5 seconds
    IEnumerator PlayerHurting()
    {
        hurting = true;
        yield return new WaitForSeconds(0.2f);
        hurting = false;
    }

    public void TickUpScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    void PlayerDies()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        GetComponent<PlayerMovement>().FreezePlayer();
        screenText.text = "Game Over!";
        Invoke("ReloadScene", 2);

        healthSlider.value = 0f;
        AudioSource.PlayClipAtPoint(deadSFX, transform.position);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(thisScene);
    }
}
