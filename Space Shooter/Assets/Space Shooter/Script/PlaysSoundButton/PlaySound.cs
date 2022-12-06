using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaySound : MonoBehaviour
{
    public AudioClip AudioClipEnter;
    public AudioClip AudioClipExit;
    private AudioSource audioSource;

    //Do this when the cursor enters the rect area of this selectable UI object.
    public void OnPointerEnter()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        audioSource.clip = AudioClipEnter;

        audioSource.PlayOneShot(AudioClipEnter);
    }
    public void OnpointerExit()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        audioSource.clip = AudioClipEnter;

        audioSource.PlayOneShot(AudioClipExit);
    }
}
