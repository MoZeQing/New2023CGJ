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
            playSoundParams.VolumeInSoundGroup = drSound.Volume;
            playSoundParams.SpatialBlend = drSound.SpatialBlend;
            return soundComponent.PlaySound(AssetUtility.GetMusicAsset(drSound.AssetName), drSound.Group, 30, playSoundParams, bindingEntity != null ? bindingEntity.Entity : null, userData);
        }
    }
}
