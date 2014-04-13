function hasHtml5Validation() {
    return typeof document.createElement('input').checkValidity === 'function';
}
$(function () {
    if (hasHtml5Validation()) {
        console.log("going!");
        $('form').submit(function (e) {
            if (!this.checkValidity()) {
                e.preventDefault();
                $(this).addClass('invalid');
            } else {
                $(this).removeClass('invalid');
            }
        });
    }
});