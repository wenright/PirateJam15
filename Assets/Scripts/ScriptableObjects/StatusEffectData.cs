using UnityEngine;

[CreateAssetMenu(fileName = "NewStatusEffect", menuName = "Status Effect Data")]
public class StatusEffectData : ScriptableObject
{
    public Damageable.DamageType type;
    public float value;
    public float duration;
    public bool isStackable = false;
    public int stacks = 1;
}
