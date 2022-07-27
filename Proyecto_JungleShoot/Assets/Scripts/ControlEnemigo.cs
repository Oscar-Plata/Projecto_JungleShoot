using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEnemigo : MonoBehaviour, IDaño
{
    [Header("Comopnenetes")]
    private Rigidbody2D rb;

    private Animator an;

    private Disparar dp;

    private SpriteRenderer sr;

    private Collider2D cld;

    public Transform mira;

    [Header("Movimiento")]
    public Vector3 Velocidades;

    public int direccion;

    public float velocidadMovimiento;

    private float VelMovRnd;

    public bool puedeMoverse;

    [Header("Tiempos")]
    public float tiempoEspera;

    public float tiempoDisparo;

    [Header("Detectores")]
    public GameObject detector;

    public DetectarPlayer jugFrente;

    public DetectarPlayer jugEspalda;

    public Transform ojos;

    public LayerMask layerStage; //Capa suelo a ala cual se colisionara

    public float radioColision = 0.5f;

    public bool pared;

    // public DetectarPlayer balaEspalda;
    public bool espalda = false;

    [Header("Estado")]
    public bool vigilando; //Estado 0

    public bool patruyando; //Estado 1

    public bool atacando; //Estado 2

    public bool yaDisparo;

    public bool muerto; //Estado 3

    [Header("Vida")]
    public float vidas;

    public Color colorHit;

    private Color colorOrg;

    public bool invencible = false;

    public bool golpeado = false;

    public float tiempoInvencible = 0.5f;

    public Vector2 fuerzaGolpeado;

    public float tiempoGolpeado = .3f;

    private void Awake() //Obtener componentes inicales
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        dp = GetComponent<Disparar>();
        jugFrente = detector.GetComponent<DetectarPlayer>();
        sr = GetComponent<SpriteRenderer>();
        cld = gameObject.GetComponent<CapsuleCollider2D>();
    }

    void Start()
    {
        colorOrg = sr.color; //guardar color original
        CambiarEstado(); //cambiarEstado (Patruyar/Vigilar)
    }

    // Update is called once per frame
    void Update()
    {
        //moverse si esta patruyando
        if (patruyando) rb.velocity = new Vector2(VelMovRnd * direccion, rb.velocity.y);

        //detectar jugador por frente o espalda
        atacando = jugFrente.detectado;
        espalda = jugEspalda.detectado;
        if (espalda && !atacando || DetectarPared()) cambiarDireccion(); //voltear al player si esta por la espalda
        puedeSerDañado();

        //Empezar el comportamiento de ataque
        if (atacando && !muerto)
        {
            vigilando = false;
            patruyando = false;
            StartCoroutine("DispararAPlayer");
        }
        Velocidades = rb.velocity; //ver velocidades en el inspector
    }

    private bool
    DetectarPared() //Detectar si choca con escenario para cambiar de posicion
    {
        bool check = Physics2D.OverlapCircle(ojos.position, radioColision, layerStage);
        return check;
    }

    private void CambiarEstado() //Cambiar comportamiento al azar
    {
        if (!atacando && !muerto)
        {
            int selector = 0;
            selector = Random.Range(0, 2);

            //Debug.Log("Estado: " + selector);
            cambiarDireccion();
            switch (selector)
            {
                case 0:
                    // Debug.Log("Vigilando");
                    vigilando = true;
                    patruyando = false;
                    an.SetBool("Caminar", false);
                    break;
                case 1:
                    // Debug.Log("Patruyando");
                    patruyando = true;
                    vigilando = false;
                    an.SetBool("Caminar", true);
                    VelMovRnd = velocidadMovimiento + Random.Range(-1f, +1f);
                    break;
            }
            StartCoroutine("tiempoCambio"); //volver a cambiar despues de un tiempo
        }
    }

    private IEnumerator tiempoCambio()
    {
        yield return new WaitForSeconds(tiempoEspera + Random.Range(-5f, +5f));
        CambiarEstado();
    }

    private void cambiarDireccion()
    {
        bool valor = Random.value > 0.5 ? true : false; //cambiar dirreccion al azar
        if (valor)
            direccion = 1; // Va a la derecha
        else
            direccion = -1; // Va a la izquierda
        transform.localScale = new Vector3(direccion, transform.localScale.y, transform.localScale.z);
    }

    private IEnumerator DispararAPlayer()
    {
        if (!yaDisparo && !muerto)
        {
            yaDisparo = true; //booleana para controlar el disparo
            dp.dirreccion = new Vector2(direccion, 0);
            yield return new WaitForSeconds(Random.Range(0.01f, +0.5f)); //Delay para disparar
            dp.Disparo(); //disparar
            yield return new WaitForSeconds(tiempoDisparo + Random.Range(-0.2f, +0.2f)); //delay entre disparos
            yaDisparo = false;
            an.SetBool("Disparar", false); //Terminar estado de disparo
            if (!atacando) CambiarEstado(); //cambiar comportamiento al dejar de atacar
        }
    }

    public void RecibirDaño(float cantidad)
    {
        if (!muerto && !invencible)
        {
            vidas -= (int) cantidad; //quitar vida
            StartCoroutine("serInvencible"); //ser invencible por unos segundos
            StartCoroutine("serGolpeado"); //comportamiento de ser golpeado

            //knockback al ser golpeado
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(direccion * -1 * fuerzaGolpeado.x, fuerzaGolpeado.y), ForceMode2D.Impulse);

            //detectar si se le acabaron las vidas
            if (vidas <= 0)
            {
                muerto = true;
                Morir(true);
            }
        }
    }

    private IEnumerator serInvencible()
    {
        invencible = true;
        yield return new WaitForSeconds(tiempoInvencible);

        //restaurar
        invencible = false;
    }

    public bool puedeSerDañado()
    {
        return !invencible;
    }

    private IEnumerator serGolpeado()
    {
        golpeado = true;
        an.SetBool("Daño", true); //activar animacion de ser golpeado
        sr.color = colorHit; //cambiar color de golpe
        rb.gravityScale = .5f;

        //delay del golpe
        yield return new WaitForSeconds(tiempoGolpeado);
        if (golpeado && !atacando) cambiarDireccion(); //voltear si es golpeado

        //restaurar Valores originales
        golpeado = false;
        sr.color = colorOrg;
        rb.gravityScale = 1f;
    }

    public void despuesDeGolpear()
    {
        an.SetBool("Daño", false); //terminar animacion de ser golpeado
    }

    public void Morir(bool check)
    {
        rb.velocity = Vector2.zero; //quitar velocidades
        patruyando = atacando = vigilando = false; // deshabilitar comportamientos

        //Estado de muerte
        puedeMoverse = false;
        muerto = true;

        //deshabilitar animaciones
        an.SetBool("Disparar", false);
        an.SetBool("Caminar", false);
        an.SetBool("Daño", false);
        an.SetBool("Morir", true);
        rb.gravityScale = 0;
        cld.enabled = false;
    }

    public void Destruir()
    {
        //dropear
        //destruir objeto
        an.SetBool("Morir", false);
        Destroy (gameObject);
    }

    public void CurarVida(float cantidad)
    {
        //efecto Curar NO SE USA AUN
        vidas += cantidad;
    }


#region Sonido
    public void MandarSonido(AudioClip sonido)
    {
        soundManager.Instance.PlayEfecto(sonido, Random.Range(2f, 2.5f));
    }
#endregion
}
