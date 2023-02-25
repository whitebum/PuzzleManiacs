using PuzzleManiacs.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PuzzleManiacs.Level
{
    public sealed class SceneManager : Singleton<SceneManager>
    {
        [SerializeField] private Animator animator;

        [SerializeField] private string openTrigger;
        private int openTriggerHash;

        [SerializeField] private string closeTrigger;
        private int closeTriggerHash;

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }

        protected override void Awake()
        {
            base.Awake();

            animator = animator ?? GetComponent<Animator>();

            openTriggerHash = !string.IsNullOrEmpty(openTrigger) ? Animator.StringToHash(openTrigger) : 0;
            closeTriggerHash = !string.IsNullOrEmpty(closeTrigger) ? Animator.StringToHash(closeTrigger) : 0;
        }

        public void LoadScene(string sceneName)
        {
            if (animator != null)
            {
                animator.SetTrigger(openTriggerHash);
                animator.Update(0.0f);
            }

            StartCoroutine(WaitForAnimationCoroutine(openTrigger, () => StartCoroutine(LoadSceneCoroutine(sceneName))));
        }

        private IEnumerator WaitForAnimationCoroutine(string animation, UnityAction action)
        {
            if (animator != null)
            {
                yield return new WaitUntil(() =>
                {
                    return animator.GetCurrentAnimatorStateInfo(0).IsName(animation) && 
                           animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.999f;
                });
            }

            action.Invoke();

            yield return null;
        }

        private IEnumerator LoadSceneCoroutine(string sceneName)
        {
            var nextScene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

            while (true)
            {
                if (nextScene.isDone == true)
                {
                    if (animator != null)
                    {
                        animator.SetTrigger(closeTriggerHash);
                        animator.Update(0.0f);

                        yield return new WaitUntil(() =>
                        {
                            return animator.GetCurrentAnimatorStateInfo(0).IsName(closeTrigger) &&
                                   animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.999f;
                        });

                        yield break;
                    }
                }

                yield return null;
            }
        }
    }
}