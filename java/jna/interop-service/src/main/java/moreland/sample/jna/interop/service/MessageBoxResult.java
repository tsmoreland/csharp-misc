package moreland.sample.jna.interop.service;

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

    private int value;
    MessageBoxResult(int value) {
        this.value = value;
    }
}
