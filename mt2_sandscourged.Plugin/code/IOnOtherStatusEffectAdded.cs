using System;
using System.Collections.Generic;
using System.Text;

// Taken from Conductor
// https://github.com/Monster-Train-2-Modding-Group/Conductor/blob/main/Conductor/Interfaces/IOnOtherStatusEffectAdded.cs
namespace mt2_sandscourged.Plugin
{
    public interface IOnOtherStatusEffectAdded
    {
        /// <summary>
        /// Called after the amount of status given is modified by dualism, card traits, and room modifiers.
        /// </summary>
        /// <param name="myStacks">Number of stacks of the status effect class</param>
        /// <param name="statusId">Status effect id being added</param>
        /// <param name="numStacks">Number of stacks of status id being added</param>
        /// <returns>The modified amount of status effect to add</returns>
        int OnOtherStatusEffectBeingAdded(int myStacks, string statusId, int numStacks);
    }
}