public static class GameSession
{
    // Index 0 = Tutorial, 1 = W1, 2 = W2, etc.
    // By default, only Tutorial (0) is true.
    public static bool[] WorldUnlocked = new bool[] { true, false, false, false, false };

    public static void UnlockWorld(int index)
    {
        if (index < WorldUnlocked.Length)
        {
            WorldUnlocked[index] = true;
        }
    }
}