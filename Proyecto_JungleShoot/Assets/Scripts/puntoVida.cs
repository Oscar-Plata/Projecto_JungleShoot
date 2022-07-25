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

    public Color colorPS;

    public GameObject sprite;

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
            Debug.Log (objeto);
            if (objeto != null) Obtener();
        }
    }

    public void Destruir(float tiempo)
    {
        Destroy (gameObject, tiempo);
    }

    public void Obtener()
    {
        // ParticleSystem.MainModule psmain = objetoPS.main;
        // psmain.startColor = colorPS;
        objeto.CurarVida (puntosVida);
        an.SetTrigger("Tomado");
        Destruir(0.5f);
    }
}
