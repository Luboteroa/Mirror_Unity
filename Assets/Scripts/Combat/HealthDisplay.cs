using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private GameObject healthBarParent;
    [SerializeField] private Image healthBarImage;

    private void Awake()
    {
        _health.ClientOnHealthUpdated += HandleHealthUpdate;
    }

    private void OnDestroy()
    {
        _health.ClientOnHealthUpdated -= HandleHealthUpdate;
    }

    private void HandleHealthUpdate(int currentHealth, int maxHealth)
    {
        healthBarImage.fillAmount = (float)currentHealth/maxHealth;
    }
}
