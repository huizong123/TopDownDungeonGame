using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Fighter // Inherit from fighter script
{
    protected override void Death()
    {
        Destroy(gameObject);
    }
}
