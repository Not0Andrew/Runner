using System;
using UnityEngine;

namespace Runner.Config
{
    [Serializable]
    public sealed class SceneFlowSettings
    {
        [SerializeField] private string bootstrapSceneName = "Bootstrap";
        [SerializeField] private string menuSceneName = "Menu";
        [SerializeField] private string gameSceneName = "Game";

        public string BootstrapSceneName => bootstrapSceneName;
        public string MenuSceneName => menuSceneName;
        public string GameSceneName => gameSceneName;
    }
}
