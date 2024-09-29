using GameFramework.DataTable;
using GameFramework.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public static class SoundExtension
    {
        public static int? PlaySound(this SoundComponent soundComponent, int soundId, Entity bindingEntity = null, object userData = null)
        {
            IDataTable<DRSound> dtSound = GameEntry.DataTable.GetDataTable<DRSound>();
            DRSound drSound = dtSound.GetDataRow(soundId);
            if (drSound == null)
            {
                Log.Warning("Can not load sound '{0}' from data table.", soundId.ToString());
                return null;
            }

            PlaySoundParams playSoundParams = PlaySoundParams.Create();
            playSoundParams.Priority = drSound.Priority;
            playSoundParams.Loop = drSound.Loop;
            playSoundParams.VolumeInSoundGroup = drSound.Volume/* * GameEntry.SaveLoad.Voice*/;
            playSoundParams.SpatialBlend = drSound.SpatialBlend;
            if(drSound.Group=="BGM")
                GameEntry.Sound.GetSoundGroup("BGM").StopAllLoadedSounds();
            return soundComponent.PlaySound(AssetUtility.GetMusicAsset(drSound.AssetName), drSound.Group, GameEntry.Utils.SoundSort, playSoundParams, bindingEntity != null ? bindingEntity.Entity : null, userData);
        }

        public static int? PlaySound(this SoundComponent soundComponent, string soundAssetName,string soundGroup="BGM", Entity bindingEntity = null, object userData = null)
        {
            GameEntry.Sound.GetSoundGroup("BGM").StopAllLoadedSounds();

            PlaySoundParams playSoundParams = PlaySoundParams.Create();
            playSoundParams.Priority = 1;
            playSoundParams.Loop = true;
            playSoundParams.VolumeInSoundGroup = 1/* * GameEntry.SaveLoad.Voice*/;
            playSoundParams.SpatialBlend = 1;
            return soundComponent.PlaySound(AssetUtility.GetMusicAsset(soundAssetName), soundGroup, GameEntry.Utils.SoundSort, playSoundParams, bindingEntity != null ? bindingEntity.Entity : null, userData);
        }
    }
}
