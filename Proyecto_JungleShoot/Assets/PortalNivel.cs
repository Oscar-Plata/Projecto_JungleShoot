using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalNivel : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            StartCoroutine(pasarNivel());
        }
    }

    IEnumerator pasarNivel()
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("Nivel");
    }
}
