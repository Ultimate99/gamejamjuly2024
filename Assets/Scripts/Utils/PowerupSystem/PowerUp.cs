using UnityEngine;

namespace Utils.PowerupSystem
{
    public abstract class PowerUp : ScriptableObject
    {
        public abstract void Apply(GameObject target);
        public abstract void ToggleActivation();
        public abstract string getSequence();
        public abstract bool getIsActive();
    }
}