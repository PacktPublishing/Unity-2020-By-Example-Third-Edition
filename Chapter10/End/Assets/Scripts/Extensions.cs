using System.Collections.Generic;

public static class Extensions
{
    public static void Shuffle<T>(this IList<T> ThisList)
    {
        var Count = ThisList.Count;
        var Last = Count - 1;
        for (var i = 0; i < Last; ++i)
        {
            var RandomIndex = UnityEngine.Random.Range(i, Count);
            var Temp = ThisList[i];
            ThisList[i] = ThisList[RandomIndex];
            ThisList[RandomIndex] = Temp;
        }
    }
}
