using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : Singleton<Score>
{
    protected override bool DontDestroy => true;

    public int score;
}
