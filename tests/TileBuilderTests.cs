using System;
using NUnit.Framework;
using UnityEngine;

namespace flexington.WFC.Tests
{
    [TestFixture]
    public class TileBuilderTests
    {
        private TileBuilder<TestTile> _tileBuilder;

        [SetUp]
        public void SetUp()
        {
            _tileBuilder = new TileBuilder<TestTile>();
        }

        [Test]
        public void WithSprite_WhenSpriteIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            Sprite sprite = null;

            // Act
            void Act() => _tileBuilder.WithSprite(sprite).Build();

            // Assert
            Assert.Throws<ArgumentNullException>(Act);
        }

        [Test]
        public void WithSprite_WhenSpriteIsNotNull_SetsSprite()
        {
            // Arrange
            Sprite sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), Vector2.zero);

            // Act
            ITile tile = _tileBuilder.WithSprite(sprite).Build();

            // Assert
            Assert.AreEqual(sprite, tile.Sprite);
        }
    }

    public class TestTile : ITile
    {
        public Sprite Sprite { get; set; }
    }
}