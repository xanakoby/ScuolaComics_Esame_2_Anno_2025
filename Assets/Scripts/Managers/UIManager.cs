using DesignPatterns.Generics;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>, ISubscriber
{
    [Header("Turret Buttons")]
    public List<TurretButton> turretButtons;
    [SerializeField] TextMeshProUGUI playerCoins;

    [Header("Turret Panel")]
    [SerializeField] GameObject turretPanel;
    [SerializeField] TextMeshProUGUI turretInfo;
    public Button startGame;
    [SerializeField] Button upgradeTurret;
    [SerializeField] Button sellTurret;

    public override void Awake()
    {
        base.Awake();

        UnshowTurretStats();

        Publisher.Subscribe(this, typeof(TurretInfoMessage));
    }

        private void Start()
    {
        UpdateTurretButtons();
    }

    private void Update()
    {
        // Controlla e aggiorna i pulsanti ogni frame (opzionale, ma semplice)
        // TODO: si potrebbe gestire meglio usando i DesignPattern...
        UpdateTurretButtons();
        UpdatePlayerCoins();
    }

    private void UpdatePlayerCoins()
    {
        playerCoins.text = $"{GameManager.Instance.CurrentCoins}";
    }

    public void UpdateTurretButtons()
    {
        int playerCoins = GameManager.Instance.CurrentCoins;

        foreach (var button in turretButtons)
        {
            button.UpdateButtonState(playerCoins);
        }
    }

    //mi mostro trammite un messaggio
    public void ShowTurretStats()
    {
        turretPanel.SetActive(true);
    }
    public void UnshowTurretStats()
    {
        turretPanel.SetActive(false);
    }

    public void OnPublish(IPublisherMessage message)
    {
        if (message is TurretInfoMessage _turretInfoMessage)
        {
            ShowTurretStats();


            turretInfo.text = 
                "Costo: " + _turretInfoMessage.cost.ToString() + "\n" +
                "Danno: " + _turretInfoMessage.bulletsDamage.ToString() + "\n" +
                "Fire Rate: " + _turretInfoMessage.fireRate.ToString() + "\n"+ 
                "Livello Upgrade: "
                ;
        }
        else if (message is SellTurretMessage)
        {
            UnshowTurretStats();
        }
    }

    public void OnDisableSubscriber()
    {
        Publisher.Unsubscribe(this, typeof(TurretInfoMessage));
    }
    private void OnDestroy()
    {
        OnDisableSubscriber();
    }
}
