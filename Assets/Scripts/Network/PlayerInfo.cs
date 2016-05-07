using UnityEngine;
using System.Collections;

/*
 *this class contains Player info which we get from  
 * Server By Socket
 */
public class PlayerInfo {
    
    private int score;
    private int rank;
    private string name;
    private int numberOfGoal;

    public void setScore(int score)
    {
        this.score = score;
    }
    public void setRank(int rank)
    {
        this.rank = rank;
    }
    public void setName(string name)
    {
        this.name = name;
    }

    public void setNumberOfGoal(int num)
    {
        numberOfGoal = num;
    }
    public int getScore()
    {
        return score;
    }
    public int getRank()
    {
        return rank;
    }
    public string getName()
    {
        return name;
    }
    public int getNumberOfGoal()
    {
        return numberOfGoal;
    }
}
