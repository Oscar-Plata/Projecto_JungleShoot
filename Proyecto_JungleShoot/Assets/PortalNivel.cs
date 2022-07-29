using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PortalNivel : MonoBehaviour
{
    public GameObject GameOver;

    public GameObject SalirGAMEOVER;

    public AudioClip sonidoGameOver;

    public GameObject player;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            other.gameObject.SetActive(false);
            StartCoroutine(FinDelJuego());
        }
    }

    IEnumerator FinDelJuego()
    {
        yield return new WaitForSeconds(1f);
        Transiciones.Instance.activarTrnasicion();
        yield return new WaitForSeconds(.5f);
        GameOver.gameObject.SetActive(true);

        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(SalirGAMEOVER, new BaseEventData(eventSystem));
        yield return new WaitForSeconds(.5f);
        soundManager.Instance.PausarSonidos();
        soundManager.Instance.PlayEfecto(sonidoGameOver, 1);

        Time.timeScale = 0;
    }

    public void Salir()
    {
        ShakeManager.Instance.enJuego = false;
        ScoreManager.Instance.erase();
        soundManager.Instance.StopFondo();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Time.timeScale = 1;
    }
}
