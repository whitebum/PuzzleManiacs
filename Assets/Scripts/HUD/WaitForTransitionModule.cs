using System;
using UnityEngine;

namespace PuzzleManiacs.HUD
{
    /// <summary>
    /// 갖가지 트랜지션 작업을 대기할 때 사용되는 모듈.
    /// </summary>
    [Serializable]
    public struct WaitForTransitionModule : IEquatable<WaitForTransitionModule>
    {
        /// <summary>
        /// 대기 시간.
        /// </summary>
        [Tooltip("대기 시간.")]
        public float time;

        /// <summary>
        /// 대기할 애니메이션.
        /// </summary>
        [Tooltip("대기할 애니메이션.")]
        public string animation;

        /// <summary>
        /// 대기 트랜지션.
        /// </summary>
        [Tooltip("대기 트랜지션.")]
        public OpenToCloseTransition transition;

        /// <summary>
        /// 대기 오디오.
        /// </summary>
        [Tooltip("대기 오디오.")]
        public AudioClip audio;

        public bool Equals(WaitForTransitionModule other)
        {
            return string.Equals(animation, other.animation) && time.Equals(other.time) && 
                   Equals(transition, other.transition) && Equals(audio, other.audio);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is WaitForTransitionModule && Equals((WaitForTransitionModule)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = time.GetHashCode();
                hashCode = (hashCode * 397) ^ (animation != null ? animation.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (transition != null ? transition.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (audio != null ? audio.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(WaitForTransitionModule left, WaitForTransitionModule right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(WaitForTransitionModule left, WaitForTransitionModule right)
        {
            return !left.Equals(right);
        }
    }
}
