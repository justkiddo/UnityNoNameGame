using UniRx;
using UnityEngine;
using Zenject;

namespace root
{
    public class GameplayInfo : IInitializable
    {
        public ReactiveProperty<float> SoundVolume { get; } = new FloatReactiveProperty();
        public ReactiveProperty<int> SavedCheckpoint { get; } = new IntReactiveProperty(0);
        
        public void Initialize()
        {
            SoundVolume.Value = PlayerPrefs.GetFloat(BaseIds.SoundVolumeKey);
            SavedCheckpoint.Value = PlayerPrefs.GetInt(BaseIds.SavedCheckpointKey);
        
            SoundVolume.Subscribe(_ => OnSoundVolumeChange());
            SavedCheckpoint.Subscribe(_ => OnSavCheckpointChange());
        
        }

        private void OnSavCheckpointChange()
        {
            PlayerPrefs.SetInt(BaseIds.SavedCheckpointKey, SavedCheckpoint.Value);
            PlayerPrefs.Save();
        }



        private void OnSoundVolumeChange()
        {
            PlayerPrefs.SetFloat(BaseIds.SoundVolumeKey , SoundVolume.Value);
            PlayerPrefs.Save();
        }
    }
}