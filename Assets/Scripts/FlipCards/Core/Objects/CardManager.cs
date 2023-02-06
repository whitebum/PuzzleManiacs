using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PuzzleExpress.FlipCards.Core.Objects
{
    public sealed class CardManager : MonoBehaviour
    {
        /// <summary>
        /// 최대 선택할 수 있는 기본 카드 장수.
        /// </summary>
        private const int defaultSelectedCardsCount = 2;

        /// <summary>
        /// 최대 선택할 수 있는 카드 장수.
        /// </summary>
        [Tooltip("최대 선택할 수 있는 카드 장수.")]
        [SerializeField] private int maxSelectedCardsCount;

        /// <summary>
        /// 복사할 원본 오브젝트들.
        /// </summary>
        [Tooltip("복사할 원본 오브젝트들.")]
        [SerializeField] private List<Card> originals;

        /// <summary>
        /// 카드 뭉치.
        /// </summary>
        private List<Card> deck;

        /// <summary>
        /// 현재 선택된 카드들.
        /// </summary>
        private List<Card> selectedCards;

        private void Reset()
        {
            maxSelectedCardsCount = defaultSelectedCardsCount;
        }

        private void Awake()
        {
            if (originals != null && originals.Count > 1)
            {
                deck = new List<Card>();
                deck.Capacity = originals.Count * originals.Count;

                selectedCards = new List<Card>();
                selectedCards.Capacity = maxSelectedCardsCount <= 0 ? defaultSelectedCardsCount : maxSelectedCardsCount;

                foreach (var original in originals)
                {
                    for (byte count = 0; count < 2; ++count)
                    {
                        var newCard = Instantiate(original);
                        newCard.transform.SetParent(transform);
                        newCard.onSelected.AddListener(() => selectedCards.Add(newCard));

                        deck.Add(newCard);
                    }
                }

                var rnd = new System.Random();
                deck = deck.OrderBy(card => rnd.Next()).ToList();
            }
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            if (selectedCards.Count == 2)
            {
                if (selectedCards[0].type == selectedCards[1].type)
                {
                    foreach (var card in selectedCards)
                        card.FlipToBackSide();
                }
                else
                {
                    foreach (var card in selectedCards)
                        card.FlipToBackSide();
                }

                selectedCards.Clear();

                Debug.Log($"{selectedCards.Count}  {selectedCards.Capacity}");
            }
        }
    }
}