using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class Timer : MonoBehaviour
{   

    [Header("UI References")]
    public TextMeshProUGUI timerText;

    [Header("Settings")]
    public float resetTime = 10f;
    public bool showMilliseconds = false;

    private float currentTimer;
    private Coroutine timerCoroutine;
    public float pickupRadius = 1.5f;
    public bool isActive = false;
    private FPSController fpsController;


    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        fpsController = playerObj.GetComponent<FPSController>();
    }
    public void Update()
    {
        // Проверяем, находится ли игрок рядом и нажал ли E
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
            {
                StartTimer();
                break;
            }
    }
}
    //void OnEnable()
    //{
        
    //}

    void OnDisable()
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);
    }

    public void StartTimer()
    {
        fpsController.UpdateScoreUI();
        fpsController.resetScore();
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);
        isActive = true;
        currentTimer = resetTime;
        timerCoroutine = StartCoroutine(TimerCountdown());
    }

    IEnumerator TimerCountdown()
    {
        while (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            UpdateUI();
            yield return null;
        }

        OnTimerComplete();
    }

    void UpdateUI()
    {

        // Текстовое отображение
        if (showMilliseconds)
            timerText.text = $"{currentTimer:F1} сек";
        else
            timerText.text = Mathf.CeilToInt(currentTimer).ToString();
    }

    void OnTimerComplete()
    {
        timerText.text = "0";
        isActive = false;

     

        // Здесь можно добавить событие завершения
        // Например: EventManager.Instance.TargetReset();
    }
}