using ButchersGames;
using UnityEngine;
using UnityEngine.UI;

public class UILose : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private Button button;
    [SerializeField] private LevelManager levelManager;

    public void Activate()
    {
        panel.gameObject.SetActive(true);
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(levelManager.RestartLevel);
    }

    public void Deactivate()
    {
        panel.gameObject.SetActive(false);
    }
}
