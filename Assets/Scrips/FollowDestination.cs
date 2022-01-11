using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.AI;

public class FollowDestination : MonoBehaviour
{
    private NavMeshAgent theAgent = null;//el componente agente
                                        // es quien realmente tiene
                                        // que buscar el destino

    public Transform destination; //el destino

    private void Awake()
    {
        //inicializamos el agente con mi componente NawMeshAgent
        theAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        theAgent.SetDestination(destination.position);//establecemos el destino del agente
    }
}
