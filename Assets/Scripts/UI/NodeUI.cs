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
            this._target = target;

            transform.position = this._target.GETBuildPosition();

            if (!this._target.isUpgraded)
            {
                upgradeCost.text = "$" + this._target.turretBlueprint.upgradeCost;
                upgradeButton.interactable = true;
            }
            else
            {
                upgradeCost.text = "DONE";
                upgradeButton.interactable = false;
            }

            sellAmount.text = "$" + this._target.turretBlueprint.GetSellAmount();

            ui.SetActive(true);
        }
        public void Hide()
        {
            //Debug.Log("hide");
            ui.SetActive(false);
        }
        public void Upgrade()
        {
            _target.UpgradeTurret();
            BuildManager.Instance.DeselectNode();
        }
        public void Sell()
        {
            _target.SellTurret();
            BuildManager.Instance.DeselectNode();
        }
    }
}
