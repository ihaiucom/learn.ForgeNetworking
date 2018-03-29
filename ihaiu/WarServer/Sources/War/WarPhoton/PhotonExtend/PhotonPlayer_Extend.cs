
public static class PhotonPlayer_Extend
{
    public static int GetLegionId(this PhotonPlayer photonPlayer)
    {
        if(photonPlayer.CustomProperties.ContainsKey(TSPlayerPropertiesKey.LegionId))
        {
            return (int)photonPlayer.CustomProperties[TSPlayerPropertiesKey.LegionId];
        }
        return -1;
    }
}