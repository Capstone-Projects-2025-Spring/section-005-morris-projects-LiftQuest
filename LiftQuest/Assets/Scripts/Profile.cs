using UnityEngine;

//namespace Profile;

public class Profile
{
    public string username;
    public string password;
    public double resting_measurement;
    public double above_head_measurement;
    public double floor_measurement;
    public int stage_completed;

    public Profile(string username, string password, double resting_measurement, 
        double above_head_measurement, double floor_measurement, int stage_completed)
    {
        this.username = username;
        this.password = password;
        this.resting_measurement = resting_measurement;
        this.above_head_measurement = above_head_measurement;
        this.floor_measurement = floor_measurement;
        this.stage_completed = stage_completed;
    }
}
