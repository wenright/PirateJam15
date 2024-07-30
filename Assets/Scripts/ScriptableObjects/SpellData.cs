using System;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Projectile,
    Beam,
    Chain
}

[CreateAssetMenu(fileName = "NewSpellData", menuName = "Spell Data")]
public class SpellData : ScriptableObject
{
    [Header("Basics")]
    public string displayName;
    public string description;
    public Sprite icon;
    
    public WeaponType weaponType = WeaponType.Projectile;
    
    public AudioClip attackSound;
    
    [Header("Damage")]
    public float damage;
    public float attackCooldown = 1.0f;
    public float knockback = 0.0f;
    
    [Header("Effects")]
    public GameObject hitParticlePrefab;
    public Sprite projectileSprite;
    
    [Header("Projectile data")]
    public int numProjectiles = 1;
    [ShowIf("numProjectiles", 2)] public float multishotSpread = 10.0f;
    public float projectileSpread = 0.0f;
    public float projectileSpeed = 20.0f;
    public float projectileRotationSpeed = 0.0f;
    public bool burst = false;
    [ShowIf("burst", true)] public int burstAmount = 1;
    [ShowIf("burst", true)] public float burstRate = 0.1f;
    public bool homing = false;
    [ShowIf("homing", true), Tooltip("How quickly the projectile will turn towards the target")] public float homingStrength = 1.0f;
    [ShowIf("homing", true)] public float homingDistance = 3.0f;
    public bool piercing = false;
    
    public List<StatusEffectData> onHitStatusEffects;
}
