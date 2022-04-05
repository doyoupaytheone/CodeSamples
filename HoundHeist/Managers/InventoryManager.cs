//Created by Justin Simmons

using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Collectible> ownedCollectibles = new List<Collectible>();
    public List<Weapon> ownedWeapons = new List<Weapon>();
    public Weapon currentWeapon;

    #region AccessInventory

    public void AddCollectibleToInventory(Collectible collectible) => ownedCollectibles.Add(collectible);

    public void AddWeaponToInventory(Weapon weapon) => ownedWeapons.Add(weapon);

    public void RemoveCollectibleFromInventory(Collectible collectible) => ownedCollectibles.Remove(collectible);

    public void RemoveWeaponFromInventory(Weapon weapon) => ownedWeapons.Remove(weapon);

    public bool CheckCollectibleInventoryFor(Collectible collectible)
    {
        return ownedCollectibles.Contains(collectible);
    }

    public bool CheckWeaponToInventoryFor(Weapon weapon)
    {
        return ownedWeapons.Contains(weapon);
    }

    #endregion
}
