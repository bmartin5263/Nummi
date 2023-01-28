namespace Nummi.Core.Util; 

public readonly struct EnumeratorReader<T> {
    private IEnumerator<T> Enumerator { get; }

    public EnumeratorReader(IEnumerator<T> enumerator) {
        Enumerator = enumerator;
    }

    public T? Advance(int steps) {
        T? result = default;
        for (int i = 0; i < steps; ++i) {
            result = Advance();
        }

        return result;
    }

    public T? Advance() {
        return Enumerator.MoveNext() ? Enumerator.Current : default;
    }
}