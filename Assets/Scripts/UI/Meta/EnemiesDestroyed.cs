using DG.Tweening;
using TMPro;
using UnityEngine;

public class EnemiesDestroyed : MonoBehaviour
{
    [SerializeField] private float _size1;
    [SerializeField] private float _size2;
    [SerializeField] private float _blinkDelay;

    private string _youKilled = "рш смхврнфхк <size=120%>";
    private string _enemies = "<size=100%> лнмярп";
    private string _ending1 = "ю!";
    private string _ending2 = "нб!";
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();

        int enemiesDestroyed = ServiceLocator.Get<GameDataService>().EnemiesKilled;

        if (enemiesDestroyed == 0)
        {
            _text.text = "";
        }
        else
        {
            _text.text = GetText(enemiesDestroyed);
        }

        DOTween.Sequence()
            .Append(transform.DOScale(_size2, _blinkDelay))
            .SetLoops(-1, LoopType.Yoyo);
    }

    private string GetText(int enemiesDestroyed)
    {
        string result = _youKilled + enemiesDestroyed + _enemies;

        if (enemiesDestroyed % 100 > 10 && enemiesDestroyed % 100 < 20)
        {
            result += _ending2;
        }
        else if (enemiesDestroyed % 10 == 1)
        {
            result += _ending1;
        }
        else
        {
            result += _ending2;
        }

        return result;
    }

    private void OnDestroy()
    {
        DOTween.Kill(this);
    }
}
