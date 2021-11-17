//Created by Justin Simmons

using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "WeaponType", menuName = "ScriptableObjects/ObjectTypes/WeaponType", order = 2)]
public class WeaponType : ScriptableObject
{
    public GameObject bulletPrefab;
    public bool doesBulletExplodeOnImpact;
    public float weaponRange;
    public float weaponAmmoCapacity;
    public float weaponFireTime;
    public float bulletSpeed;
    public float bulletDamage;
    public float explosionRadius;
    public float explosionDamage;
}

//EDITOR CLASS IS BEING USED TO OVERRIDE AND CUSTOMIZE THE DISPLAY OF THE SCRIPT IN THE INSPECTOR//
[CustomEditor(typeof(WeaponType))]
public class WeaponTypeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var weaponTypeScript = target as WeaponType;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Weapon Settings", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        weaponTypeScript.weaponRange = EditorGUILayout.Slider(new GUIContent("Weapon Range", "Effects the distance that the weapon will trigger and the bullet will travel."), weaponTypeScript.weaponRange, 0f, 100f);
        weaponTypeScript.weaponAmmoCapacity = EditorGUILayout.Slider(new GUIContent("Weapon Ammo Capacity", "The total number of bullets the weapon can hold."), weaponTypeScript.weaponAmmoCapacity, 0f, 100f);
        weaponTypeScript.weaponFireTime = EditorGUILayout.Slider(new GUIContent("Weapon Fire Time", "The time between firing a bullet in seconds."), weaponTypeScript.weaponFireTime, 0f, 10f);
        EditorGUI.indentLevel--;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Bullet Settings", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        weaponTypeScript.bulletSpeed = EditorGUILayout.Slider(new GUIContent("Bullet Speed", "The speed the bullet travels after being fired."), weaponTypeScript.bulletSpeed, 0f, 100f);
        weaponTypeScript.bulletDamage = EditorGUILayout.Slider(new GUIContent("Bullet Damage", "The amount of damage the bullet does on impact of an enemy."), weaponTypeScript.bulletDamage, 0f, 100f);
        weaponTypeScript.doesBulletExplodeOnImpact = EditorGUILayout.Toggle(new GUIContent("Does Bullet Explode on Impact?", "Decides if a bullet is explosive or not."), weaponTypeScript.doesBulletExplodeOnImpact);

        if (weaponTypeScript.doesBulletExplodeOnImpact)
        {
            EditorGUI.indentLevel++;
            weaponTypeScript.explosionRadius = EditorGUILayout.Slider(new GUIContent("Explosion Radius", "The radius of the bullet's explosion effects."), weaponTypeScript.explosionRadius, 1f, 100f);
            weaponTypeScript.explosionDamage = EditorGUILayout.Slider(new GUIContent("Explosion Damage", "The damage taken when effected by explosion."), weaponTypeScript.explosionDamage, 1f, 100f);
        }

        if (GUI.changed)
            EditorUtility.SetDirty(weaponTypeScript);

        serializedObject.ApplyModifiedProperties();
    }
}