using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SevenBag
{
    private const int MINO_TYPES = 7;

    private IMino[] nextMinos;
    private int suffleNum = 30;
    private int current = 0;
    public int Current => current;

    public SevenBag()
    {
        nextMinos = new IMino[MINO_TYPES];
        List<int> pool = new List<int> { 0, 1, 2, 3, 4, 5, 6 };
        int a, b, temp;
        for (int _ = 0; _ < suffleNum; ++_)
        {
            a = Random.Range(0, MINO_TYPES);
            b = Random.Range(0, MINO_TYPES);
            temp = pool[a];
            pool[a] = pool[b];
            pool[b] = temp;
        }
        for(int i=0; i<pool.Count; ++i)
        {
            nextMinos[i] = MinoFactory.Create((MinoFactory.MinoType)pool[i]);
        }
    }

    public IMino GetNext()
    {
        if (current < 7)
            return nextMinos[current++];
        else
            return null;
    }

    public IMino[] GetRemainder()
    {
        IMino[] toReturn = new IMino[MINO_TYPES-current];
        for (int i = current, j = 0; i < MINO_TYPES; ++i, ++j)
            toReturn[j] = nextMinos[i];
        return toReturn;
    }
}