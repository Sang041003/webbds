
    $(document).ready(function () {
        // Show the alert if there is a ViewBag.ErrorMessage
        var errorMessage = '@ViewBag.ErrorMessage';
    if (errorMessage) {
        showErrorMessage(errorMessage);
        }
    });

    function showErrorMessage(message) {
        var alertElement = $('<div class="alert alert-danger" role="alert">' + message + '</div>');
    $('#notification-container').append(alertElement);
    alertElement.addClass('show');

    // Hide the alert after 5 seconds
    setTimeout(function () {
        alertElement.removeClass('show');
    setTimeout(function () {
        alertElement.remove();
            }, 2000);
        }, 5000);
    }
