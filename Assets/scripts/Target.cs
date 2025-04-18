    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Target : MonoBehaviour
    {
        private Animator animator;
        private bool isHit = false;
        private AudioSource audioSource;
        public AudioClip fallSound;
        public AudioClip resetSound;
        private FPSController fpsController;
        private Timer timer;

    [Range(0f, 1f)] public float soundVolume = 0.8f;
        
       


    void Start()
        {
        animator = GetComponent<Animator>();

            if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        GameObject playerObj = GameObject.FindWithTag("Player");
        fpsController = playerObj.GetComponent<FPSController>();
        GameObject timerObj = GameObject.FindWithTag("Timer");
        timer = timerObj.GetComponent<Timer>();
    }

        public void Hit()
        {
            if (isHit) return; // ���� ��� ��������, ����������
            animator.SetTrigger("Fall");
            isHit = true;

            if (fallSound != null)
                audioSource.PlayOneShot(fallSound, soundVolume * 0.5f);

            if(timer.isActive == true)
        {
            fpsController.AddPoint();
            fpsController.UpdateScoreUI();
        }   
        
        
           

            StartCoroutine(ResetTargetAfterDelay(4f));
            // ��������� ���������, ����    ���� ���� ������� ������
            GetComponent<Collider>().enabled = false;

            
        }

        IEnumerator ResetTargetAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            // ���������� ������ � �������� ���������
            ResetTarget();
        }

        void ResetTarget()
        {
            animator.SetTrigger("Reset");

            if (resetSound != null)
                
                audioSource.PlayOneShot(resetSound, soundVolume * 0.7f);

          
            // �������� ���������
            GetComponent<Collider>().enabled = true;

            // ���������� ���� ���������
            isHit = false;
        }
}
