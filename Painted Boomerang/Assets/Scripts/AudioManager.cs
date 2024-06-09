using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    //Audio sources
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    //Audio clips
    //Music
    public AudioClip musicClip;

    //SFX



    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
