using System.Collections.Generic;
using UnityEngine;

namespace Runner.Customization
{
    public sealed class EnvironmentThemeApplier : MonoBehaviour
    {
        [Header("Auto scan renderers")]
        [SerializeField] private bool autoCollectSceneRenderers = true;
        [SerializeField] private bool includeInactiveRenderers = false;
        [SerializeField] private List<Renderer> extraRenderers = new List<Renderer>();
        [SerializeField] private List<Renderer> excludedRenderers = new List<Renderer>();

        [Header("Theme B Palette")]
        [SerializeField] private Color themeBTint = new Color(0.86f, 0.92f, 1f, 1f);
        [SerializeField] private bool applyEmission = true;
        [SerializeField] private Color themeBEmission = new Color(0.08f, 0.12f, 0.18f, 1f);

        [Header("Lighting")]
        [SerializeField] private bool changeDirectionalLight = true;
        [SerializeField] private Light directionalLight;
        [SerializeField] private Color themeBLightColor = new Color(0.78f, 0.84f, 1f);
        [SerializeField] private float themeBLightIntensity = 0.85f;

        [Header("World Fog / Ambient")]
        [SerializeField] private bool useFog = true;
        [SerializeField] private Color themeBFogColor = new Color(0.25f, 0.32f, 0.45f, 1f);
        [SerializeField] private float themeBFogDensity = 0.012f;
        [SerializeField] private bool changeAmbientLight = true;
        [SerializeField] private Color themeBAmbient = new Color(0.2f, 0.25f, 0.33f, 1f);
        [SerializeField] private bool tintSkybox = true;
        [SerializeField] private Color themeBSkyboxTint = new Color(0.8f, 0.9f, 1f, 1f);

        private readonly List<Renderer> _resolvedRenderers = new List<Renderer>();
        private MaterialPropertyBlock _propertyBlock;
        private Color _defaultDirectionalLightColor;
        private float _defaultDirectionalLightIntensity;
        private Color _defaultAmbientLight;
        private bool _defaultFogEnabled;
        private Color _defaultFogColor;
        private float _defaultFogDensity;
        private Material _defaultSkybox;
        private bool _hasSkyboxTint;
        private string _skyboxTintProp;
        private Color _defaultSkyboxTint;

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
            CacheDefaultRenderSettings();
            ResolveRenderers();
        }

        private void Start()
        {
            Apply(EnvironmentThemeStorage.Get());
        }

        public void Apply(EnvironmentThemeType theme)
        {
            bool isThemeB = theme == EnvironmentThemeType.ThemeB;

            if (isThemeB)
            {
                ApplyThemeBToRenderers();
                ApplyThemeBWorldSettings();
            }
            else
            {
                RestoreDefaultRenderers();
                RestoreDefaultWorldSettings();
            }

            DynamicGI.UpdateEnvironment();
        }

        private void ResolveRenderers()
        {
            _resolvedRenderers.Clear();

            if (autoCollectSceneRenderers)
            {
                Renderer[] found = includeInactiveRenderers
                    ? FindObjectsOfType<Renderer>(true)
                    : FindObjectsOfType<Renderer>(false);

                for (int i = 0; i < found.Length; i++)
                    TryAddRenderer(found[i]);
            }

            for (int i = 0; i < extraRenderers.Count; i++)
                TryAddRenderer(extraRenderers[i]);
        }

        private void TryAddRenderer(Renderer rendererRef)
        {
            if (rendererRef == null)
                return;

            for (int i = 0; i < excludedRenderers.Count; i++)
            {
                if (excludedRenderers[i] == rendererRef)
                    return;
            }

            if (_resolvedRenderers.Contains(rendererRef))
                return;

            _resolvedRenderers.Add(rendererRef);
        }

        private void ApplyThemeBToRenderers()
        {
            for (int i = 0; i < _resolvedRenderers.Count; i++)
            {
                Renderer rendererRef = _resolvedRenderers[i];
                if (rendererRef == null)
                    continue;

                Material[] sharedMaterials = rendererRef.sharedMaterials;
                int materialCount = sharedMaterials != null ? sharedMaterials.Length : 0;

                for (int m = 0; m < materialCount; m++)
                {
                    Material source = sharedMaterials[m];
                    if (source == null)
                        continue;

                    rendererRef.GetPropertyBlock(_propertyBlock, m);
                    ApplyTintToBlock(source, _propertyBlock);
                    if (applyEmission)
                        ApplyEmissionToBlock(source, _propertyBlock);

                    rendererRef.SetPropertyBlock(_propertyBlock, m);
                }
            }
        }

