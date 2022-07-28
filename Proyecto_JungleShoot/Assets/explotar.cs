using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explotar : MonoBehaviour
{
    public CircleCollider2D area;

    public float radioExplosion = 4.5f;

    public float daño;

    public string tagProteccion;

    private void Awake()
    {
        area.radius = radioExplosion;
    }

    public void OnTriggerEnter2D(Collider2D otro)
    {
        if (!otro.transform.tag.Equals(tagProteccion))
        {
            IDaño objeto = otro.GetComponent<IDaño>();
            if (objeto != null)
            {
                objeto.RecibirDaño (daño);
            }
        }
    }
}
