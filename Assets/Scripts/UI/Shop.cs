using Managers;
using UnityEngine;

namespace UI
{
    public class Shop : MonoBehaviour
    {
        public TurretBlueprint standardTurret;
        public TurretBlueprint missleLauncher;
        public TurretBlueprint leaserBeamer;

        private BuildManager _buildManager;

        private void Start()
        {
            this._buildManager = BuildManager.Instance;
        }

        public void SelectStandardTurret()
        {
            _buildManager.SelectTurretToBuild(standardTurret);
        }

        public void SelectMissleLauncherItem()
        {
            _buildManager.SelectTurretToBuild(missleLauncher);
        }

        public void SelectLeaserBeamer()
        {
            _buildManager.SelectTurretToBuild(leaserBeamer);
        }
    }
}