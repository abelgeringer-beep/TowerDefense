using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class SceneFader : MonoBehaviour
    {
        public Image img;
        public AnimationCurve curve;

        private void Start()
        {
            StartCoroutine(FadeIn());
        }

        public void FadeTo(string scene)
        {
            StartCoroutine(FadeOut(scene));
        }

        IEnumerator FadeIn()
        {
            float t = 1f; // time
            img.enabled = true;
            while (t > 0f)
            {
                t -= Time.deltaTime; // because of IEnumerator we can do this alltho it isn't an Update method
                float a = curve.Evaluate(t);
                img.color = new Color(0f, 0f, 0f, a);
                yield return 0;
            }
        }

        IEnumerator FadeOut(string scene)
        {
            float t = 0f; // time
            while (t < 1f)
            {
                t += Time.deltaTime; // because of IEnumerator we can do this alltho it isn't an Update method
                float a = curve.Evaluate(t);
                img.color = new Color(0f, 0f, 0f, a);
                yield return 0;
            }

            SceneManager.LoadScene(scene);
        }
    }
}