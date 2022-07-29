using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class pausa : MonoBehaviour
{
    private ControlPlayer controlPlayer;

    private InputAction menu;

    [SerializeField]
    private GameObject pauseUI;

    [SerializeField]
    private bool isPaused;

    public MovimientoPlayer playerScript;

    void Awake()
    {
        controlPlayer = new ControlPlayer();
    }

    void Update()
    {
    }

    private void OnEnable()
    {
        menu = controlPlayer.Menu.Pausa;
        menu.Enable();

        menu.performed += Pause;
    }

    private void OnDisable()
    {
        menu.Disable();
    }

    public void Pause(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            ActivateMenu();
        }
        else
        {
            DeactivateMenu();
        }
    }

    public void Salir()
    {
        DeactivateMenu();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ActivateMenu()
    {
        if (playerScript != null) playerScript.pausaPlayer = true; //Que el input mientras esta en pausa no afecte al player
        soundManager.Instance.PausarSonidos(); //pausar Musica
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseUI.SetActive(true);
    }

    public void DeactivateMenu()
    {
        if (playerScript != null) playerScript.pausaPlayer = false; //Que el input mientras esta en pausa no afecte al player
        soundManager.Instance.ReanudarSonidos(); //Reanudar Musica
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseUI.SetActive(false);
        isPaused = false;
    }
}
