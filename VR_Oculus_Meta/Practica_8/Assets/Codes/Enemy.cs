using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EstadoEnemigo
{
    Idle = 1,
    Run = 2,
    Attack = 3,
    Death = 4
}

public class Enemy : MonoBehaviour
{

    private Animator enemigoAnim;
    private EstadoEnemigo estadoActual;

    private Vector3 posicionInicial;

    private TriggerMainCharacter guardia;
    private TriggerMainCharacter persecucion;
    private TriggerMainCharacter golpe;
    private TriggerMainCharacter muerte;

    public GameObject personajeASeguir;

    private EnemyNavMesh pathFinder;

    private float tiempoMuerte = -1.0f;
    public float tiempoDesintrgrarCuerpo = 1.5f;

    public bool golpeAsestado = false;
    
    void Start()
    {
        /// Posición a la que debe volver en caso de perder al objetivo
        posicionInicial = this.transform.position;

        /// Obtenemos los componentes para detección de colisiones
        guardia = this.transform.Find("watch").GetComponent<TriggerMainCharacter>();
        guardia.mainCharacter = personajeASeguir.GetComponent<CharacterController>();

        persecucion = this.transform.Find("follow").GetComponent<TriggerMainCharacter>();
        persecucion.mainCharacter = personajeASeguir.GetComponent<CharacterController>();

        golpe = this.transform.Find("hit").GetComponent<TriggerMainCharacter>();
        golpe.mainCharacter = personajeASeguir.GetComponent<CharacterController>();

        muerte = this.transform.Find("death").GetComponent<TriggerMainCharacter>();

        /// Máquina de estados de las animaciones del zombie
        enemigoAnim = this.GetComponent<Animator>();

        /// Componente que implementa el pathfinding del zombie
        pathFinder = this.GetComponent<EnemyNavMesh>();

        /// Estado inicial del zombie (idle)
        estadoActual = EstadoEnemigo.Idle;
    }

    
    void Update()
    {

        float distanciaPosicionInicial = Vector3.Distance(this.transform.position, posicionInicial);

        switch (estadoActual)
        {
        case EstadoEnemigo.Idle:

            pathFinder.move = false;

            /// Cambio de estado
            if (muerte.collision == true)
            {
                estadoActual = EstadoEnemigo.Death;
            }
            else if (guardia.collision == true || distanciaPosicionInicial > 1.0f)
            {
                estadoActual = EstadoEnemigo.Run;
            }

            break;

        case EstadoEnemigo.Run:

            pathFinder.move = true;
            bool perseguir = guardia.collision || persecucion.collision;

            if (perseguir)
                pathFinder.movePosition = personajeASeguir.transform.position;
            else
                pathFinder.movePosition = posicionInicial;

            /// Cambio de estado
            if (muerte.collision == true)
            {
                estadoActual = EstadoEnemigo.Death;
            }
            else if (golpe.collision == true)
            {
                estadoActual = EstadoEnemigo.Attack;
            }
            else if (!perseguir && distanciaPosicionInicial < 0.5f)
            {
                estadoActual = EstadoEnemigo.Idle;
            }

            break;

        case EstadoEnemigo.Attack:

            pathFinder.move = false;

            float tiempoAnimacionCompleto = enemigoAnim.GetCurrentAnimatorStateInfo(0).normalizedTime;
            float porcentajeAnimacion = tiempoAnimacionCompleto - Mathf.Floor(tiempoAnimacionCompleto);

            bool reproduciendoAnimacionAtaque = enemigoAnim.GetCurrentAnimatorStateInfo(0).IsName("attack");

            golpeAsestado = false;

            /// Cambio de estado
            if (muerte.collision == true)
            {
                estadoActual = EstadoEnemigo.Death;
            }
            /// Si en el frame donde da el golpe el personaje principal sigue dentro del área de colisión,
            /// habrá asestado el golpe
            else if (reproduciendoAnimacionAtaque && porcentajeAnimacion > 0.38f && porcentajeAnimacion < 0.40f && golpe.collision == true)
            {
                golpeAsestado = true;
            }
            /// Cuando la animación este por terminar, cambiamos el estado
            else if (reproduciendoAnimacionAtaque && porcentajeAnimacion > 0.9f)
            {
                if (guardia.collision == false && persecucion.collision == false)
                    estadoActual = EstadoEnemigo.Idle;
                else
                    estadoActual = EstadoEnemigo.Run;
            }

            break;

        case EstadoEnemigo.Death:

            pathFinder.move = false;

            bool reproduciendoAnimacionMuerte = enemigoAnim.GetCurrentAnimatorStateInfo(0).IsName("death");
            float porcentajeAnimacionMuerte = enemigoAnim.GetCurrentAnimatorStateInfo(0).normalizedTime;
            porcentajeAnimacionMuerte = porcentajeAnimacionMuerte - Mathf.Floor(porcentajeAnimacionMuerte);

            /// Borrado del cuerpo
            if (tiempoMuerte < 0.0f)
            {
                if (reproduciendoAnimacionMuerte && (porcentajeAnimacionMuerte > 0.95))
                    tiempoMuerte = Time.time;
            }
            else if (tiempoMuerte + tiempoDesintrgrarCuerpo < Time.time)
                Destroy(gameObject);
            
            break;
        }

        enemigoAnim.SetInteger("state", (int) estadoActual);

    }
}