        private void RestoreDefaultRenderers()
        {
            for (int i = 0; i < _resolvedRenderers.Count; i++)
            {
                Renderer rendererRef = _resolvedRenderers[i];
                if (rendererRef == null)
                    continue;

                int materialCount = rendererRef.sharedMaterials != null ? rendererRef.sharedMaterials.Length : 0;
                for (int m = 0; m < materialCount; m++)
                    rendererRef.SetPropertyBlock(null, m);
            }
        }

        private void ApplyTintToBlock(Material source, MaterialPropertyBlock block)
        {
            if (source.HasProperty("_BaseColor"))
            {
                Color baseColor = source.GetColor("_BaseColor");
                block.SetColor("_BaseColor", Multiply(baseColor, themeBTint));
            }

            if (source.HasProperty("_Color"))
            {
                Color color = source.GetColor("_Color");
                block.SetColor("_Color", Multiply(color, themeBTint));
            }
        }

        private void ApplyEmissionToBlock(Material source, MaterialPropertyBlock block)
        {
            if (source.HasProperty("_EmissionColor"))
            {
                Color emission = source.GetColor("_EmissionColor");
                block.SetColor("_EmissionColor", Multiply(emission, themeBEmission));
            }
        }

        private void CacheDefaultRenderSettings()
        {
            _defaultSkybox = RenderSettings.skybox;

            if (changeDirectionalLight && directionalLight != null)
            {
                _defaultDirectionalLightColor = directionalLight.color;
                _defaultDirectionalLightIntensity = directionalLight.intensity;
            }

            _defaultAmbientLight = RenderSettings.ambientLight;
            _defaultFogEnabled = RenderSettings.fog;
            _defaultFogColor = RenderSettings.fogColor;
            _defaultFogDensity = RenderSettings.fogDensity;

            Material skybox = RenderSettings.skybox;
            if (skybox != null)
            {
                _hasSkyboxTint = TryFindSkyboxTintProperty(skybox, out _skyboxTintProp);
                if (_hasSkyboxTint)
                    _defaultSkyboxTint = skybox.GetColor(_skyboxTintProp);
            }
        }

        private void ApplyThemeBWorldSettings()
        {
            if (changeDirectionalLight && directionalLight != null)
            {
                directionalLight.color = themeBLightColor;
                directionalLight.intensity = themeBLightIntensity;
            }

            if (useFog)
            {
                RenderSettings.fog = true;
                RenderSettings.fogColor = themeBFogColor;
                RenderSettings.fogDensity = themeBFogDensity;
            }

            if (changeAmbientLight)
                RenderSettings.ambientLight = themeBAmbient;

            if (tintSkybox && RenderSettings.skybox != null && _hasSkyboxTint)
                RenderSettings.skybox.SetColor(_skyboxTintProp, themeBSkyboxTint);
        }

        private void RestoreDefaultWorldSettings()
        {
            if (changeDirectionalLight && directionalLight != null)
            {
                directionalLight.color = _defaultDirectionalLightColor;
                directionalLight.intensity = _defaultDirectionalLightIntensity;
            }

            if (useFog)
            {
                RenderSettings.fog = _defaultFogEnabled;
                RenderSettings.fogColor = _defaultFogColor;
                RenderSettings.fogDensity = _defaultFogDensity;
            }

            if (changeAmbientLight)
                RenderSettings.ambientLight = _defaultAmbientLight;

            if (_defaultSkybox != null)
                RenderSettings.skybox = _defaultSkybox;

            if (tintSkybox && RenderSettings.skybox != null && _hasSkyboxTint)
                RenderSettings.skybox.SetColor(_skyboxTintProp, _defaultSkyboxTint);
        }

        private static bool TryFindSkyboxTintProperty(Material skybox, out string propertyName)
        {
            if (skybox.HasProperty("_Tint"))
            {
                propertyName = "_Tint";
                return true;
            }

            if (skybox.HasProperty("_SkyTint"))
            {
                propertyName = "_SkyTint";
                return true;
            }

            if (skybox.HasProperty("_Color"))
            {
                propertyName = "_Color";
                return true;
            }

            propertyName = null;
            return false;
        }

        private static Color Multiply(Color a, Color b)
        {
            return new Color(a.r * b.r, a.g * b.g, a.b * b.b, a.a);
        }
    }
}
