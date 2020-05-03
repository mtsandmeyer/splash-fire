using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string nextLevel;
    public Text screenText;
    public AudioClip winSFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(winSFX, transform.position);
            screenText.text = "You Win!";
            Invoke("LoadNextLevel", 2);
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }
}
