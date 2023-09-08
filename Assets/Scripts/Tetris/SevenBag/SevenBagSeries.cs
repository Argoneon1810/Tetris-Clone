public class SevenBagSeries
{
    private const int MINO_TYPES = 7;
    private SevenBag[] sevenBags;

    public SevenBagSeries()
    {
        sevenBags = new SevenBag[7];
        sevenBags[0] = new SevenBag();
        sevenBags[1] = new SevenBag();
    }

    public IMino GetNext()
    {
        IMino mino = sevenBags[0].GetNext();
        if(mino == null)
        {
            sevenBags[0] = sevenBags[1];
            sevenBags[1] = new SevenBag();
            mino = sevenBags[0].GetNext();
        }
        return mino;
    }

    public IMino[] PeekNext(int length)
    {
        IMino[] nextMinos = sevenBags[0].GetRemainder();
        IMino[] toReturn = new IMino[length];
        if(nextMinos.Length > length)
            for (int i = 0; i < length; ++i)
                toReturn[i] = nextMinos[i];
        else
        {
            for (int i = 0; i < nextMinos.Length; ++i)
                toReturn[i] = nextMinos[i];
            if (nextMinos.Length < length)
            {
                IMino[] nextNextMinos = sevenBags[1].GetRemainder();
                for (int i = nextMinos.Length, j = 0; i < toReturn.Length; ++i, ++j)
                    toReturn[i] = nextNextMinos[j];
            }
        }
        return toReturn;
    }
}