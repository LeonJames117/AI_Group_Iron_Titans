using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RoundSystem : MonoBehaviour
{
    [SerializeField] Vector3 spawnPoint;
    [SerializeField] Canvas roundUI;
    TextMeshProUGUI roundText;

    [SerializeField] EnemyWave[] waves;
    [SerializeField] float waveRestTime;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip horn_audioClip;

    int wave = 0;
    bool waveComplete = true;

    // Start is called before the first frame update
    void Start()
    {
        roundText = roundUI.GetComponentInChildren<TextMeshProUGUI>();
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
    }

    void WaveComplete() 
    {
        waveComplete = true;
    }

    IEnumerator StartRound() 
    {
        waveComplete = false;
        wave++;
        roundUI.gameObject.SetActive(true);
        roundText.text = "ROUND " + wave.ToString();
        //spawn enemies
        audioSource.PlayOneShot(horn_audioClip, 0.4f);
        yield return new WaitForSeconds(2.0f);

        roundUI.gameObject.SetActive(false);
    }

    [System.Serializable]
    public class EnemyWave
    {
        [SerializeField] GameObject[] enemy_types;
        [SerializeField] float[] enemy_numbers;
    }
}
