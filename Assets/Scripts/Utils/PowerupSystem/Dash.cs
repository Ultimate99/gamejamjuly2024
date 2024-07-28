using System.Collections;
using UnityEngine;

namespace Utils.PowerupSystem
{
    [CreateAssetMenu(menuName = "Powerups/Dash")]
    public class Dash : PowerUp
    {
        [SerializeField] string powerupSequence = "QEQ";
        bool isActive = false;

        public override void Apply(GameObject target)
        {
            target.GetComponent<MovementControler>().isDashingActive = isActive;
        }

        public override void ToggleActivation()
        {
            isActive = !isActive;
        }


        public override string getSequence()
        {
            return powerupSequence;
        }

        public override bool getIsActive()
        {
            return isActive;
        }

    }
}
