using UniRx;
using UnityEngine;
using Zenject;

namespace root
{
    public class GameplayInfo : IInitializable
    {
        public ReactiveProperty<float> SoundVolume { get; } = new FloatReactiveProperty();
        public ReactiveProperty<int> CurrentCheckpoint { get; } = new IntReactiveProperty(0);
        public ReactiveProperty<int> SavedCheckpoint { get; } = new IntReactiveProperty(0);
    
        private const string SoundVolumeKey = "SOUND.VOLUME";
        private const string CurrentCheckpointKey = "CURRENT.CHECKPOINT";
        private const string SavedCheckpointKey = "SAVED.CHECKPOINT";
    


        public void Initialize()
        {
            SoundVolume.Value = PlayerPrefs.GetFloat(SoundVolumeKey);
            CurrentCheckpoint.Value = PlayerPrefs.GetInt(CurrentCheckpointKey);
            SavedCheckpoint.Value = PlayerPrefs.GetInt(SavedCheckpointKey);
        
            SoundVolume.Subscribe(_ => OnSoundVolumeChange());
            CurrentCheckpoint.Subscribe(_ => OnCurCheckpointChange());
            SavedCheckpoint.Subscribe(_ => OnSavCheckpointChange());
        
        }

        private void OnSavCheckpointChange()
        {
            PlayerPrefs.SetInt(SavedCheckpointKey, SavedCheckpoint.Value);
            PlayerPrefs.Save();
        }

        private void OnCurCheckpointChange()
        {
            PlayerPrefs.SetInt(CurrentCheckpointKey, CurrentCheckpoint.Value);
            PlayerPrefs.Save();
        }

        private void OnSoundVolumeChange()
        {
            PlayerPrefs.SetFloat( SoundVolumeKey , SoundVolume.Value);
            PlayerPrefs.Save();
        }
    }
}