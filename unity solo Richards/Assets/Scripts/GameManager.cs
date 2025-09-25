using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    Image healthBar;

    PlayerController player;

    GameObject pauseMenu;

    public bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerController>();
        healthBar = GameObject.FindGameObjectWithTag("ui_health").GetComponent<Image>();

        pauseMenu = GameObject.FindGameObjectWithTag("pause");
        pauseMenu.SetActive
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = (float)player.health / (float)player.maxHealth;
    }

    public void Pause()
    {
        if (!isPaused)
        {
            isPaused = true;

            pauseMenu.SetActive(true);

            Time.timeScale = 1;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
