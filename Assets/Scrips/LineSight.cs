using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSight : MonoBehaviour
{
    public enum SightSensitivity//dos tipos de visibilidad de este enemigo
    {
        STRICT,//estricto, tiene al jugador dentro de su campo de visión (angulo y cercanía) y no hay obstaculos entre medias
        LOOSE//impreciso, tiene al jugador dentro de su ángulo de visión pero hoy obstáculos entre medias o está demasiado lejos.
    }
    //variabla para guardar el estado de la visibilidad en cada momento
    public SightSensitivity sensitivity = SightSensitivity.STRICT;
    //variable para guardar si podemos ver a nuestro objetivo
    public bool canSeeTarget = false;
    //campo de visión del enemigo: cuantos grados vamo a poder ver
    public float fieldOfView = 45f;
    //para saber donde estamos posicionados
    private Transform theTransform = null;
    //para saber donde está posicionado el jugador
    private Transform target = null;
    //para guardar la colocaión de los ojos
    public Transform eyePoint = null;
    //rango de vision:collider que marca el límite de mi campo de visión
    private SphereCollider theCollider = null;
    //la última localización donde fue visto el jugador
    public Vector3 lastKnowSight=Vector3.zero;

    private void Awake()//inicializamos las variables privadas
    {
        theTransform = GetComponent<Transform>();
        theCollider = GetComponent<SphereCollider>();//tengo que crear este componente en el inspector
        lastKnowSight = theTransform.position;//lo inicializamos a mi posición para que tenga un valor inicial
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
    }

    private bool InFieldOfView()//devolverá true o false en función de que el jugador se encuentre
    {                           //dentro del angulo de visión del enemigo
        //calculo el vector que empieza en el ojo del enemigo y acaba en la posición del jugador
        Vector3 directionToTheTarget = target.position - eyePoint.position;
        //calculo el ángulo entre la dirección donde miran los ojos y el vector diretionToTheTarget
        //si es igual o inferior a 45º entonces el jugador está dentro de mi ángulo de visión
        float angle = Vector3.Angle(eyePoint.forward, directionToTheTarget);
        if (angle<=fieldOfView)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool ClearLineOfSight()//devuelve true si tengo una visión limpia del jugador, sin obstáculos de por medio
    {   //para ello lanzaremos un raycast desde los ojos del enemigo a ver si se encuentra con el jugador
        //primero comprobaremos si el raycast se encuentra con un collider y, si es así, comprobaremos que sea el del player
        
        //calculo el vector que empieza en el ojo del enemigo y acaba en la posición del jugador
        //normalized sirve para nomrlizarla, para que tenga módulo 1 y sea má corta
        Vector3 directionToTheTargetNormalized = (target.position - eyePoint.position).normalized;
        RaycastHit raycastInfo;
        if (Physics.Raycast(eyePoint.position,directionToTheTargetNormalized,out raycastInfo,theCollider.radius))
        {
            //si choca el ray, que empieza en los ojo, hacia la dirección (normalizada) del jugador, 
            //guardo los datos del choque en raycastInfo si ovurre dentro de un radio determinado
            //como he pasado el parámetro raycastInfo por referencia (out), este será modificado dentro de la fución
            if (raycastInfo.transform.CompareTag("Player"))
            {
                //si el raycast ha chocado con el player
                return true;
            }
        }

        return false;
    }

    private void UpdateSight()//método al que llamaremos para actualizar la visióndel enemigo
    {
        switch (sensitivity)//vamos a programar lo que tiene que tener en cuenta en un caso o en otro
        {
            case SightSensitivity.STRICT://dentro de ángulo de visión y a la vez tener una línea clara de visión (sin obstáculos)
            {
                canSeeTarget = InFieldOfView() && ClearLineOfSight();
                break;
            }
            case SightSensitivity.LOOSE://dentro del ángulo de visión o tener línea clara sin obstáculos.
            {
                canSeeTarget = InFieldOfView() || ClearLineOfSight();
                break;
            }
        }
        
    }

    private void OnTriggerStay(Collider other)//método que se ejecutará cuando el player esté dentro del collider del enemigo
    {
        if (other.CompareTag("Player"))
        {
            UpdateSight();//el enemigo estará constantemente actualizando la visibilidad
            if (canSeeTarget)
            {

                lastKnowSight = target.position;//actualiza la última posición conocida del jugador
            }
        }
    }

    private void OnTriggerExit(Collider other)//método que se ejecutará cuando el player abandone el interior del collider del enemigo
    {
        if (other.CompareTag("Player"))
        {
            canSeeTarget = false;//porque ya no podremos ver al jugador
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
