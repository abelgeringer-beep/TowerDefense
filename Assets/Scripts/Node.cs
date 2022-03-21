using Managers;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Color notEnoughMoneyColor;
    public Color startColor;
    public Vector3 positionOffset;

    [HideInInspector] public GameObject turret;
    [HideInInspector] public TurretBlueprint turretBlueprint;
    [HideInInspector] public bool isUpgraded;

    private Renderer _rend;
    private BuildManager _buildManager;

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (turret != null)
        {
            _buildManager.SelectNode(this);
            return;
        }

        if (!_buildManager.CanBuild)
            return;

        BuildTurret(_buildManager.GetTurretToBuild());
    }

    public void UpgradeTurret()
    {
        if (PlayerStats.Money < turretBlueprint.upgradeCost)
            return;

        PlayerStats.Money -= turretBlueprint.upgradeCost;

        PhotonNetwork.Destroy(this.turret);

        GameObject turret = PhotonNetwork.Instantiate(turretBlueprint.upgradedPrefab.name, GetBuildPosition(), Quaternion.identity);
        
        this.turret = turret;
        
        GameObject effect = PhotonNetwork.Instantiate(_buildManager.buildEffect.name, GetBuildPosition(), Quaternion.identity);
        System.Threading.Thread.Sleep(5000);
        PhotonNetwork.Destroy(effect);
        
        isUpgraded = true;
    }

    private void BuildTurret(TurretBlueprint blueprint)
    {
        if (PlayerStats.Money < blueprint.cost)
            return;

        PlayerStats.Money -= blueprint.cost;
        GameObject turret = PhotonNetwork.Instantiate(blueprint.prefab.name, GetBuildPosition(), Quaternion.identity);

        this.turret = turret;

        turretBlueprint = blueprint;

        GameObject effect = PhotonNetwork.Instantiate(_buildManager.buildEffect.name, GetBuildPosition(), Quaternion.identity);
        System.Threading.Thread.Sleep(5000);
        PhotonNetwork.Destroy(effect);
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    private void Start()
    {
        _rend = GetComponent<Renderer>();
        startColor = _rend.material.color;
        _buildManager = BuildManager.Instance;
    }

    public void SellTurret()
    {
        PlayerStats.Money += turretBlueprint.GetSellAmount();

        GameObject effect = Instantiate(_buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        System.Threading.Thread.Sleep(5000);
        PhotonNetwork.Destroy(effect);

        PhotonNetwork.Destroy(turret);
        turretBlueprint = null;
        isUpgraded = false;
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!_buildManager.CanBuild)
            return;

        _rend.material.color = _buildManager.HasMoney ? hoverColor : notEnoughMoneyColor;
    }

    private void OnMouseExit()
    {
        _rend.material.color = startColor;
    }
}