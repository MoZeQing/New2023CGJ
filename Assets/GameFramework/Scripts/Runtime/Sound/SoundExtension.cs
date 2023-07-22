using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Sound;
using GameFramework;
using GameFramework.DataTable;

namespace GameMain
{
    public static class SoundExtension
    {
     /// <summary>
     /// …Ë÷√“Ù¿÷◊È“Ù¡ø
     /// </summary>
     /// <param name="soundComponent"></param>
     /// <param name="soundGroupName"></param>
     /// <param name="volume"></param>
        public static void SetVolume(this SoundComponent soundComponent, string soundGroupName, float volume)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return;
            }

            soundGroup.Volume = volume;
        }
    }

}
