using System.Collections;
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
    
    [PunRPC]
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

        StartCoroutine(BuildTurret(_buildManager.GetTurretToBuild()));
    }

    [PunRPC]
    public IEnumerator UpgradeTurret()
    {
        if (PlayerStats.Money < turretBlueprint.upgradeCost)
            yield break;

        PlayerStats.Money -= turretBlueprint.upgradeCost;

        PhotonNetwork.Destroy(this.turret);
        
        GameObject turret = PhotonNetwork.Instantiate(turretBlueprint.upgradedPrefab.name, GetBuildPosition(), Quaternion.identity);
        
        this.turret = turret;
        
        GameObject effect = PhotonNetwork.Instantiate(_buildManager.buildEffect.name, GetBuildPosition(), Quaternion.identity);
        yield return new WaitForSeconds(5f);
        PhotonNetwork.Destroy(effect);
        
        isUpgraded = true;
    }

    [PunRPC]
    private IEnumerator BuildTurret(TurretBlueprint blueprint)
    {
        if (PlayerStats.Money < blueprint.cost) 
            yield break;
        
        PlayerStats.Money -= blueprint.cost;
        GameObject turret = PhotonNetwork.Instantiate(blueprint.prefab.name, GetBuildPosition(), Quaternion.identity);
        
        this.turret = turret;

        turretBlueprint = blueprint;

        GameObject effect =
            PhotonNetwork.Instantiate(_buildManager.buildEffect.name, GetBuildPosition(), Quaternion.identity);
        yield return new WaitForSeconds(5f);
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

    [PunRPC]
    public IEnumerator SellTurret()
    {
        PlayerStats.Money += turretBlueprint.GetSellAmount();

        PhotonNetwork.Destroy(turret);
        turretBlueprint = null;
        isUpgraded = false;
        
        GameObject effect = Instantiate(_buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        yield return new WaitForSeconds(5f);
        PhotonNetwork.Destroy(effect);
    }

    [PunRPC]
    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!_buildManager.CanBuild)
            return;

        _rend.material.color = _buildManager.HasMoney ? hoverColor : notEnoughMoneyColor;
    }

    [PunRPC]
    private void OnMouseExit()
    {
        _rend.material.color = startColor;
    }
}