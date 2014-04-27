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

function toggleParticipantInputs(obj) {
    console.log("Hello");
    $(obj).parent().parent().parent().find('.p-name-tex-box').toggle();
    $(obj).parent().parent().parent().find('select').attr('disabled', function (idx, oldAttr) { return !oldAttr; });
}

