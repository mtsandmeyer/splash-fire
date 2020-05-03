using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootProjectile : MonoBehaviour
{
    public Image reticleImage;
    public Color reticleColor;
    // Start is called before the first frame update
    void Start()
    {
        reticleColor = reticleImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        ReticleEffect();
    }


    void ReticleEffect()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Dementor"))
            {
                reticleImage.color = new Color(reticleColor.r, reticleColor.g, reticleColor.b, 1f);
                reticleImage.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
            }
            else
            {
                reticleImage.color = Color.Lerp(reticleImage.color, reticleColor, 2* Time.deltaTime);
                reticleImage.transform.localScale = Vector3.Lerp(reticleImage.transform.localScale, new Vector3(0.8f, 0.8f, 1f), Time.deltaTime);
            }
        }
    }
}
