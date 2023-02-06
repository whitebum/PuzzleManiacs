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
        /// �ִ� ������ �� �ִ� �⺻ ī�� ���.
        /// </summary>
        private const int defaultSelectedCardsCount = 2;

        /// <summary>
        /// �ִ� ������ �� �ִ� ī�� ���.
        /// </summary>
        [Tooltip("�ִ� ������ �� �ִ� ī�� ���.")]
        [SerializeField] private int maxSelectedCardsCount;

        /// <summary>
        /// ������ ���� ������Ʈ��.
        /// </summary>
        [Tooltip("������ ���� ������Ʈ��.")]
        [SerializeField] private List<Card> originals;

        /// <summary>
        /// ī�� ��ġ.
        /// </summary>
        private List<Card> deck;

        /// <summary>
        /// ���� ���õ� ī���.
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