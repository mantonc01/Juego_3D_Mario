using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static int coinsNumber;

    private void Start()
    {
        
        coinsNumber++;
        Debug.Log("Ya estoy en la escena, moneda -> " + coinsNumber);

    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Algo ha colisionado conmigo");
        if (other.CompareTag("Player"))//si lo que colisiona conmigo es el jugador
        {
            GameManager.sharedInstance.CheckCoinsNumber();//miro a ver si es la ultima moneda
            Destroy(gameObject);
            Debug.Log("moneda destruida");
        }
    }
}
