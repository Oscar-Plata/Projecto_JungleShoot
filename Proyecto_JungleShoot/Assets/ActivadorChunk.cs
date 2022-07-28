using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivadorChunk : MonoBehaviour
{
    public GameObject[] ListaObjetos;

    public bool detectado;

    public string tagADetectar;

    private void Awake()
    {
        foreach (var item in ListaObjetos)
        {
            item.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(tagADetectar))
        {
            detectado = true;
            foreach (var item in ListaObjetos)
            {
                item.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals(tagADetectar))
        {
            detectado = false;
            this.gameObject.SetActive(false);
        }
    }
}
