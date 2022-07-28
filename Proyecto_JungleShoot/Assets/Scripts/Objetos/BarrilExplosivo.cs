using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrilExplosivo : MonoBehaviour, IDaño
{
    public float vidaBarril = 5;

    public float tiempoHit;

    private SpriteRenderer sr;

    private Color colorOrg;

    public Color colorHit;

    private bool muerto;

    public AudioClip sonidoHit;

    public AudioClip sonidoRoto;

    public Animator an;

    public ParticleSystem efectoExplosion;

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
    }

    public void sonidoExplosion(AudioClip sonido)
    {
        soundManager.Instance.PlayEfecto(sonido, Random.Range(0.8f, 1.2f));
    }

    public void RecibirDaño(float cantidad)
    {
        if (muerto) return;
        if (vidaBarril <= 0)
        {
            muerto = true;
            Morir(true);
        }
        vidaBarril -= cantidad;
        StartCoroutine(Cambiarcolor());
        soundManager.Instance.PlayEfecto(sonidoHit, Random.Range(.8f, 1.2f));
    }

    public void Morir(bool check)
    {
        an.SetBool("Explotar", true);
        if (efectoExplosion != null) efectoExplosion.Play();
        soundManager.Instance.PlayEfecto(sonidoRoto, Random.Range(.8f, 1.2f));
    }

    public void CurarVida(float cantidad)
    {
        Debug.Log("Healt");
    }

    public bool puedeSerDañado()
    {
        bool res = true;
        if (muerto) res = false;
        return res;
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
