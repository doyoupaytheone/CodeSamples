//Created by Justin Simmons

using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "CharacterType", menuName = "ScriptableObjects/ObjectTypes/CharacterType", order = 1)]
public class CharacterType : ScriptableObject
{
    public int characterTypeIndex;
    public int characterAttackPriorityIndex;
    public float characterHealth;
    public float characterSpeed;
    public string attackPriority;
    public GameObject zombieInfectionPrefab;
}

//EDITOR CLASS IS BEING USED TO OVERRIDE AND CUSTOMIZE THE DISPLAY OF THE SCRIPT IN THE INSPECTOR//
[CustomEditor(typeof(CharacterType))]
public class CharacterTypeEditor : Editor
{
    string[] typeOptions = new string[] { "Survivor", "Zombie" };
    string[] priorityOptions = new string[] { "Survivor", "Zombie" };

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var characterTypeScript = target as CharacterType;

        EditorGUILayout.Space();
        characterTypeScript.characterTypeIndex = EditorGUILayout.Popup("Character Type", characterTypeScript.characterTypeIndex, typeOptions, EditorStyles.popup);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Character Settings", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        characterTypeScript.characterAttackPriorityIndex = EditorGUILayout.Popup("Attack Priority", characterTypeScript.characterAttackPriorityIndex, priorityOptions, EditorStyles.popup);

        if (characterTypeScript.characterTypeIndex == 1)
        {
            characterTypeScript.characterHealth = EditorGUILayout.Slider(new GUIContent("Character Health", "Maximum health of the character."), characterTypeScript.characterHealth, 1f, 1000f);
            characterTypeScript.characterSpeed = EditorGUILayout.Slider(new GUIContent("Character Speed", "The speed at which the character moves."), characterTypeScript.characterSpeed, 1f, 50f);
            characterTypeScript.zombieInfectionPrefab = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Zombie Infection Prefab", "The prefab that spawns when a survivor is infected."), characterTypeScript.zombieInfectionPrefab, typeof(GameObject), false);
        }

        if (GUI.changed)
            EditorUtility.SetDirty(characterTypeScript);

        serializedObject.ApplyModifiedProperties();
    }
}