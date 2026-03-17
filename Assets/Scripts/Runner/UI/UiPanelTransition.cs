using System.Collections;
using UnityEngine;

namespace Runner.UI
{
    [DisallowMultipleComponent]
    public sealed class UiPanelTransition : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float duration = 0.2f;
        [SerializeField] private AnimationCurve curve = null;
        [SerializeField] private bool disableGameObjectOnHide = true;

        private Coroutine _transitionRoutine;

        private void Awake()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();

            if (curve == null || curve.length == 0)
                curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        }

        public void Show(bool instant = false)
        {
            SetVisible(true, instant);
        }

        public void Hide(bool instant = false)
        {
            SetVisible(false, instant);
        }

        public void SetVisible(bool visible, bool instant = false)
        {
            if (canvasGroup == null)
            {
                gameObject.SetActive(visible);
                return;
            }

            if (_transitionRoutine != null)
                StopCoroutine(_transitionRoutine);

            if (instant || duration <= 0f)
            {
                ApplyImmediate(visible);
                return;
            }

            _transitionRoutine = StartCoroutine(Animate(visible));
        }

        private IEnumerator Animate(bool visible)
        {
            if (visible && gameObject.activeSelf == false)
                gameObject.SetActive(true);

            float startAlpha = canvasGroup.alpha;
            float targetAlpha = visible ? 1f : 0f;
            float elapsed = 0f;

            canvasGroup.blocksRaycasts = visible;
            canvasGroup.interactable = visible;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float evaluated = curve.Evaluate(t);
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, evaluated);
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;

            if (visible == false && disableGameObjectOnHide)
                gameObject.SetActive(false);

            _transitionRoutine = null;
        }

        private void ApplyImmediate(bool visible)
        {
            if (visible && gameObject.activeSelf == false)
                gameObject.SetActive(true);

            canvasGroup.alpha = visible ? 1f : 0f;
            canvasGroup.blocksRaycasts = visible;
            canvasGroup.interactable = visible;

            if (visible == false && disableGameObjectOnHide)
                gameObject.SetActive(false);
        }
    }
}
