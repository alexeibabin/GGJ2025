using System;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public GameObject livingArt;

    public GameObject deathArt;

    public void PlayDeathSequence()
    {
        livingArt.SetActive(false);
        deathArt.SetActive(true);
    }
}
