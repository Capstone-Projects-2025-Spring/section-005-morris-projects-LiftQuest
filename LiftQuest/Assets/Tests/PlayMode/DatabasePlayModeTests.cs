using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using System.Collections;

public class DatabaseManagerPlayModeTests
{
    private GameObject go;
    private DatabaseManager dbManager;

    [SetUp]
    public void SetUp()
    {
        go = new GameObject("DatabaseManager");
        dbManager = go.AddComponent<DatabaseManager>();

        // Assign all necessary InputFields
        dbManager.username = CreateInputField("username");
        dbManager.password = CreateInputField("password");
        dbManager.password2 = CreateInputField("password2");

        dbManager.resting_measurement = CreateInputField("resting");
        dbManager.above_head_measurement = CreateInputField("above_head");
        dbManager.floor_measurement = CreateInputField("floor");
        dbManager.stage_completed = CreateInputField("stage");

        dbManager.login_username = CreateInputField("login_username");
        dbManager.login_password = CreateInputField("login_password");
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(go);
    }

    private InputField CreateInputField(string name)
    {
        var go = new GameObject(name);
        var inputField = go.AddComponent<InputField>();
        return inputField;
    }

    [UnityTest]
    public IEnumerator Create_Profile_Empty_Username_Should_Log_Error()
    {
        dbManager.username.text = "";
        dbManager.password.text = "password";
        dbManager.password2.text = "password";

        LogAssert.Expect(LogType.Error, "Username cannot be empty.");
        dbManager.CreateProfile();

        yield return null;
    }

    [UnityTest]
    public IEnumerator Create_Profile_Mismatched_Passwords_Should_Log_Error()
    {
        dbManager.username.text = "username";
        dbManager.password.text = "pass";
        dbManager.password2.text = "word";

        LogAssert.Expect(LogType.Error, "Passwords must match.");
        dbManager.CreateProfile();

        yield return null;
    }

    [UnityTest]
    public IEnumerator Logout_Clears_ProfileID()
    {
        PlayerPrefs.SetString("ProfileID", "test123");
        dbManager.Logout();

        yield return null;

        Assert.IsFalse(PlayerPrefs.HasKey("ProfileID"));
    }


    /*[UnityTest]
    public IEnumerator Create_Profile_Without_Firebase_Should_Log_Error()
    {
        dbManager.username.text = "validUser";
        dbManager.password.text = "123";
        dbManager.password2.text = "123";

        LogAssert.Expect(LogType.Error, "Database reference is null. Ensure Firebase is initialized.");
        dbManager.CreateProfile();

        yield return null;
    }*/
}
