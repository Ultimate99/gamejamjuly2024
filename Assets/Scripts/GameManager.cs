using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.HealthSystem;
using UnityEngine.UI;
using Utils.PowerupSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Image> images;
    [SerializeField] private int amount;
    [SerializeField] Player player;
    [SerializeField] GameObject target;
    [SerializeField] List<PowerUp> powerUps;
    [SerializeField] TextMeshProUGUI text;

    string powerupActivation = "";

    private HeartContainer heartContainer;

    private void Start()
    {
        heartContainer = new HeartContainer(
            images.Select(image => new Heart(image)).ToList());

        player.Healed += (sender, args) => heartContainer.Replenish(args.Amount);
        player.Damaged += (sender, args) => heartContainer.Deplete(args.Amount);

        text.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            player.Heal(amount);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            player.Damage(amount);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (powerupActivation.Length >= 3 ) { powerupActivation = ""; }
            powerupActivation += "Q";

        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (powerupActivation.Length >= 3) { powerupActivation = ""; }
            powerupActivation += "E";
        }

        text.text = powerupActivation;

        PowerupHandle();
    }

    private void PowerupHandle()
    {
        if(powerupActivation == "")
        {
            return;
        }

        if (powerupActivation.Equals(powerUps[0].getSequence()))
        {
            powerUps[0].ToggleActivation();
            powerUps[0].Apply(target);
            if (powerUps[1].getIsActive()) { 
                powerUps[1].ToggleActivation();
                powerUps[1].Apply(target);
            }
            powerupActivation = "";
        }
        if (powerupActivation.Equals(powerUps[1].getSequence()))
        {
            powerUps[1].ToggleActivation();
            powerUps[1].Apply(target);
            if (powerUps[0].getIsActive())
            {
                powerUps[0].ToggleActivation();
                powerUps[0].Apply(target);
            }
            powerupActivation = "";
        }
        //if (powerupActivation.Equals(powerUps[2].getSequence()))
        //{
        //    powerUps[2].ToggleActivation();
        //    powerUps[2].Apply(target);
        //    powerupActivation = "";
        //}

    }
}
