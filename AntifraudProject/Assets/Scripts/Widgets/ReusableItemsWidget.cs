using System;
using System.Collections.Generic;
using UnityEngine;

namespace Widgets
{
    public class ReusableItemsWidget : Widget
    {
        [SerializeField] private List<Transform> _items;

        private void Start()
        {
            if (_items == null || _items.Count == 0)
                throw new Exception("At least one item is required.");
        }

        public void SetItems<TView, TData>(IReadOnlyList<TData> dataList, Action<int, TView, TData> itemInitializeLogic)
        {
            if (dataList.Count > _items.Count)
            {
                for (int i = 0; i < dataList.Count - _items.Count; ++i)
                {
                    _items.Add(Instantiate(_items[0], transform));
                }
            }

            for (var i = 0; i < _items.Count; ++i)
            {
                var item = _items[i];
                if (i < dataList.Count)
                {
                    itemInitializeLogic(i, item.GetComponent<TView>(), dataList[i]);
                    item.gameObject.SetActive(true);
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
            }
        }
    }
}