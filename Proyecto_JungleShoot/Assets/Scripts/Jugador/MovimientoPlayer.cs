using System.Collections;
using System.Collections.Generic;
// using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MovimientoPlayer : MonoBehaviour, IDaño
{
#region varables
    [Header("Componentes")]
    public Rigidbody2D rb;

    public Transform pies;

    private Animator an;

    public Vector2 inputAxs;

    public Disparar dp;

    public Canvas GameOver;

    public GameObject SalirGAMEOVER;

    [Header("Particulas")]
    public ParticleSystem efectoDust;

    public ParticleSystem efectoCurar;

    public ParticleSystem efetoInvencible;

    public ParticleSystem efectoHit;

    [Header("Sonidos")]
    public AudioClip sonidoDash;

    public AudioClip sonidoReload;

    public AudioClip sonidoCurar;

    public AudioClip sonidoGameOver;

    private SpriteRenderer sr;

    public GameObject efectoDash;

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

    public int fantasmas;

    public float tiempoGhost;

    // [Header("Camara")]
    // public CinemachineVirtualCamera cv;
    // public float fuerzaTemblor = 10;
    // public float tiempoTemblor = 0.5f;
    // public bool vibrando;
    // CinemachineBasicMultiChannelPerlin cvRuido;
    [Header("Ataque")]
    public bool atacando = false;

    public Vector2 direccionAtaque;

    public float tiempoAtaque = 0.1f; //tiempo que dura la animacion de rodar

    [Header("Vida")]
    public float vidas;

    public float vidasTotales = 3;

    public float topeVidas;

    public bool muerto = false;

    public Color colorHit;

    private Color colorOrg;

    public bool invencible = false;

    public bool golpeado = false;

    public float tiempoInvencible = 0.5f;

    public Vector2 fuerzaGolpeado;

    public float tiempoGolpeado = .3f;

    private GameObject fant;

    public int continues = 3;

    public float tiempoRespawn = 3.5f;

    [Header("pausa")]
    public bool pausaPlayer;


#endregion //cooldown entre ruedos



#region metodosUnity
    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        an = this.GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        GameOver.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        pausaPlayer = false;
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
        if (!puedeMoverse || pausaPlayer) return;

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
        if (transform.position.y < -15f)
        {
            muerto = true;
            Morir(true);
        }
    }

    private void FixedUpdate()
    {
        if (pausaPlayer) return;

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
            if (ruedosRestantes != ruedosTotal)
            {
                StartCoroutine("genFantasmas", 2);
                soundManager.Instance.PlayEfecto(sonidoReload, 1.0f);
            }
            puedeRodar = true;
            ruedosRestantes = ruedosTotal;
            tiempoRodarInicio = 0;
        }
    }
#endregion



#region Movimiento
    /*
    Metodo que controla la velocidad de movimiento del player segun el input
    */
    public void OnMovimientoX(InputValue iv)
    {
        if (muerto) return;
        float valor = iv.Get<float>();
        an.SetBool("caminar", true);
        velocidadActual = valor * velocidadMovimiento; //calculo de velocidad actual
        inputAxs.x = valor;

        // Modificar la Escala en x del player para que voltee a un lado u otro
        if (valor < 0)
            direccionAnterior = -1;
        else if (valor > 0) direccionAnterior = 1;

        if (valor < -0.5)
            direccionAtaque.x = -1;
        else if (valor > 0.5) direccionAtaque.x = 1;

        if (valor == 0)
        {
            transform.localScale = new Vector3(direccionAnterior, transform.localScale.y, transform.localScale.z);
            an.SetBool("caminar", false); //Animacion de caminar
            direccionAtaque.x = 0;
        }
        else
            transform.localScale = new Vector3(direccionAnterior, transform.localScale.y, transform.localScale.z);
    }

    public void OnMovimientoY(InputValue iv)
    {
        float valor = iv.Get<float>();
        if (valor < -0.5)
            direccionAtaque.y = -1;
        else if (valor > 0.5)
            direccionAtaque.y = 1;
        else
            direccionAtaque.y = 0;
        inputAxs.y = valor;
    }
#endregion



