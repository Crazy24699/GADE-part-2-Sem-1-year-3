using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{

    public GameObject LoadingPanel;

    public int MaxLoadingTime;
    public float CurrentTime;

    // Start is called before the first frame update
    void Start()
    {
        CurrentTime = MaxLoadingTime;
        RunTimer();
    }

    public void SetLoadingTime()
    {
        CurrentTime = MaxLoadingTime;
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
            CurrentTime -= 1;
            //Debug.Log(CurrentTime);
        }
        if(CurrentTime <= 0)
        {
            CurrentTime = 0;
            //LoadingPanel.SetActive(false);
        }
    }
}
