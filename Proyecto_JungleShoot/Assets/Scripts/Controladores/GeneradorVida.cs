using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorVida : MonoBehaviour, IDaño
{
    public float vidas = 20;

    public AudioClip sonidoHit;

    public AudioClip sonidoRoto;

    public Generador Spawner;

    public GameObject pared;

    public Animator an;

    public float tiempoHit;

    public DropItems dpi;

    private SpriteRenderer sr;

    private Color colorOrg;

    public Color colorHit;

    private bool muerto;

    private void Awake()
    {
        an = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        if (pared != null) pared.gameObject.SetActive(true);
    }

    void Start()
    {
        colorOrg = sr.color;
        muerto = false;
    }

    public void CurarVida(float cantidad)
    {
        Debug.Log("Curaga+++");
    }

    public void Morir(bool check)
    {
        dpi.Dropear();
        soundManager.Instance.PlayEfecto(sonidoRoto, Random.Range(.8f, 1.2f));
        an.SetBool("Romper", true);
    }

    public bool puedeSerDañado()
    {
        return true;
    }

    public void RecibirDaño(float cantidad)
    {
        if (muerto) return;
        if (vidas <= 0)
        {
            muerto = true;
            Morir(true);
        }
        vidas -= cantidad;
        StartCoroutine(Cambiarcolor());
        soundManager.Instance.PlayEfecto(sonidoHit, Random.Range(.8f, 1.2f));
    }

    private IEnumerator Cambiarcolor()
    {
        sr.color = colorHit;
        yield return new WaitForSeconds(tiempoHit);
        sr.color = colorOrg;
    }

    public void Destruir()
    {
        if (pared != null) pared.gameObject.SetActive(false);
        Spawner.activadorMaster = false;
        int enemigos = Spawner.transform.childCount;
        for (var i = 0; i < enemigos; i++)
        {
            GameObject aux = Spawner.transform.GetChild(i).gameObject;
            IDaño objeto = aux.GetComponent<IDaño>();
            if (objeto != null) objeto.Morir(true);
        }
        //gameObject.SetActive(false); //Destroy (gameObject);
    }
}
