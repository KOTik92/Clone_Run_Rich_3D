using ButchersGames;
using UnityEngine;
using UnityEngine.UI;

public class UIWin : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private Button button;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private AudioSource audioWin;

    public void Activate()
    {
        panel.gameObject.SetActive(true);
        audioWin.Play();
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(levelManager.NextLevel);
    }

    public void Deactivate()
    {
        panel.gameObject.SetActive(false);
    }
}
