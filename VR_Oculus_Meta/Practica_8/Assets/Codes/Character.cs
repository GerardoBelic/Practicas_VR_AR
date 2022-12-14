using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterController personajeControl;
    public Vector3 velocidadPersonaje;
    public bool estaEnPiso;
    public float gravedad, aceleracionPersonaje, fuerzaSalto;

    private Transform sight;
    public GameObject bullet;

    public Vector3 mueve;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    public bool canMove = false;

    void Start()
    {
        sight = this.transform.Find("sight").transform;
    }

    void Update()
    {

        if (canMove)
        {
            muevePersonaje();
            mueveCamara();
            dispararArma();
        }
            
            
    }

    private void muevePersonaje()
    {
        /// Posicionamiento del personaje sobre el suelo
        estaEnPiso = personajeControl.isGrounded;
        if (estaEnPiso && velocidadPersonaje.y < 0)
            velocidadPersonaje.y = -0.01f;

        mueve = Vector3.zero;

        /// Movimiento del personaje
        mueve.x += Input.GetAxis("Horizontal") * Time.deltaTime * 2.0f;
        mueve.z += Input.GetAxis("Vertical") * Time.deltaTime * 2.0f;

        mueve.x += Input.GetAxis("LS_h") * Time.deltaTime * 2.0f;
        mueve.z += Input.GetAxis("LS_v") * Time.deltaTime * 2.0f;

        /// Rotamos el movimiento de acuerdo a donde miremos
        mueve = Quaternion.AngleAxis(yaw, Vector3.up) * mueve;

        mueve = Vector3.Normalize(mueve);

        /// Saltar
        if ((Input.GetButtonDown("Jump") || Input.GetButtonDown("A") ) && estaEnPiso)
            velocidadPersonaje.y += fuerzaSalto;

        /// Aplicar movimiento
        personajeControl.Move(mueve * Time.deltaTime * aceleracionPersonaje);

        velocidadPersonaje.y += gravedad * Time.deltaTime * Time.deltaTime;
        personajeControl.Move(velocidadPersonaje);

    }

    public void reiniciarCamara()
    {
        yaw = 0.0f;
        pitch = 0.0f;
    }

    private void mueveCamara()
    {
        /// Movimiento de la camara
        yaw += Input.GetAxis("RS_h") * Time.deltaTime * 150.0f;
        pitch += Input.GetAxis("RS_v") * Time.deltaTime * 150.0f;

        if (pitch > 90.0f)
            pitch = 89.9f;
        else if (pitch < -90.0f)
            pitch = -89.9f;
        
        Quaternion quat = Quaternion.Euler(pitch, yaw, 0.0f);
        this.transform.rotation = quat;
    }

    private float lastBulletFired = 0.0f;
    public float fireRate = 0.8f;

    private void dispararArma()
    {
        /// Disparar balas
        if ((Input.GetKeyDown(KeyCode.Q) == true || OVRInput.GetDown(OVRInput.Button.One) || Input.GetAxis("RT") > 0.5f) && (lastBulletFired + fireRate < Time.time))
        {
            GameObject currBullet = Instantiate(bullet, sight.position, sight.rotation);

            Rigidbody physicsBullet = currBullet.GetComponent<Rigidbody>();
            physicsBullet.AddForce(sight.forward * 10.0f, ForceMode.Impulse);

            lastBulletFired = Time.time;
        }
    }

}
