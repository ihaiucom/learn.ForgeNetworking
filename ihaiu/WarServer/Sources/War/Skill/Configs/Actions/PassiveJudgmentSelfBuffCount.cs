namespace Games.Wars
{
    /// <summary>
    /// 自身buff层数
    /// </summary>
    public class PassiveJudgmentSelfBuffCount : PassiveJudgment
    {
        public  int         buffId                  = 0;
        public  CompareType compare                 = CompareType.Equal;
        public  int         buffCount               = 0;
    }
}