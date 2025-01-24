using System;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public GameObject livingArt;

    public GameObject deathArt;

    private void OnEnable()
    {
        livingArt.SetActive(false);
        deathArt.SetActive(true);
    }
}
