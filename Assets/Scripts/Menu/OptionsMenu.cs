using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class OptionsMenu : MonoBehaviour
    {
        public GameObject optionsMenu;

        // Update is called once per frame
        [Obsolete("Obsolete")]
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }

        [Obsolete("Obsolete")]
        public void Resume()
        {
            TogglePause(false);
        }

        [Obsolete("Obsolete")]
        public void Reset()
        {
            TogglePause(false);

            if (SceneManager.GetActiveScene().name == "Wild_West")
            {
                FindObjectOfType<PlayerHealth>()?.Die();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        [Obsolete("Obsolete")]
        public void MainMenu()
        {
            TogglePause(false);
            SceneManager.LoadScene(0);
        }

        [Obsolete("Obsolete")]
        private void TogglePause()
        {
            bool isPaused = !optionsMenu.activeSelf;
            optionsMenu.SetActive(isPaused);
            Time.timeScale = isPaused ? 0 : 1;

            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (var audioSource in audioSources)
            {
                if (isPaused)
                {
                    audioSource.Pause();
                }
                else
                {
                    audioSource.UnPause();
                }
            }
        }
    
        [Obsolete("Obsolete")]
        private void TogglePause(bool isPaused)
        {
            optionsMenu.SetActive(isPaused);
            Time.timeScale = isPaused ? 0 : 1;

            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (var audioSource in audioSources)
            {
                if (isPaused)
                {
                    audioSource.Pause();
                }
                else
                {
                    audioSource.UnPause();
                }
            }
        }
    }
}