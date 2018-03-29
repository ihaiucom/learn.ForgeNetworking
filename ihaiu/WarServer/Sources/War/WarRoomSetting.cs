namespace Games.Wars
{
    public class WarRoomSetting
    {

        public UnitSoliderAIType soliderBehaviourType;
        public BillingType billingType;
        //是否需要告诉服务器退出战斗了
        public bool SendToServer;
        
        public static WarRoomSetting Default = new WarRoomSetting() { soliderBehaviourType = UnitSoliderAIType.None, billingType = BillingType.None, SendToServer = true };
        public static WarRoomSetting MulitPve = new WarRoomSetting() { soliderBehaviourType = UnitSoliderAIType.None, billingType = BillingType.None, SendToServer = false };
        public static WarRoomSetting ActivityGlod = new WarRoomSetting() { soliderBehaviourType = UnitSoliderAIType.PathOver, billingType = BillingType.SoliderFinal, SendToServer = true };
        public static WarRoomSetting ActivityExp = new WarRoomSetting() { soliderBehaviourType = UnitSoliderAIType.LiveOver, billingType = BillingType.SoliderFinal, SendToServer = true };
        public static WarRoomSetting PVPLadder = new WarRoomSetting() { soliderBehaviourType = UnitSoliderAIType.None, billingType = BillingType.DeathHero, SendToServer = true };

        public static WarRoomSetting Create(StageType stageType,ActivityType activityType)
        {
            switch (stageType)
            {
                case StageType.PVEActivity:
                    {
                        switch (activityType)
                        {
                            case ActivityType.Gold:
                                return ActivityGlod;
                            case ActivityType.Exp:
                                return ActivityExp;
                        }
                    }
                    return ActivityGlod;
                case StageType.PVE:
                    return MulitPve;
                case StageType.PVPLadder:
                    return PVPLadder;
            }
            return Default;
        }
    }
}