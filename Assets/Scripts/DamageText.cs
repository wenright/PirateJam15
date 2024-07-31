using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    [ReadOnly] public float damage;
    [ReadOnly] public Damageable damagee;
    [ReadOnly] public Damageable.DamageType damageType;
    public TextMeshPro text;
    public Vector3 startPos;

    private readonly Dictionary<Damageable.DamageType, Color> damageTypeToColor = new()
    {
        { Damageable.DamageType.DEFAULT, new Color(229/255.0f, 206/255.0f, 180/255.0f) },
        { Damageable.DamageType.CRIT, new Color(207/255.0f, 138/255.0f, 203/255.0f) },
        { Damageable.DamageType.FIRE, new Color(180/255.0f, 82/255.0f, 82/255.0f) },
        { Damageable.DamageType.POISON, new Color(136/255.0f, 176/255.0f, 96/255.0f) },
        { Damageable.DamageType.ICE, new Color(104/255.0f, 194/255.0f, 211/255.0f) },
    };

    private float startHeight = 1.0f;
    private float endHeight = 0.5f;
    private float angle = 5f;
    private float variance = 0.75f;
    private float duration = 0.75f;
    private float dotFontSize = 10;

    public void SetDamage(float damage, Damageable damagee, Damageable.DamageType damageType)
    {
        this.damage = damage;
        this.damagee = damagee;
        this.damageType = damageType;

        startPos = transform.position + Vector3.up * startHeight;

        RefreshText();
    }

    public void UpdateDamage(float additionalDamage, Vector3 position)
    {
        damage += additionalDamage;

        startPos = position + Vector3.up * startHeight;

        RefreshText();
    }

    public void RefreshText(bool roundNumber = true)
    {
        // Show tenths decimal place if < 10. Can be disabled to not replace number if using a string instead
        if (roundNumber)
        {
            string damageText;
            if (damage < 10)
            {
                damageText = (Mathf.Round(damage * 10f) / 10f).ToString();
            }
            else
            {
                damageText = Mathf.RoundToInt(damage).ToString();
            }
            
            text.text = damageText;
        }

        // Determine which color should be used
        if (damageTypeToColor.TryGetValue(damageType, out var value))
        {
            text.color = value;
        }

        // DOT damage should have a smaller font size, and should appear to the side so it doesn't overlap normal dmg
        if (damageType != Damageable.DamageType.DEFAULT)
        {
            text.fontSize = dotFontSize;
            startPos += Vector3.right * 1.0f;
        }

        Vector3 endPos = startPos + new Vector3(0, endHeight + (variance * Random.Range(-0.5f, 0.5f)), 0);

        transform.DOKill();

        transform.rotation = Quaternion.Euler(0, 0, Random.Range(-angle, angle) * variance);
        transform.DORotate(new Vector3(0, 0, Random.Range(-angle, angle) * variance), duration).SetEase(Ease.OutQuad);

        transform.localScale = Vector3.one * 0.75f;
        transform.DOScale(Vector3.one, duration).SetEase(Ease.OutQuad);

        transform.position = startPos;
        transform.DOMove(endPos, duration).SetEase(Ease.OutQuad).onComplete += () =>
        {
            Destroy(gameObject);
        };
    }
}
