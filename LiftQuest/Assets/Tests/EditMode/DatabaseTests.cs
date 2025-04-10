using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
//using Moq; // For mocking
using System.Collections;

[TestFixture]
public class DatabaseTests
{
    private DatabaseManager dbManager;
    //private Mock<InputField> mockUsernameInputField;
    
    [Test]
    public void SimpleAdditionTest()
    {
        int result = 2 + 2;
        Assert.AreEqual(4, result);
    }

    [Test]
    public void StringShouldNotBeNull()
    {
        string message = "Hello, world!";
        Assert.IsNotNull(message);
    }

    [Test]
    public void InputField_AssignAndReadValue_WorksCorrectly()
    {
        // Arrange: create GameObject and add InputField
        var go = new GameObject("UsernameField");
        var inputField = go.AddComponent<InputField>();
        inputField.text = "TestUser";

        // Act + Assert
        Assert.AreEqual("TestUser", inputField.text);
        
        // Cleanup
        Object.DestroyImmediate(go);
    }
}


/*using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using Moq; // For mocking
using System.Collections;

[TestFixture]
public class DatabaseManagerTests
{
    private DatabaseManager dbManager;
    private Mock<InputField> mockUsernameInputField;

    [SetUp]
    public void SetUp()
    {
        // Create a mock InputField for username
        mockUsernameInputField = new Mock<InputField>();

        // Create a DatabaseManager instance
        dbManager = new GameObject().AddComponent<DatabaseManager>();
        dbManager.username = mockUsernameInputField.Object;
    }

    [Test]
    public void CreateProfile_UsernameIsEmpty_ShouldLogError()
    {
        // Set the username to be empty
        mockUsernameInputField.Setup(x => x.text).Returns("");

        // Call CreateProfile
        dbManager.CreateProfile();

        // Check if the appropriate error is logged
        Assert.IsTrue(Debug.unityLogger.logHandler.IsLogTypePresent(LogType.Error));
    }
}*/
