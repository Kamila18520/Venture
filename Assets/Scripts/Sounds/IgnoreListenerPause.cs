using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreListenerPause : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    // Muzyka bêdzie gra³a mimo pauzy gry
    private void Start()
    {
        music = GetComponent<AudioSource>();
        if (music != null)
        {
            music.ignoreListenerPause = true; 
        }
    }
}
