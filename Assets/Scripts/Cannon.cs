using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{   
    Turret parentTurret;

    // Start is called before the first frame update
    void Start()
    {
        parentTurret = GetComponentInParent<Turret>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, parentTurret.GetRotation());
    }
}
