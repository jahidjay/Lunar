using UnityEngine;
using UnityEngine.UI;


public class LunarHealthBar : MonoBehaviour
{
    [SerializeField] private LunarHealth playerHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;
    public float totalInitialHealth;
    private void Start()
    {
        totalInitialHealth = playerHealth.currentHealth;
        totalhealthBar.fillAmount = playerHealth.currentHealth / totalInitialHealth;
    }
    private void Update()
    {
        currenthealthBar.fillAmount = playerHealth.currentHealth / totalInitialHealth;
    }
}
