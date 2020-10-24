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

import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.MethodSource;

import java.util.Arrays;
import java.util.EnumSet;
import java.util.stream.Stream;

import static org.junit.jupiter.api.Assertions.assertEquals;

public class MessageBoxTypeTests {

    @ParameterizedTest(name = "{index} => {0}")
    @MethodSource("getMessageBoxTypeIntegerValues")
    public void toInteger_returnsEmptyEnumSet_whenValueIsValid(Pair<Integer, EnumSet<MessageBoxType>> type) {
        var expectedValue = type.item1;
        var enumSet = type.item2;

        var actualValue = MessageBoxType.toInteger(enumSet);

        assertEquals(expectedValue, actualValue);
    }

    private static Stream<Pair<Integer, EnumSet<MessageBoxType>>> getMessageBoxTypeIntegerValues() {
        var singleValues = Arrays.stream(MessageBoxType
            .class
            .getEnumConstants())
            .map(type -> new Pair<>(type.getValue(), EnumSet.of(type)));
        var multiValues = Stream.of(
            new Pair<>(
                MessageBoxType.OK.getValue() | MessageBoxType.OK_CANCEL.getValue(),
                EnumSet.of(MessageBoxType.OK, MessageBoxType.OK_CANCEL)),
            new Pair<>(
                MessageBoxType.YES_NO.getValue() | MessageBoxType.YES_NO_CANCEL.getValue() | MessageBoxType.CANCEL_TRY_CONTINUE.getValue(),
                EnumSet.of(MessageBoxType.YES_NO, MessageBoxType.YES_NO_CANCEL, MessageBoxType.CANCEL_TRY_CONTINUE))
        );
        return Stream.concat(singleValues, multiValues);
    }

}
