using System.Collections;
using OnDot.Util;
using UnityEngine;

namespace OnDot.System
{
    public class ScreenFaderManager : PersistentSingleton<ScreenFaderManager>
    {
        public enum FadeType
        {
            Black,
            Loading
        }

        [SerializeField] private CanvasGroup blackCanvasGroup;
        [SerializeField] private CanvasGroup loadingCanvasGroup;
        [SerializeField] private float fadeDuration = 1f;

        public static void DirectFadeOut(FadeType fadeType = FadeType.Black)
        {
            CanvasGroup canvasGroup;
            switch (fadeType)
            {
                case FadeType.Black:
                    canvasGroup = Instance.blackCanvasGroup;
                    break;
                default:
                    canvasGroup = Instance.loadingCanvasGroup;
                    break;
            }
            canvasGroup.gameObject.SetActive(true);

            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = false;
        }

        public static void FadeOut(FadeType fadeType = FadeType.Black)
        {
            Instance.StartCoroutine(Instance.IFadeOut(fadeType));
        }

        private IEnumerator IFadeOut(FadeType fadeType)
        {
            CanvasGroup canvasGroup;
            switch (fadeType)
            {
                case FadeType.Black:
                    canvasGroup = Instance.blackCanvasGroup;
                    break;
                default:
                    canvasGroup = Instance.loadingCanvasGroup;
                    break;
            }
            canvasGroup.gameObject.SetActive(true);

            yield return Instance.StartCoroutine(Instance.Fade(1f, canvasGroup));
        }

        public static void FadeIn()
        {
            Instance.StartCoroutine(Instance.IFadeIn());
        }

        private IEnumerator IFadeIn()
        {
            CanvasGroup canvasGroup;
            if (Instance.blackCanvasGroup.alpha > 0.1f)
            {
                canvasGroup = Instance.blackCanvasGroup;
            }
            else
            {
                canvasGroup = Instance.loadingCanvasGroup;
            }

            yield return Instance.StartCoroutine(Instance.Fade(0f, canvasGroup));

            canvasGroup.gameObject.SetActive(false);
        }

        private IEnumerator Fade(float finalAlpha, CanvasGroup canvasGroup)
        {
            canvasGroup.blocksRaycasts = true;
            float fadeSpeed = Mathf.Abs(canvasGroup.alpha - finalAlpha) / fadeDuration;
            while (!Mathf.Approximately(canvasGroup.alpha, finalAlpha))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
                yield return null;
            }
            canvasGroup.alpha = finalAlpha;
            canvasGroup.blocksRaycasts = false;
        }
    }
}