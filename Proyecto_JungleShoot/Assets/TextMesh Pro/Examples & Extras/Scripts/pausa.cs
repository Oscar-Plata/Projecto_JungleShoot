using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class pausa : MonoBehaviour
{
    private ControlPlayer controlPlayer;
    private InputAction menu;

    [SerializeField] private GameObject pauseUI;
    [SerializeField] private bool isPaused;

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

    void Pause(InputAction.CallbackContext context){
        isPaused = !isPaused;
        if (isPaused){
            ActivateMenu();
        }
        else
        {
            DeactivateMenu();
        }
    }
    
    public void Salir()
        {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

    void ActivateMenu()
    {
        Time.timeScale=0;
        AudioListener.pause=true;
        pauseUI.SetActive(true);
    }

    public void DeactivateMenu()
    {
        Time.timeScale=1;
        AudioListener.pause=false;
        pauseUI.SetActive(false);
        isPaused=false;
    }

}
