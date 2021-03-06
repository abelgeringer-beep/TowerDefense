using UI;
using UnityEngine;

namespace Managers
{
    public class BuildManager : MonoBehaviour
    {
        public static BuildManager Instance;

        public GameObject buildEffect;

        private Node _selectedNode;
        private TurretBlueprint _turretToBuild;

        public NodeUI nodeUI;

        public GameObject sellEffect;

        public bool CanBuild => _turretToBuild != null; //property
        public bool HasMoney => PlayerStats.Money >= _turretToBuild.cost;

        private void Awake()
        {
            if (Instance != null)
            {
                return;
            }

            Instance = this;
        }

        public void SelectNode(Node node)
        {
            if (_selectedNode == node) // if you click the turret again the UI over it hides
            {
                DeselectNode();
                return;
            }

            _selectedNode = node;
            _turretToBuild = null;

            nodeUI.SetTarget(node);
        }

        public void DeselectNode()
        {
            _selectedNode = null;
            nodeUI.Hide();
        }

        public void SelectTurretToBuild(TurretBlueprint turret)
        {
            _turretToBuild = turret;
            DeselectNode();
        }

        public TurretBlueprint GetTurretToBuild()
        {
            return _turretToBuild;
        }
    }
}