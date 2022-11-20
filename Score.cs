public class Score : Singleton<Score>
{
    protected override bool DontDestroy => true;

    public int score;
}
