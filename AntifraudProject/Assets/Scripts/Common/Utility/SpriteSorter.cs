using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using UnityEngine;

namespace Common.Utility
{
    public class SpriteSorter : MonoBehaviour
    {
        private Dictionary<SpriteRenderer, int> _spritesDefaultOrders;
        
        private void Awake()
        {
            _spritesDefaultOrders = GetComponentsInChildren<SpriteRenderer>()
                .ToDictionary(it => it, it => it.sortingOrder);
        }

        public void AddToOrder(int orderDelta)
        {
            foreach (var pair in _spritesDefaultOrders)
            {
                pair.Key.sortingOrder += orderDelta;
            }
        }

        public void ResetOrders()
        {
            _spritesDefaultOrders.ForEach(pair => pair.Key.sortingOrder = pair.Value);
        }
    }
}