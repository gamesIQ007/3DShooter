using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Источник шума
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class NoiseAudioSource : MonoBehaviour
    {
        /// <summary>
        /// Источник звука
        /// </summary>
        private AudioSource audioSource;

        /// <summary>
        /// Звуковой файл
        /// </summary>
        public AudioClip clip { get { return audioSource.clip; } set { audioSource.clip = value; } }


        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }


        /// <summary>
        /// Воспроизвести
        /// </summary>
        public void Play()
        {
            audioSource.Play();

            foreach (var dest in Destructible.AllDestructibles)
            {
                if (dest is ISoundListener)
                {
                    (dest as ISoundListener).Heard(Vector3.Distance(transform.position, dest.transform.position));
                }
            }
        }
    }
}