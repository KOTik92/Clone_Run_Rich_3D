using UnityEngine;

public class TurnTrigger : MonoBehaviour
{
    [SerializeField] float turnAngle = 90f;
    
    public float TurnAngle => turnAngle;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);
    }
}
