using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CatVoiceManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] differentMeows;
    [SerializeField] private GameObject textAnswerCat;
    private AudioSource catVoice;
    

    private void Start()
    {
        catVoice = gameObject.GetComponent<AudioSource>();
        Debug.Log("Мяуконей в массиве: " + differentMeows.Length);

    }

    public void CatMeows()
    {
        AudioClip meow = differentMeows[Random.Range(0, 2)];
        catVoice.PlayOneShot(meow);
    }

    public void CatQuestionMeow()
    {
        AudioClip meow = differentMeows[Random.Range(2, differentMeows.Length)];
        if (meow == differentMeows[2])
        {
            textAnswerCat.GetComponent<MoveAnswerText>().StartMotion("Yes");
        }
        else if (meow == differentMeows[3])
        {
            textAnswerCat.GetComponent<MoveAnswerText>().StartMotion("No");
        }
        catVoice.PlayOneShot(meow);
    }
}