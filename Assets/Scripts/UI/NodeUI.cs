using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NodeUI : MonoBehaviour
    {
        public GameObject ui;

        private Node _target;

        public Text upgradeCost;
        public Button upgradeButton;

        public Text sellAmount;
        public void SetTarget(Node target)
        {
            _target = target;

            transform.position = _target.GetBuildPosition();

            if (!_target.isUpgraded)
            {
                upgradeCost.text = "$" + _target.turretBlueprint.upgradeCost;
                upgradeButton.interactable = true;
            }
            else
            {
                upgradeCost.text = "DONE";
                upgradeButton.interactable = false;
            }

            sellAmount.text = "$" + _target.turretBlueprint.GetSellAmount();

            ui.SetActive(true);
        }

        public void Hide()
        {
            ui.SetActive(false);
        }

        public void Upgrade()
        {
            StartCoroutine(_target.UpgradeTurret());
            BuildManager.Instance.DeselectNode();
        }

        public void Sell()
        {
            StartCoroutine(_target.SellTurret());
            BuildManager.Instance.DeselectNode();
        }
    }
}