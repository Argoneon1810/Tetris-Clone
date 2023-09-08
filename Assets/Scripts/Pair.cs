public class Pair<T, K>
{
    private T first;
    private K second;
    public T First => first;
    public K Second => second;
    public Pair(T first, K second) {
        this.first = first;
        this.second = second;
    }
}