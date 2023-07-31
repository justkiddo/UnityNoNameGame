using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace root
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI continueButtonText;
        [SerializeField] private TextMeshProUGUI menuButtonText;
        [SerializeField] private TextMeshProUGUI quitText;
        private IUnityLocalization _localization;


        [Inject]
        private void Construct(IUnityLocalization localization)
        {
            _localization = localization;
        }

        private void Awake()
        {
            AddListeners();
        }

        private void AddListeners()
        {
            quitText.text = _localization.Translate("quit.button");
            continueButtonText.text = _localization.Translate("pause.continue");
            menuButtonText.text = _localization.Translate("tab.menu");
        }
    }
}