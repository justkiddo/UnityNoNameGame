using UnityEngine;

namespace root
{
    public class AudioSystem : MonoBehaviour
    {
        [SerializeField] private AudioClip gameplayMusicOne;
        [SerializeField] private AudioClip gameplayMusicTwo;
        [SerializeField] private AudioClip jumpSound;
        [SerializeField] private AudioClip missedAttackSound;
        [SerializeField] private AudioClip attackSound;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.PlayOneShot(gameplayMusicOne, 1f);
        }

        public void Jump()
        {
            _audioSource.PlayOneShot(jumpSound, 0.6f);
        }

        public void Attack()
        {
            _audioSource.PlayOneShot(attackSound, 0.6f);
        }
        
        public void MissedAttack()
        {
            _audioSource.PlayOneShot(missedAttackSound, 0.6f);
        }
        
    }
}