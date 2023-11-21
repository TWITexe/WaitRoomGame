using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CatVoiceManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] differentMeows;
    private AudioSource catVoice;

    private void Start()
    {
        catVoice = gameObject.GetComponent<AudioSource>();
        Debug.Log("Мяуконей в массиве: " + differentMeows.Length);
    }

    public void CatMeows()
    {
        AudioClip meow = differentMeows[Random.Range(0, differentMeows.Length)];
        catVoice.PlayOneShot(meow);
    }
}