﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SciChart.Wpf.UI.Reactive.Behaviours;
using SciChart.Wpf.UI.Reactive.Observability;
using SciChart.Wpf.UI.Reactive.Tests.Stubs;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace SciChart.Wpf.UI.Reactive.Tests.Behaviours
{
    [TestFixture]
    public class BehaviourCollectionTests
    {
        public class MyObservableObject : ObservableObjectBase
        {            
        }

        public class MyBehaviour : Behaviour<MyObservableObject>
        {
            public MyBehaviour(MyObservableObject target) : base(target)
            {
            }

            public string Id { get; set; }

            public override void Dispose()
            {
                base.Dispose();

                IsDisposed = true;
            }

            public bool IsDisposed { get; set; }
        }

        [Test]
        public void WhenAddBehaviourShouldAdd()
        {
            // Arrange
            var container = new UnityContainer();
            var parent = new MyObservableObject();
            var collection = new BehaviourCollection(parent, container);

            // Act
            var b = collection.Add<MyBehaviour>();

            // Assert
            Assert.That(b, Is.Not.Null);
            Assert.That(collection.Contains<MyBehaviour>(), Is.True);
            Assert.That(b.Target, Is.EqualTo(parent));
        }

        [Test]
        public void WhenAddBehaviourTwiceShouldReplace()
        {
            // Arrange
            var container = new UnityContainer();
            var parent = new MyObservableObject();
            var collection = new BehaviourCollection(parent, container);

            // Act
            var b0 = collection.Add<MyBehaviour>();
            b0.Id = "b0";
            var b1 = collection.Add<MyBehaviour>();
            b1.Id = "b1";

            // Assert
            Assert.That(b0, Is.Not.Null);
            Assert.That(b1, Is.Not.Null);
            Assert.That(collection.Contains<MyBehaviour>(), Is.True);
            Assert.That(b0.IsDisposed, Is.True);
            Assert.That(b1.IsDisposed, Is.False);
            Assert.That(b1.Target, Is.EqualTo(parent));
        }

        [Test]
        public void WhenDisposingParentShouldDispose()
        {
            // Arrange
            var container = new UnityContainer();
            var parent = new MyObservableObject();
            var collection = new BehaviourCollection(parent, container);

            // Act
            var b0 = collection.Add<MyBehaviour>();
            parent.Dispose();

            // Assert            
            Assert.That(b0.IsDisposed, Is.True);
        }
    }
}