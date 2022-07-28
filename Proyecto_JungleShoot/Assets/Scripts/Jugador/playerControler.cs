using System.Collections;
using UnityEngine;

public class playerControler : MonoBehaviour
{
    private Rigidbody2D rb;

    public float movimientoVEL = 5.5f;

    public float fuerzaSAL = 6.5f;

    private Vector2 direccion;

    // AWake se ejecuta primero que nada al iniciar el juego
    void Awake()
    {
        Debug.Log("METODO AWAKE");
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("METODO START");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("UPDATE");
        Movimiento();
    }

    private void Movimiento()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        direccion = new Vector2(x, y);
        Caminar();

        DuracionSalto();
        if (Input.GetButtonDown("Jump"))
        {
            Salto();
        }
    }

    private void Caminar()
    {
        rb.velocity = new Vector2(direccion.x * movimientoVEL, rb.velocity.y);
    }

    void DuracionSalto()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity +=
                Vector2.up * Physics2D.gravity.y * (4.75f - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity +=
                Vector2.up * Physics2D.gravity.y * (4.75f - 1) * Time.deltaTime;
        }
    }

    public void Salto()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * fuerzaSAL;
    }
}
