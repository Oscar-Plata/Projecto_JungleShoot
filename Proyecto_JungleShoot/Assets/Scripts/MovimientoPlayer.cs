using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoPlayer : MonoBehaviour, IDaño
{
#region varables
    [Header("Componentes")]
    public Rigidbody2D rb;

    public Transform pies;

    private Animator an;

    public Vector2 inputAxs;

    public Disparar dp;

    public ParticleSystem efectoDust;

    private SpriteRenderer sr;

    // public ParticleSystem efectoDash;
    [Header("Movimiento")]
    public Vector2 Velocidades;

    public float velocidadMovimiento = 8.5f;

    private float velocidadActual = 0f;

    private int direccionAnterior = 1;

    public bool puedeMoverse = true;

    [Header("Salto")]
    public float fuerzaSalto = 15f;

    public int saltosExtra = 2;

    public int saltosRestantes;

    public float caida = 2.5f;

    public float subida = 2f;

    public bool saltando;

    [Header("Colision")]
    public float radioColision; //separacion suelo-pies

    public LayerMask layerPiso; //Capa suelo a ala cual se colisionara

    public LayerMask layerEsquina; //Capa esquina a ala cual se colisionara

    public bool enSuelo;

    public bool enEsquina;

    [Header("Rodar")]
    public float tiempoRodar = 0.2f; //tiempo que dura la animacion de rodar

    public float esperaRodar = 2f; //cooldown entre ruedos

    public float fuerzaRodar = 20f; //fuerza horizontal que se aplicara

    public int ruedosTotal = 2; //Total de veces que puede rodar seguidas

    public int ruedosRestantes = 2; //acciones de rodar Restantes

    private float tiempoJuego; //Tiempo que el jugador existe en pantalla

    private float tiempoRodarInicio = 0; //guarda el instante donde se rodo

    public bool rodando = false; //Inidicador de que esta rodando

    public bool puedeRodar = true; //Booloeano que permite rodar denuevo

    [Header("Camara")]
    public float fuerzaTemblor = 10;

    public float tiempoTemblor = 0.5f;

    public bool vibrando;

    CinemachineBasicMultiChannelPerlin cvRuido;

    [Header("Ataque")]
    public bool atacando = false;

    public Vector2 direccionAtaque;

    public float tiempoAtaque = 0.2f; //tiempo que dura la animacion de rodar

    public float tiempoAtaqueInicio;

    [Header("Vida")]
    public float vidas;

    public float vidasTotales = 3;

    public bool muerto = false;

    public Color colorHit;

    private Color colorOrg;

    public bool invencible = false;

    public bool golpeado = false;

    public float tiempoInvencible = 0.5f;

    public Vector2 fuerzaGolpeado;

    public float tiempoGolpeado = .3f;


#endregion //cooldown entre ruedos


    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        an = this.GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        colorOrg = sr.color;
        saltosRestantes = saltosExtra;
        tiempoJuego = 0;
        puedeRodar = true;
        puedeMoverse = true;
        ruedosRestantes = ruedosTotal;
        vidas = vidasTotales;
    }

    // Update is called once per frame
    void Update()
    {
        if (!muerto) puedeMoverse = true;
        if (!puedeMoverse) return;

        //Movimiento
        if (!rodando && !golpeado) rb.velocity = new Vector2(velocidadActual, rb.velocity.y);

        //Verificar si esta pisando el suelo
        Pisar();

        //verificar que no este en un desnivel y mantenerse pegado al suelo en las esquinas
        if (enEsquina && !saltando && !rodando) rb.velocity = new Vector2(rb.velocity.x * 1.5f, 1);

        //reinicia contador de saltos
        if (enSuelo || enEsquina)
        {
            saltosRestantes = saltosExtra;
            an.SetBool("enSuelo", true);
        }
        if (!enSuelo && !enEsquina) an.SetBool("enSuelo", false);

        Velocidades = rb.velocity;
    }

    private void FixedUpdate()
    {
        //Calcula el tiempo del jugador en pantalla
        tiempoJuego += Time.deltaTime;

        //Controalr la velocidad de caida del personaje al saltar
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * caida * Time.deltaTime;
            an.SetFloat("velAire", -1);
        }
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * subida * Time.deltaTime;
            an.SetFloat("velAire", 1);
        }

        //comprobar que el coldown entre cada rodar pase y se hayan acabado los ruedos posibles
        if (tiempoJuego - tiempoRodarInicio > esperaRodar)
        {
            //restaura las variables para poder rodar denuevo
            puedeRodar = true;
            ruedosRestantes = ruedosTotal;
            tiempoRodarInicio = 0;
        }
    }


#region Movimiento
    /*
    Metodo que controla la velocidad de movimiento del player segun el input
    */
    public void OnMovimientoX(InputValue iv)
    {
        float valor = iv.Get<float>();
        velocidadActual = valor * velocidadMovimiento; //calculo de velocidad actual
        inputAxs.x = valor;
        an.SetBool("caminar", true);

        // Modificar la Escala en x del player para que voltee a un lado u otro
        if (valor < 0)
            direccionAnterior = -1;
        else if (valor > 0) direccionAnterior = 1;

        if (valor == 0)
        {
            transform.localScale = new Vector3(direccionAnterior, transform.localScale.y, transform.localScale.z);
            an.SetBool("caminar", false); //Animacion de caminar
        }
        else
            transform.localScale = new Vector3(direccionAnterior, transform.localScale.y, transform.localScale.z);
    }

    public void OnMovimientoY(InputValue iv)
    {
        float valor = iv.Get<float>();
        inputAxs.y = valor;
    }
