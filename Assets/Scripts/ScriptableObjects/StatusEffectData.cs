using UnityEngine;

[CreateAssetMenu(fileName = "NewStatusEffect", menuName = "Status Effect Data")]
public class StatusEffectData : ScriptableObject
{
    public enum Type
    {
        MoveSpeed,
        Fire,
        Poison,
        Ice,
    }

    public Type type;
    public float value;
    public float duration;
    public bool isStackable = false;
    public int stacks = 1;

    [AssetPath.Attribute(typeof(Sprite))]
    public string spritePath;

    [HideInInspector]
    public Sprite sprite;

    private void OnEnable()
    {
        sprite = AssetPath.Load<Sprite>(spritePath);
    }
}
