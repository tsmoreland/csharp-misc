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

import java.util.Optional;
import java.util.Set;

public interface MessageBox {
    /**
     * Creates, displays, and operates a message box. The message box contains
     * an application-defined message and title, plus any combination of
     * predefined icons and push buttons. The buttons are in the language of
     * the system user interface.
     * @param text The message to be displayed. If the string consists of more
     *             than one line, you can separate the lines using a carriage
     *             return and/or linefeed character between each line.
     * @param caption The dialog box title. If this parameter is null,
     *                the default title is Error.
     * @param type The contents and behavior of the dialog box.
     * @return  on failure {@Code Optional.empty()}; on success,
     * if a message box has a Cancel button, the function returns the
     * {@Code MessageBoxResult.CANCEL} value if either the ESC key is pressed
     * or the Cancel button is selected. If the message box has no Cancel
     * button, pressing ESC will no effect - unless an {@Code MessageBoxType.OK}
     * button is present. If an {@Code MessageBoxType.OK} button is displayed
     * and the user presses ESC, the return value will be
     * {@Code MessageBoxResult.OK}.
     */
    Optional<MessageBoxResult> display(String text, String caption, Set<MessageBoxType> type);
}
