using UnityEngine;
using UnityEngine.UI;


public class LunarHealthBar : MonoBehaviour
{
    [SerializeField] private LunarHealth playerHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;

    private void Start()
    {
        totalhealthBar.fillAmount = playerHealth.currentHealth / 30;
    }
    private void Update()
    {
        currenthealthBar.fillAmount = playerHealth.currentHealth / 30;
    }
}
