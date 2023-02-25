using System;
using System.Collections.Generic;
using UnityEngine;

namespace System.Runtime.CompilerServices
{
    public static class IsExternalInit { }
}

namespace PuzzleManiacs.Core
{
    /// <summary>
    /// 게임 내에서 사용되는 키를 관리하는 정적 클래스.
    /// </summary>
    public static class InputUtility
    {
        /// <summary>
        /// 키 설정 레코드.
        /// </summary>
        public record KeySetting
        {
            public string key { get; init; }
            public KeyCode keyCode { get; init; }

        }

        /// <summary>
        /// '선택 키'의 설정 값.
        /// </summary>
        private static readonly KeySetting selectKeySetting = new KeySetting 
        { 
            key     = "Select Key", 
            keyCode = KeyCode.Return 
        };

        /// <summary>
        /// 현재 저장되어 있는 '선택 키'를 가져오거나, '선택 키'를 정의할 수 있습니다.
        /// </summary>
        public static KeyCode SelectKey
        {
            get => PlayerPrefs.HasKey(selectKeySetting.key) ? (KeyCode)PlayerPrefs.GetInt(selectKeySetting.key) : selectKeySetting.keyCode;
            set => PlayerPrefs.SetInt(selectKeySetting.key, (int)value);
        }
    }
}
