using System.Collections;
using UnityEngine;

public class Disparar : MonoBehaviour
{
    [Header("Componentes")]
    public Transform puntoMira;

    public ParticleSystem particulas;

    public GameObject Bala;

    private Vector3 posicion;

    private GameObject balaGen;

    private Animator an;

    [Header("Propiedades")]
    public int balasPorDisparo;

    public float velocidad;

    [Range(0f, 3f)]
    public float dispersionBala;

    [Range(0f, 0.1f)]
    public float tiempoEntreBalas;

    public Vector2 dirreccion;

    public void Disparo()
    {
        an = this.GetComponent<Animator>();
        StartCoroutine(DelayDisparo());
    }

    private IEnumerator DelayDisparo()
    {
        for (int i = 0; i < balasPorDisparo; i++)
        {
            emitir();
            yield return new WaitForSeconds(tiempoEntreBalas);
            if (this.tag.Equals("Player"))
                an.SetTrigger("disparo");
            else if (this.tag.Equals("Enemigo")) an.SetBool("Disparar", true);
            posicion = new Vector3(puntoMira.position.x, puntoMira.position.y + Random.Range(-1 * dispersionBala, dispersionBala), puntoMira.position.z);
            balaGen = (GameObject) Instantiate(Bala, posicion, Quaternion.identity);
            balaGen.GetComponent<Bala>().dirrecion = dirreccion;
            balaGen.GetComponent<Bala>().velocidad = velocidad;
        }
    }

    private void emitir()
    {
        particulas.Play();
    }
}
