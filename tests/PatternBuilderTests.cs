using System;
using NUnit.Framework;
using Moq;
using UnityEngine;

namespace flexington.WFC.Tests
{
    [TestFixture]
    public class PatternBuilderTests
    {
        private Mock<ITile> _tileMock;
        private Sprite _sprite;

        [SetUp]
        public void BeforeEach()
        {
            _tileMock = new Mock<ITile>();
            _sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), Vector2.one);
        }

        [Test]
        public void WithSize_WhenWidthIsLessThanOrEqualToZero_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            PatternBuilder<TestPattern> patternBuilder = new PatternBuilder<TestPattern>();
            int width = 0;
            int height = 1;

            // Act
            void Act() => patternBuilder.WithSize(width, height);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(Act);
        }

        [Test]
        public void WithSize_WhenHeightIsLessThanOrEqualToZero_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            PatternBuilder<TestPattern> patternBuilder = new PatternBuilder<TestPattern>();
            int width = 1;
            int height = 0;

            // Act
            void Act() => patternBuilder.WithSize(width, height);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(Act);
        }

        [Test]
        public void WithSize_WithValidArguments_ReturnsPatternBuilder()
        {
            // Arrange
            PatternBuilder<TestPattern> patternBuilder = new PatternBuilder<TestPattern>();
            int width = 1;
            int height = 1;

            // Act
            var result = patternBuilder.WithSize(width, height);

            // Assert
            Assert.IsInstanceOf<IPatternBuilder<TestPattern>>(result);
        }

        [Test]
        public void WithTiles_WhenTilesAreNull_ThrowsArgumentNullException()
        {
            // Arrange
            PatternBuilder<TestPattern> patternBuilder = new PatternBuilder<TestPattern>();
            ITile[,] tiles = null;

            // Act
            void Act() => patternBuilder.WithTiles(tiles).Build();

            // Assert
            Assert.Throws<ArgumentNullException>(Act);
        }

        [Test]
        public void WithTiles_WithValidArguments_ReturnsPatternBuilder()
        {
            // Arrange
            PatternBuilder<TestPattern> patternBuilder = new PatternBuilder<TestPattern>();

            ITile[,] tiles = new ITile[2, 2];

            // Act
            var result = patternBuilder.WithTiles(tiles);

            // Assert
            Assert.IsInstanceOf<IPatternBuilder<TestPattern>>(result);
        }

        [Test]
        public void SetTile_WhenTilesAreNull_ThrowsInvalidOperationException()
        {
            // Arrange
            PatternBuilder<TestPattern> patternBuilder = new PatternBuilder<TestPattern>();
            int x = 0;
            int y = 0;

            // Act
            void Act() => patternBuilder.SetTile(x, y, _tileMock.Object);

            // Assert
            Assert.Throws<InvalidOperationException>(Act);
        }

        [Test]
        public void SetTile_WhenXIsLessThanZero_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            PatternBuilder<TestPattern> patternBuilder = new PatternBuilder<TestPattern>();
            int width = 1;
            int height = 1;
            int x = -1;
            int y = 0;

            // Act
            void Act() => patternBuilder.WithSize(width, height).SetTile(x, y, _tileMock.Object);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(Act);
        }

        [Test]
        public void SetTile_WhenYIsLessThanZero_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            PatternBuilder<TestPattern> patternBuilder = new PatternBuilder<TestPattern>();
            int width = 1;
            int height = 1;
            int x = 0;
            int y = -1;

            // Act
            void Act() => patternBuilder.WithSize(width, height).SetTile(x, y, _tileMock.Object);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(Act);
        }

        [Test]
        public void SetTile_WhenXIsGreaterThanWidth_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            PatternBuilder<TestPattern> patternBuilder = new PatternBuilder<TestPattern>();
            int width = 1;
            int height = 1;
            int x = 2;
            int y = 0;

            // Act
            void Act() => patternBuilder.WithSize(width, height).SetTile(x, y, _tileMock.Object);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(Act);
        }

        [Test]
        public void SetTile_WhenYIsGreaterThanHeight_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            PatternBuilder<TestPattern> patternBuilder = new PatternBuilder<TestPattern>();
            int width = 1;
            int height = 1;
            int x = 0;
            int y = 2;

            // Act
            void Act() => patternBuilder.WithSize(width, height).SetTile(x, y, _tileMock.Object);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(Act);
        }

        [Test]
        public void SetTile_WhenTileIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            PatternBuilder<TestPattern> patternBuilder = new PatternBuilder<TestPattern>();
            int width = 1;
            int height = 1;
            int x = 0;
            int y = 0;

            // Act
            void Act() => patternBuilder.WithSize(width, height).SetTile(x, y, null);

            // Assert
            Assert.Throws<ArgumentNullException>(Act);
        }

        [Test]
        public void SetTile_WithValidArguments_ReturnsPatternBuilder()
        {
            // Arrange
            PatternBuilder<TestPattern> patternBuilder = new PatternBuilder<TestPattern>();
            int width = 1;
            int height = 1;
            int x = 0;
            int y = 0;

            // Act
            var result = patternBuilder.WithSize(width, height).SetTile(x, y, _tileMock.Object);

            // Assert
            Assert.IsInstanceOf<IPatternBuilder<TestPattern>>(result);
        }

        [Test]
        public void Build_WhenTilesAreNull_ThrowsInvalidOperationException()
        {
            // Arrange
            PatternBuilder<TestPattern> patternBuilder = new PatternBuilder<TestPattern>();

            // Act
            void Act() => patternBuilder.Build();

            // Assert
            Assert.Throws<InvalidOperationException>(Act);
        }

        [Test]
        public void Build_WithValidArguments_ReturnsPattern()
        {
            // Arrange
            PatternBuilder<TestPattern> patternBuilder = new PatternBuilder<TestPattern>();
            int width = 1;
            int height = 1;
            int x = 0;
            int y = 0;

            // Act
            var result = patternBuilder.WithSize(width, height).SetTile(x, y, _tileMock.Object).Build();

            // Assert
            Assert.IsInstanceOf<TestPattern>(result);
        }

        public class TestPattern : IPattern
        {
            public IHash Hash { get; private set; }
            public ITile[,] Tiles { get; set; }

            public TestPattern() { }
        }
    }

}