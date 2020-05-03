using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileShooter : MonoBehaviour
{
    public RawImage reticleImage;
    public Color hitColor;
    private Color defaultColor;
    public GameObject projectilePrefab;

    private void Start()
    {
        defaultColor = reticleImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        ReticleEffect();

        if (Input.GetMouseButtonDown(0))
        {
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        GameObject projectile;
        projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        projectile.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * 20);
    }


    void ReticleEffect()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {

            if (hit.collider.CompareTag("Prop") || hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("HardEnemy"))
            {
                reticleImage.color = hitColor;
            }
            else
            {
                reticleImage.color = defaultColor;
            }
        }
    }
}
