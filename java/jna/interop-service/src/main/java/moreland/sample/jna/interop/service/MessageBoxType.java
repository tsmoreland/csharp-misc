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

import java.util.EnumSet;
import java.util.Objects;
import java.util.Set;

public enum MessageBoxType {

    /**
     * The message box contains three push buttons:
     * Abort, Retry, and Ignore.
     */
    ABORT_RETRY_IGNORE(0x00000002),

    /**
     * The message box contains three push buttons:
     * Cancel, Try Again, Continue. Use this message
     * box type instead of
     * {@Code MessageBoxType.ABORT_RETRY_IGNORE}.
     */
    HELP(0x00004000),

    /**
     * Adds a Help button to the message box.
     * When the user clicks the Help button or presses F1,
     * the system sends a WM_HELP message to the owner.
     */
    CANCEL_TRY_CONTINUE(0x00000006),

    /**
     * The message box contains one push button:
     * OK. This is the default.
     */
    OK(0x00000000),
    /**
     * The message box contains two push buttons:
     * OK and Cancel.
     */
    OK_CANCEL(0x00000001),
    /**
     * The message box contains two push buttons:
     * Retry and Cancel.
     */
    RETRY_CANCEL(0x00000005),
    /**
     * The message box contains two push buttons:
     * Yes and No.
     */
    YES_NO(0x00000004),
    /**
     * The message box contains three push buttons:
     * Yes, No, and Cancel.
     */
    YES_NO_CANCEL(0x00000003),

    /**
     * An exclamation-point icon appears in the message box.
     */
    ICON_EXCLAMATION(0x00000030),
    /**
     * An exclamation-point icon appears in the message box.
     */
    ICONWARNING(0x00000030),
    /**
     * An icon consisting of a lowercase letter i in a circle appears in the
     * message box.
     */
    ICONINFORMATION(0x00000040),
    /**
     * An icon consisting of a lowercase letter i in a circle appears in the
     * message box.
     */
    ICONASTERISK(0x00000040),
    /**
     * A question-mark icon appears in the message box. The question-mark
     * message icon is no longer recommended because it does not clearly
     * represent a specific type of message and because the phrasing of a
     * message as a question could apply to any message type. In addition,
     * users can confuse the message symbol question mark with Help information.
     * Therefore, do not use this question mark message symbol in your message
     * boxes. The system continues to support its inclusion only for backward
     * compatibility.
     */
    ICONQUESTION(0x00000020),
    /**
     * A stop-sign icon appears in the message box.
     */
    ICONSTOP(0x00000010),
    /**
     * A stop-sign icon appears in the message box.
     */
    ICONERROR(0x00000010),
    /**
     * A stop-sign icon appears in the message box.
     */
    ICONHAND(0x00000010),

    /**
     * The first button is the default button.
     * {@Code DEF_BUTTON1} is the default unless {@Code DEF_BUTTON2},
     * {@Code DEF_BUTTON3}, or {@Code DEF_BUTTON4} is specified.
     */
    DEF_BUTTON1(0x00000000),

    /**
     * The second button is the default button.
     */
    DEF_BUTTON2(0x00000100),
    /**
     * The third button is the default button.
     */
    DEF_BUTTON3(0x00000200),
    /**
     * The fourth button is the default button.
     */
    DEF_BUTTON4(0x00000300),

    /**
     * The user must respond to the message box before continuing work in the
     * window identified by the hWnd parameter. However, the user can move to
     * the windows of other threads and work in those windows.
     * Depending on the hierarchy of windows in the application, the user may
     * be able to move to other windows within the thread. All child windows
     * of the parent of the message box are automatically disabled, but pop-up
     * windows are not.
     * {@Code APPL_MODAL}(is the default if neither {@Code SYSTEM_MODAL} nor
     * {@Code TASK_MODAL}), is specified.
     */
    APPL_MODAL(0x00000000),

    /**
     * Same as {@Code APPL_MODAL} except that the message box has the
     * WS_EX_TOPMOST style. Use system-modal message boxes to notify the user
     * of serious, potentially damaging errors that require immediate attention
     * (for example, running out of memory). This flag has no effect on the
     * user's ability to interact with windows other than those associated
     * with hWnd.
     */
    SYSTEM_MODAL(0x00001000),
    /**
     * Same as {@Code APPL_MODAL}(except that all the top-level windows
     * belonging to the current thread are disabled if the hWnd parameter is
     * NUL),. Use this flag when the calling application or library does not
     * have a window handle available but still needs to prevent input to
     * other windows in the calling thread without suspending other threads.
     */
    TASK_MODAL(0x00002000),

    /**
     * Same as desktop of the interactive window station. For more information,
     * see Window Stations.
     * If the current input desktop is not the default desktop, MessageBox does
     * not return until the user switches to the default desktop.
     */
    DEFAULT_DESKTOP_ONLY(0x00020000),

    /**
     * The text is right-justified.
     */
    RIGHT(0x00080000),
    /**
     * Displays message and caption text using right-to-left reading order on
     * Hebrew and Arabic systems.
     */
    RTL_READING(0x00100000),
    /**
     * The message box becomes the foreground window. Internally, the system
     * calls the SetForegroundWindow function for the message box.
     */
    SET_FOREGROUND(0x00010000),
    /**
     * The message box is created with the WS_EX_TOPMOST window style.
     */
    TOP_MOST(0x00040000),

    /**
     * The caller is a service notifying the user of an event. The function displays a message box on the current active desktop, even if there is no user logged on to the computer.
     * Terminal Services: If the calling thread has an impersonation token, the function directs the message box to the session specified in the impersonation token.
     * If this flag is set, the hWnd parameter must be NULL. This is so that the message box can appear on a desktop other than the desktop corresponding to the hWnd.
     */
    SERVICE_NOTIFICATION(0x00200000);

    /**
     * returns the raw value of the EnumSet
     * @return
     */
    public int getValue() {
        return value;
    }

    /**
     * integer value resulting from or operations on all values present in {@Code values}
     * @param values source of values to convert
     * @return integer value resulting from or operations on all values present in {@Code values}
     * @exception IllegalArgumentException if {@Code} is null
     */
    public static int toInteger(Set<MessageBoxType> values) {
        if (Objects.isNull(values))
            throw new IllegalArgumentException("values is null");

        return values
            .stream()
            .map(MessageBoxType::getValue)
            .reduce(0, (left, right) -> left | right);
    }

    private int value;

    MessageBoxType(int value) {
        this.value = value;
    }
}