#endregion



#region  Salto
    /*
    Metodo que se llama desde el imput system, para realizar saltos
    */
    public void OnSaltar()
    {
        //verifica que pueda saltar y tenga saltos disponibles
        if (saltosRestantes > 0 && puedeMoverse)
        {
            an.SetBool("salto", true);
            StartCoroutine(IniciarSalto()); //llamada a corrutina triggerSalto

            //impulso extra de salto en ultimo salto
            if (saltosRestantes == 1 && !enSuelo && !enEsquina)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * fuerzaSalto * 1.35f, ForceMode2D.Impulse); //añade una fuerza de salto al rigidbody
            }
            efectoDust.Play();
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse); //añade una fuerza de salto al rigidbody
            saltosRestantes--; //Disminuye el contador de saltos restantes
        }
    }

    private void Pisar()
    {
        enSuelo = Physics2D.OverlapCircle(pies.position, radioColision, layerPiso); //Detectar colision de pies con suelo mediante fisicas
        enEsquina = Physics2D.OverlapCircle(pies.position, radioColision, layerEsquina); //Detectar colision de pies con suelo mediante fisicas
    }

    //corrutina para iniciar "trigger" de que salto el personaje
    public IEnumerator IniciarSalto()
    {
        saltando = true;
        yield return new WaitForSeconds(0.25f);
        saltando = false;
    }

    public void finSaltoAnim()
    {
        an.SetBool("salto", false);
    }
#endregion



#region Rodar/Dash
    public void OnRodar()
    {
        if (puedeRodar && puedeMoverse)
        {
            //Debug.Log("Rodar");
            rodando = true; //activa el indicador que esta rodando para limitar otras accciones
            StartCoroutine(IniciarRodar());

            //Si queda el ultimo ruedo activar limitador de ruedo hasta cumplir tiempo
            if (ruedosRestantes == 1) puedeRodar = false;

            // efectoDash.Play();
            //rodar segun la direccion donde se ve
            if (direccionAnterior == -1)
            {
                rb.AddForce(Vector2.left * fuerzaRodar, ForceMode2D.Impulse); //añade una fuerza de rodar izquierda al rigidbody
            }
            else
            {
                rb.AddForce(Vector2.right * fuerzaRodar, ForceMode2D.Impulse); //añade una fuerza de rodar  derecha al rigidbody
            }
            ruedosRestantes--; //Reducir cantidad de ruedos posibles
            tiempoRodarInicio = tiempoJuego; //obtener instante en el tiempo
        }
    }

    public IEnumerator IniciarRodar()
    {
        rodando = true;
        yield return new WaitForSeconds(tiempoRodar);
        FinalizarRodar();
    }

    //Metodo que se activa al terminar la animacion de rdar
    public void FinalizarRodar()
    {
        //Debug.Log("Fin DE RODAR");
        rodando = false;
    }
#endregion


    public void OnReset()
    {
        Debug.Log("Reset Position");
        this.transform.position = new Vector3(-1, 1, 0);
        rb.velocity = new Vector2(0, 0);
        rodando = false;
        enSuelo = true;
        saltosRestantes = saltosExtra;
        puedeRodar = true;
        puedeMoverse = true;
        vidas = vidasTotales;
        muerto = false;
        ruedosRestantes = ruedosTotal;
    }

    public void OnAtacar()
    {
        if (!muerto)
        {
            float miraY = 0;
            float miraX = direccionAnterior;
            if (inputAxs.y > 0.3) miraY = 1;
            if (inputAxs.y < -0.3) miraY = -1;
            if (inputAxs.y == 1)
            {
                miraY = 1;
                miraX = 0;
            }
            if (inputAxs.y == -1)
            {
                miraY = -1;
                miraX = 0;
            }
            dp.dirreccion = new Vector2(miraX, miraY);
            dp.Disparo();
        }
    }

    public void RecibirDaño(float cantidad)
    {
        if (!muerto && !invencible)
        {
            vidas -= (int) cantidad;

            StartCoroutine("serGolpeado");
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(direccionAnterior * -1 * fuerzaGolpeado.x, fuerzaGolpeado.y), ForceMode2D.Impulse);
            StartCoroutine("serInvencible", tiempoInvencible);

            //StartCoroutine("ParpadearHit", 3);
            if (vidas <= 0)
            {
                muerto = true;
                Morir(true);
            }
        }
    }

    private IEnumerator serInvencible(float tiempo)
    {
        invencible = true;
        yield return new WaitForSeconds(tiempo);
        invencible = false;
    }

    private IEnumerator serGolpeado()
    {
        golpeado = true;
        sr.color = colorHit;
        yield return new WaitForSeconds(tiempoGolpeado);
        golpeado = false;
        sr.color = colorOrg;
    }

    // private IEnumerator parpadearHit(int veces)
    // {
    //     for (int i = 0; i < veces; i++)
    //     {
    //         yield return new WaitForSeconds(f);
    //         sr.color = colorOrg;
    //     }
    // }
    public void Morir(bool check)
    {
        //an.SetBool("Morir", true);
        puedeMoverse = false;
        //this.gameObject.SetActive(false);
    }

    public void CurarVida(float cantidad)
    {
        vidas += cantidad;
        if (vidas > vidasTotales) vidasTotales++;
        //efecto Curar
    }
}
