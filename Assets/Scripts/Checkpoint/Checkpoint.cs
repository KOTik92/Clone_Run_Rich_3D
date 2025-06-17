using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;

    public void Activate()
    {
        animator.SetBool("Activate", true);
        audioSource.Play();
    }
}
