using System;
using System.Collections.Generic;
using Runner.Core;
using UnityEngine;

namespace Runner.Customization
{
    public sealed class VisualCustomizationApplier : MonoBehaviour
    {
        [Serializable]
        private sealed class CharacterSkinBinding
        {
            public CharacterSkins Skin;
            public GameObject Target;
        }

        [Serializable]
        private sealed class MazeSkinBinding
        {
            public MazeSkins Skin;
            public GameObject Target;
        }

        [Serializable]
        private sealed class BonusStyleBinding
        {
            public int StyleId;
            public GameObject Target;
        }

        [Header("Mappings")]
        [SerializeField] private List<CharacterSkinBinding> characterBindings = new List<CharacterSkinBinding>();
        [SerializeField] private List<MazeSkinBinding> mazeBindings = new List<MazeSkinBinding>();
        [SerializeField] private List<BonusStyleBinding> bonusStyleBindings = new List<BonusStyleBinding>();

        private void Start()
        {
            Apply();
        }

        public void Apply()
        {
            PlayerData data = LoadData();
            if (data == null)
                return;

            ApplyCharacter(data.SelectedCharacterSkin);
            ApplyMaze(data.SelectedMazeSkin);
            ApplyBonusStyle(data.SelectedBonusStyle);
        }

        private PlayerData LoadData()
        {
            if (RunnerRuntimeContext.PersistentData?.PlayerData != null)
                return RunnerRuntimeContext.PersistentData.PlayerData;

            var persistent = new PersistentData();
            var provider = new DataLocalProvider(persistent);
            if (provider.TryLoad() == false)
                return null;

            return persistent.PlayerData;
        }

        private void ApplyCharacter(CharacterSkins skin)
        {
            for (int i = 0; i < characterBindings.Count; i++)
                SetActive(characterBindings[i].Target, characterBindings[i].Skin == skin);
        }

        private void ApplyMaze(MazeSkins skin)
        {
            for (int i = 0; i < mazeBindings.Count; i++)
                SetActive(mazeBindings[i].Target, mazeBindings[i].Skin == skin);
        }

        private void ApplyBonusStyle(int styleId)
        {
            for (int i = 0; i < bonusStyleBindings.Count; i++)
                SetActive(bonusStyleBindings[i].Target, bonusStyleBindings[i].StyleId == styleId);
        }

        private static void SetActive(GameObject target, bool active)
        {
            if (target != null)
                target.SetActive(active);
        }
    }
}
