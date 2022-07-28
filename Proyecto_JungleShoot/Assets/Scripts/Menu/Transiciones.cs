using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transiciones : MonoBehaviour
{
    public static Transiciones Instance;

    public float tiempoTransicion;

    public Animator an;

    private void Awake()
    {
        if (Transiciones.Instance == null)
        {
            Transiciones.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy (gameObject);
        }
    }

    public void activarTrnasicion()
    {
        an.SetTrigger("Activar");
    }
}
