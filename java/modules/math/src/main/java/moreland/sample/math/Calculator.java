package moreland.sample.math;

public interface Calculator<T> {
    T add(T a, T b);
    T subtract(T a, T b);
    T multiple(T a, T b);
}