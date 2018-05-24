﻿using Unity;
using NUnit.Framework;
using SciChart.Wpf.UI.Reactive.Traits;

namespace SciChart.Wpf.UI.Reactive.Tests.Traits
{
    [TestFixture]
    public class ViewModelTraitCollectionTests
    {              
        [Test]
        public void WhenAddTraitShouldAdd()
        {
            // Arrange
            var container = new UnityContainer();
            var parent = new StubObservableObject();
            var collection = new ViewModelTraitCollection(parent, container);

            // Act
            var b = collection.Add<StubViewModelTrait>();

            // Assert
            Assert.That(b, Is.Not.Null);
            Assert.That(collection.Contains<StubViewModelTrait>(), Is.True);
            Assert.That(b.Target, Is.EqualTo(parent));
        }

        [Test]
        public void WhenAddTraitTwiceShouldReplace()
        {
            // Arrange
            var container = new UnityContainer();
            var parent = new StubObservableObject();
            var collection = new ViewModelTraitCollection(parent, container);

            // Act
            var b0 = collection.Add<StubViewModelTrait>();
            b0.Id = "b0";
            var b1 = collection.Add<StubViewModelTrait>();
            b1.Id = "b1";

            // Assert
            Assert.That(b0, Is.Not.Null);
            Assert.That(b1, Is.Not.Null);
            Assert.That(collection.Contains<StubViewModelTrait>(), Is.True);
            Assert.That(b0.IsDisposed, Is.True);
            Assert.That(b1.IsDisposed, Is.False);
            Assert.That(b1.Target, Is.EqualTo(parent));
        }

        [Test]
        public void WhenDisposingParentShouldDispose()
        {
            // Arrange
            var container = new UnityContainer();
            var parent = new StubObservableObject();
            var collection = new ViewModelTraitCollection(parent, container);
            var child = new StubDisposable();

            // Act
            var b0 = collection.Add<StubViewModelTrait>();
            child.DisposeWith(b0);
            parent.Dispose();

            // Assert            
            Assert.That(parent.IsDisposed, Is.True);
            Assert.That(b0.IsDisposed, Is.True);
            Assert.That(child.IsDisposed, Is.True);
        }        
    }
}