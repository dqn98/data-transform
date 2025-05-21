namespace DataTransform.Constants;

public static class Events
{
    private const string PntTnxInputAmountScreenViewed = "pntTnx input amount scrn viewed";
    private const string PntTnxInputAmountScreenViewedParsed = "Input amount screen viewed";

    private const string PntTnxInputAmountButtonClicked = "pntTnx input amount bttn clicked";
    private const string PntTnxInputAmountButtonClickedParsed = "Input Amount Button Clicked";

    private const string PntTnxPickUpScreenViewed = "pntTnx pick up scrn viewed";
    private const string PntTnxPickUpScreenViewedParsed = "pick Up Screen Viewed";

    private const string PntTnxDropOffScreenViewed = "pntTnx drop off scrn viewed";
    private const string PntTnxDropOffScreenViewedParsed = "Drop off scrn viewed";

    private const string PntTnxDropOffButtonClicked = "pntTnx drop off bttn clicked";
    private const string PntTnxDropOffButtonClickedParsed = "Drop Off Button Clicked";
    
    private const string PntTnxConfirmScreenViewed = "pntTnx confirm scrn viewed";
    private const string PntTnxConfirmScreenViewedParsed = "Confirm Screen Viewed";

    private const string PntTnxTtpOiScreenViewed = "pntTnx ttpoi scrn viewed";
    private const string PntTnxTtpOiScreenViewedParsed = "Tap to Pay on iPhone Screen Viewed";

    public const string PntTnxPaymentSuccessfulScreenViewed = "pntTnx payment sucessful scrn viewed";
    private const string PntTnxPaymentSuccessfulScreenViewedParsed = "Payment Successful Screen Viewed";

    public const string PntTnxPaymentDeclineScreenViewed = "pntTnx payment decline scrn viewed";
    private const string PntTnxPaymentDeclineScreenViewedParsed = "Payment Decline Screen Viewed";

    public const string PntTnxPaymentErrorScreenViewed = "pntTnx payment error scrn viewed";
    private const string PntTnxPaymentErrorScreenViewedParsed = "Payment Error Screen Viewed";
    
    public static readonly Dictionary<string, string> ParsedEventMap = new()
    {
        { PntTnxInputAmountScreenViewed, PntTnxInputAmountScreenViewedParsed },
        { PntTnxInputAmountButtonClicked, PntTnxInputAmountButtonClickedParsed },
        { PntTnxPickUpScreenViewed, PntTnxPickUpScreenViewedParsed },
        { PntTnxDropOffScreenViewed, PntTnxDropOffScreenViewedParsed },
        { PntTnxDropOffButtonClicked, PntTnxDropOffButtonClickedParsed },
        { PntTnxConfirmScreenViewed, PntTnxConfirmScreenViewedParsed },
        { PntTnxTtpOiScreenViewed, PntTnxTtpOiScreenViewedParsed },
        { PntTnxPaymentSuccessfulScreenViewed, PntTnxPaymentSuccessfulScreenViewedParsed },
        { PntTnxPaymentDeclineScreenViewed, PntTnxPaymentDeclineScreenViewedParsed },
        { PntTnxPaymentErrorScreenViewed, PntTnxPaymentErrorScreenViewedParsed }
    };
}