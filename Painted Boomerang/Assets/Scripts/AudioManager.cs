using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    //Audio sources
    [Header("----------Sources----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    //Audio clips
    [Header("----------Clips----------")]
    //Music
    public AudioClip musicClip;

    //SFX
    public AudioClip spinClip;
    public AudioClip moveClip;
    public AudioClip hitClip;

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
