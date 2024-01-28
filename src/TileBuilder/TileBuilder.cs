using System;
using UnityEngine;

namespace flexington.WFC
{
    public class TileBuilder<T> : ITileBuilder<T> where T : ITile, new()
    {
        private Sprite _sprite;

        public TileBuilder<T> WithSprite(Sprite sprite)
        {
            _sprite = sprite;
            return this;
        }

        public ITile Build()
        {
            if (_sprite == null) throw new ArgumentNullException(nameof(_sprite));

            return new T { Sprite = _sprite };
        }
    }
}