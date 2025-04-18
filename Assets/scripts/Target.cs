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
        [Range(0f, 1f)] public float soundVolume = 0.8f;

        
        void Start()
        {
            animator = GetComponent<Animator>();

            if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        public void Hit()
        {
            if (isHit) return; // Если уже поражена, игнорируем
            animator.SetTrigger("Fall");
            isHit = true;

            if (fallSound != null)
                audioSource.PlayOneShot(fallSound, soundVolume * 0.5f);

            StartCoroutine(ResetTargetAfterDelay(10f));
            // Отключаем коллайдер, чтоб    льзя было попасть дважды
            GetComponent<Collider>().enabled = false;

            
        }

        IEnumerator ResetTargetAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            // Возвращаем мишень в исходное состояние
            ResetTarget();
        }

        void ResetTarget()
        {
            animator.SetTrigger("Reset");

            if (resetSound != null)
                audioSource.PlayOneShot(resetSound, soundVolume * 0.7f);

            Debug.Log("АНИМЕЕЕ");
            // Включаем коллайдер
            GetComponent<Collider>().enabled = true;

            // Сбрасываем флаг попадания
            isHit = false;
        }
}
