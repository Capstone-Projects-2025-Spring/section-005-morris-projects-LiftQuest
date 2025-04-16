using NUnit.Framework;

public class ProfileData
{
    public string username;
    public string password;
    public string password2;
    public string restingMeasurement;
    public string aboveHeadMeasurement;
    public string floorMeasurement;
    public string stageCompleted;
}

public static class ProfileValidator
{
    public static string Validate(ProfileData data)
    {
        if (string.IsNullOrWhiteSpace(data.username))
            return "Username cannot be empty.";

        if (string.IsNullOrWhiteSpace(data.password) || string.IsNullOrWhiteSpace(data.password2))
            return "Password cannot be empty.";

        if (data.password != data.password2)
            return "Passwords do not match.";

        return null; // No error
    }
}

public class DatabaseManagerEditModeTests
{
    [Test]
    public void Empty_Username_Returns_Error()
    {
        var data = new ProfileData
        {
            username = "",
            password = "password",
            password2 = "password"
        };

        string error = ProfileValidator.Validate(data);
        Assert.AreEqual("Username cannot be empty.", error);
    }

    [Test]
    public void Empty_Passwords_Returns_Error()
    {
        var data = new ProfileData
        {
            username = "username",
            password = "",
            password2 = ""
        };

        string error = ProfileValidator.Validate(data);
        Assert.AreEqual("Password cannot be empty.", error);
    }

    [Test]
    public void Passwords_Dont_Match_Returns_Error()
    {
        var data = new ProfileData
        {
            username = "username",
            password = "pass",
            password2 = "word"
        };

        string error = ProfileValidator.Validate(data);
        Assert.AreEqual("Passwords do not match.", error);
    }

    [Test]
    public void All_Fields_Valid_Returns_Null()
    {
        var data = new ProfileData
        {
            username = "username",
            password = "password",
            password2 = "password",
            restingMeasurement = "10",
            aboveHeadMeasurement = "20",
            floorMeasurement = "5",
            stageCompleted = "1"
        };

        string error = ProfileValidator.Validate(data);
        Assert.IsNull(error);
    }

    [Test]
    public void Whitespace_Username_Returns_Error()
    {
        var data = new ProfileData
        {
            username = "   ",
            password = "password",
            password2 = "password"
        };

        string error = ProfileValidator.Validate(data);
        Assert.AreEqual("Username cannot be empty.", error);
    }

    [Test]
    public void One_Password_Filled_Returns_Error()
    {
        var data = new ProfileData
        {
            username = "username",
            password = "password",
            password2 = ""
        };

        string error = ProfileValidator.Validate(data);
        Assert.AreEqual("Password cannot be empty.", error);
    }


}


