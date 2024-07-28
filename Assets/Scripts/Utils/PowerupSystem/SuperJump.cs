using UnityEngine;

namespace Utils.PowerupSystem
{
    [CreateAssetMenu(menuName = "Powerups/Superjump")]
    public class SuperJump : PowerUp
    {
        [SerializeField] float multiplyJump = 1.5f;
        [SerializeField] string powerupSequence = "QQE";
        bool isActive = false;

        public override void Apply(GameObject target)
        {
            if (isActive)
            {
                target.GetComponent<MovementControler>().stats.JumpPower *= multiplyJump;
            }
            else
            {
                target.GetComponent<MovementControler>().stats.JumpPower /= multiplyJump;
            }
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
