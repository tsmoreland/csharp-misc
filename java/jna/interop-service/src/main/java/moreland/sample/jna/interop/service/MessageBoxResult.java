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

package moreland.sample.jna.interop.service;

import java.util.Arrays;
import java.util.Optional;

public enum MessageBoxResult {

    /**
     * The Abort button was selected.
     */
    ABORT(3),
    /**
     * The Cancel button was selected.
     */
    CANCEL(2),

    /**
     * The Continue button was selected.
     */
    CONTINUE(11),
    /**
     * The Ignore button was selected.
     */
    IGNORE(5),

    /**
     * The No button was selected.
     */
    NO(7),
    /**
     * The OK button was selected.
     */
    OK(1),

    /**
     * The Retry button was selected.
     */
    RETRY(4),
    /**
     * The Try Again button was selected.
     */
    TRY_AGAIN(10),
    /**
     * The Yes button was selected.
     */
    YES(6);

    public int getValue() {
        return value;
    }

    /**
     * Attempts to create MessageBoxResult from integer value
     * @param value integer value to convert
     * @return Optional containing the converted value on success;
     *         otherwise an empty
     */
    public static Optional<MessageBoxResult> fromInteger(int value) {
        return Arrays
            .stream(MessageBoxResult.class.getEnumConstants())
            .filter(result -> result.getValue() == value)
            .findFirst();
    }

    private int value;
    MessageBoxResult(int value) {
        this.value = value;
    }
}
