using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectarPlayer : MonoBehaviour
{
    private Rigidbody2D rb;

    public bool detectado;

    public bool enDeteccion;

    public string tagADetectar;

    void Awake()
    {
        detectado = false;
        enDeteccion = false;
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(tagADetectar))
        {
            detectado = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag.Equals(tagADetectar))
        {
            enDeteccion = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            detectado = false;
            enDeteccion = false;
        }
    }
}
