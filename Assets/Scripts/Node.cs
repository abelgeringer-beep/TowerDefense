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
    [HideInInspector] public bool isUpgraded = false;

    private Renderer _rend;
    BuildManager _buildManager;

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

        //destroy old one 
        Destroy(this.turret);

        //build new one
        GameObject turret = PhotonNetwork.IsConnected 
            ? PhotonNetwork.Instantiate(turretBlueprint.upgradedPrefab.name, GetBuildPosition(), Quaternion.identity)
            : Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
        
        this.turret = turret;
        
        GameObject effect = PhotonNetwork.IsConnected
            ? PhotonNetwork.Instantiate(_buildManager.buildEffect.name, GetBuildPosition(), Quaternion.identity)
            : Instantiate(_buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        isUpgraded = true;
    }

    private void BuildTurret(TurretBlueprint blueprint)
    {
        if (PlayerStats.Money < blueprint.cost)
            return;

        PlayerStats.Money -= blueprint.cost;
        GameObject turret = PhotonNetwork.IsConnected 
            ? PhotonNetwork.Instantiate(blueprint.prefab.name, GetBuildPosition(), Quaternion.identity)
            : Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);

        this.turret = turret;

        turretBlueprint = blueprint;

        GameObject effect = PhotonNetwork.IsConnected
            ? PhotonNetwork.Instantiate(_buildManager.buildEffect.name, GetBuildPosition(), Quaternion.identity)
            : Instantiate(_buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        
        Destroy(effect, 5f);
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
        Destroy(effect, 5f);

        Destroy(turret);
        turretBlueprint = null;
        isUpgraded = false;
    }

    void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!_buildManager.CanBuild)
            return;

        if (_buildManager.HasMoney)
            _rend.material.color = hoverColor;

        else
            _rend.material.color = notEnoughMoneyColor;
    }

    void OnMouseExit()
    {
        _rend.material.color = startColor;
    }
}