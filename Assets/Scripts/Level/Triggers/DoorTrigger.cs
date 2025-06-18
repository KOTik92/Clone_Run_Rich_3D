using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [Space]
    [SerializeField] private int level;

    public int Level => level;

    public void Open()
    {
        animator.SetBool("Open", true);
        audioSource.Play();
    }
}
