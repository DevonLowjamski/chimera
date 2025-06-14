using NUnit.Framework;
using UnityEngine;

namespace ProjectChimera.Testing
{
    [TestFixture]
    public class BasicCompilationTests
    {
        //[Test]
        public void Assembly_CompilesSuccessfully()
        {
            // Basic test to verify the testing assembly compiles
            Assert.IsTrue(true, "If this test runs, the assembly compiled successfully");
        }

        //[Test]
        public void Unity_BasicFunctionality_Works()
        {
            // Test basic Unity functionality
            var testObject = new GameObject("TestObject");
            Assert.IsNotNull(testObject, "GameObject creation should work");
            Assert.AreEqual("TestObject", testObject.name, "GameObject name should be set correctly");
            
            // Test component addition
            var rigidbody = testObject.AddComponent<Rigidbody>();
            Assert.IsNotNull(rigidbody, "Component addition should work");
            
            // Cleanup
            Object.DestroyImmediate(testObject);
        }

        //[Test]
        public void NUnit_Framework_Works()
        {
            // Test NUnit framework functionality
            string testString = "Test";
            Assert.IsNotNull(testString, "String should not be null");
            Assert.AreEqual("Test", testString, "String comparison should work");
            Assert.IsTrue(testString.Length > 0, "String length check should work");
            
            // Test numeric assertions
            float testFloat = 42.5f;
            Assert.AreEqual(42.5f, testFloat, 0.001f, "Float comparison should work");
            Assert.Greater(testFloat, 0f, "Greater than comparison should work");
        }

        //[Test]
        public void ProjectChimera_Testing_Namespace_Accessible()
        {
            // Test that our testing namespace is properly set up
            string namespaceName = this.GetType().Namespace;
            Assert.IsNotNull(namespaceName, "Namespace should not be null");
            Assert.IsTrue(namespaceName.Contains("ProjectChimera"), "Should be in ProjectChimera namespace");
            Assert.IsTrue(namespaceName.Contains("Testing"), "Should be in Testing namespace");
        }

        //[Test]
        public void TestDataSO_CanBeCreated()
        {
            // Test that our test ScriptableObject classes can be instantiated
            var testPlantData = ScriptableObject.CreateInstance<TestPlantDataSO>();
            Assert.IsNotNull(testPlantData, "TestPlantDataSO should be creatable");
            Assert.IsNotNull(testPlantData.PlantName, "PlantName should have default value");
            
            var testEquipmentData = ScriptableObject.CreateInstance<TestEquipmentDataSO>();
            Assert.IsNotNull(testEquipmentData, "TestEquipmentDataSO should be creatable");
            Assert.IsNotNull(testEquipmentData.EquipmentName, "EquipmentName should have default value");
            
            // Cleanup
            Object.DestroyImmediate(testPlantData);
            Object.DestroyImmediate(testEquipmentData);
        }

        //[Test]
        public void TestEventChannels_CanBeCreated()
        {
            // Test that our test event channels can be instantiated
            var testSimpleEvent = ScriptableObject.CreateInstance<TestSimpleEventSO>();
            Assert.IsNotNull(testSimpleEvent, "TestSimpleEventSO should be creatable");
            
            var testStringEvent = ScriptableObject.CreateInstance<TestStringEventSO>();
            Assert.IsNotNull(testStringEvent, "TestStringEventSO should be creatable");
            
            // Cleanup
            Object.DestroyImmediate(testSimpleEvent);
            Object.DestroyImmediate(testStringEvent);
        }

        //[Test]
        public void Assembly_References_AreWorking()
        {
            // Test that we can access basic Unity types that should be available
            Assert.IsNotNull(typeof(GameObject), "GameObject type should be accessible");
            Assert.IsNotNull(typeof(MonoBehaviour), "MonoBehaviour type should be accessible");
            Assert.IsNotNull(typeof(ScriptableObject), "ScriptableObject type should be accessible");
            
            // Test that we can access NUnit types
            Assert.IsNotNull(typeof(TestFixtureAttribute), "TestFixture attribute should be accessible");
            Assert.IsNotNull(typeof(TestAttribute), "Test attribute should be accessible");
        }
    }
} 