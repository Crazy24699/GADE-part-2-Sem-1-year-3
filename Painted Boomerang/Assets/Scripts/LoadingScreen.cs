using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{

    public GameObject LoadingPanel;

    public int MaxLoadingTime;
    public float CurrentTime;

    public Slider LoadingSlider;

    // Start is called before the first frame update
    void Start()
    {
        CurrentTime = MaxLoadingTime;
        RunTimer();
    }

    public void SetLoadingTime()
    {
        CurrentTime = MaxLoadingTime;
        LoadingSlider.minValue = 0;
        LoadingSlider.maxValue = MaxLoadingTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void RunTimer()
    {
        await LoadingTimer();
    }

    public async Task LoadingTimer()
    {
        while (CurrentTime > 0 && Application.isPlaying) 
        {
            await Task.Delay(750);
            LoadingSlider.minValue = 0;
            LoadingSlider.maxValue = MaxLoadingTime;
            CurrentTime -= 1;
            LoadingSlider.value = CurrentTime;
            //Debug.Log(CurrentTime);
        }
        if(CurrentTime <= 0)
        {
            CurrentTime = 0;
            LoadingSlider.enabled = false;
            LoadingPanel.SetActive(false);
            this.GetComponent<LoadingScreen>().enabled = false ;
            //LoadingPanel.SetActive(false);
        }
    }
}
