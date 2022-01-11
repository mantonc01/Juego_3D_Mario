using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public enum GameState
{
    menu,
    inTheGame,
    gameOver
}
public class GameManager : MonoBehaviour
{
    public Transform CoinsInGame;

    public static GameManager sharedInstance;

    public GameState currentGameState = GameState.menu;

    private void Awake()
    {
        sharedInstance = this;
    }

    public void CheckCoinsNumber()
    {
        Debug.Log("Bloques: " + CoinsInGame.childCount);
        if (CoinsInGame.childCount==1)//pongo a 1 ya que está a punto de desaparecer
        {
            GameOver();
        }
    }
    
    // Start is called before the first frame update
    public void StartGame()
    {
        Timer.sharedInstance.StartTimer();
        ChangeStateGame(GameState.inTheGame);
    }

    public void GameOver()
    {
        ChangeStateGame(GameState.gameOver);
    }

    public void BackToMainMenu()
    {
        ChangeStateGame(GameState.menu);
    }

    void Start()
    {
        GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = false;
        currentGameState = GameState.menu;
    }

    void ChangeStateGame(GameState newGameState)
    {
        if (newGameState==GameState.menu)
        {
            //Se muestra el menú principal
        }
        else
        {
            if (newGameState==GameState.inTheGame)
            {
                //configuramos la acción de juego
                Debug.Log("Inicia el juego!!");
                GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = true;
                currentGameState = GameState.inTheGame;
                Timer.sharedInstance.StartTimer();
            }
            else
            {
                if (newGameState==GameState.gameOver)
                {
                    //se muestra la pantalla de gameOver
                    currentGameState = GameState.gameOver;
                    GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = false;
                    if (Timer.sharedInstance.startCountDown)
                    {
                        Timer.sharedInstance.startCountDown = false;//paro la cuenta
                        Debug.Log("enhorabuena has conseguido recoger todas la monedas");
                        GameObject[] fireworks = GameObject.FindGameObjectsWithTag("Fireworks");//meto en un array todos los firewords encontrados
                        foreach (var fire in fireworks)
                        {
                            fire.GetComponent<ParticleSystem>().Play();//les doy al play para ver los fuegos artificiales
                        }
                    }
                    else
                    {
                        Debug.Log("lo siento, no has conseguido recoger todas la monedas el tiempo terminó");
                    }
                }
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (currentGameState==GameState.menu)
            {
                StartGame();
            }
            if (currentGameState==GameState.gameOver)
            {
                SceneManager.LoadScene("SampleScene");
            }
            
        }
    }
}
