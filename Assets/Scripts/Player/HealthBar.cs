using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerMovementController player;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    private void Start()
    {
        totalHealthBar.fillAmount = player.currentHealth / 10;
    }

    private void Update()
    {
        currentHealthBar.fillAmount = player.currentHealth / 10;
    }
}
