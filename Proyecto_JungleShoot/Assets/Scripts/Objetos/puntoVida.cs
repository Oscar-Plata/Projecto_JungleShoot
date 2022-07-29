using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puntoVida : MonoBehaviour, IObjeto
{
    public string tagObtenible;

    public float puntosVida;

    private Animator an;

    private IDaño objeto;

    public ParticleSystem objetoPS;

    public AudioClip sonidoObtener;

    public int scoreObtanied;

    //public Color colorPS;
    public GameObject sprite;

    public int score;

    void Awake()
    {
        an = sprite.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        Debug.Log(otro.tag);
        if (otro.tag.Equals(tagObtenible))
        {
            objeto = otro.GetComponent<IDaño>();
            if (objeto != null) Obtener();
        }
    }

    public void Destruir(float tiempo)
    {
        Destroy (gameObject, tiempo);
    }

    public void Obtener()
    {
        //Debug.Log("play ps");
        soundManager.Instance.PlayEfecto(sonidoObtener, Random.Range(0.8f, 1.2f));
        objetoPS.Play();
        objeto.CurarVida (puntosVida);
        an.SetTrigger("Tomado");
        ScoreManager.Instance.addScore (score);
        Destruir(0.5f);
    }
}
