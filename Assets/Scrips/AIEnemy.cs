using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
//using Random = System.Random;

using Random = UnityEngine.Random;//con esta clase si se puede usar Random.Range ---preguntar a Oscar---




public class AIEnemy : MonoBehaviour
{
    //declaramos los 3 estados por los que puede pasar el enemigo dentro de un enumerado
    public enum EnemyState
    {
        PATROL,//patrullando
        CHASE,//perseguir
        ATTACK//atacar
    }
    //[SerializeField] muestra en el inspector la variable privad curentState
    [SerializeField] private EnemyState _currentState = EnemyState.PATROL;//estado por defecto

    public EnemyState CurrentState//propiede para acceder _currentState mediante get y set
    {
        get
        {
            return _currentState;//para obtener el valor de la variable, o también se puede poner: get=> _currentState;
        }
        set
        {
            _currentState = value;//el valor que elijamos para modificar _currentState
            StopAllCoroutines();//para mos todos las corrutinas para luego decidir cual arrancar
            switch (value)
            {
                case EnemyState.PATROL:
                {
                    StartCoroutine(AIPatrol());
                    break;
                }
                case EnemyState.CHASE:
                {
                    StartCoroutine(AIChase());
                    break;
                }
                case EnemyState.ATTACK:
                {
                    StartCoroutine(AIAttack());
                    break;
                }
            }
        }
    }

    private LineSight TheLineSight;//objeto linea de visión de la clase que hicimos anteriormente. lan inicializaremos en el Awake
    private NavMeshAgent theAgent;//la inicializaremos en el Awake
    private Transform target;//la inicializaremos en el Awake
    [SerializeField] 
    private Transform destination;//El destino por donde patrullará el enemigo. la asignaremos en el Start
    //La he serializado y le he asignado el destination en Unity y funciona--comentar a Oscar
    private void Awake()
    {
        TheLineSight = GetComponent<LineSight>();
        theAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        CurrentState = EnemyState.PATROL;//por defecto,cuando arranque el script, el enemigo patrullará-- CAMBIO CHASE POR PATROL
        GameObject[] randomDestinations = GameObject.FindGameObjectsWithTag("Destination");
        //GameObject randomDestinations = GameObject.FindWithTag("Destination");
        
        destination = randomDestinations[Random.Range(0, randomDestinations.Length)].GetComponent<Transform>();
        //destination = randomDestinations.GetComponent<Transform>();
        //para usar Random.Range se necesita usar using Random = UnityEngine.Random; o using Random = System.Random;---preguntar a Oscar----
        //Debug.Log("destination "+destination.position.magnitude);
    }

    public IEnumerator AIPatrol()
    {
        //yield break;//de momento dejamos esta sentencia, sería para parar la corrutina.
        while (CurrentState==EnemyState.PATROL)
        {
            TheLineSight.sensitivity = LineSight.SightSensitivity.STRICT;
            theAgent.isStopped = false;//para que siga patrullando con sensibilidad estricta
            theAgent.SetDestination(destination.position);//hacia el destino que tenga asignado

            while (theAgent.pathPending==true)//mientras esperamos a que se calcule la ruta a seguir en la patrulla
            {
                yield return null;//pausamos la corrutina y esperamos al siguiente frame a ver si ya está calculado la ruta a seguir
            }

            if (TheLineSight.canSeeTarget)//si vemos al objetivo (al jugador)
            {
                theAgent.isStopped = false;//dejamos de patrullar
                CurrentState = EnemyState.CHASE;//y cambiamos el estado al de perseguir
                yield break;//finalizamos la corrutina AIPatrol
            }

            yield return null;//llego aquí cuando la ruta está calculada pero no he visto al objetivo
                                // por lo que pauso la corrutina hasta el siguiente frame
        }
        
    }

    public IEnumerator AIChase()
    {
        //yield break;//de momento dejamos esta sentencia, sería para parar la corrutina.
        while (CurrentState==EnemyState.CHASE)
        {
            TheLineSight.sensitivity = LineSight.SightSensitivity.LOOSE; //para que sea más dificil que pierda de vista al objetivo
            theAgent.isStopped = false;//para que siga persiguiendo con sensibilidad imprecisa
            theAgent.SetDestination(TheLineSight.lastKnowSight);//hacia el último sitio donde vió al objetivo
            while (theAgent.pathPending==true)//mientras esperamos a que se calcule la ruta a seguir hasta el objetivo
            {
                yield return null;//pausamos la corrutina y esperamos al siguiente frame a ver si ya está calculada la ruta
            }

            if (theAgent.remainingDistance <= theAgent.stoppingDistance)//si la distancia que falta es <= que la de parada(la pondremos en el inspector)
            {
                theAgent.isStopped = true;//dejamos de perseguir
                if (TheLineSight.canSeeTarget==false)//si hemos perdido de vista al objetivo
                {
                    CurrentState = EnemyState.PATROL;//volvemo al estado de patrullar
                }
                else//no lo he perdido de vista entonces ataco
                {
                    CurrentState = EnemyState.ATTACK;
                }
                yield break;//como he dado alcance al enemigo finaliza la corrutina
            }

            yield return null;//como no he dado alcance al enemigo, pauso la corrutina hasta el siguiente frame
        }
    }
    public IEnumerator AIAttack()
    {
        //yield break; //de momento dejamos esta sentencia, sería para parar la corrutina.
        while (CurrentState == EnemyState.ATTACK)
        {
            theAgent.isStopped = false;//para que theAgent continue avanzando
            theAgent.SetDestination(target.position);//hacia el objetivo
            while (theAgent.pathPending==true)//mientras esperamos a que se calcule la ruta a seguir hasta el objetivo
            {
                yield return null;//pausamos la corrutina y esperamos al siguiente frame a ver si ya está calculada la ruta

            }

            if (theAgent.remainingDistance > theAgent.stoppingDistance)//si estamos a una distancia superior a la de pararnos
            {
                theAgent.isStopped = true;//dejamos de atacar
                CurrentState = EnemyState.CHASE; //y volvemos al estado de persecución
                yield break;
            }
            else
            {
                //hacer daño al jugador
                Debug.Log("Daño");
            }

            yield return null;//pauso la corrutina hasta el siguiente frame
        }
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }
}
