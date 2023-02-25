using PuzzleManiacs.HUD;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PuzzleManiacs.Level
{
    /// <summary>
    /// BGM의 반복 재생에 관한 데이터들 담고 있는 테이블.
    /// </summary>
    [CreateAssetMenu(fileName = "New BGM Table", menuName = "Data Table/BGM", order = int.MaxValue)]
    public sealed class BGMLoopData : ScriptableObject, IEquatable<BGMLoopData>
    {
        /// <summary>
        /// BGM의 원본 파일.
        /// </summary>
        [Tooltip("BGM의 원본 파일.")]
        public AudioClip clip;

        /// <summary>
        /// BGM의 이름.
        /// </summary>
        [Tooltip("BGM의 이름.")]
        public string bgmName;

        /// <summary>
        /// BGM의 시작 지점.
        /// </summary>
        [Tooltip("BGM의 시작 지점.")]
        public float startFrom;

        /// <summary>
        /// BGM의 반복 시작 지점.
        /// </summary>
        [Tooltip("BGM의 반복 시작 지점.")]
        public float loopStart;

        /// <summary>
        /// BGM의 반복 종료 지점.
        /// </summary>
        [Tooltip("BGM의 반복 종료 지점.")]
        public float loopEnd;

        private void Reset()
        {
            bgmName     = "Unknown BGM";
            startFrom   = 0.0f;
            loopStart   = 0.0f;
            loopEnd     = 999.9f;
        }

        public bool Equals(BGMLoopData other)
        {
            return Equals(clip, other.clip) && string.Equals(bgmName, other.bgmName) &&
                   startFrom.Equals(other.startFrom) && loopStart.Equals(other.loopStart) && loopEnd.Equals(other.loopEnd);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is BGMLoopData && Equals((BGMLoopData)obj);
        }

        public override int GetHashCode()
        {
            //return base.GetHashCode();
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + (clip != null ? clip.GetHashCode() : 0);
                hash = hash * 23 + (bgmName != null ? bgmName.GetHashCode() : 0);
                hash = hash * 23 + (!float.IsNaN(startFrom) ? startFrom.GetHashCode() : 0);
                hash = hash * 23 + (!float.IsNaN(loopStart) ? loopStart.GetHashCode() : 0);
                hash = hash * 23 + (!float.IsNaN(loopEnd) ? loopEnd.GetHashCode() : 0);
                return hash;
            }
        }

        public static bool operator ==(BGMLoopData left, BGMLoopData right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BGMLoopData left, BGMLoopData right)
        {
            return !left.Equals(right);
        }
    }
}