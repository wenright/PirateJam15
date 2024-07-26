using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    [ReadOnly] public float damage;
    [ReadOnly] public Damageable damagee;
    [ReadOnly] public Damageable.DamageType damageType;
    public TextMeshPro inMaskText;
    public TextMeshPro outMaskText;

    private readonly Dictionary<Damageable.DamageType, Color> damageTypeToColor = new()
    {
        { Damageable.DamageType.DEFAULT, new Color(0, 0, 0) },
        { Damageable.DamageType.CRIT, new Color(218.0f/255, 65.0f/255, 103.0f/255) },
        { Damageable.DamageType.FIRE, new Color(255.0f/255, 65.0f/255, 103.0f/255) },
        { Damageable.DamageType.POISON, new Color(97.0f/255, 231.0f/255, 134.0f/255) },
        { Damageable.DamageType.ICE, new Color(32.0f/255, 164.0f/255, 243.0f/255) },
    };

    private float startHeight = 0.5f;
    private float endHeight = 0.5f;
    private float angle = 5f;
    private float variance = 0.75f;
    private float duration = 0.75f;
    private Vector3 startPos;
    private float dotFontSize = 10;

    public void SetDamage(float damage, Damageable damagee, Damageable.DamageType damageType)
    {
        this.damage = damage;
        this.damagee = damagee;
        this.damageType = damageType;

        startPos = transform.position + Vector3.up * startHeight;

        UpdateText();
    }

    public void UpdateDamage(float additionalDamage)
    {
        damage += additionalDamage;

        UpdateText();
    }

    private void UpdateText()
    {
        // Show tenths decimal place if < 10
        string damageText;
        if (damage < 10)
        {
            damageText = (Mathf.Round(damage * 10f) / 10f).ToString();
        }
        else
        {
            damageText = Mathf.RoundToInt(damage).ToString();
        }

        inMaskText.text = damageText;
        outMaskText.text = damageText;

        if (damageTypeToColor.TryGetValue(damageType, out var value))
        {
            inMaskText.color = value;
        }

        // DOT damage should have a smaller font size
        if (damageType != Damageable.DamageType.DEFAULT)
        {
            inMaskText.fontSize = dotFontSize;
            outMaskText.fontSize = dotFontSize;
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
