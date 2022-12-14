using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

enum EstadoJuego
{
    Empezar,
    PrepararJuego,
    Jugando,
    Perder,
    Ganar,
    Reiniciar
}

/**
    TODO:   En PrepararJuego, posicionar la cámara en el punto de inicio
            Agregar al enemigo el estado de "matoJugador" donde si en un frame en especifico el jugador sigue dentro de la colisión de hit, habrá asestado el golpe
            Agregar que se peuda perder por recibir un golpe o que se acabe el tiempo
*/

public class GameLogic : MonoBehaviour
{

    public GameObject spawnsEnemies;
    public List<GameObject> spawnList;
    public List<GameObject> enemies;

    public GameObject enemyPrefab;
    public GameObject personajeASeguir;
    private CharacterController controlCamara;

    public GameObject meta;
    private TriggerMainCharacter metaCollision;

    private EstadoJuego estadoActual;

    public TextMeshProUGUI textoIniciarJuego;
    public TextMeshProUGUI textoTemporizador;
    public TextMeshProUGUI textoVictoriaDerrota;
    public RawImage pantallaSangre;

    public Character controlPersonaje;
    public GameObject laberinto;

    private Vector3 posicionInicial;

    private float start;
    private float currTime;

    void Start()
    {
        /// Estado inicial del juego
        estadoActual = EstadoJuego.Empezar;

        /// Obtenemos la lista de posiciones de un objeto que guarda varias posiciones como hijos
        foreach (Transform child in spawnsEnemies.transform)
        {
            spawnList.Add(child.gameObject);
        }

        /// Obtenemos la referencia para detectar colisiones entre la meta y la cámara
        metaCollision = meta.GetComponent<TriggerMainCharacter>();
        metaCollision.mainCharacter = personajeASeguir.GetComponent<CharacterController>();

        /// Obtenemos la cadena para cambiar los textos dependiendo del estado
        textoIniciarJuego.text = "Para empezar presiona A";
        textoIniciarJuego.enabled = true;
        
        textoTemporizador.enabled = false;
        textoVictoriaDerrota.enabled = false;

        /// Obtenemos la posicion inicial de la cámara donde debe ser reiniciada
        posicionInicial = this.transform.Find("Punto de inicio").GetComponent<Transform>().position;

        /// Para reinciar la cámara es necesario hacerlo con el Character Controller
        controlCamara = personajeASeguir.GetComponent<CharacterController>();
    }

    private string getTimerString(float start, float currTime)
    {
        int segundosRestantes = Mathf.RoundToInt((2.0f * 60.0f - (currTime - start)));
        int fraccionMinutos = segundosRestantes / 60;
        int fraccionSegundos = segundosRestantes % 60;

        string tiempoRestante = fraccionMinutos.ToString() + ":"
                                + ((fraccionSegundos < 10) ? "0" : "") + fraccionSegundos.ToString();

        return tiempoRestante;
    }

    void Update()
    {
        
        switch (estadoActual)
        {
        case EstadoJuego.Empezar:

            /// Cambio de estado
            if (Input.GetButtonDown("A"))
            {
                estadoActual = EstadoJuego.PrepararJuego;
            }

            break;

        case EstadoJuego.PrepararJuego:

            /// Oculatamos los mensajes
            textoIniciarJuego.enabled = false;
            textoTemporizador.enabled = false;
            textoVictoriaDerrota.enabled = false;

            /// Eliminar zombies que sigan vivos
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                {
                    Destroy(enemy);
                }
            }

            enemies.Clear();

            /// Generar a todos los zombies en sus spawns
            foreach (GameObject spawn in spawnList)
            {
                GameObject enemy = Instantiate(enemyPrefab, spawn.transform.position, spawn.transform.rotation);
                enemy.transform.localScale = spawn.transform.localScale;
                Enemy enemyBehaviour = enemy.GetComponent<Enemy>();
                enemyBehaviour.personajeASeguir = personajeASeguir;

                enemies.Add(enemy);
            }

            /// Desactivamos el texto
            textoIniciarJuego.enabled = false;

            /// Habilitamos movimiento del personaje
            controlPersonaje.canMove = true;

            /// Empezamos a tomar el tiempo del temporizador
            start = Time.time;

            /// Activamos el texto del temporizador
            textoTemporizador.enabled = true;

            /// Cambiamos la posición de la cámara y reiniciamos su quaternion
            /// No preguntes, solo gózalo
            laberinto.SetActive(false);
            controlCamara.Move(posicionInicial - personajeASeguir.transform.position);
            controlCamara.Move(posicionInicial - personajeASeguir.transform.position);
            controlCamara.Move(posicionInicial - personajeASeguir.transform.position);
            controlCamara.Move(posicionInicial - personajeASeguir.transform.position);
            controlPersonaje.reiniciarCamara();
            laberinto.SetActive(true);

            /// Quitamos la sangre por si está activa
            pantallaSangre.enabled = false;

            /// Cambio de estado
            estadoActual = EstadoJuego.Jugando;

            break;

        case EstadoJuego.Jugando:

            currTime = Time.time;
            textoTemporizador.text = getTimerString(start, currTime);

            /// Cambio de estado
            /// Por un problema con la actualización de las colisiones me tengo que esperar
            /// un par de frames mas para que se actualice el estado)
            if (metaCollision.collision == true && start + 0.5f < currTime)
            {
                estadoActual = EstadoJuego.Ganar;
                break;
            }
            /// Despues de 2 minutos se habrá perdido
            else if (start + 120.0f < currTime)
            {
                estadoActual = EstadoJuego.Perder;
            }
            
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null && enemy.GetComponent<Enemy>().golpeAsestado == true)
                {
                    estadoActual = EstadoJuego.Perder;
                    pantallaSangre.enabled = true;
                }
            }

            break;

        case EstadoJuego.Perder:

            /// Desactivamos la movilización del personaje
            controlPersonaje.canMove = false;

            /// Mostramos un texto de victoria
            textoVictoriaDerrota.text = "No has podido escapar";
            textoVictoriaDerrota.enabled = true;

            /// Cambio de estado
            /// Despues de unos segundos preguntamos si quiere volver a jugar
            if (currTime + 3.0f < Time.time)
            {
                textoIniciarJuego.text = "Para volver a intentarlo, presiona A";
                textoIniciarJuego.enabled = true;

                estadoActual = EstadoJuego.Reiniciar;
            }

            break;

        case EstadoJuego.Ganar:

            /// Desactivamos la movilización del personaje
            controlPersonaje.canMove = false;

            /// Mostramos un texto de victoria
            textoVictoriaDerrota.text = "Has escapado";
            textoVictoriaDerrota.enabled = true;

            /// Cambio de estado
            /// Despues de unos segundos preguntamos si quiere volver a jugar
            if (currTime + 3.0f < Time.time)
            {
                textoIniciarJuego.text = "Para volver a jugar, presiona A";
                textoIniciarJuego.enabled = true;

                estadoActual = EstadoJuego.Reiniciar;
            }

            break;

        case EstadoJuego.Reiniciar:


            /// Cambio de estado
            if (Input.GetButtonDown("A"))
            {
                estadoActual = EstadoJuego.PrepararJuego;
            }

            break;
        }

    }
}
