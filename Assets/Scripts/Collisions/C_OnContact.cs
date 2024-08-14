using System;
using UnityEngine;

public class C_OnContact : MonoBehaviour
{
    public LayerMask mask;
    [SerializeField] private C_Results[] my_Results;

    public void SetLayerMask(int newMask) => mask = newMask;
    public int GetLayerMask() => mask;

    public C_Results[] OnContact() => my_Results;
}

[Serializable]
public struct C_Results
{
    public C_Types c_type;
    public int damage;
}
public enum C_Types { Damage, Death, Poison };
