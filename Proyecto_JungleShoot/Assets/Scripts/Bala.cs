using System.Collections;
using UnityEngine;

public class Bala : MonoBehaviour
{
    private Rigidbody2D rb;

    public Vector2 dirrecion;

    public float velocidad;

    public float tiempoVida;

    private float instanteVida;

    public Rigidbody2D rbChoque;

    public string tagDaño;

    //private float vivo = 0;
    void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        instanteVida = Time.deltaTime;

        //rb.velocity = new Vector2(dirrecion.x * velocidad, dirrecion.y * velocidad);
        rb.AddForce(new Vector2(dirrecion.x * velocidad, dirrecion.y * velocidad), ForceMode2D.Impulse);
        StartCoroutine(matarBala());
    }

    public IEnumerator matarBala()
    {
        yield return new WaitForSeconds(tiempoVida);

        //Debug.Log("destroy");
        Destroy (gameObject);
    }

    private void OnTriggerEnter2D(Collider2D choque)
    {
        // Debug.Log(choque.tag);
        if (choque.tag.Equals(tagDaño))
        {
            Debug.Log("Daño a enemigo"); //detectar colision al enemigo
            Destroy (gameObject);
            Destroy(choque.gameObject);
        }
        else if (choque.tag.Equals("Escenario"))
        {
            Debug.Log("Choque con Escenario"); //detectar colision al escenario
            Destroy (gameObject);
        }
        // switch (choque.tag)
        // {
        //     case "Enemigo":
        //         Debug.Log("Daño a enemigo"); //detectar colision al enemigo
        //         Destroy (gameObject);
        //         Destroy(choque.gameObject);
        //         break;
        //     case "Escenario":
        //         Debug.Log("Choque con Escenario"); //detectar colision al escenario
        //         Destroy (gameObject);
        //         break;
        // }
    }
}
