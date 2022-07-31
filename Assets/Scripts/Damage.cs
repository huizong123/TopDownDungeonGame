using UnityEngine;

public struct Damage // Remove monobehavior as is not required and change class to struct
{
    public Vector3 origin; // The position of the origin of damage
    public int damageAmount; // How much damage transferred to enemy
    public float pushForce; // Push the receiver based on the push force and the origin


}
