using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRoles
{
    public enum Role { Undefined, Soldier, SoldierAim, SoldierSpirit, SoldierStamina, SoldierStrength, Cook }

    public static Building.Type getBuildingTypeFromCharacterRole(Role characterRole)
    {
        switch(characterRole)
        {
            case Role.Soldier:
                float r = Random.value;
                if(r < 0.25f)
                {
                    return Building.Type.AimTraining;
                }
                else if (r < 0.5f)
                {
                    return Building.Type.SpiritTraining;
                }
                else if (r < 0.75f)
                {
                    return Building.Type.StaminaTraining;
                }
                else
                {
                    return Building.Type.StrengthTraining;
                }

            case Role.SoldierAim:
                return Building.Type.AimTraining;

            case Role.SoldierSpirit:
                return Building.Type.SpiritTraining;

            case Role.SoldierStamina:
                return Building.Type.StaminaTraining;

            case Role.SoldierStrength:
                return Building.Type.StrengthTraining;

            case Role.Cook:
                return Building.Type.Kitchen;

            default:
                return Building.Type.None;
        }

    }
}
