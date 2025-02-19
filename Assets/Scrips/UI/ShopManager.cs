using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public TurretBP standardTurret;
    public TurretBP CraneTurret;
    public TurretBP DogTurret;
    public TurretBP DragonTurret;
    public TurretBP GoatTurret;
    public TurretBP HorseTurret;

    BuildManager buildManager;
    int _stars;

    void Start()
    {
        buildManager = BuildManager.instance;
    }
    public void SelectStandardTuret()
    {
        Debug.Log("Standard Turret Selected");
        buildManager.SelectTurretToBuild(standardTurret);
    }
    public void SelectCraneTuret()
    {
        Debug.Log("Standard Turret Selected");
        buildManager.SelectTurretToBuild(CraneTurret);
    }

    public void SelectDogeTuret()
    {
        Debug.Log("Standard Turret Selected");
        buildManager.SelectTurretToBuild(DogTurret);
    }

    public void SelectDragonTuret()
    {
        Debug.Log("Standard Turret Selected");
        buildManager.SelectTurretToBuild(DragonTurret);
    }

    public void SelectGoatTuret()
    {
        Debug.Log("Standard Turret Selected");
        buildManager.SelectTurretToBuild(GoatTurret);
    }

    public void SelectHorseTuret()
    {
        Debug.Log("Standard Turret Selected");
        buildManager.SelectTurretToBuild(HorseTurret);
    }
}