using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement playerMovement = new PlayerMovement();
    [SerializeField] private PlayerLevel playerLevel = new PlayerLevel();
    [SerializeField] private Image tutor;
    [Space] 
    [SerializeField] private UILose uiLose;
    [SerializeField] private UIWin uiWin;

    private bool _isMove;

    public void Init()
    {
        _isMove = false;
        animator.SetBool("Lose", false);
        animator.SetBool("Move", false);
        animator.SetBool("Win", false);
        uiLose.Deactivate();
        uiWin.Deactivate();
        playerMovement.Init(transform, animator);
        playerLevel.Init(animator);
        playerLevel.CanvasProgress.gameObject.SetActive(false);
        tutor.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        playerLevel.OnLose += Lose;
    }

    private void OnDisable()
    {
        playerLevel.OnLose -= Lose;
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
        playerLevel.CanvasProgress.gameObject.SetActive(true);
        _isMove = true;
    }

    private void Lose()
    {
        _isMove = false;
        animator.SetBool("Lose", true);
        playerLevel.CanvasProgress.gameObject.SetActive(false);
        uiLose.Activate();
    }

    private void Win()
    {
        _isMove = false;
        animator.SetBool("Win", true);
        playerLevel.CanvasProgress.gameObject.SetActive(false);
        uiWin.Activate();
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
        
        if (other.TryGetComponent(out DoorTrigger doorTrigger))
        {
            if (playerLevel.CurrentLevel >= doorTrigger.Level)
            {
                doorTrigger.Open();
            }
            else
            {
                Win();
            }
        }
    }
}
