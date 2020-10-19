package com.moreland.sample.modules;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertEquals;

final class SimpleCalculatorTest {

    private Calculator<Integer> calculator;

    @BeforeEach
    void BeforeEach() {
        calculator = new SimpleCalculator();
    }


    @Test
    void add_returns3_whenAdding1And2() {
        int result = calculator.add(1, 2);
        assertEquals(3, result);
    }

}
