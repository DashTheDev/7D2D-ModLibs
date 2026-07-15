namespace DashTheDev.SDTD.ModCore;

public static class GeneralUtility
{
    public static bool IsRunningOnServer()
    {
        if (SingletonMonoBehaviour<ConnectionManager>.Instance == null)
        {
            return false;
        }

        return SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer;
    }

    public static bool IsNotRunningOnServer()
    {
        return !IsRunningOnServer();
    }
}