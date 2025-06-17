using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement playerMovement = new PlayerMovement();
    [SerializeField] private PlayerLevel playerLevel = new PlayerLevel();
    [SerializeField] private Image tutor;

    private bool _isMove;

    private void Awake()
    {
        playerMovement.Init(transform, animator);
        playerLevel.Init(animator);
    }

    private void Update()
    {
        if (_isMove)
        {
            playerMovement.Move();
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartGame();
            }
        }
    }

    private void StartGame()
    {
        tutor.gameObject.SetActive(false);
        _isMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TurnTrigger turnTrigger))
        {
            playerMovement.StartRotation(turnTrigger.transform.position, turnTrigger.TurnAngle);
        }
        
        if (other.TryGetComponent(out AddMoneyTriggers addMoneyTriggers))
        {
            playerLevel.AddMoney(addMoneyTriggers.Money);
            addMoneyTriggers.Deactivate();
        }
        
        if (other.TryGetComponent(out RemoveMoneyTrigger removeMoneyTrigger))
        {
            playerLevel.RemoveMoney(removeMoneyTrigger.Money);
            removeMoneyTrigger.Deactivate();
        }
        
        if (other.TryGetComponent(out Checkpoint checkpoint))
        {
            checkpoint.Activate();
        }
    }

    public void Reset()
    {
        _isMove = false;
    }
}
