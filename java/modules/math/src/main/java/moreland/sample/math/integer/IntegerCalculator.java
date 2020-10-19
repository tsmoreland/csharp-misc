package moreland.sample.math.integer;

import moreland.sample.math.Calculator;

public class IntegerCalculator implements Calculator<Integer> {

    @Override
    public Integer add(Integer a, Integer b) {
        return a + b;
    }

    @Override
    public Integer subtract(Integer a, Integer b) {
        return a - b;
    }

    @Override
    public Integer multiple(Integer a, Integer b) {
        return a * b;
    }
}
