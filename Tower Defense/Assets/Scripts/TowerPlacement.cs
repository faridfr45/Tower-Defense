using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    private Tower _placedTower;
    public GameObject towerObject;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.A)){
            HapusTower();
        }
    }

    // Fungsi yang terpanggil sekali ketika ada object Rigidbody yang menyentuh area collider
    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (_placedTower != null)
        {
            return;
        }

        Tower tower = collision.GetComponent<Tower> ();
        towerObject = collision.gameObject;
        
        if (tower != null)
        {
            tower.SetPlacePosition (transform.position);
            tower.transform.parent = this.transform;
            _placedTower = tower;
        }
    }
 
    // Kebalikan dari OnTriggerEnter2D, fungsi ini terpanggil sekali ketika object tersebut meninggalkan area collider
    private void OnTriggerExit2D (Collider2D collision)
    {
        if (_placedTower == null)
        {
            return;
        }
        
        _placedTower.SetPlacePosition (null);
    }

    public void HapusTower(){
        Destroy(towerObject);
        
        _placedTower = null;
    }
}
