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

        Destroy(this.turret);

        GameObject turret = InstantiateSM(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
        
        this.turret = turret;
        
        GameObject effect = InstantiateSM(_buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);
        
        isUpgraded = true;
    }

    private void BuildTurret(TurretBlueprint blueprint)
    {
        if (PlayerStats.Money < blueprint.cost)
            return;

        PlayerStats.Money -= blueprint.cost;
        GameObject turret = InstantiateSM(blueprint.prefab, GetBuildPosition(), Quaternion.identity);

        this.turret = turret;

        turretBlueprint = blueprint;

        GameObject effect = InstantiateSM(_buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        
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

    private void OnMouseExit()
    {
        _rend.material.color = startColor;
    }

    // For instantiating GameObjects weather the player is in multiplayer mode or Single-player
    private GameObject InstantiateSM(GameObject inst, Vector3 position, Quaternion rotation)
    { 
        return PhotonNetwork.IsConnected && PhotonNetwork.InRoom
            ? PhotonNetwork.Instantiate(inst.name, position, rotation)
            : Instantiate(inst, position, rotation);
        
    }
}