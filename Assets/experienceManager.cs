using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class experienceManager : MonoBehaviour
{
    public int currentExperience = 0;
    public int experienceToLevelUp = 100;
    public int currentLevel = 1;

    private void Start()
    {
        //UISingleton.Instance.updateExperience(currentExperience);
        //UISingleton.Instance.updateLevel(currentLevel);
    }

    public void addExperience(int exp)
    {
        currentExperience += exp;
        //UISingleton.Instance.updateExperience(currentExperience);
        Debug.Log(currentExperience + " / " + experienceToLevelUp);
        if (currentExperience >= experienceToLevelUp)
        {
            currentExperience -= experienceToLevelUp;
            experienceToLevelUp += 50;
            currentLevel++;
            //UISingleton.Instance.updateExperience(currentExperience);
            //UISingleton.Instance.updateLevel(currentLevel);
        }
    }
}
