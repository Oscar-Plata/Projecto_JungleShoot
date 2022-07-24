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

    public GameObject detector;

    public DetectarPlayer dtp;

    public Transform jugador;

    [Header("Movimiento")]
    public Vector3 Velocidades;

    public int direccion;

    public float velocidadMovimiento;

    private float VelMovRnd;

    public bool puedeMoverse;

    [Header("Tiempos")]
    public float tiempoEspera;

    public float tiempoDisparo;

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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        dp = GetComponent<Disparar>();
        dtp = detector.GetComponent<DetectarPlayer>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        colorOrg = sr.color;
        CambiarEstado();
    }

    // Update is called once per frame
    void Update()
    {
        if (patruyando) rb.velocity = new Vector2(VelMovRnd * direccion, rb.velocity.y);

        if (golpeado) transform.LookAt(jugador);

        atacando = dtp.detectado;
        if (atacando && !muerto)
        {
            vigilando = false;
            patruyando = false;
            StartCoroutine("DispararAPlayer");
        }
        Velocidades = rb.velocity;
    }

    private void CambiarEstado()
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
            StartCoroutine("tiempoCambio");
        }
    }

    private IEnumerator tiempoCambio()
    {
        yield return new WaitForSeconds(tiempoEspera + Random.Range(-5f, +5f));
        CambiarEstado();
    }

    private void cambiarDireccion()
    {
        bool valor = Random.value > 0.5 ? true : false;

        //Debug.Log (valor);
        if (valor)
            direccion = 1; // Va a la derecha
        else
            direccion = -1; // Va a la izquierda
        transform.localScale = new Vector3(direccion, transform.localScale.y, transform.localScale.z);
    }

    private IEnumerator DispararAPlayer()
    {
        if (!yaDisparo)
        {
            yaDisparo = true;
            dp.dirreccion = new Vector2(direccion, 0);
            yield return new WaitForSeconds(Random.Range(0.01f, +0.3f));
            dp.Disparo();
            yield return new WaitForSeconds(tiempoDisparo + Random.Range(-0.2f, +0.2f));
            yaDisparo = false;
            an.SetBool("Disparar", false);
            CambiarEstado();
        }
    }

    public void RecibirDaño(float cantidad)
    {
        if (!muerto && !invencible)
        {
            vidas -= (int) cantidad;

            StartCoroutine("serGolpeado");
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(direccion * -1 * fuerzaGolpeado.x, fuerzaGolpeado.y), ForceMode2D.Impulse);
            StartCoroutine("serInvencible");
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
        invencible = false;
    }

    private IEnumerator serGolpeado()
    {
        golpeado = true;
        an.SetBool("Daño", true);
        sr.color = colorHit;
        yield return new WaitForSeconds(tiempoGolpeado);
        golpeado = false;
        sr.color = colorOrg;
        an.SetBool("Daño", false);
    }

    public void Morir(bool check)
    {
        patruyando = atacando = vigilando = false;
        puedeMoverse = false;
        muerto = true;
        an.SetBool("Disparar", false);
        an.SetBool("Caminar", false);
        an.SetBool("Daño", false);
        an.SetBool("Morir", true);
        Destroy (gameObject);
    }

    public void Destruir()
    {
        Destroy (gameObject);
    }

    public void CurarVida(float cantidad)
    {
        vidas += cantidad;
        //efecto Curar
    }
}
