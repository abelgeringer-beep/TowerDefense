using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MoneyUI : MonoBehaviour
    {
        public Text moneyText;

        public void Update()
        {
            moneyText.text = "$" + PlayerStats.Money.ToString();
        }
    }
}
