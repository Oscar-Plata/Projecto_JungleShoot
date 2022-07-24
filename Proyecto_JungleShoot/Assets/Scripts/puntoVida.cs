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

    void Awake()
    {
        an = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider otro)
    {
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
        ParticleSystem.MainModule psmain = objetoPS.main;
        psmain.startColor = colorPS;
        objeto.CurarVida (puntosVida);
        an.SetTrigger("Tomar");
        Destruir(0.5f);
    }
}
