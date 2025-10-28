document.addEventListener('DOMContentLoaded', function () {
    var profileBtn = document.querySelector('.profile-btn');
    var dropdown = document.querySelector('.profile-dropdown');
    if (profileBtn && dropdown) {
        profileBtn.addEventListener('mouseenter', function () {
            dropdown.style.display = 'block';
        });
        profileBtn.addEventListener('mouseleave', function () {
            setTimeout(function () {
                if (!dropdown.matches(':hover')) {
                    dropdown.style.display = 'none';
                }
            }, 100);
        });
        dropdown.addEventListener('mouseleave', function () {
            dropdown.style.display = 'none';
        });
        dropdown.addEventListener('mouseenter', function () {
            dropdown.style.display = 'block';
        });
    }
});
