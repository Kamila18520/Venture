using UnityEngine;

public class ButtonOnClickMusic : MonoBehaviour
{

    [SerializeField] private AudioSource _AudioSource;

    public void PlayClickSound()
    {
      
        if (!_AudioSource.isPlaying)
        {
            _AudioSource.Play();
        }

    }

}
