using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float limitTime = 60f;

    private float countdown = 0f;
    
    public static Timer sharedInstance;

    public bool startCountDown = false;
    
    private void Awake()
    {
        sharedInstance = this;
    }

    public void StartTimer()
    {
        countdown = limitTime;
        startCountDown = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        countdown = limitTime;

    }

    // Update is called once per frame
    void Update()
    {
        if (startCountDown)
        {
            countdown -= Time.deltaTime;
                    //Debug.Log("Cuenta atras, " + countdown);
                    if (countdown<=0)
                    {
                        startCountDown = false;
                        GameManager.sharedInstance.GameOver();

                    }
        }
        
    }
}
