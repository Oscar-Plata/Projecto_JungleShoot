using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CajaRompible : MonoBehaviour, IDaño
{
    public float vidasCaja = 2;

    public AudioClip sonidoHit;

    public AudioClip sonidoRoto;

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
        dpi.Dropear(1.0f);
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
        if (vidasCaja <= 0)
        {
            muerto = true;
            Morir(true);
        }
        vidasCaja -= cantidad;
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
        gameObject.SetActive(false); //Destroy (gameObject);
    }
}
