using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GunUI : MonoBehaviour
{
    private TMP_Text _tmp;
    private int _maxBullets;
    private PlayerGun _playerGun;
    
    // Start is called before the first frame update
    private void Start()
    {
        _tmp = GetComponent<TMP_Text>();
        _playerGun = GameManager.Instance.GetPlayer().GetComponent<PlayerGun>();
        _maxBullets = _playerGun.GetMaxBullets();
        _tmp.text = _maxBullets + "/" + _maxBullets;
    }

    public void SetCurrentBullets(int bullets)
    {
        _maxBullets = _playerGun.GetMaxBullets();
        _tmp.text = bullets + "/" + _maxBullets;
    }
}
