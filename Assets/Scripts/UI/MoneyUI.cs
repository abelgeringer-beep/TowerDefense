using TMPro;
using UnityEngine;

namespace UI
{
    public class MoneyUI : MonoBehaviour
    {
        public TextMeshProUGUI moneyText;

        public void Update()
        {
            moneyText.text = "$" + PlayerStats.Money;
        }
    }
}