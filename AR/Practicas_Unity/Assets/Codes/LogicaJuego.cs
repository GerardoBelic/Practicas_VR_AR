using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogicaJuego : MonoBehaviour
{

    public GameObject textoVictoriaODerrota;
    private TextMeshProUGUI texto;
    public GameObject botonEmpezar;
    public GameObject botonReiniciar;

    public GameObject[] niveles;
    private GameObject nivelActual;
    private int numNivel = 0;

    public bool empezar = false;
    public bool referenciaActiva = false;
    private bool referenciaActivaFirst = false;
    public bool reiniciar;

    public GameObject textoTemporizador;
    private TextMeshProUGUI temporizador; 
    private float start;
    private float currTime;
    private bool playing = false;

    // Start is called before the first frame update
    void Start()
    {
        texto = textoVictoriaODerrota.GetComponent<TextMeshProUGUI>();
        temporizador = textoTemporizador.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

        if (playing == true)
            currTime = Time.time;
        
        int segundosRestantes = Mathf.RoundToInt((5.0f * 60.0f - (currTime - start)));
        int fraccionMinutos = segundosRestantes / 60;
        int fraccionSegundos = segundosRestantes % 60;
        temporizador.text = fraccionMinutos.ToString() + ":" + ((fraccionSegundos < 10) ? "0" : "") + fraccionSegundos.ToString();

        if (referenciaActiva == true && referenciaActivaFirst == false)
        {
            botonEmpezar.SetActive(true);
            referenciaActiva = false;
            referenciaActivaFirst = true;
        }

        //Si se precionó el botón de empezar, lo escondemos e iniciamos el primer nivel
        if (empezar == true)
        {
            numNivel = 0;
            botonEmpezar.SetActive(false);
            textoVictoriaODerrota.SetActive(false);
            nivelActual = Instantiate(niveles[numNivel++], this.transform);
            empezar = false;
            start = Time.time;
            textoTemporizador.SetActive(true);
            playing = true;
        }

        if (reiniciar == true)
        {
            numNivel = 0;
            botonReiniciar.SetActive(false);
            textoVictoriaODerrota.SetActive(false);
            nivelActual = Instantiate(niveles[numNivel++], this.transform);
            reiniciar = false;
            start = Time.time;
            textoTemporizador.SetActive(true);
            playing = true;
        }

        if (nivelActual != null)
        {
            ControlNivel statusNivel = nivelActual.GetComponent<ControlNivel>();

            if (statusNivel.ganar == true)
            {

                Destroy(nivelActual.gameObject);

                if (numNivel == 1 || numNivel == 2)
                {
                    nivelActual = Instantiate(niveles[numNivel++], this.transform);
                }
                else if (numNivel == 3)
                {
                    botonEmpezar.SetActive(true);
                    texto.text = "¡Ganaste!";
                    textoVictoriaODerrota.SetActive(true);
                    playing = false;
                }
            }
            else if (statusNivel.perder == true || (currTime - start) > 5 * 60.0)
            {
                Destroy(nivelActual.gameObject);

                botonReiniciar.SetActive(true);
                texto.text = "Perdiste";
                textoVictoriaODerrota.SetActive(true);
                playing = false;
            }
        }
    }

    public void setReferenciaActiva(bool isActive)
    {
        referenciaActiva = isActive;
    }

    public void setEmpezar(bool start)
    {
        empezar = start;
    }

    public void setReiniciar(bool reset)
    {
        reiniciar = reset;
    }

}
