using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public int timeToDestroy = 5;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
