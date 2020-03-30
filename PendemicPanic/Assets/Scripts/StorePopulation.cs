using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorePopulation : MonoBehaviour
{
    public int population;
    private void OnCollisionEnter(Collision collision)
    {
        population++;
    }

    private void OnCollisionExit(Collision collision)
    {
        population--;
    }
}