#region  Salto
    /*
    Metodo que se llama desde el imput system, para realizar saltos
    */
    public void OnSaltar()
    {
        if (pausaPlayer) return;

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
            if (efectoDust != null) efectoDust.Play();
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
        if (pausaPlayer) return;
        if (puedeRodar && puedeMoverse)
        {
            rodando = true; //activa el indicador que esta rodando para limitar otras accciones
            StartCoroutine(IniciarRodar());

            //Si queda el ultimo ruedo activar limitador de ruedo hasta cumplir tiempo
            if (ruedosRestantes == 1) puedeRodar = false;
            soundManager.Instance.PlayEfecto(sonidoDash, Random.Range(.8f, 1.6f));

            //rodar segun la direccion donde se ve
            if (direccionAnterior == -1)
            {
                rb.AddForce(Vector2.left * fuerzaRodar, ForceMode2D.Impulse); //añade una fuerza de rodar izquierda al rigidbody
            }
            else
            {
                rb.AddForce(Vector2.right * fuerzaRodar, ForceMode2D.Impulse); //añade una fuerza de rodar  derecha al rigidbody
            }
            StartCoroutine("genFantasmas", fantasmas);
            StartCoroutine("serInvencible", tiempoInvencible + .5f);
            ruedosRestantes--; //Reducir cantidad de ruedos posibles
            tiempoRodarInicio = tiempoJuego; //obtener instante en el tiempo
            ShakeManager.Instance.agitarCamara(3f, 0.1f);
        }
    }

    public IEnumerator genFantasmas(int cantidad)
    {
        for (var i = 0; i < cantidad; i++)
        {
            fant = (GameObject) Instantiate(efectoDash, transform.position, Quaternion.identity);
            fant.GetComponent<efectoGhost>().srOtro = GetComponent<SpriteRenderer>();
            fant.GetComponent<efectoGhost>().trfOtro = this.transform;
            yield return new WaitForSeconds(tiempoGhost);
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



#region Debug
    public void OnReset()
    {
        if (pausaPlayer) return;
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
        an.SetBool("morir", false);
        continues = 3;
    }


#endregion



#region atacar

    public void OnAtacar()
    {
        if (muerto || pausaPlayer) return;
        if (!rodando && !atacando)
        {
            atacando = true;
            ShakeManager.Instance.agitarCamara(0.05f, 0.05f);
            an.SetFloat("miraX", direccionAtaque.x);
            an.SetFloat("miraY", direccionAtaque.y);
            if ((enSuelo || enEsquina) && (direccionAtaque.y == -1 && direccionAtaque.x == 0))
            {
                direccionAtaque.x = direccionAnterior;
                direccionAtaque.y = 0;
                an.SetBool("agacharse", true);
                an.SetFloat("miraY", 0);
                an.SetTrigger("disparo");
                dp.dirreccion = direccionAtaque;
                direccionAtaque.y = -1;
                direccionAtaque.x = 0;
                dp.Disparo();
                StartCoroutine(cooldownAtaque());
                return;
            }
            if (direccionAtaque.x == 0 && direccionAtaque.y == 0)
            {
                direccionAtaque.x = direccionAnterior;
            }

            an.SetTrigger("disparo");
            dp.dirreccion = direccionAtaque;

            dp.Disparo();
            StartCoroutine(cooldownAtaque());
        }
    }

    public IEnumerator cooldownAtaque()
    {
        yield return new WaitForSeconds(tiempoAtaque);
        atacando = false;
        an.SetBool("agacharse", false);
    }
#endregion



#region vidaDaño

    public void RecibirDaño(float cantidad)
    {
        if (!muerto && !invencible)
        {
            vidas -= (int) cantidad;

            StartCoroutine("serGolpeado");
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(direccionAnterior * -1 * fuerzaGolpeado.x, fuerzaGolpeado.y), ForceMode2D.Impulse);
            StartCoroutine("serInvencible", tiempoInvencible);
            ShakeManager.Instance.agitarCamara();
            if (vidas <= 0)
            {
                muerto = true;
                Morir(true);
            }
        }
    }

    private IEnumerator serInvencible(float tiempo)
    {
        efetoInvencible.Play();
        invencible = true;
        yield return new WaitForSeconds(tiempo);
        efetoInvencible.Stop();
        invencible = false;
    }

    private IEnumerator serGolpeado()
    {
        if (efectoHit != null) efectoHit.Play();
        an.SetBool("hit", true);
        golpeado = true;
        sr.color = colorHit;
        yield return new WaitForSeconds(tiempoGolpeado);
        golpeado = false;
        sr.color = colorOrg;
        an.SetBool("hit", false);
    }

    public void Morir(bool check)
    {
        ShakeManager.Instance.agitarCamara(20, 2.0f);
        an.SetBool("morir", true);
        puedeMoverse = false;
        rb.velocity = Vector2.zero;

        //rb.gravityScale = 0;
        continues--;
        if (continues > 0)
        {
            StartCoroutine(Respawn());
        }
        else
        {
            Debug.Log("GAMEOVER");
            StartCoroutine(GameOVerScreen());
        }
    }

    IEnumerator GameOVerScreen()
    {
        Transiciones.Instance.activarTrnasicion();
        yield return new WaitForSeconds(1.0f);
        Time.timeScale = 0;
        soundManager.Instance.PausarSonidos();
        soundManager.Instance.PlayEfecto(sonidoGameOver, 1);
        GameOver.gameObject.SetActive(true);
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(SalirGAMEOVER, new BaseEventData(eventSystem));
    }

    public void CurarVida(float cantidad)
    {
        soundManager.Instance.PlayEfecto(sonidoCurar, Random.Range(0.8f, 1.6f));
        vidas += cantidad;
        if (vidas > vidasTotales) vidasTotales++;
        if (vidas >= topeVidas) vidas = topeVidas;
        if (vidasTotales >= topeVidas) vidasTotales = topeVidas;
        if (efectoCurar != null) efectoCurar.Play();
    }

    public bool puedeSerDañado()
    {
        return invencible;
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(tiempoRespawn);

        Transiciones.Instance.activarTrnasicion();
        yield return new WaitForSeconds(.1f);
        this.transform.position = new Vector3(0, 4, 0);
        rb.velocity = new Vector2(0, 0);
        rodando = false;
        enSuelo = true;
        saltosRestantes = saltosExtra;
        puedeRodar = true;
        puedeMoverse = true;
        vidas = vidasTotales;
        efectoCurar.Play();
        muerto = false;
        ruedosRestantes = ruedosTotal;
        an.SetBool("morir", false);
        rb.gravityScale = 1;
    }
#endregion



#region Sonido
    public void MandarSonido(AudioClip sonido)
    {
        soundManager.Instance.PlayEfecto(sonido, Random.Range(0.8f, 1.2f));
    }
#endregion
}
