/*
 * Copyright © 2020 Terry Moreland
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

package moreland.sample.modules.math;

/**
 * simple calculator
 */
public interface Calculator {
    /**
     * simple addition
     * @param a left hand side of addition
     * @param b right hand side of addition
     * @return sum of {@code a} and {@code b}
     */
    double add(double a, double b);
    /**
     * simple division
     * @param a dividend
     * @param b divisor
     * @return result of {@code a} divided {@code b}
     */
    double divide(double a, double b);
    /**
     * simple multiplication
     * @param a left hand side of multiplication
     * @param b right hand side of multiplication
     * @return sum of {@code a} and {@code b}
     */
    double multiply(double a, double b);
    /**
     * simple subtraction
     * @param a left hand side of subtraction
     * @param b right hand side of subtraction
     * @return result of subtrating {@code b} from {@code a}
     */
    double subtract(double a, double b);
}
