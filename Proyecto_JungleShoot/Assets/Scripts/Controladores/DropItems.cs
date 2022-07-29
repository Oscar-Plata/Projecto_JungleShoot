using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItems : MonoBehaviour
{
    public GameObject[] listaItems;

    [Range(0.0f, 1.0f)]
    public float porcentajeDrop;

    public int cantidadItems;

    [SerializeField]
    private Transform posicion;

    public ParticleSystem efectoDrop;

    public void Dropear(float porcentaje)
    {
        porcentajeDrop = porcentaje;
        float rndDrop = Random.Range(0.0f, 1.0f);
        Debug.Log("DROP %: " + rndDrop);
        if (rndDrop <= porcentajeDrop)
        {
            for (var i = 0; i < cantidadItems; i++)
            {
                int item = Random.Range(0, listaItems.Length);
                StartCoroutine(drop(item));
            }
        }
    }

    public void Dropear()
    {
        float rndDrop = Random.Range(0.0f, 1.0f);

        //Debug.Log("DROP %: " + rndDrop);
        if (rndDrop <= porcentajeDrop)
        {
            for (var i = 0; i < cantidadItems; i++)
            {
                int item = Random.Range(0, listaItems.Length);
                StartCoroutine(drop(item));
            }
        }
    }

    private IEnumerator drop(int item)
    {
        yield return new WaitForSeconds(0.2f);
        Vector3 pos = posicion.position + new Vector3(Random.Range(0f, 1.5f), Random.Range(0f, 1.50f), 0);
        if (efectoDrop != null)
        {
            efectoDrop.transform.position = pos;
            efectoDrop.Play();
        }
        yield return new WaitForSeconds(0.1f);
        Instantiate(listaItems[item], pos, Quaternion.identity);
    }
}
