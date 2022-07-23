using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEnemigo : MonoBehaviour
{
    public Rigidbody2D rb;

    public Animator an;

    [Header("Movimiento")]
    public Vector3 Velocidades;

    public int direccion;

    public float velocidadMovimiento;

    private float VelMovRnd;

    public bool puedeMoverse;

    [Header("Tiempos")]
    public float tiempoEspera;

    public float tiempoAccion;

    [Header("Estado")]
    public bool vigilando; //Estado 0

    public bool patruyando; //Estado 1

    public bool atacando; //Estado 2

    public bool muerto; //Estado 3

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        an = this.GetComponent<Animator>();
    }

    void Start()
    {
        CambiarEstado();
    }

    // Update is called once per frame
    void Update()
    {
        if (patruyando) rb.velocity = new Vector2(VelMovRnd * direccion, rb.velocity.y);
        Velocidades = rb.velocity;
    }

    private void CambiarEstado()
    {
        int selector = 0;
        selector = Random.Range(0, 2);
        Debug.Log("Estado: " + selector);
        cambiarDireccion();
        switch (selector)
        {
            case 0:
                Debug.Log("Vigilando");
                vigilando = true;
                patruyando = false;
                an.SetBool("Caminar", false);
                break;
            case 1:
                Debug.Log("Patruyando");
                patruyando = true;
                vigilando = false;
                an.SetBool("Caminar", true);
                VelMovRnd = velocidadMovimiento + Random.Range(-1f, +1f);
                break;
        }
        StartCoroutine("tiempoCambio");
    }

    private IEnumerator tiempoCambio()
    {
        yield return new WaitForSeconds(tiempoEspera + Random.Range(-5f, +5f));
        CambiarEstado();
    }

    private void cambiarDireccion()
    {
        bool valor = Random.value > 0.5 ? true : false;
        Debug.Log (valor);
        if (valor)
            direccion = 1; // Va a la derecha
        else
            direccion = -1; // Va a la izquierda
        transform.localScale = new Vector3(direccion, transform.localScale.y, transform.localScale.z);
    }
}
