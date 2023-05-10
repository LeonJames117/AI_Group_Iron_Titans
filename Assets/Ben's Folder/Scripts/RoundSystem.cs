using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class RoundSystem : MonoBehaviour
{
    [SerializeField] Vector3 spawnPoint;
    [SerializeField] Canvas roundUI;
    TextMeshProUGUI roundText;

    [SerializeField] Wave[] waves;
    [SerializeField] float waveRestTime;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip horn_audioClip;
    [SerializeField] AudioClip win_audioClip;

    PlayerCharacter player;

    int wave = 0;
    bool waveComplete = true;
    public bool gameOver = false;



    // Start is called before the first frame update
    void Start()
    {
        roundText = roundUI.GetComponentInChildren<TextMeshProUGUI>();
        player = FindObjectOfType<PlayerCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waveComplete) 
        {
            waveRestTime += Time.deltaTime;

            if(waveRestTime > 2.0f) 
            {
                StartCoroutine(StartRound());
                waveRestTime = 0;
            }
        }

        if (gameOver) 
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                SceneManager.LoadScene(1);
            }
        }
    }

    public void WaveComplete() 
    {
        waveComplete = true;
        wave++;
    }

    IEnumerator StartRound() 
    {
        waveComplete = false;
        roundUI.gameObject.SetActive(true);
        player.ResetHealth();

        SpawnWave();

        yield return new WaitForSeconds(2.0f);

        if (!gameOver) 
        {
            roundUI.gameObject.SetActive(false);
        }
    }

    void SpawnWave() 
    {
        if(wave > waves.Length - 1) 
        {
            GameWin();
        }
        else 
        {
            print("spawn");
            Instantiate(waves[wave].gameObject);
            roundText.text = "ROUND " + (wave + 1).ToString();
            audioSource.PlayOneShot(horn_audioClip, 0.4f);
        }
    }

    void GameWin() 
    {
        roundText.text = "YOU WIN";
        audioSource.PlayOneShot(win_audioClip, 0.4f);
        gameOver = true;


    }

}
