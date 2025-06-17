using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct Levels
{
    public int minMoney;
    public int numberAnim;
    public GameObject skin;
    public string text;
}

[Serializable]
public class PlayerLevel
{ 
    [SerializeField] private Image progressFillImage;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Levels[] levels;
    [SerializeField] private int startingLevel = 1;
    [Space]
    [SerializeField] private AudioClip audioAddMoney;
    [SerializeField] private AudioClip audioRemoveMoney;
    [SerializeField] private AudioSource audioSource;
    [Space] 
    [SerializeField] private ParticleSystem particleAddMoney;
    [SerializeField] private ParticleSystem particleRemoveMoney;
    
    private int _currentLevel;
    private int _currentMoney;
    private Animator _animator;

    public void Init(Animator animator)
    {
        _animator = animator;
        _currentLevel = Mathf.Clamp(startingLevel, 0, levels.Length - 1);
        _currentMoney = levels[_currentLevel].minMoney;
        
        UpdateSkinAndAnimation();
        UpdateProgressBar();
    }

    private void UpdateSkinAndAnimation()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].skin != null)
            {
                levels[i].skin.SetActive(i == _currentLevel);
            }
        }
        
        if (levelText != null)
        {
            levelText.text = levels[_currentLevel].text;
        }
        
        if (_animator != null)
        {
            _animator.SetInteger("Stage", levels[_currentLevel].numberAnim);
        }
    }

    private void UpdateProgressBar()
    {
        if (progressFillImage == null) return;
        
        if (IsMaxLevel())
        {
            progressFillImage.fillAmount = 1f;
            return;
        }
        
        float progress = Mathf.InverseLerp(
            levels[_currentLevel].minMoney,
            levels[_currentLevel + 1].minMoney,
            _currentMoney);
        
        progressFillImage.fillAmount = progress;
    }

    public void AddMoney(int money)
    {
        audioSource.PlayOneShot(audioAddMoney);
        particleAddMoney.Play();
        
        if (money <= 0) return;
        if (IsMaxLevel()) return;

        int newMoney = _currentMoney + money;
        int nextLevelThreshold = levels[_currentLevel + 1].minMoney;
        
        if (newMoney < nextLevelThreshold)
        {
            _currentMoney = newMoney;
        }
        else
        {
            _currentLevel++;
            int remains = newMoney - nextLevelThreshold;
            _currentMoney = nextLevelThreshold + remains;
            
            while (!IsMaxLevel() && _currentMoney >= levels[_currentLevel + 1].minMoney)
            {
                _currentMoney -= levels[_currentLevel + 1].minMoney;
                _currentLevel++;
            }
            
            UpdateSkinAndAnimation();
        }
        
        UpdateProgressBar();
    }

    public void RemoveMoney(int money)
    {
        audioSource.PlayOneShot(audioRemoveMoney);
        particleRemoveMoney.Play();
        if (money <= 0) return;

        int newMoney = _currentMoney - money;
        int currentLevelThreshold = levels[_currentLevel].minMoney;
        
        if (newMoney >= currentLevelThreshold)
        {
            _currentMoney = newMoney;
        } 
        else
        {
            _currentLevel--;
            _currentMoney = levels[_currentLevel + 1].minMoney - (currentLevelThreshold - newMoney);
            
            while (!IsMinLevel() && _currentMoney < levels[_currentLevel].minMoney)
            {
                _currentLevel--;
                _currentMoney += levels[_currentLevel + 1].minMoney;
            }
            
            UpdateSkinAndAnimation();
        }
        
        UpdateProgressBar();
    }
    
    private bool IsMaxLevel() => _currentLevel >= levels.Length - 1;
    private bool IsMinLevel() => _currentLevel <= 0;
}
