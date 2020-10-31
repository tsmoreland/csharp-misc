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

package moreland.sample.jna.interop.service.win32;

import moreland.sample.jna.interop.service.MessageBox;
import moreland.sample.jna.interop.service.MessageBoxResult;
import moreland.sample.jna.interop.service.MessageBoxType;
import moreland.sample.jna.interop.service.win32.internal.User32LibraryInterface;

import java.util.Optional;
import java.util.Set;

public final class Win32MessageBoxAnsi implements MessageBox {

    private User32LibraryInterface user32Library;

    /**
     * {@inheritDoc}
     */
    public Win32MessageBoxAnsi() {
        this(User32LibraryInterface.INSTANCE);
    }

    Win32MessageBoxAnsi(User32LibraryInterface user32Library) {
        super();
        this.user32Library = user32Library;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public Optional<MessageBoxResult> display(String text, String caption, Set<MessageBoxType> type) {
        
        short language = 0;
        try {
            var nativeResult = user32Library.MessageBoxExA(null, text, caption, MessageBoxType.toInteger(type), language);
            if (nativeResult == 0) {
                return Optional.empty();
            }

            var result = MessageBoxResult.fromInteger(nativeResult);
            if (!result.isPresent()) {
                // ... log failure ...
            }
            return result;

        } catch (UnsatisfiedLinkError e) {
            System.out.println(e.getMessage());
            Throwable inner;
            while ((inner = e.getCause()) != null) {
                System.out.println("\t" + inner.getMessage());
            }
            return Optional.empty();
        }
    }
}
