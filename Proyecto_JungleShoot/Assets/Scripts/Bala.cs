using System.Collections;
using UnityEngine;

public class Bala : MonoBehaviour
{
    private Rigidbody2D rb;

    public Vector2 dirrecion;

    public float velocidad;

    public float tiempoVida;

    public float dañoBala = 1.0f;

    public string tagDaño;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        //Mover Bala con fuerza
        rb.AddForce(new Vector2(dirrecion.x * velocidad, dirrecion.y * velocidad), ForceMode2D.Impulse);
        StartCoroutine(matarBala()); //Destruir bala por tiempo
    }

    public IEnumerator matarBala()
    {
        yield return new WaitForSeconds(tiempoVida);
        Destroy (gameObject);
    }

    private void OnTriggerEnter2D(Collider2D choque)
    {
        //Detectar si choca con un objeto con el tagEspecificado
        if (choque.tag.Equals(tagDaño))
        {
            //Detectar si el objeto puede ser dañado
            IDaño objeto = choque.GetComponent<IDaño>();
            if (objeto != null) objeto.RecibirDaño(dañoBala); //Dañar objeto
            Destroy (gameObject); //Destruir Bala
            Debug.Log("Quitar vida");
        }
        else if (choque.tag.Equals("Escenario")) Destroy(gameObject); //Destruir bala al chocar con escenario
    }
}
