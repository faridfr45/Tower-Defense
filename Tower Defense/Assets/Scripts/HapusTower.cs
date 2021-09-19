using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapusTower : MonoBehaviour
{
    public TowerPlacement towerPlacement;

    private void OnMouseDown() {
        towerPlacement.HapusTower();
    }

}
