using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI currencyText;

    [Header("Scene References")]
    [SerializeField] private String deathSceneName;
    
    [Header("Buy Shooter UI")]
    [SerializeField] private Button buyShooter;
    [SerializeField] private GameObject shooterPrefab;
    [SerializeField] private int shooterCost;
    
    [Header("Buy Blitzer UI")]
    [SerializeField] private Button buyBlitzer;
    [SerializeField] private GameObject blitzerPrefab;
    [SerializeField] private int blitzerCost;
    
    [Header("Buy Sniper UI")]
    [SerializeField] private Button buySniper;
    [SerializeField] private GameObject sniperPrefab;
    [SerializeField] private int sniperCost;
    
    [Header("Buy Sandpile UI")]
    [SerializeField] private Button buySandpile;
    [SerializeField] private GameObject sandpilePrefab;
    [SerializeField] private int sandpileCost;
    
    private bool _generateCurrency = true;
    private int _currency = 0;
    private int _health = 10;
    private bool _expendableReady = false;
    private GameObject _expendable;

    // Start is called before the first frame update
    void Start()
    {
        GlobalEventHandler.handler.damageTaken += CastleDamager;
        buyShooter.onClick.AddListener(ShooterBought);
        buyBlitzer.onClick.AddListener(BlitzerBought);
        buySniper.onClick.AddListener(SniperBought);
        buySandpile.onClick.AddListener(SandpileBought);
        StartCoroutine(CurrencyLoop());
        
        healthText.text = _health.ToString();
    }
    
    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        if (Input.GetMouseButtonDown(0) && _expendableReady)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            Instantiate(_expendable, pos, transform.rotation);
            _expendableReady = false;
            _expendable = null;
        }
    }

    public void DisableCurrency() {_generateCurrency = false;}
    public void EnableCurrency() {_generateCurrency = true;}
    
    void ShooterBought()
    {
        if (_currency >= shooterCost)
        {
            _currency -= shooterCost;
            GlobalEventHandler.selectedTower = shooterPrefab;
            GlobalEventHandler.handler.TowerSeleceted();
        }
    }
    void BlitzerBought()
    {
        if (_currency >= blitzerCost)
        {
            _currency -= blitzerCost;
            GlobalEventHandler.selectedTower = blitzerPrefab;
            GlobalEventHandler.handler.TowerSeleceted();
        }
    }

    void SniperBought()
    {
        if (_currency >= sniperCost)
        {
            _currency -= sniperCost;
            GlobalEventHandler.selectedTower = sniperPrefab;
            GlobalEventHandler.handler.TowerSeleceted();
        }
    }
    void SandpileBought()
    {
        if (_currency >= sandpileCost)
        {
            _currency -= sandpileCost;
            _expendable = sandpilePrefab;
            _expendableReady = true;
        }
    }

    void CastleDamager()
    {
        _health -= GlobalEventHandler.lastDamage;
        healthText.text = _health.ToString();

        if (_health == 0)
        {
            SceneManager.LoadScene("Main");
        }
    }

    IEnumerator CurrencyLoop()
    {
        while (_generateCurrency)
        {
            _currency++;
            currencyText.text = _currency.ToString();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
