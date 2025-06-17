using UnityEngine;
using UnityEngine.UI;

public class RemoveMoneyTrigger : MonoBehaviour
{
    [SerializeField] private int money;
    [SerializeField] private Button.ButtonClickedEvent onDeactivate;

    public int Money => money;
    
    public void Deactivate()
    {
        onDeactivate?.Invoke();
    }
}
